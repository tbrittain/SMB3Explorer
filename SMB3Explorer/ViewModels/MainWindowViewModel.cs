using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SMB3Explorer.Services.DataService;
using SMB3Explorer.Services.NavigationService;
using SMB3Explorer.Services.SystemInteropWrapper;
using SMB3Explorer.Utils;

namespace SMB3Explorer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public INavigationService NavigationService { get; }

    private readonly ISystemInteropWrapper _systemInteropWrapper;
    private readonly IDataService _dataService;

    public MainWindowViewModel(INavigationService navigationService, ISystemInteropWrapper systemInteropWrapper,
        IDataService dataService)
    {
        NavigationService = navigationService;
        _systemInteropWrapper = systemInteropWrapper;
        _dataService = dataService;
    }

    public Task Initialize()
    {
        NavigationService.NavigateTo<LandingViewModel>();
        return Task.CompletedTask;
    }

    private static string CurrentVersion => Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "Unknown";

    public static string CurrentVersionString => $"Version {CurrentVersion}";

    [RelayCommand]
    private Task OpenExportsFolder()
    {
        var defaultDirectory = CsvUtils.DefaultDirectory;

        if (!_systemInteropWrapper.DirectoryExists(defaultDirectory))
        {
            _systemInteropWrapper.DirectoryCreate(defaultDirectory);
        }

        SafeProcess.Start(defaultDirectory, _systemInteropWrapper);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenWiki()
    {
        const string wikiUrl = "https://github.com/tbrittain/SMB3Explorer/wiki";
        SafeProcess.Start(wikiUrl, _systemInteropWrapper);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task SubmitFeatureRequest()
    {
        const string featureRequestUrl = "https://github.com/tbrittain/SMB3Explorer/discussions/new?category=ideas";
        SafeProcess.Start(featureRequestUrl, _systemInteropWrapper);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenDiscussions()
    {
        const string discussionsUrl = "https://github.com/tbrittain/SMB3Explorer/discussions";
        SafeProcess.Start(discussionsUrl, _systemInteropWrapper);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenIssues()
    {
        const string issuesUrl = "https://github.com/tbrittain/SMB3Explorer/issues";
        SafeProcess.Start(issuesUrl, _systemInteropWrapper);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task SubmitBugReport()
    {
        const string bugUrl = "https://github.com/tbrittain/SMB3Explorer/issues/new?labels=bug&template=bug_report.md";
        SafeProcess.Start(bugUrl, _systemInteropWrapper);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenGithubRepo()
    {
        const string githubUrl = "https://github.com/tbrittain/SMB3Explorer";
        SafeProcess.Start(githubUrl, _systemInteropWrapper);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task PurgeApplicationData()
    {
        var appDataSummary = AppData.GetApplicationDataSize(_systemInteropWrapper, _dataService.CurrentFilePath);
        var message = $"Are you sure you want to delete {appDataSummary.NumberOfFiles} .sqlite files " +
                      $"totalling {FormattedStringBytes.BytesToString(appDataSummary.TotalSize)} bytes?";

        var result = _systemInteropWrapper.ShowMessageBox(message, "Purge Application Data", MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes) return;

        Mouse.OverrideCursor = Cursors.Wait;
        var failedPurgeResults = await AppData.PurgeApplicationData(_systemInteropWrapper,
            _dataService.CurrentFilePath);
        Mouse.OverrideCursor = Cursors.Arrow;
        
        if (failedPurgeResults.Count == 0)
        {
            _systemInteropWrapper.ShowMessageBox("All files deleted successfully.", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var failedMessage = new StringBuilder($"Failed to delete {failedPurgeResults.Count} files:");
        foreach (var _ in failedPurgeResults)
        {
            failedMessage.AppendLine(
                $"{Environment.NewLine}{_.FileName} ({FormattedStringBytes.BytesToString(_.Size)})");
        }

        _systemInteropWrapper.ShowMessageBox(failedMessage.ToString(), "Failed to delete", MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }
}