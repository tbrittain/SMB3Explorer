using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using SMB3Explorer.Services;

namespace SMB3Explorer.ViewModels;

public class MainWindowViewModel
{
    public MainWindowViewModel(IDataService dataService)
    {
        DataService = dataService;
        SelectSaveFileCommand = new AsyncRelayCommand(SetSaveFile);
    }
    
    public ICommand SelectSaveFileCommand { get; }
    
    private IDataService DataService { get; }
    
    private const string DefaultSaveFileName = "savedata.sav";

    private static string GetUserProvidedFile(string directoryPath)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Save files (*.sav)",
            InitialDirectory = directoryPath
        };

        return openFileDialog.ShowDialog() != true
            ? string.Empty
            : openFileDialog.FileName;
    }

    private static Task<string> GetSaveFile()
    {
        var baseDirectoryPath = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            "Metalhead", "Super Mega Baseball 3");

        if (!Directory.Exists(baseDirectoryPath))
        {
            var result =
                MessageBox.Show("Default save file directory does not exist. " +
                                "Would you like to select a save file directly?",
                    "Default location not found", MessageBoxButton.YesNo);
            return Task.FromResult(result == MessageBoxResult.Yes
                ? GetUserProvidedFile(baseDirectoryPath)
                : string.Empty);
        }

        var subdirectories = Directory.GetDirectories(baseDirectoryPath);
        // TODO: Handle case where there are no subdirectories
        var steamUserDirectory = subdirectories.SingleOrDefault(x =>
        {
            var lastBackslashIndex = x.LastIndexOf('\\');
            var lastPart = x[(lastBackslashIndex + 1)..];
            return long.TryParse(lastPart, out _);
        });

        if (steamUserDirectory != null)
        {
            var defaultFilePath = Path.Combine(steamUserDirectory, DefaultSaveFileName);

            if (File.Exists(defaultFilePath)) return Task.FromResult(defaultFilePath);
        }

        var result2 = MessageBox.Show("Default file does not exist. " +
                                      "Would you like to select a save file directly?",
            "Default file not found", MessageBoxButton.YesNo);

        return Task.FromResult(result2 == MessageBoxResult.Yes
            ? GetUserProvidedFile(steamUserDirectory ?? baseDirectoryPath)
            : string.Empty);
    }
    
    private async Task SetSaveFile()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var filePath = await GetSaveFile();
        if (string.IsNullOrEmpty(filePath))
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        var (ok, exception) = await DataService.SetupDbConnection(filePath);
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