using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Serilog;
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
        
        Log.Information("Initializing HomeViewModel");

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
            
            Log.Information("Changed current league to {League}", value?.LeagueName ?? "None");

            if (_selectedFranchise is null)
            {
                Log.Debug("Disabling export commands");
            }
            else
            {
                Log.Debug("Enabling export commands");
            }

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

    public bool FranchiseSelected => SelectedFranchise is not null;

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

        var ok = MessageBox.Show("Export successful. Would you like to open the file?", "Success",
            MessageBoxButton.YesNo, MessageBoxImage.Information);

        if (ok == MessageBoxResult.Yes) SafeProcess.Start(filePath, _systemIoWrapper);

        Mouse.OverrideCursor = Cursors.Arrow;
        Log.Information("Finished exporting franchise career batting statistics to {FilePath}", filePath);
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

        var ok = MessageBox.Show("Export successful. Would you like to open the file?", "Success",
            MessageBoxButton.YesNo, MessageBoxImage.Information);

        if (ok == MessageBoxResult.Yes) SafeProcess.Start(filePath, _systemIoWrapper);

        Mouse.OverrideCursor = Cursors.Arrow;
        Log.Information("Finished exporting franchise career pitching statistics to {FilePath}", filePath);
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

        var ok = MessageBox.Show("Export successful. Would you like to open the file?", "Success",
            MessageBoxButton.YesNo, MessageBoxImage.Information);

        if (ok == MessageBoxResult.Yes) SafeProcess.Start(filePath, _systemIoWrapper);

        Mouse.OverrideCursor = Cursors.Arrow;
        Log.Information("Finished exporting franchise season batting statistics to {FilePath}", filePath);
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

        var ok = MessageBox.Show("Export successful. Would you like to open the file?", "Success",
            MessageBoxButton.YesNo, MessageBoxImage.Information);

        if (ok == MessageBoxResult.Yes) SafeProcess.Start(filePath, _systemIoWrapper);

        Mouse.OverrideCursor = Cursors.Arrow;
        Log.Information("Finished exporting franchise season pitching statistics to {FilePath}", filePath);
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
        
        var ok = MessageBox.Show("Export successful. Would you like to open the file?", "Success",
            MessageBoxButton.YesNo, MessageBoxImage.Information);
        
        if (ok == MessageBoxResult.Yes) SafeProcess.Start(filePath, _systemIoWrapper);
        
        Mouse.OverrideCursor = Cursors.Arrow;
        Log.Information("Finished exporting franchise season standings to {FilePath}", filePath);
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

        var ok = MessageBox.Show("Export successful. Would you like to open the file?", "Success",
            MessageBoxButton.YesNo, MessageBoxImage.Information);
        
        if (ok == MessageBoxResult.Yes) SafeProcess.Start(filePath, _systemIoWrapper);
        
        Mouse.OverrideCursor = Cursors.Arrow;
        Log.Information("Exported franchise playoff standings to {FilePath}", filePath);
    }
    
    private bool CanExport() => FranchiseSelected;

    private void GetFranchises()
    {
        LoadingSpinnerVisible = Visibility.Visible;

        Log.Information("Getting franchises...");
        _dataService.GetFranchises()
            .ContinueWith(async task =>
            {
                if (task.Exception is not null)
                {
                    DefaultExceptionHandler.HandleException(_systemIoWrapper, "Failed to get franchises.", task.Exception);
                    LoadingSpinnerVisible = Visibility.Collapsed;
                    return;
                }

                if (task.Result.Any())
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

                LoadingSpinnerVisible = Visibility.Collapsed;
            });
    }
}