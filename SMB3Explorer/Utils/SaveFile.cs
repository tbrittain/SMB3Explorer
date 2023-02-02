using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace SMB3Explorer.Utils;

public static class SaveFile
{
    private const string DefaultSaveFileName = "savedata.sav";
    
    public static Task<string> GetSaveFilePath()
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
}