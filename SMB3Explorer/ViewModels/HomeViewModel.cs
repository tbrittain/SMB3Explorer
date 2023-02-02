using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using SMB3Explorer.Models;
using SMB3Explorer.Services;
using SMB3Explorer.Utils;

namespace SMB3Explorer.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly IApplicationContext _applicationContext;

    [ObservableProperty]
    private bool _franchisesLoading;
    private ObservableCollection<FranchiseSelection> _franchises = new();
    private FranchiseSelection? _selectedFranchise;

    public FranchiseSelection? SelectedFranchise
    {
        get => _selectedFranchise;
        set
        {
            SetProperty(ref _selectedFranchise, value);
            _applicationContext.SelectedLeagueId = value?.LeagueId;
        }
    }

    public ObservableCollection<FranchiseSelection> Franchises
    {
        get => _franchises;
        private set => SetProperty(ref _franchises, value);
    }

    public HomeViewModel(INavigationService navigationService, IDataService dataService,
        IApplicationContext applicationContext)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _applicationContext = applicationContext;

        Task.Run(async () => await GetFranchises());
    }

    private async Task GetFranchises()
    {
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            FranchisesLoading = true;
        });

        var (franchises, exception) = await _dataService.GetFranchises();

        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            FranchisesLoading = false;

            if (exception != null)
            {
                DefaultExceptionHandler.HandleException("Failed to get franchises.", exception);
                // TODO: Handle case that franchises could not be loaded
                return;
            }

            Franchises = new ObservableCollection<FranchiseSelection>(franchises);
        });
    }
}