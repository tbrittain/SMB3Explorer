using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Data.Sqlite;
using Microsoft.Win32;

namespace SMB3Explorer;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            MessageBox.Show("This application is only supported on Windows.");
            Environment.Exit(1);
        }
    }
    
    private SqliteConnection? Connection { get; set; } = null;

    private static string GetUserProvidedFile(string directoryPath)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Save files (*.sav)",
            InitialDirectory = directoryPath,
        };

        return openFileDialog.ShowDialog() != true
            ? string.Empty
            : openFileDialog.FileName;
    }

    private const string DefaultSaveFileName = "savedata.sav";

    private static string GetSaveFile()
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
            return result == MessageBoxResult.Yes
                ? GetUserProvidedFile(baseDirectoryPath)
                : string.Empty;
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

        return result2 == MessageBoxResult.Yes
            ? GetUserProvidedFile(steamUserDirectory ?? baseDirectoryPath)
            : string.Empty;
    }

    private void Button_OnClick(object sender, RoutedEventArgs e)
    {
        var filePath = GetSaveFile();
        if (string.IsNullOrEmpty(filePath)) return;
        
        using (var compressedStream = File.OpenRead(filePath))
        using (var decompressedStream = new MemoryStream())
        {
            //TODO: Decompress stream and write to decompressedStream
        }
    }
}