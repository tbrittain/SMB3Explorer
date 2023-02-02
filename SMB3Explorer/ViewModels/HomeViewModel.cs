using CommunityToolkit.Mvvm.ComponentModel;
using SMB3Explorer.Services;

namespace SMB3Explorer.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty]
    private INavigationService _navigationService;
    
    [ObservableProperty]
    private IDataService _dataService;
    
    [ObservableProperty]
    private IApplicationContext _applicationContext;

    public HomeViewModel(INavigationService navigationService, IDataService dataService, IApplicationContext applicationContext)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _applicationContext = applicationContext;
        
        // TODO: When the ViewModel is initialized, get franchise data from the data service
        // and display it in the UI, allowing the user to select a franchise to set the ApplicationContext as the current franchise.
    }
}