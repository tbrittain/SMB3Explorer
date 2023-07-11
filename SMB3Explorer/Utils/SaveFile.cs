using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using OneOf;
using OneOf.Types;
using SMB3Explorer.Enums;
using SMB3Explorer.Services.SystemIoWrapper;
using static SMB3Explorer.Constants.FileExports;

namespace SMB3Explorer.Utils;

public static class SaveFile
{
    public const string DefaultSaveFileName = "savedata.sav";
    private const string SaveGameFileFilter = "Save files (*.sav)|*.sav";

    public static OneOf<string, Error<string>> GetSmb4ExistingSaveFilePath(ISystemIoWrapper systemIoWrapper, Guid leagueId)
    {
        if (!systemIoWrapper.DirectoryExists(BaseGameSmb4DirectoryPath))
        {
            return new Error<string>("Default save file directory does not exist");
        }
        
        var subdirectories = systemIoWrapper.DirectoryGetDirectories(BaseGameSmb4DirectoryPath);

        var steamUserDirectories = subdirectories
            .Where(x =>
            {
                var lastBackslashIndex = x.LastIndexOf('\\');
                var lastPart = x[(lastBackslashIndex + 1)..];
                return long.TryParse(lastPart, out _);
            })
            .ToList();

        if (steamUserDirectories.Count != 1)
        {
            return new Error<string>("Steam user directory may have changed. " +
                                     "Please select a save file directly");
        }
        
        var steamUserDirectory = steamUserDirectories[0];

        var files = systemIoWrapper.DirectoryGetFiles(steamUserDirectory, "*.sav");
        var expectedFileName = $"league-{leagueId.ToString().ToUpperInvariant()}.sav";
        
        var leagueSaveFile = files
            .FirstOrDefault(x => x.Contains(expectedFileName, StringComparison.OrdinalIgnoreCase));

        if (leagueSaveFile is null)
        {
            return new Error<string>("Could not find save file for selected league");
        }

        return leagueSaveFile;
    }

    public static OneOf<string, None> GetSmb4SaveFileDirectory(ISystemIoWrapper systemIoWrapper)
    {
        if (!systemIoWrapper.DirectoryExists(BaseGameSmb3DirectoryPath))
        {
            return new None();
        }
        
        var subdirectories = systemIoWrapper.DirectoryGetDirectories(BaseGameSmb4DirectoryPath);

        var steamUserDirectories = subdirectories
            .Where(x =>
            {
                var lastBackslashIndex = x.LastIndexOf('\\');
                var lastPart = x[(lastBackslashIndex + 1)..];
                return long.TryParse(lastPart, out _);
            })
            .ToList();

        if (steamUserDirectories.Count != 1)
            return new None();

        var steamUserDirectory = steamUserDirectories[0];
        return steamUserDirectory;
    }

    public static OneOf<string, None> GetSaveFilePath(ISystemIoWrapper systemIoWrapper, SelectedGame selectedGame)
    {
        var directoryPath = selectedGame switch
        {
            SelectedGame.Smb3 => BaseGameSmb3DirectoryPath,
            SelectedGame.Smb4 => BaseGameSmb4DirectoryPath,
            _ => throw new ArgumentOutOfRangeException(nameof(selectedGame), selectedGame, null)
        };

        if (!systemIoWrapper.DirectoryExists(directoryPath))
        {
            var result =
                systemIoWrapper.ShowMessageBox("Default save file directory does not exist. " +
                                               "Would you like to select a save file directly?",
                    "Default location not found", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No) return new None();
            return GetUserProvidedFile(directoryPath, systemIoWrapper);
        }

        var subdirectories = systemIoWrapper.DirectoryGetDirectories(directoryPath);

        var steamUserDirectories = subdirectories
            .Where(x =>
            {
                var lastBackslashIndex = x.LastIndexOf('\\');
                var lastPart = x[(lastBackslashIndex + 1)..];
                return long.TryParse(lastPart, out _);
            })
            .ToList();

        string message = "Select a save file", caption = "Please select a save file";
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

                if (selectedGame is SelectedGame.Smb4)
                {
                    directoryPath = steamUserDirectory;
                    break;
                }

                var defaultFilePath = Path.Combine(steamUserDirectory, DefaultSaveFileName);

                if (systemIoWrapper.FileExists(defaultFilePath)) return defaultFilePath;

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

        if (selectedGame is SelectedGame.Smb4 && steamUserDirectories.Count == 1)
        {
            return GetUserProvidedFile(directoryPath, systemIoWrapper);
        }

        var result2 = systemIoWrapper.ShowMessageBox(message, caption, MessageBoxButton.YesNo);
        if (result2 == MessageBoxResult.No) return new None();
        return GetUserProvidedFile(directoryPath, systemIoWrapper);
    }

    public static OneOf<string, None> GetUserProvidedFile(string directoryPath,
        ISystemIoWrapper systemIoWrapper, string filter = SaveGameFileFilter)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = filter,
            InitialDirectory = directoryPath
        };

        if (systemIoWrapper.ShowOpenFileDialog(openFileDialog) is not true) return new None();

        return openFileDialog.FileName;
    }
}