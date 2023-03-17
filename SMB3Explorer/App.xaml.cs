using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SMB3Explorer.Services;
using SMB3Explorer.Services.ApplicationContext;
using SMB3Explorer.Services.CsvWriterWrapper;
using SMB3Explorer.Services.DataService;
using SMB3Explorer.Services.NavigationService;
using SMB3Explorer.Services.SystemInteropWrapper;
using SMB3Explorer.ViewModels;
using SMB3Explorer.Views;

namespace SMB3Explorer;

public partial class App
{
    private IServiceProvider ServiceProvider { get; set; } = null!;
    private IServiceCollection Services { get; set; } = null!;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Services = new ServiceCollection();
        await ConfigureServices(Services);
        ServiceProvider = Services.BuildServiceProvider();
        
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
        await ((MainWindowViewModel) mainWindow.DataContext).Initialize();
    }

    private static Task ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IApplicationContext, ApplicationContext>();
        services.AddSingleton<ISystemIoWrapper, SystemIoWrapper>();

        services.AddSingleton<MainWindow>(serviceProvider => new MainWindow
        {
            DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>()
        });

        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<LandingViewModel>();
        services.AddTransient<HomeViewModel>();
        services.AddTransient<ICsvWriterWrapper, CsvWriterWrapper>();

        // NavigationService calls this Func to get the ViewModel instance
        services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider =>
            viewModelType => (ViewModelBase) serviceProvider.GetRequiredService(viewModelType));
        
        return Task.CompletedTask;
    }
}