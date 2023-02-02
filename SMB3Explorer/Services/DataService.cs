using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Ionic.Zlib;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services;

public sealed class DataService : INotifyPropertyChanged, IDataService
{
    private SqliteConnection? _connection;
    private string _currentFilePath = string.Empty;

    public SqliteConnection? Connection
    {
        get => _connection;
        set
        {
            SetField(ref _connection, value);
            ConnectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsConnected => Connection is not null;
    public event EventHandler<EventArgs>? ConnectionChanged;

    public string CurrentFilePath
    {
        get => _currentFilePath;
        private set => SetField(ref _currentFilePath, value);
    }

    public Task<(bool, Exception?)> EstablishDbConnection(string filePath)
    {
        try
        {
            using var compressedStream = File.OpenRead(filePath);
            using var zlibStream = new ZlibStream(compressedStream, CompressionMode.Decompress);
            using var decompressedStream = new MemoryStream();

            var buffer = new byte[4096];
            int count;

            while ((count = zlibStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                decompressedStream.Write(buffer, 0, count);
            }

            decompressedStream.Position = 0;

            var decompressedFileName = $"smb3_explorer_{DateTime.Now:yyyyMMddHHmmssfff}.sqlite";
            var decompressedFilePath = Path.Combine(Path.GetTempPath(), decompressedFileName);

            CurrentFilePath = decompressedFilePath;

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
            var commandText = SqlRunner.GetSqlCommand(SqlFile.GetAvailableTables);
            command.CommandText = commandText;
            var reader = command.ExecuteReader();

            List<string> tableNames = new();
            while (reader.Read())
            {
                var tableName = reader.GetString(0);
                tableNames.Add(tableName);
            }

            // Using t_stats as a test table since it is an important one for this application
            if (!tableNames.Contains("t_stats"))
                return Task.FromResult(
                    (false, (Exception?) new Exception("Invalid save file, missing expected tables")));
        }
        catch (Exception e)
        {
            return Task.FromResult((false, (Exception?) e));
        }

        return Task.FromResult((true, null as Exception));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }
}