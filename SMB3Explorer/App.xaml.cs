using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SMB3Explorer.Services.ApplicationContext;
using SMB3Explorer.Services.CsvWriterWrapper;
using SMB3Explorer.Services.DataService;
using SMB3Explorer.Services.HttpClient;
using SMB3Explorer.Services.NavigationService;
using SMB3Explorer.Services.SystemInteropWrapper;
using SMB3Explorer.Utils;
using SMB3Explorer.ViewModels;
using SMB3Explorer.Views;

namespace SMB3Explorer;

public partial class App
{
    private IServiceProvider ServiceProvider { get; set; } = null!;
    private IServiceCollection Services { get; set; } = null!;

    public App()
    {
        DispatcherUnhandledException += App_DispatcherUnhandledException;
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Logger.InitializeLogger();
        Services = new ServiceCollection();
        await ConfigureServices(Services);
        ServiceProvider = Services.BuildServiceProvider();
        
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
        await ((MainWindowViewModel) mainWindow.DataContext).Initialize();
    }

    private static Task ConfigureServices(IServiceCollection services)
    {
        Log.Information("Configuring services...");
        services.AddHttpClient();
        services.AddSingleton<IHttpService, HttpService>();
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
        
        Log.Information("Finished configuring services");
        return Task.CompletedTask;
    }

    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        Log.Fatal(e.Exception, "Unhandled exception");

        var systemIoWrapper = ServiceProvider.GetRequiredService<ISystemIoWrapper>();
        DefaultExceptionHandler.HandleException(systemIoWrapper,
            "An unexpected error occurred that caused the termination of the program.", e.Exception);
        e.Handled = true;

        Shutdown();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.CloseAndFlush();
        base.OnExit(e);
    }
}