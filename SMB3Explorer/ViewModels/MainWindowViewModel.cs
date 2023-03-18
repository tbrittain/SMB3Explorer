using System.Reflection;
using System.Threading.Tasks;
using SMB3Explorer.Services.NavigationService;

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

    private static string CurrentVersion => Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "Unknown";
    
    public static string CurrentVersionString => $"Version {CurrentVersion}";
}