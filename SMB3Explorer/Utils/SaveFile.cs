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
    private const string SaveGameFileFilter = "Save files (*.sav)|*.sav";
    
    public static async Task<string> GetSaveFilePath()
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
            
            if (result == MessageBoxResult.No) return string.Empty;
            return await GetUserProvidedFile(baseDirectoryPath);
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

            if (File.Exists(defaultFilePath)) return defaultFilePath;
        }

        var result2 = MessageBox.Show("Default file does not exist. " +
                                      "Would you like to select a save file directly?",
            "Default file not found", MessageBoxButton.YesNo);

        if (result2 == MessageBoxResult.No) return string.Empty;
        return await GetUserProvidedFile(baseDirectoryPath);
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