using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SMB3Explorer.Services;
using SMB3Explorer.ViewModels;
using SMB3Explorer.Views;

namespace SMB3Explorer;

public partial class App
{
    private IServiceProvider ServiceProvider { get; set; } = null!;
    private IServiceCollection Services { get; set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Services = new ServiceCollection();
        ConfigureServices(Services);
        ServiceProvider = Services.BuildServiceProvider();
        
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
        ((MainWindowViewModel) mainWindow.DataContext).Initialize();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<INavigationService, NavigationService>();

        services.AddSingleton<MainWindow>(serviceProvider => new MainWindow
        {
            DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>()
        });

        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<LandingViewModel>();
        services.AddTransient<HomeViewModel>();

        services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider =>
            viewModelType => (ViewModelBase) serviceProvider.GetRequiredService(viewModelType));
    }
}