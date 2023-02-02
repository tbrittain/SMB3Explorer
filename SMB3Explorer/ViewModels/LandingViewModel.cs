using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SMB3Explorer.Services;
using SMB3Explorer.Utils;

namespace SMB3Explorer.ViewModels;

public partial class LandingViewModel : ViewModelBase
{
    private readonly IDataService _dataService;
    
    [ObservableProperty]
    private INavigationService _navigationService;

    public LandingViewModel(IDataService dataService, INavigationService navigationService)
    {
        NavigationService = navigationService;
        _dataService = dataService;

        _dataService.ConnectionChanged += DataServiceOnConnectionChanged;
    }

    private void DataServiceOnConnectionChanged(object? sender, EventArgs e)
    {
        SelectSaveFileCommand?.NotifyCanExecuteChanged();
    }

    private bool CanSelectSaveFile()
    {
        return !_dataService.IsConnected;
    }
    
    [RelayCommand]
    private void NavigateToMain()
    {
        NavigationService.NavigateTo<HomeViewModel>();
    }

    [RelayCommand(CanExecute = nameof(CanSelectSaveFile))]
    private async Task SelectSaveFile()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var filePath = await SaveFile.GetSaveFilePath();
        if (string.IsNullOrEmpty(filePath))
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        var (ok, exception) = await _dataService.EstablishDbConnection(filePath);
        Mouse.OverrideCursor = Cursors.Arrow;

        if (!ok)
        {
            const string initialMessage = "Failed to connect to SMB3 database. " +
                                          "A full stack trace has been copied to your clipboard. " +
                                          "Press OK to report this issue on GitHub.";

            var formattedMessage = $"{initialMessage}{Environment.NewLine}{exception?.Message ?? "Unknown error"}";

            var openBrowser = MessageBox.Show(formattedMessage,
                "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);

            Clipboard.SetText(exception?.StackTrace ?? "Unknown error");

            if (openBrowser != MessageBoxResult.OK) return;

            const string url = "https://github.com/tbrittain/SMB3Explorer/issues/new";
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));

            return;
        }

        MessageBox.Show("Successfully connected to SMB3 database");
        NavigateToMainCommand.Execute(null);
    }

    protected override void Dispose(bool disposing)
    {
        _dataService.ConnectionChanged -= DataServiceOnConnectionChanged;
        base.Dispose(disposing);
    }
}