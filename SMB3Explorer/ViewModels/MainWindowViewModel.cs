using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Serilog;
using SMB3Explorer.Models.Internal;
using SMB3Explorer.Services.DataService;
using SMB3Explorer.Services.HttpService;
using SMB3Explorer.Services.NavigationService;
using SMB3Explorer.Services.SystemIoWrapper;
using SMB3Explorer.Utils;
using static SMB3Explorer.Constants.FileExports;
using static SMB3Explorer.Constants.Github;

namespace SMB3Explorer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public INavigationService NavigationService { get; }

    private readonly ISystemIoWrapper _systemIoWrapper;
    private readonly IDataService _dataService;
    private readonly IHttpService _httpService;
    private bool _isUpdateAvailable;
    private string _updateVersion = string.Empty;
    private AppUpdateResult? _appUpdateResult;
    private Visibility _updateAvailableVisibility = Visibility.Collapsed;
    private Visibility _deselectSaveGameVisibility = Visibility.Collapsed;

    public MainWindowViewModel(INavigationService navigationService, ISystemIoWrapper systemIoWrapper,
        IDataService dataService, IHttpService httpService)
    {
        NavigationService = navigationService;
        _systemIoWrapper = systemIoWrapper;
        _dataService = dataService;
        _httpService = httpService;

        Log.Information("Initializing MainWindowViewModel");

        _dataService.ConnectionChanged += DataServiceOnConnectionChanged;
    }

    private void DataServiceOnConnectionChanged(object? sender, EventArgs e)
    {
        DeselectSaveGameVisibility = _dataService.IsConnected
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    public Task Initialize()
    {
        NavigationService.NavigateTo<LandingViewModel>();
        _ = Task.Run(async () => await CheckForUpdates());
        return Task.CompletedTask;
    }

    private static Version CurrentVersion
    {
        get
        {
            var currentVersion = Assembly.GetEntryAssembly()?.GetName().Version;
            return currentVersion is null
                ? new Version(0, 0, 0, 0)
                : new Version(currentVersion.Major, currentVersion.Minor, currentVersion.Build);
        }
    }

    public static string CurrentVersionString => $"Version {CurrentVersion}";

    private bool IsUpdateAvailable
    {
        get => _isUpdateAvailable;
        set
        {
            SetField(ref _isUpdateAvailable, value);
            OnPropertyChanged(nameof(UpdateAvailableDisplayText));
            UpdateAvailableVisibility = Visibility.Visible;
        }
    }

    private AppUpdateResult? AppUpdateResult
    {
        get => _appUpdateResult;
        set
        {
            SetField(ref _appUpdateResult, value);
            IsUpdateAvailable = true;
        }
    }

    public Visibility UpdateAvailableVisibility
    {
        get => _updateAvailableVisibility;
        set => SetField(ref _updateAvailableVisibility, value);
    }

    public Visibility DeselectSaveGameVisibility
    {
        get => _deselectSaveGameVisibility;
        set => SetField(ref _deselectSaveGameVisibility, value);
    }

    public string UpdateAvailableDisplayText => IsUpdateAvailable
        ? $"Update Available: {AppUpdateResult?.Version.ToString()}"
        : "No Updates Available";

    [RelayCommand]
    private Task OpenExportsFolder()
    {
        Log.Information("Opening exports folder");

        var defaultDirectory = BaseExportsDirectory;

        if (!_systemIoWrapper.DirectoryExists(defaultDirectory))
        {
            Log.Debug("Exports folder does not exist, creating");
            _systemIoWrapper.DirectoryCreate(defaultDirectory);
        }

        SafeProcess.Start(defaultDirectory, _systemIoWrapper);

        Log.Information("Opened exports folder");
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenWiki()
    {
        Log.Information("Opening wiki");

        SafeProcess.Start(WikiUrl, _systemIoWrapper);

        Log.Information("Opened wiki");
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task SubmitFeatureRequest()
    {
        Log.Information("Opening feature request page");
        
        SafeProcess.Start(FeatureRequestUrl, _systemIoWrapper);

        Log.Information("Opened feature request page");
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenDiscussions()
    {
        Log.Information("Opening discussions page");

        
        SafeProcess.Start(DiscussionsUrl, _systemIoWrapper);

        Log.Information("Opened discussions page");
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenIssues()
    {
        Log.Information("Opening issues page");

        
        SafeProcess.Start(IssuesUrl, _systemIoWrapper);

        Log.Information("Opened issues page");
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task SubmitBugReport()
    {
        Log.Information("Opening bug report page");

        SafeProcess.Start(NewBugUrl, _systemIoWrapper);

        Log.Information("Opened bug report page");
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenGithubRepo()
    {
        Log.Information("Opening github repo");

        
        SafeProcess.Start(RepoUrl, _systemIoWrapper);

        Log.Information("Opened github repo");
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task PurgeApplicationData()
    {
        Log.Information("Purging application data");

        var appDataSummary = AppData.GetApplicationDataSize(_systemIoWrapper, _dataService.CurrentFilePath);
        if (appDataSummary.NumberOfFiles == 0)
        {
            Log.Debug("No application data to delete");
            _systemIoWrapper.ShowMessageBox("No application data to delete.", "No Application Data",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var message = $"Are you sure you want to delete {appDataSummary.NumberOfFiles} .sqlite files " +
                      $"totalling {appDataSummary.TotalSize.ToFileSizeString()}?";

        var result = _systemIoWrapper.ShowMessageBox(message, "Purge Application Data", MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes) return;

        Mouse.OverrideCursor = Cursors.Wait;
        var failedPurgeResults = await AppData.PurgeApplicationData(_systemIoWrapper,
            _dataService.CurrentFilePath);
        Mouse.OverrideCursor = Cursors.Arrow;

        if (failedPurgeResults.Count == 0)
        {
            Log.Debug("All files deleted successfully");
            _systemIoWrapper.ShowMessageBox("All files deleted successfully.", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        Log.Warning("Failed to delete {Count} files", failedPurgeResults.Count);
        var failedMessage = new StringBuilder($"Failed to delete {failedPurgeResults.Count} files:");
        foreach (var _ in failedPurgeResults)
        {
            Log.Warning("Failed to delete {FileName} ({Size})", _.FileName, _.Size.ToFileSizeString());
            failedMessage.AppendLine(
                $"{Environment.NewLine}{_.FileName} ({_.Size.ToFileSizeString()})");
        }

        _systemIoWrapper.ShowMessageBox(failedMessage.ToString(), "Failed to delete", MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }

    [RelayCommand]
    private Task OpenUpdateVersionReleasePage()
    {
        Log.Information("Opening update version release page");

        SafeProcess.Start(AppUpdateResult!.Value.ReleasePageUrl, _systemIoWrapper);

        Log.Information("Opened update version release page");
        return Task.CompletedTask;
    }

    private async Task CheckForUpdates()
    {
        Log.Information("Checking for updates...");
        var updateResult = await _httpService.CheckForUpdates();

        if (updateResult.TryPickT2(out var error, out var rest))
        {
            _systemIoWrapper.ShowMessageBox($"Failed to check for updates: {error.Value}", "Update Check Failed",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (rest.TryPickT1(out var _, out var appUpdateResult))
        {
            Log.Debug("No update available");
            // No update available
            return;
        }

        AppUpdateResult = appUpdateResult;

        Log.Debug("Displaying update available message box");
        var message = $"An update is available ({CurrentVersion} --> {appUpdateResult.Version}, released " +
                      $"{appUpdateResult.DaysSinceRelease} days ago). Would you like open the release page?";

        var messageBoxResult = _systemIoWrapper.ShowMessageBox(message, "Update Available",
            MessageBoxButton.YesNo, MessageBoxImage.Information);

        if (messageBoxResult != MessageBoxResult.Yes) return;

        SafeProcess.Start(appUpdateResult.ReleasePageUrl, _systemIoWrapper);
    }

    [RelayCommand]
    private async Task DeselectSaveGame()
    {
        await _dataService.Disconnect();
        NavigationService.NavigateTo<LandingViewModel>();
    }
}