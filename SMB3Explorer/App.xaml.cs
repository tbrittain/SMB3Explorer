using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using SMB3Explorer.Services;
using SMB3Explorer.ViewModels;
using SMB3Explorer.Views;

namespace SMB3Explorer;

public partial class App
{
    public IServiceProvider ServiceProvider { get; private set; } = null!;
    public IServiceCollection Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Services = new ServiceCollection();
        ConfigureServices(Services);
        ServiceProvider = Services.BuildServiceProvider();
        
        var mainWindow = ServiceProvider.GetRequiredService<MainLandingPage>();
        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<Frame>(_ => new Frame());
        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<INavigationService, NavigationService>();

        services.AddTransient<MainLandingPage>();
        services.AddTransient<MainLandingViewModel>();
    }
}