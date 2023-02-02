using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SMB3Explorer.Services;
using SMB3Explorer.ViewModels;

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
        
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IDataService, DataService>();

        services.AddTransient<MainWindow>();
        services.AddTransient<MainWindowViewModel>();
    }
}