using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Ionic.Zlib;
using Microsoft.Data.Sqlite;
using Microsoft.Win32;

namespace SMB3Explorer;

public partial class MainWindow
{
    private const string DefaultSaveFileName = "savedata.sav";

    public MainWindow()
    {
        InitializeComponent();

        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            MessageBox.Show("This application is only supported on Windows.");
            Environment.Exit(1);
        }
    }

    private SqliteConnection? Connection { get; set; }

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

    private async void Button_OnClick(object sender, RoutedEventArgs e)
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var filePath = await GetSaveFile();
        if (string.IsNullOrEmpty(filePath))
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        var (ok, exception) = await SetupDbConnection(filePath);
        Mouse.OverrideCursor = Cursors.Arrow;

        if (!ok)
        {
            const string initialMessage = "Failed to connect to SMB3 database. " +
                                          "A full stack trace has been copied to your clipboard. " +
                                          "Press OK to report this issue on GitHub.";

            var formattedMessage = $"{initialMessage}{Environment.NewLine}{exception?.Message ?? "Unknown error"}";

            var openBrowser = MessageBox.Show(this, formattedMessage,
                "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);

            Clipboard.SetText(exception?.StackTrace ?? "Unknown error");

            if (openBrowser != MessageBoxResult.OK) return;

            const string url = "https://github.com/tbrittain/SMB3Explorer2/issues/new";
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));

            return;
        }

        MessageBox.Show("Successfully connected to SMB3 database");
    }

    private Task<(bool, Exception?)> SetupDbConnection(string filePath)
    {
        try
        {
            using var compressedStream = File.OpenRead(filePath);
            using var zlibStream = new ZlibStream(compressedStream, CompressionMode.Decompress);
            using var decompressedStream = new MemoryStream();

            zlibStream.CopyTo(decompressedStream);
            decompressedStream.Position = 0;

            var decompressedFileName = $"smb3_explorer_{DateTime.Now:yyyyMMddHHmmssfff}.sqlite";
            var decompressedFilePath = Path.Combine(Path.GetTempPath(), decompressedFileName);

            using (var fileStream = File.Create(decompressedFilePath))
            {
                decompressedStream.CopyTo(fileStream);
            }

            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = decompressedFilePath,
                Mode = SqliteOpenMode.ReadOnly,
                Cache = SqliteCacheMode.Shared
            };

            Connection = new SqliteConnection(connectionStringBuilder.ToString());
            Connection.Open();
            
            // Test connection by querying the schema and getting the table names
            var command = Connection.CreateCommand();
            command.CommandText = "SELECT name FROM sqlite_master WHERE type='table'";
            var reader = command.ExecuteReader();
            
            List<string> tableNames = new();
            while (reader.Read())
            {
                var tableName = reader.GetString(0);
                tableNames.Add(tableName);
            }

            // Using t_stats as a test table since it is an important one for this application
            if (!tableNames.Contains("t_stats"))
            {
                return Task.FromResult((false, (Exception?) new Exception("Invalid save file, missing expected tables")));
            }
        }
        catch (Exception e)
        {
            return Task.FromResult((false, (Exception?) e));
        }

        return Task.FromResult((true, null as Exception));
    }
}