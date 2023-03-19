using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using OneOf.Types;
using SMB3Explorer.Models.Internal;
using SMB3Explorer.Services.DataService;
using SMB3Explorer.Services.HttpClient;
using SMB3Explorer.Services.NavigationService;
using SMB3Explorer.Services.SystemInteropWrapper;
using SMB3Explorer.Utils;

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
        var defaultDirectory = CsvUtils.DefaultDirectory;

        if (!_systemIoWrapper.DirectoryExists(defaultDirectory))
        {
            _systemIoWrapper.DirectoryCreate(defaultDirectory);
        }

        SafeProcess.Start(defaultDirectory, _systemIoWrapper);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenWiki()
    {
        const string wikiUrl = "https://github.com/tbrittain/SMB3Explorer/wiki";
        SafeProcess.Start(wikiUrl, _systemIoWrapper);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task SubmitFeatureRequest()
    {
        const string featureRequestUrl = "https://github.com/tbrittain/SMB3Explorer/discussions/new?category=ideas";
        SafeProcess.Start(featureRequestUrl, _systemIoWrapper);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenDiscussions()
    {
        const string discussionsUrl = "https://github.com/tbrittain/SMB3Explorer/discussions";
        SafeProcess.Start(discussionsUrl, _systemIoWrapper);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenIssues()
    {
        const string issuesUrl = "https://github.com/tbrittain/SMB3Explorer/issues";
        SafeProcess.Start(issuesUrl, _systemIoWrapper);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task SubmitBugReport()
    {
        const string bugUrl = "https://github.com/tbrittain/SMB3Explorer/issues/new?labels=bug&template=bug_report.md";
        SafeProcess.Start(bugUrl, _systemIoWrapper);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenGithubRepo()
    {
        const string githubUrl = "https://github.com/tbrittain/SMB3Explorer";
        SafeProcess.Start(githubUrl, _systemIoWrapper);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task PurgeApplicationData()
    {
        var appDataSummary = AppData.GetApplicationDataSize(_systemIoWrapper, _dataService.CurrentFilePath);
        if (appDataSummary.NumberOfFiles == 0)
        {
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
            _systemIoWrapper.ShowMessageBox("All files deleted successfully.", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var failedMessage = new StringBuilder($"Failed to delete {failedPurgeResults.Count} files:");
        foreach (var _ in failedPurgeResults)
        {
            failedMessage.AppendLine(
                $"{Environment.NewLine}{_.FileName} ({_.Size.ToFileSizeString()})");
        }

        _systemIoWrapper.ShowMessageBox(failedMessage.ToString(), "Failed to delete", MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }
    
    [RelayCommand]
    private Task OpenUpdateVersionReleasePage()
    {
        SafeProcess.Start(AppUpdateResult!.Value.ReleasePageUrl, _systemIoWrapper);
        return Task.CompletedTask;
    }

    private async Task CheckForUpdates()
    {
        var updateResult = await _httpService.CheckForUpdates();

        if (updateResult.TryPickT2(out var error, out var rest))
        {
            _systemIoWrapper.ShowMessageBox($"Failed to check for updates: {error.Value}", "Update Check Failed",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (rest.TryPickT1(out var _, out var appUpdateResult))
        {
            // No update available
            return;
        }
        
        AppUpdateResult = appUpdateResult;
        
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