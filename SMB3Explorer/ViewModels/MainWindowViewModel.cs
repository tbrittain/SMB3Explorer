using System.Threading.Tasks;
using SMB3Explorer.Services;

namespace SMB3Explorer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public INavigationService NavigationService { get; }

    public MainWindowViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
    }
    
    public Task Initialize()
    {
        NavigationService.NavigateTo<LandingViewModel>();
        return Task.CompletedTask;
    }
}