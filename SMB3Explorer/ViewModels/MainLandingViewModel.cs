using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SMB3Explorer.Services;
using SMB3Explorer.Utils;

namespace SMB3Explorer.ViewModels;

public class MainLandingViewModel
{
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;

    public MainLandingViewModel(IDataService dataService, INavigationService navigationService)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        SelectSaveFileCommand = new AsyncRelayCommand(SetSaveFile);
    }

    public ICommand SelectSaveFileCommand { get; }

    private async Task SetSaveFile()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var filePath = await SaveFile.GetSaveFilePath();
        if (string.IsNullOrEmpty(filePath))
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        var (ok, exception) = await _dataService.SetupDbConnection(filePath);
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
        
    }
}