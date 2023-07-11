using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using OneOf;
using OneOf.Types;
using Serilog;
using SMB3Explorer.ApplicationConfig;
using SMB3Explorer.ApplicationConfig.Models;
using SMB3Explorer.Enums;
using SMB3Explorer.Models.Internal;
using SMB3Explorer.Services.ApplicationContext;
using SMB3Explorer.Services.DataService;
using SMB3Explorer.Services.NavigationService;
using SMB3Explorer.Services.SystemIoWrapper;
using SMB3Explorer.Utils;
using static SMB3Explorer.Constants.FileExports;

namespace SMB3Explorer.ViewModels;

public partial class LandingViewModel : ViewModelBase
{
    private readonly IApplicationConfig _applicationConfig;
    private readonly IApplicationContext _applicationContext;
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
    private readonly ISystemIoWrapper _systemIoWrapper;
    private Smb4LeagueSelection? _selectedExistingLeague;
    private SelectedGame _selectedGame;

    public LandingViewModel(IDataService dataService, INavigationService navigationService,
        ISystemIoWrapper systemIoWrapper, IApplicationContext applicationContext, IApplicationConfig applicationConfig)
    {
        _navigationService = navigationService;
        _systemIoWrapper = systemIoWrapper;
        _applicationContext = applicationContext;
        _applicationConfig = applicationConfig;
        _dataService = dataService;

        Log.Information("Initializing LandingViewModel");

        var configResult = _applicationConfig.GetConfigOptions();
        if (configResult.TryPickT1(out var error, out var configOptions))
        {
            Log.Error("Could not retrieve config options for previously selected leagues: {Error}", error);
            MessageBox.Show($"Could not retrieve config options for previously selected leagues: {error.Value}");
            Smb4LeagueSelections = new List<Smb4LeagueSelection>();
        }

        Smb4LeagueSelections = configOptions.Leagues
            .Select(x => new Smb4LeagueSelection(x.Name, x.Id, x.PlayerTeam, x.NumSeasons)
            {
                NumTimesAccessed = x.NumTimesAccessed,
                FirstAccessed = x.FirstAccessed,
                LastAccessed = x.LastAccessed
            })
            .OrderByDescending(x => x.NumTimesAccessed)
            .ToList();
        SelectedGame = configOptions.GamePreference;

        _dataService.ConnectionChanged += DataServiceOnConnectionChanged;
    }

    public List<Smb4LeagueSelection> Smb4LeagueSelections { get; set; }

    public bool AtLeastOneExistingLeague => Smb4LeagueSelections.Any();

    public SelectedGame SelectedGame
    {
        get => _selectedGame;
        set
        {
            SetField(ref _selectedGame, value);

            _applicationContext.SelectedGame = value;

            var configResult = _applicationConfig.GetConfigOptions();
            if (configResult.TryPickT0(out var configOptions, out _))
            {
                if (configOptions.GamePreference != value)
                {
                    configOptions.GamePreference = value;
                    _applicationConfig.SaveConfigOptions(configOptions);
                }
            }

            OnPropertyChanged(nameof(Smb3ButtonVisibility));
            OnPropertyChanged(nameof(Smb4ButtonVisibility));
        }
    }

    public Visibility Smb3ButtonVisibility =>
        SelectedGame is SelectedGame.Smb3 ? Visibility.Visible : Visibility.Collapsed;

    public Visibility Smb4ButtonVisibility =>
        SelectedGame is SelectedGame.Smb4 ? Visibility.Visible : Visibility.Collapsed;

    public Smb4LeagueSelection? SelectedExistingLeague
    {
        get => _selectedExistingLeague;
        set
        {
            SetField(ref _selectedExistingLeague, value);
            ConnectToPreviouslyConnectedSaveGameCommand.NotifyCanExecuteChanged();
        }
    }

