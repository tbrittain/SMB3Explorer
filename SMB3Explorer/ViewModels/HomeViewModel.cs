﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Serilog;
using SMB3Explorer.Enums;
using SMB3Explorer.Models.Internal;
using SMB3Explorer.Services.ApplicationContext;
using SMB3Explorer.Services.DataService;
using SMB3Explorer.Services.NavigationService;
using SMB3Explorer.Services.SystemIoWrapper;
using SMB3Explorer.Utils;

namespace SMB3Explorer.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly IApplicationContext _applicationContext;
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
    private readonly ISystemIoWrapper _systemIoWrapper;

    private ObservableCollection<FranchiseSelection> _franchises = [];
    private bool _interactionEnabled;
    private FranchiseSelection? _selectedFranchise;
    private bool _atLeastOneFranchiseSeasonExists;

    public HomeViewModel(INavigationService navigationService, IDataService dataService,
        IApplicationContext applicationContext, ISystemIoWrapper systemIoWrapper)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _applicationContext = applicationContext;
        _systemIoWrapper = systemIoWrapper;
        
        Log.Information("Initializing HomeViewModel");

        _applicationContext.PropertyChanged += ApplicationContextOnPropertyChanged;

        GetFranchises();
    }

    public FranchiseSelection? SelectedFranchise
    {
        get => _selectedFranchise;
        set
        {
            SetField(ref _selectedFranchise, value);
            _applicationContext.SelectedFranchise = value;
            
            var leagueName = value?.LeagueNameSafe ?? "None";
            Log.Information("Set selected league to {LeagueNameSafe}", leagueName);
            OnPropertyChanged(nameof(FranchiseSelected));
        }
    }

    public ObservableCollection<FranchiseSelection> Franchises
    {
        get => _franchises;
        private set => SetField(ref _franchises, value);
    }

    public bool InteractionEnabled
    {
        get => _interactionEnabled;
        set => SetField(ref _interactionEnabled, value);
    }

    public Visibility NoFranchiseSeasonsVisibility
    {
        get
        {
            if (!FranchiseSelected) return Visibility.Collapsed;

            if (AtLeastOneFranchiseSeasonExists) return Visibility.Collapsed;

            return Visibility.Visible;
        }
    }

    private bool FranchiseSelected => SelectedFranchise is not null;

    private bool AtLeastOneFranchiseSeasonExists
    {
        get => _atLeastOneFranchiseSeasonExists;
        set
        {
            _atLeastOneFranchiseSeasonExists = value;
            OnPropertyChanged(nameof(NoFranchiseSeasonsVisibility));
        }
    }

    private void ApplicationContextOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(ApplicationContext.MostRecentFranchiseSeason):
            {
                AtLeastOneFranchiseSeasonExists = _applicationContext.MostRecentFranchiseSeason is not null;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    ExportFranchiseCareerBattingStatisticsCommand.NotifyCanExecuteChanged();
                    ExportFranchiseCareerPitchingStatisticsCommand.NotifyCanExecuteChanged();
                    ExportFranchiseCareerPlayoffPitchingStatisticsCommand.NotifyCanExecuteChanged();
                    ExportFranchiseCareerPlayoffBattingStatisticsCommand.NotifyCanExecuteChanged();

                    ExportFranchiseSeasonBattingStatisticsCommand.NotifyCanExecuteChanged();
                    ExportFranchiseSeasonPlayoffBattingStatisticsCommand.NotifyCanExecuteChanged();
                    ExportFranchiseSeasonPitchingStatisticsCommand.NotifyCanExecuteChanged();
                    ExportFranchiseSeasonPlayoffPitchingStatisticsCommand.NotifyCanExecuteChanged();

                    ExportFranchiseTeamSeasonStandingsCommand.NotifyCanExecuteChanged();
                    ExportFranchiseTeamPlayoffStandingsCommand.NotifyCanExecuteChanged();

                    ExportTopPerformersBattingCommand.NotifyCanExecuteChanged();
                    ExportTopRookiesBattingCommand.NotifyCanExecuteChanged();
                    ExportTopPerformersPitchingCommand.NotifyCanExecuteChanged();
                    ExportTopRookiesPitchingCommand.NotifyCanExecuteChanged();
                    ExportTopPerformersBattingPlayoffsCommand.NotifyCanExecuteChanged();
                    ExportTopPerformersPitchingPlayoffsCommand.NotifyCanExecuteChanged();
                    
                    ExportMostRecentSeasonPlayersCommand.NotifyCanExecuteChanged();
                    ExportMostRecentSeasonTeamsCommand.NotifyCanExecuteChanged();
                    ExportMostRecentSeasonScheduleCommand.NotifyCanExecuteChanged();
                    ExportMostRecentSeasonPlayoffScheduleCommand.NotifyCanExecuteChanged();
                });

                break;
            }
        }
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportFranchiseCareerBattingStatistics()
    {
        await HandleFranchiseCareerBattingExport();
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportFranchiseCareerPlayoffBattingStatistics()
    {
        await HandleFranchiseCareerBattingExport(false);
    }

    private async Task HandleFranchiseCareerBattingExport(bool isRegularSeason = true)
    {
        Log.Information("Exporting franchise career batting statistics when isRegularSeason = {IsRegularSeason}", isRegularSeason);
        Mouse.OverrideCursor = Cursors.Wait;

        var playersEnumerable = _dataService.GetFranchiseCareerBattingStatistics(isRegularSeason);

        var battingType = isRegularSeason ? "regular_season" : "playoffs";
        var fileName = $"{_applicationContext.SelectedFranchise!.LeagueNameSafe}_career_batting_{battingType}_" +
                       $"{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);

        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportFranchiseCareerPitchingStatistics()
    {
        await HandleFranchiseCareerPitchingExport();
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportFranchiseCareerPlayoffPitchingStatistics()
    {
        await HandleFranchiseCareerPitchingExport(false);
    }

    private async Task HandleFranchiseCareerPitchingExport(bool isRegularSeason = true)
    {
        Log.Information("Exporting franchise career pitching statistics where isRegularSeason = {IsRegularSeason}", isRegularSeason);
        Mouse.OverrideCursor = Cursors.Wait;

        var playersEnumerable = _dataService.GetFranchiseCareerPitchingStatistics(isRegularSeason);

        var pitchingType = isRegularSeason ? "regular_season" : "playoffs";
        var fileName = $"{_applicationContext.SelectedFranchise!.LeagueNameSafe}_career_pitching_{pitchingType}_" +
                       $"{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);

        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportFranchiseSeasonBattingStatistics()
    {
        await HandleFranchiseSeasonBattingExport();
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportFranchiseSeasonPlayoffBattingStatistics()
    {
        await HandleFranchiseSeasonBattingExport(false);
    }

    private async Task HandleFranchiseSeasonBattingExport(bool isRegularSeason = true)
    {
        Log.Information("Exporting franchise season batting statistics where isRegularSeason = {IsRegularSeason}", isRegularSeason);
        Mouse.OverrideCursor = Cursors.Wait;

        var playersEnumerable = _dataService.GetFranchiseSeasonBattingStatistics(isRegularSeason);

        var battingType = isRegularSeason ? "regular_season" : "playoffs";
        var fileName = $"{_applicationContext.SelectedFranchise!.LeagueNameSafe}_season_batting_{battingType}_" +
                       $"{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);

        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportFranchiseSeasonPitchingStatistics()
    {
        await HandleFranchiseSeasonPitchingExport();
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportFranchiseSeasonPlayoffPitchingStatistics()
    {
        await HandleFranchiseSeasonPitchingExport(false);
    }

    private async Task HandleFranchiseSeasonPitchingExport(bool isRegularSeason = true)
    {
        Log.Information("Exporting franchise season pitching statistics where isRegularSeason={IsRegularSeason}", isRegularSeason);
        Mouse.OverrideCursor = Cursors.Wait;

        var playersEnumerable = _dataService.GetFranchiseSeasonPitchingStatistics(isRegularSeason);

        var pitchingType = isRegularSeason ? "regular_season" : "playoffs";
        var fileName = $"{_applicationContext.SelectedFranchise!.LeagueNameSafe}_season_pitching_{pitchingType}_" +
                       $"{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);

        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportFranchiseTeamSeasonStandings()
    {
        Log.Information("Exporting franchise season standings...");
        Mouse.OverrideCursor = Cursors.Wait;

        var teamsEnumerable = _dataService.GetFranchiseSeasonStandings();

        var fileName = $"{_applicationContext.SelectedFranchise!.LeagueNameSafe}_season_standings_" +
                       $"{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, teamsEnumerable, fileName);

        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportFranchiseTeamPlayoffStandings()
    {
        Log.Information("Exporting franchise playoff standings...");
        Mouse.OverrideCursor = Cursors.Wait;

        var teamsEnumerable = _dataService.GetFranchisePlayoffStandings();

        var fileName = $"{_applicationContext.SelectedFranchise!.LeagueNameSafe}_playoff_standings_" +
                       $"{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, teamsEnumerable, fileName);

        HandleExportSuccess(filePath);
    }


    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportTopPerformersBatting()
    {
        await HandleTopPerformersBattingExport(MostRecentSeasonFilter.RegularSeason);
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportTopRookiesBatting()
    {
        await HandleTopPerformersBattingExport(MostRecentSeasonFilter.Rookies);
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportTopPerformersBattingPlayoffs()
    {
        await HandleTopPerformersBattingExport(MostRecentSeasonFilter.Playoffs);
    }

    private async Task HandleTopPerformersBattingExport(MostRecentSeasonFilter filter)
    {
        Mouse.OverrideCursor = Cursors.Wait;

        if (filter is MostRecentSeasonFilter.Playoffs)
        {
            var playoffsExist = await _dataService.DoesMostRecentSeasonPlayoffExist();
            if (!playoffsExist)
            {
                MessageBox.Show("The current season does not yet contain playoff data");
                Mouse.OverrideCursor = Cursors.Arrow;
                return;
            }
        }

        var playersEnumerable = _dataService.GetMostRecentSeasonTopBattingStatistics(filter);

        var exportType = filter switch
        {
            MostRecentSeasonFilter.RegularSeason => "all_season",
            MostRecentSeasonFilter.Rookies => "rookies_season",
            MostRecentSeasonFilter.Playoffs => "all_playoffs",
            _ => throw new ArgumentOutOfRangeException(nameof(filter), filter, null)
        };

        var mostRecentSeason = _applicationContext.MostRecentFranchiseSeason;
        var fileName = $"{_applicationContext.SelectedFranchise!.LeagueNameSafe}_top_batting_{exportType}_" +
                       $"{mostRecentSeason!.SeasonNum}_{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);

        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportTopPerformersPitching()
    {
        await HandleTopPerformersPitchingExport(MostRecentSeasonFilter.RegularSeason);
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportTopRookiesPitching()
    {
        await HandleTopPerformersPitchingExport(MostRecentSeasonFilter.Rookies);
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportTopPerformersPitchingPlayoffs()
    {
        await HandleTopPerformersPitchingExport(MostRecentSeasonFilter.Playoffs);
    }

    private async Task HandleTopPerformersPitchingExport(MostRecentSeasonFilter filter)
    {
        Mouse.OverrideCursor = Cursors.Wait;
        
        if (filter is MostRecentSeasonFilter.Playoffs)
        {
            var playoffsExist = await _dataService.DoesMostRecentSeasonPlayoffExist();
            if (!playoffsExist)
            {
                MessageBox.Show("The current season does not yet contain playoff data");
                Mouse.OverrideCursor = Cursors.Arrow;
                return;
            }
        }

        var playersEnumerable = _dataService.GetMostRecentSeasonTopPitchingStatistics(filter);

        var exportType = filter switch
        {
            MostRecentSeasonFilter.RegularSeason => "all_season",
            MostRecentSeasonFilter.Rookies => "rookies_season",
            MostRecentSeasonFilter.Playoffs => "all_playoffs",
            _ => throw new ArgumentOutOfRangeException(nameof(filter), filter, null)
        };

        var mostRecentSeason = _applicationContext.MostRecentFranchiseSeason;
        var fileName = $"{_applicationContext.SelectedFranchise!.LeagueNameSafe}_top_pitching_{exportType}_" +
                       $"{mostRecentSeason!.SeasonNum}_{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);

        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportMostRecentSeasonPlayers()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var playersEnumerable = _dataService.GetMostRecentSeasonPlayers();
        
        var mostRecentSeason = _applicationContext.MostRecentFranchiseSeason;
        var fileName = $"{_applicationContext.SelectedFranchise!.LeagueNameSafe}_players" +
                       $"_season_{mostRecentSeason!.SeasonNum}_{DateTime.Now:yyyyMMddHHmmssfff}.csv";
        
        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);
        
        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportMostRecentSeasonTeams()
    {
        Mouse.OverrideCursor = Cursors.Wait;
        
        var teamsEnumerable = _dataService.GetMostRecentSeasonTeams();
        
        var mostRecentSeason = _applicationContext.MostRecentFranchiseSeason;
        var fileName = $"{_applicationContext.SelectedFranchise!.LeagueNameSafe}_teams" +
                       $"_season_{mostRecentSeason!.SeasonNum}_{DateTime.Now:yyyyMMddHHmmssfff}.csv";
        
        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, teamsEnumerable, fileName);
        
        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportMostRecentSeasonSchedule()
    {
        Mouse.OverrideCursor = Cursors.Wait;
        
        var scheduleEnumerable = _dataService.GetMostRecentSeasonSchedule();
        
        var mostRecentSeason = _applicationContext.MostRecentFranchiseSeason;
        var fileName = $"{_applicationContext.SelectedFranchise!.LeagueNameSafe}_schedule" +
                       $"_season_{mostRecentSeason!.SeasonNum}_{DateTime.Now:yyyyMMddHHmmssfff}.csv";
        
        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, scheduleEnumerable, fileName);
        
        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportMostRecentSeasonPlayoffSchedule()
    {
        Mouse.OverrideCursor = Cursors.Wait;
        
        var playoffsExist = await _dataService.DoesMostRecentSeasonPlayoffExist();
        if (!playoffsExist)
        {
            MessageBox.Show("The current season does not yet contain playoff data");
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        var playoffScheduleEnumerable = _dataService.GetMostRecentSeasonPlayoffSchedule();
        var mostRecentSeason = _applicationContext.MostRecentFranchiseSeason;
        
        var fileName = $"{_applicationContext.SelectedFranchise!.LeagueNameSafe}_schedule" +
                       $"_playoffs_season_{mostRecentSeason!.SeasonNum}_{DateTime.Now:yyyyMMddHHmmssfff}.csv";
        
        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playoffScheduleEnumerable, fileName);
        
        HandleExportSuccess(filePath);
    }

    private bool CanExport()
    {
        return FranchiseSelected && AtLeastOneFranchiseSeasonExists;
    }

    private void GetFranchises()
    {
        _dataService.GetFranchises()
            .ContinueWith(async task =>
            {
                if (task.Exception != null)
                {
                    DefaultExceptionHandler.HandleException(_systemIoWrapper, "Failed to get franchises.",
                        task.Exception);
                    return;
                }

                if (task.Result.Count != 0)
                {
                    Log.Debug("{Count} Franchises found", task.Result.Count);
                    Franchises = new ObservableCollection<FranchiseSelection>(task.Result);
                    InteractionEnabled = true;
                }
                else
                {
                    Log.Debug("No franchises found, navigating to landing page");
                    MessageBox.Show("No franchises found. Please select a different save file.");
                    await _dataService.Disconnect();
                    _navigationService.NavigateTo<LandingViewModel>();
                }
            });
    }

    private void HandleExportSuccess(string filePath)
    {
        var ok = MessageBox.Show("Export successful. Would you like to open the file?", "Success",
            MessageBoxButton.YesNo, MessageBoxImage.Information);

        if (ok == MessageBoxResult.Yes) SafeProcess.Start(filePath, _systemIoWrapper);

        Mouse.OverrideCursor = Cursors.Arrow;
    }

    protected override void Dispose(bool disposing)
    {
        _applicationContext.PropertyChanged -= ApplicationContextOnPropertyChanged;
        base.Dispose(disposing);
    }
}