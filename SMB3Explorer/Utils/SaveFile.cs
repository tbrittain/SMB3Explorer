using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using SMB3Explorer.Services;

namespace SMB3Explorer.Utils;

public static class SaveFile
{
    private const string DefaultSaveFileName = "savedata.sav";
    private const string SaveGameFileFilter = "Save files (*.sav)|*.sav";

    public static string BaseGameDirectoryPath { get; } = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData), "Metalhead", "Super Mega Baseball 3");

    public static async Task<string> GetSaveFilePath(ISystemInteropWrapper systemInteropWrapper)
    {
        if (!Directory.Exists(BaseGameDirectoryPath))
        {
            var result =
                systemInteropWrapper.ShowMessageBox("Default save file directory does not exist. " +
                                          "Would you like to select a save file directly?",
                    "Default location not found", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No) return string.Empty;
            return await GetUserProvidedFile(BaseGameDirectoryPath);
        }

        var subdirectories = Directory.GetDirectories(BaseGameDirectoryPath);

        var steamUserDirectories = subdirectories.Where(x =>
            {
                var lastBackslashIndex = x.LastIndexOf('\\');
                var lastPart = x[(lastBackslashIndex + 1)..];
                return long.TryParse(lastPart, out _);
            })
            .ToList();

        string message = string.Empty, caption = string.Empty;
        switch (steamUserDirectories.Count)
        {
            case 0:
            {
                message = "No Steam user directories detected. " +
                          "Would you like to select a save file directly?";
                caption = "No Steam users detected";
                break;
            }
            case 1:
            {
                var steamUserDirectory = steamUserDirectories[0];
                var defaultFilePath = Path.Combine(steamUserDirectory, DefaultSaveFileName);

                if (File.Exists(defaultFilePath)) return defaultFilePath;
                
                message = "Default file does not exist. " +
                          "Would you like to select a save file directly?";
                caption = "Default file not found";
                break;
            }
            case > 1:
            {
                message = "Multiple Steam user directories detected. " +
                          "Would you like to select a save file directly?";
                caption = "Multiple Steam users detected";
                break;
            }
        }

        var result2 = systemInteropWrapper.ShowMessageBox(message, caption, MessageBoxButton.YesNo);
        if (result2 == MessageBoxResult.No) return string.Empty;
        return await GetUserProvidedFile(BaseGameDirectoryPath);
    }

    public static Task<string> GetUserProvidedFile(string directoryPath, string filter = SaveGameFileFilter)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = filter,
            InitialDirectory = directoryPath
        };

        return Task.FromResult(openFileDialog.ShowDialog() != true
            ? string.Empty
            : openFileDialog.FileName);
    }
}