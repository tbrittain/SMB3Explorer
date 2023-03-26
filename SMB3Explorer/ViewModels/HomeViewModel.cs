using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SMB3Explorer.Models.Internal;
using SMB3Explorer.Services.ApplicationContext;
using SMB3Explorer.Services.DataService;
using SMB3Explorer.Services.NavigationService;
using SMB3Explorer.Services.SystemInteropWrapper;
using SMB3Explorer.Utils;

namespace SMB3Explorer.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly IApplicationContext _applicationContext;
    private readonly ISystemIoWrapper _systemIoWrapper;
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;

    private ObservableCollection<FranchiseSelection> _franchises = new();
    private bool _interactionEnabled;
    private Visibility _loadingSpinnerVisible;
    private FranchiseSelection? _selectedFranchise;

    public HomeViewModel(INavigationService navigationService, IDataService dataService,
        IApplicationContext applicationContext, ISystemIoWrapper systemIoWrapper)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _applicationContext = applicationContext;
        _systemIoWrapper = systemIoWrapper;

        GetFranchises();
    }

    public FranchiseSelection? SelectedFranchise
    {
        get => _selectedFranchise;
        set
        {
            SetField(ref _selectedFranchise, value);
            _applicationContext.SelectedFranchise = value;
            OnPropertyChanged(nameof(FranchiseSelected));

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
        }
    }

    public ObservableCollection<FranchiseSelection> Franchises
    {
        get => _franchises;
        private set => SetField(ref _franchises, value);
    }

    public Visibility LoadingSpinnerVisible
    {
        get => _loadingSpinnerVisible;
        set
        {
            if (value == _loadingSpinnerVisible) return;
            _loadingSpinnerVisible = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ContentVisible));
        }
    }

    public Visibility ContentVisible => LoadingSpinnerVisible == Visibility.Collapsed
        ? Visibility.Visible
        : Visibility.Collapsed;

    public bool InteractionEnabled
    {
        get => _interactionEnabled;
        set => SetField(ref _interactionEnabled, value);
    }

    public bool FranchiseSelected => SelectedFranchise != null;

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
        Mouse.OverrideCursor = Cursors.Wait;

        var teamsEnumerable = _dataService.GetFranchisePlayoffStandings();
        
        var fileName = $"{_applicationContext.SelectedFranchise!.LeagueNameSafe}_playoff_standings_" +
                       $"{DateTime.Now:yyyyMMddHHmmssfff}.csv";
        
        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, teamsEnumerable, fileName);
        
        HandleExportSuccess(filePath);
    }

    // TODO implement in commands
    private async Task HandleTopPerformersBattingExport(bool isRookies = false)
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var playersEnumerable = _dataService.GetMostRecentSeasonTopBattingStatistics(isRookies);
        
        var rookieType = isRookies ? "rookies" : "all";
        var mostRecentSeason = _applicationContext.MostRecentFranchiseSeason;
        var fileName = $"{_applicationContext.SelectedFranchise!.LeagueNameSafe}_top_batting_{rookieType}" +
                       $"_season_{mostRecentSeason!.SeasonNum}_{DateTime.Now:yyyyMMddHHmmssfff}.csv";
        
        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);
        
        HandleExportSuccess(filePath);
    }
    
    // TODO implement in commands
    private async Task HandleTopPerformersPitchingExport(bool isRookies = false)
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var playersEnumerable = _dataService.GetMostRecentSeasonTopPitchingStatistics(isRookies);
        
        var rookieType = isRookies ? "rookies" : "all";
        var mostRecentSeason = _applicationContext.MostRecentFranchiseSeason;
        var fileName = $"{_applicationContext.SelectedFranchise!.LeagueNameSafe}_top_pitching_{rookieType}" +
                       $"_season_{mostRecentSeason!.SeasonNum}_{DateTime.Now:yyyyMMddHHmmssfff}.csv";
        
        var (filePath, _) = await CsvUtils.ExportCsv(_systemIoWrapper, playersEnumerable, fileName);
        
        HandleExportSuccess(filePath);
    }
    
    private bool CanExport() => FranchiseSelected;

    private void GetFranchises()
    {
        LoadingSpinnerVisible = Visibility.Visible;

        _dataService.GetFranchises()
            .ContinueWith(async task =>
            {
                if (task.Exception != null)
                {
                    DefaultExceptionHandler.HandleException(_systemIoWrapper, "Failed to get franchises.", task.Exception);
                    LoadingSpinnerVisible = Visibility.Collapsed;
                    return;
                }

                if (task.Result.Any())
                {
                    Franchises = new ObservableCollection<FranchiseSelection>(task.Result);
                    InteractionEnabled = true;
                }
                else
                {
                    MessageBox.Show("No franchises found. Please select a different save file.");
                    await _dataService.Disconnect();
                    _navigationService.NavigateTo<LandingViewModel>();
                }

                LoadingSpinnerVisible = Visibility.Collapsed;
            });
    }
    
    private void HandleExportSuccess(string filePath)
    {
        var ok = MessageBox.Show("Export successful. Would you like to open the file?", "Success",
            MessageBoxButton.YesNo, MessageBoxImage.Information);

        if (ok == MessageBoxResult.Yes) SafeProcess.Start(filePath, _systemIoWrapper);

        Mouse.OverrideCursor = Cursors.Arrow;
    }
}