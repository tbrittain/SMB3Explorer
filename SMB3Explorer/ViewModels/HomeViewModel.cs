using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

    private ObservableCollection<LeagueSelection> _leagues = [];
    private bool _interactionEnabled;
    private LeagueSelection? _selectedLeague;
    private bool _atLeastOneLeagueSeasonExists;

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

    public LeagueSelection? SelectedLeague
    {
        get => _selectedLeague;
        set
        {
            SetField(ref _selectedLeague, value);
            _applicationContext.SelectedLeague = value;
            
            var leagueName = value?.LeagueNameSafe ?? "None";
            Log.Information("Set selected league to {LeagueNameSafe}", leagueName);
            OnPropertyChanged(nameof(LeagueSelected));
        }
    }

    public ObservableCollection<LeagueSelection> Leagues
    {
        get => _leagues;
        private set => SetField(ref _leagues, value);
    }

    public bool InteractionEnabled
    {
        get => _interactionEnabled;
        set => SetField(ref _interactionEnabled, value);
    }

    public Visibility NoLeagueSeasonsVisibility
    {
        get
        {
            if (!LeagueSelected) return Visibility.Collapsed;

            if (AtLeastOneLeagueSeasonExists) return Visibility.Collapsed;

            return Visibility.Visible;
        }
    }

    private bool LeagueSelected => SelectedLeague is not null;

    private bool AtLeastOneLeagueSeasonExists
    {
        get => _atLeastOneLeagueSeasonExists;
        set
        {
            _atLeastOneLeagueSeasonExists = value;
            OnPropertyChanged(nameof(NoLeagueSeasonsVisibility));
        }
    }

    private void ApplicationContextOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(ApplicationContext.MostRecentLeagueSeason):
            {
                AtLeastOneLeagueSeasonExists = _applicationContext.MostRecentLeagueSeason is not null;

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

    [RelayCommand(CanExecute = nameof(CanExportHistorical))]
    private async Task ExportFranchiseCareerBattingStatistics()
    {
        await HandleFranchiseCareerBattingExport();
    }

    [RelayCommand(CanExecute = nameof(CanExportHistorical))]
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
        var fileName = $"{_applicationContext.SelectedLeague!.LeagueNameSafe}_career_batting_{battingType}_" +
                       $"{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);

        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExportHistorical))]
    private async Task ExportFranchiseCareerPitchingStatistics()
    {
        await HandleFranchiseCareerPitchingExport();
    }

    [RelayCommand(CanExecute = nameof(CanExportHistorical))]
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
        var fileName = $"{_applicationContext.SelectedLeague!.LeagueNameSafe}_career_pitching_{pitchingType}_" +
                       $"{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);

        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExportSeason))]
    private async Task ExportFranchiseSeasonBattingStatistics()
    {
        await HandleFranchiseSeasonBattingExport();
    }

    [RelayCommand(CanExecute = nameof(CanExportPlayoff))]
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
        var fileName = $"{_applicationContext.SelectedLeague!.LeagueNameSafe}_season_batting_{battingType}_" +
                       $"{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);

        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExportSeason))]
    private async Task ExportFranchiseSeasonPitchingStatistics()
    {
        await HandleFranchiseSeasonPitchingExport();
    }

    [RelayCommand(CanExecute = nameof(CanExportPlayoff))]
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
        var fileName = $"{_applicationContext.SelectedLeague!.LeagueNameSafe}_season_pitching_{pitchingType}_" +
                       $"{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);

        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExportSeason))]
    private async Task ExportFranchiseTeamSeasonStandings()
    {
        Log.Information("Exporting franchise season standings...");
        Mouse.OverrideCursor = Cursors.Wait;

        var teamsEnumerable = _dataService.GetFranchiseSeasonStandings();

        var fileName = $"{_applicationContext.SelectedLeague!.LeagueNameSafe}_season_standings_" +
                       $"{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, teamsEnumerable, fileName);

        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExportPlayoff))]
    private async Task ExportFranchiseTeamPlayoffStandings()
    {
        Log.Information("Exporting franchise playoff standings...");
        Mouse.OverrideCursor = Cursors.Wait;

        var teamsEnumerable = _dataService.GetFranchisePlayoffStandings();

        var fileName = $"{_applicationContext.SelectedLeague!.LeagueNameSafe}_playoff_standings_" +
                       $"{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, teamsEnumerable, fileName);

        HandleExportSuccess(filePath);
    }


    [RelayCommand(CanExecute = nameof(CanExportSeason))]
    private async Task ExportTopPerformersBatting()
    {
        await HandleTopPerformersBattingExport(MostRecentSeasonFilter.RegularSeason);
    }

    [RelayCommand(CanExecute = nameof(CanExportHistorical))]
    private async Task ExportTopRookiesBatting()
    {
        await HandleTopPerformersBattingExport(MostRecentSeasonFilter.Rookies);
    }

    [RelayCommand(CanExecute = nameof(CanExportPlayoff))]
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

        var mostRecentSeason = _applicationContext.MostRecentLeagueSeason;
        var fileName = $"{_applicationContext.SelectedLeague!.LeagueNameSafe}_top_batting_{exportType}_" +
                       $"{mostRecentSeason!.SeasonNum}_{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);

        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExportSeason))]
    private async Task ExportTopPerformersPitching()
    {
        await HandleTopPerformersPitchingExport(MostRecentSeasonFilter.RegularSeason);
    }

    [RelayCommand(CanExecute = nameof(CanExportHistorical))]
    private async Task ExportTopRookiesPitching()
    {
        await HandleTopPerformersPitchingExport(MostRecentSeasonFilter.Rookies);
    }

    [RelayCommand(CanExecute = nameof(CanExportPlayoff))]
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

        var mostRecentSeason = _applicationContext.MostRecentLeagueSeason;
        var fileName = $"{_applicationContext.SelectedLeague!.LeagueNameSafe}_top_pitching_{exportType}_" +
                       $"{mostRecentSeason!.SeasonNum}_{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);

        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExportSeason))]
    private async Task ExportMostRecentSeasonPlayers()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var playersEnumerable = _dataService.GetMostRecentSeasonPlayers();
        
        var mostRecentSeason = _applicationContext.MostRecentLeagueSeason;
        var fileName = $"{_applicationContext.SelectedLeague!.LeagueNameSafe}_players" +
                       $"_season_{mostRecentSeason!.SeasonNum}_{DateTime.Now:yyyyMMddHHmmssfff}.csv";
        
        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);
        
        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExportSeason))]
    private async Task ExportMostRecentSeasonTeams()
    {
        Mouse.OverrideCursor = Cursors.Wait;
        
        var teamsEnumerable = _dataService.GetMostRecentSeasonTeams();
        
        var mostRecentSeason = _applicationContext.MostRecentLeagueSeason;
        var fileName = $"{_applicationContext.SelectedLeague!.LeagueNameSafe}_teams" +
                       $"_season_{mostRecentSeason!.SeasonNum}_{DateTime.Now:yyyyMMddHHmmssfff}.csv";
        
        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, teamsEnumerable, fileName);
        
        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExportSeason))]
    private async Task ExportMostRecentSeasonSchedule()
    {
        Mouse.OverrideCursor = Cursors.Wait;
        
        var scheduleEnumerable = _dataService.GetMostRecentSeasonSchedule();
        
        var mostRecentSeason = _applicationContext.MostRecentLeagueSeason;
        var fileName = $"{_applicationContext.SelectedLeague!.LeagueNameSafe}_schedule" +
                       $"_season_{mostRecentSeason!.SeasonNum}_{DateTime.Now:yyyyMMddHHmmssfff}.csv";
        
        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, scheduleEnumerable, fileName);
        
        HandleExportSuccess(filePath);
    }

    [RelayCommand(CanExecute = nameof(CanExportPlayoff))]
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
        var mostRecentSeason = _applicationContext.MostRecentLeagueSeason;
        
        var fileName = $"{_applicationContext.SelectedLeague!.LeagueNameSafe}_schedule" +
                       $"_playoffs_season_{mostRecentSeason!.SeasonNum}_{DateTime.Now:yyyyMMddHHmmssfff}.csv";
        
        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playoffScheduleEnumerable, fileName);
        
        HandleExportSuccess(filePath);
    }

    /// <summary>
    /// Applies to exports that have a season context (franchise and season)
    /// </summary>
    /// <returns></returns>
    private bool CanExportSeason()
    {
        if (!LeagueSelected || !AtLeastOneLeagueSeasonExists)
        {
            return false;
        }

        return _selectedLeague!.Mode switch
        {
            LeagueMode.Franchise => true,
            LeagueMode.Season => true,
            _ => false
        };
    }

    /// <summary>
    /// Applies to exports that are contain a playoff format (franchise, season, and elimination)
    /// </summary>
    /// <returns></returns>
    private bool CanExportPlayoff()
    {
        if (!LeagueSelected || !AtLeastOneLeagueSeasonExists)
        {
            return false;
        }

        return _selectedLeague!.Mode switch
        {
            LeagueMode.Franchise => true,
            LeagueMode.Season => true,
            LeagueMode.Elimination => true,
            _ => false
        };
    }

    /// <summary>
    /// Applies to exports that are historical in nature (franchise only)
    /// </summary>
    /// <returns></returns>
    private bool CanExportHistorical()
    {
        if (!LeagueSelected || !AtLeastOneLeagueSeasonExists)
        {
            return false;
        }

        return _selectedLeague!.Mode switch
        {
            LeagueMode.Franchise => true,
            _ => false
        };
    }

    private void GetFranchises()
    {
        _dataService.GetLeagues()
            .ContinueWith(async task =>
            {
                if (task.Exception != null)
                {
                    DefaultExceptionHandler.HandleException(_systemIoWrapper, "Failed to get leagues.",
                        task.Exception);
                    return;
                }

                var rawResults = task.Result;

                Log.Debug("Filtering out ineligible leagues (lacking at least one game played), found {Count} total leagues",
                    rawResults.Count);
                var filteredResults = rawResults
                    .Where(x => x.Mode is not LeagueMode.None)
                    .ToList();
                Log.Debug("Filtered out ineligible leagues, left with {Count} eligible leagues", filteredResults.Count);

                if (filteredResults.Count != 0)
                {
                    Log.Debug("{Count} Franchises found", filteredResults.Count);
                    Leagues = new ObservableCollection<LeagueSelection>(filteredResults);
                    InteractionEnabled = true;
                }
                else
                {
                    Log.Debug("No leagues found, navigating to landing page");
                    MessageBox.Show("No leagues found. Please select a different save file.");
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