    private void DataServiceOnConnectionChanged(object? sender, EventArgs e)
    {
        AutomaticallySelectSaveFileCommand.NotifyCanExecuteChanged();
        ManuallySelectSaveFileCommand.NotifyCanExecuteChanged();
        UseExistingDatabaseCommand.NotifyCanExecuteChanged();

        var configResult = _applicationConfig.GetConfigOptions();
        if (configResult.TryPickT1(out var error, out var configOptions))
        {
            Log.Error("Could not retrieve config options for previously selected leagues: {Error}", error);
            MessageBox.Show($"Could not retrieve config options for previously selected leagues: {error.Value}");
            Smb4LeagueSelections = new List<Smb4LeagueSelection>();
        }

        Smb4LeagueSelections = configOptions.Leagues
            .Select(x => new Smb4LeagueSelection(x.Name, x.Id, x.PlayerTeam, x.NumSeasons)
            {
                NumTimesAccessed = x.NumTimesAccessed,
                FirstAccessed = x.FirstAccessed,
                LastAccessed = x.LastAccessed
            })
            .OrderByDescending(x => x.NumTimesAccessed)
            .ToList();

        OnPropertyChanged(nameof(AtLeastOneExistingLeague));
    }

    private bool CanSelectSaveFile()
    {
        return !_dataService.IsConnected;
    }

    private bool CanConnectToExistingLeague()
    {
        return SelectedExistingLeague is not null;
    }

