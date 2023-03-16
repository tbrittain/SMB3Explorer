using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using OneOf;
using OneOf.Types;
using SMB3Explorer.Services.SystemInteropWrapper;

namespace SMB3Explorer.Utils;

public static class SaveFile
{
    public const string DefaultSaveFileName = "savedata.sav";
    private const string SaveGameFileFilter = "Save files (*.sav)|*.sav";

    public static string BaseGameDirectoryPath { get; } = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData), "Metalhead", "Super Mega Baseball 3");

    public static OneOf<string, None> GetSaveFilePath(ISystemInteropWrapper systemInteropWrapper)
    {
        if (!systemInteropWrapper.DirectoryExists(BaseGameDirectoryPath))
        {
            var result =
                systemInteropWrapper.ShowMessageBox("Default save file directory does not exist. " +
                                          "Would you like to select a save file directly?",
                    "Default location not found", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No) return new None();
            return GetUserProvidedFile(BaseGameDirectoryPath, systemInteropWrapper);
        }

        var subdirectories = systemInteropWrapper.DirectoryGetDirectories(BaseGameDirectoryPath);

        var steamUserDirectories = subdirectories
            .Where(x =>
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

                if (systemInteropWrapper.FileExists(defaultFilePath)) return defaultFilePath;
                
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
        if (result2 == MessageBoxResult.No) return new None();
        return GetUserProvidedFile(BaseGameDirectoryPath, systemInteropWrapper);
    }

    public static OneOf<string, None> GetUserProvidedFile(string directoryPath,
        ISystemInteropWrapper systemInteropWrapper, string filter = SaveGameFileFilter)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = filter,
            InitialDirectory = directoryPath
        };

        if (systemInteropWrapper.ShowOpenFileDialog(openFileDialog) is not true) return new None();

        return openFileDialog.FileName;
    }
}