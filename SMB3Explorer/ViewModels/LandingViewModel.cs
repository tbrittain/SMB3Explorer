using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using OneOf;
using OneOf.Types;
using Serilog;
using SMB3Explorer.Services.ApplicationContext;
using SMB3Explorer.Services.DataService;
using SMB3Explorer.Services.NavigationService;
using SMB3Explorer.Services.SystemIoWrapper;
using SMB3Explorer.Utils;

namespace SMB3Explorer.ViewModels;

public partial class LandingViewModel : ViewModelBase
{
    // ReSharper disable once ConvertToConstant.Global
    public static readonly string Smb3 = "SMB3";

    // ReSharper disable once ConvertToConstant.Global
    public static readonly string Smb4 = "SMB4";
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
    private readonly ISystemIoWrapper _systemIoWrapper;
    private readonly IApplicationContext _applicationContext;
    private string _selectedGame = Smb4;

    public LandingViewModel(IDataService dataService, INavigationService navigationService,
        ISystemIoWrapper systemIoWrapper, IApplicationContext applicationContext)
    {
        _navigationService = navigationService;
        _systemIoWrapper = systemIoWrapper;
        _applicationContext = applicationContext;
        _dataService = dataService;

        Log.Information("Initializing LandingViewModel");

        _dataService.ConnectionChanged += DataServiceOnConnectionChanged;
    }

    public string SelectedGame
    {
        get => _selectedGame;
        set
        {
            SetField(ref _selectedGame, value);
            _applicationContext.SelectedGame = value switch
            {
                "SMB3" => Enums.SelectedGame.Smb3,
                "SMB4" => Enums.SelectedGame.Smb4,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };
            
            OnPropertyChanged(nameof(Smb3ButtonVisibility));
            OnPropertyChanged(nameof(Smb4ButtonVisibility));
        }
    }

    private bool IsSmb3Selected => SelectedGame == Smb3;
    
    public Visibility Smb3ButtonVisibility => IsSmb3Selected ? Visibility.Visible : Visibility.Collapsed;
    public Visibility Smb4ButtonVisibility => IsSmb3Selected ? Visibility.Collapsed : Visibility.Visible;

    private void DataServiceOnConnectionChanged(object? sender, EventArgs e)
    {
        AutomaticallySelectSaveFileCommand.NotifyCanExecuteChanged();
        ManuallySelectSaveFileCommand.NotifyCanExecuteChanged();
        UseExistingDatabaseCommand.NotifyCanExecuteChanged();
    }

    private bool CanSelectSaveFile()
    {
        return !_dataService.IsConnected;
    }

    [RelayCommand(CanExecute = nameof(CanSelectSaveFile))]
    private async Task AutomaticallySelectSaveFile()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var filePathResult = SaveFile.GetSaveFilePath(_systemIoWrapper);
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

        var filePathResult =
            SaveFile.GetUserProvidedFile(Environment.SpecialFolder.MyDocuments.ToString(), _systemIoWrapper);
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

        var filePathResult = SaveFile.GetUserProvidedFile(Environment.SpecialFolder.MyDocuments.ToString(),
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

        if (connectionResult.TryPickT1(out var error, out _))
        {
            hasError = true;
            DefaultExceptionHandler.HandleException(_systemIoWrapper, "Failed to connect to SMB3 database.",
                new Exception(error.Value));
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