    [RelayCommand(CanExecute = nameof(CanConnectToExistingLeague))]
    private async Task ConnectToPreviouslyConnectedSaveGame()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        if (SelectedExistingLeague is null)
        {
            DefaultExceptionHandler.HandleException(_systemIoWrapper, "No existing league was selected",
                new Exception("No league selected"));
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        var filePathResult = SaveFile.GetSmb4ExistingSaveFilePath(_systemIoWrapper, SelectedExistingLeague.SaveGameLeagueId);
        if (filePathResult.TryPickT1(out var error, out var filePath) || string.IsNullOrEmpty(filePath))
        {
            DefaultExceptionHandler.HandleException(_systemIoWrapper, "Could not get save file path",
                new Exception(error.Value));
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        var connectionResult = await EstablishDbConnection(filePath);
        HandleDatabaseConnection(connectionResult);
    }

    [RelayCommand(CanExecute = nameof(CanSelectSaveFile))]
    private async Task AutomaticallySelectSaveFile()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var filePathResult = SaveFile.GetSaveFilePath(_systemIoWrapper, _applicationContext.SelectedGame);
        if (filePathResult.TryPickT1(out _, out var filePath) || string.IsNullOrEmpty(filePath))
        {
            // TODO: Handle error
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        var connectionResult = await EstablishDbConnection(filePath);
        HandleDatabaseConnection(connectionResult);
    }

    [RelayCommand(CanExecute = nameof(CanSelectSaveFile))]
    private async Task ManuallySelectSaveFile()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var directoryPath = _applicationContext.SelectedGame switch
        {
            SelectedGame.Smb3 => BaseGameSmb3DirectoryPath,
            SelectedGame.Smb4 => BaseGameSmb4DirectoryPath,
            _ => throw new ArgumentOutOfRangeException(nameof(_applicationContext.SelectedGame),
                _applicationContext.SelectedGame, null)
        };

        var filePathResult =
            SaveFile.GetUserProvidedFile(directoryPath, _systemIoWrapper);
        if (filePathResult.TryPickT1(out _, out var filePath) || string.IsNullOrEmpty(filePath))
        {
            // TODO: Handle error
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        var connectionResult = await EstablishDbConnection(filePath);
        HandleDatabaseConnection(connectionResult);
    }

    [RelayCommand(CanExecute = nameof(CanSelectSaveFile))]
    private async Task UseExistingDatabase()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var filePathResult = SaveFile.GetUserProvidedFile(
            Environment.SpecialFolder.MyDocuments.ToString(),
            _systemIoWrapper,
            "SQLite databases (*.db, *.sqlite)|*.db;*.sqlite");
        if (filePathResult.TryPickT1(out _, out var filePath) || string.IsNullOrEmpty(filePath))
        {
            // TODO: Handle error
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        var connectionResult = await EstablishDbConnection(filePath, false);
        HandleDatabaseConnection(connectionResult);
    }

    private void HandleDatabaseConnection(OneOf<Success, Error> connectionResult)
    {
        if (connectionResult.TryPickT1(out _, out _)) return;

        MessageBox.Show("Successfully connected to SMB3 database at " +
                        $"{Environment.NewLine}{_dataService.CurrentFilePath}");
        _navigationService.NavigateTo<HomeViewModel>();
    }

    private async Task<OneOf<Success, Error>> EstablishDbConnection(string filePath, bool isCompressedSaveGame = true)
    {
        var hasError = false;
        var connectionResult = await _dataService.EstablishDbConnection(filePath, isCompressedSaveGame);

        if (connectionResult.TryPickT1(out var error, out var leaguesInConnection))
        {
            hasError = true;
            DefaultExceptionHandler.HandleException(_systemIoWrapper, "Failed to connect to SMB3 database.",
                new Exception(error.Value));
        }

        if (_applicationContext.SelectedGame is SelectedGame.Smb4 && isCompressedSaveGame)
        {
            var leaguesInConfig = _applicationConfig.GetConfigOptions();
            if (leaguesInConfig.TryPickT1(out var error2, out var configOptions))
            {
                hasError = true;
                DefaultExceptionHandler.HandleException(_systemIoWrapper,
                    "Failed to retrieve leagues from config file", new Exception(error2.Value));
            }

            if (!hasError)
            {
                var existingLeagues = configOptions.Leagues
                    .Select(x => new Smb4LeagueSelection(x.Name, x.Id, x.PlayerTeam, x.NumSeasons)
                    {
                        NumTimesAccessed = x.NumTimesAccessed,
                        FirstAccessed = x.FirstAccessed,
                        LastAccessed = x.LastAccessed
                    })
                    .ToList();

                var newLeagues = leaguesInConnection
                    .Where(x => !existingLeagues.Contains(x))
                    .ToList();

                if (newLeagues.Any())
                {
                    var now = DateTime.Now;
                    configOptions.Leagues.AddRange(newLeagues
                        .Select(x => new League
                        {
                            Id = x.SaveGameLeagueId,
                            Name = x.LeagueName,
                            PlayerTeam = x.PlayerTeam,
                            NumSeasons = x.NumSeasons,
                            NumTimesAccessed = 1,
                            FirstAccessed = now,
                            LastAccessed = now
                        }));

                    var configResult = _applicationConfig.SaveConfigOptions(configOptions);
                    if (configResult.TryPickT1(out var error3, out _))
                    {
                        hasError = true;
                        DefaultExceptionHandler.HandleException(_systemIoWrapper,
                            "Failed to save new league(s) to config file", new Exception(error3.Value));
                    }
                }
                else
                {
                    var smb4LeagueFileName = Path.GetFileName(filePath);
                    var smb4LeagueName = Path.GetFileNameWithoutExtension(smb4LeagueFileName);
                    smb4LeagueName = smb4LeagueName[7..];
                    var ok = Guid.TryParse(smb4LeagueName, out var smb4LeagueId);
                    if (!ok)
                    {
                        Log.Error("Failed to parse GUID from file name {FileName}. " +
                                  "This occurs when we are attempting to cache the SMB4 league in the " +
                                  "config for later on", smb4LeagueFileName);
                        
                        Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = Cursors.Arrow);
                        return new Error();
                    }
                    
                    var existingLeagueCached = existingLeagues
                        .FirstOrDefault(x => x.SaveGameLeagueId == smb4LeagueId);
                    var existingLeagueFromQuery = leaguesInConnection
                        .FirstOrDefault(x => x.SaveGameLeagueId == smb4LeagueId);

                    if (existingLeagueCached is not null && existingLeagueFromQuery is not null)
                    {
                        existingLeagueCached.NumTimesAccessed++;
                        existingLeagueCached.LastAccessed = DateTime.Now;
                        
                        configOptions.Leagues.RemoveAll(x => x.Id == smb4LeagueId);
                        configOptions.Leagues.Add(new League
                        {
                            Id = existingLeagueCached.SaveGameLeagueId,
                            Name = existingLeagueCached.LeagueName,
                            PlayerTeam = existingLeagueCached.PlayerTeam,
                            NumSeasons = existingLeagueFromQuery.NumSeasons,
                            NumTimesAccessed = existingLeagueCached.NumTimesAccessed,
                            FirstAccessed = existingLeagueCached.FirstAccessed,
                            LastAccessed = existingLeagueCached.LastAccessed
                        });

                        _applicationConfig.SaveConfigOptions(configOptions);
                    }
                }
            }
        }

        Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = Cursors.Arrow);

        return !hasError ? new Success() : new Error();
    }

    protected override void Dispose(bool disposing)
    {
        _dataService.ConnectionChanged -= DataServiceOnConnectionChanged;
        base.Dispose(disposing);
    }
}