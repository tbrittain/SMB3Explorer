using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
    private bool _comboBoxEnabled;

    public FranchiseSelection? SelectedFranchise
    {
        get => _selectedFranchise;
        set
        {
            SetField(ref _selectedFranchise, value);
            _applicationContext.SelectedLeagueId = value?.LeagueId;
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

    public bool ComboBoxEnabled
    {
        get => _comboBoxEnabled;
        set => SetField(ref _comboBoxEnabled, value);
    }

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
                    ComboBoxEnabled = true;
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