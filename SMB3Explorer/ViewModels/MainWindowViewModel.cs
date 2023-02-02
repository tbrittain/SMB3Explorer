using CommunityToolkit.Mvvm.ComponentModel;
using SMB3Explorer.Services;

namespace SMB3Explorer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private INavigationService _navigationService;

    public MainWindowViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
    }
    
    public void Initialize()
    {
        NavigationService.NavigateTo<LandingViewModel>();
    }
}