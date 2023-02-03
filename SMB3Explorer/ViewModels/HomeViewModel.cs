using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SMB3Explorer.Models;
using SMB3Explorer.Services;
using SMB3Explorer.Utils;

namespace SMB3Explorer.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly IApplicationContext _applicationContext;

    private ObservableCollection<FranchiseSelection> _franchises = new();
    private FranchiseSelection? _selectedFranchise;
    private Visibility _loadingSpinnerVisible;
    private bool _interactionEnabled;

    public FranchiseSelection? SelectedFranchise
    {
        get => _selectedFranchise;
        set
        {
            SetField(ref _selectedFranchise, value);
            _applicationContext.SelectedFranchise = value;
            OnPropertyChanged(nameof(FranchiseSelected));
            ExportFranchisePositionPlayersCommand.NotifyCanExecuteChanged();
        }
    }

    public ObservableCollection<FranchiseSelection> Franchises
    {
        get => _franchises;
        private set => SetField(ref _franchises, value);
    }

    public HomeViewModel(INavigationService navigationService, IDataService dataService,
        IApplicationContext applicationContext)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _applicationContext = applicationContext;

        GetFranchises();
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
    private async Task ExportFranchisePositionPlayers()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var playersEnumerable = _dataService.GetFranchisePositionPlayers();

        var fileName =
            $"position_players_{_applicationContext.SelectedFranchise!.LeagueName}_{DateTime.Now:yyyyMMddHHmmssfff}.csv";

        await CsvUtils.ExportCsv(playersEnumerable, fileName);

        Mouse.OverrideCursor = Cursors.Arrow;
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
                    DefaultExceptionHandler.HandleException("Failed to get franchises.", task.Exception);
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
}