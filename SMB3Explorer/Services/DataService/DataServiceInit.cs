using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using OneOf;
using OneOf.Types;
using SMB3Explorer.Services.SystemInteropWrapper;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services.DataService;

public partial class DataService
{
    public async Task<string> DecompressSaveGame(string filePath, ISystemIoWrapper systemIoWrapper)
    {
        await using var compressedStream = systemIoWrapper.FileOpenRead(filePath);
        await using var zlibStream = systemIoWrapper.GetZlibDecompressionStream(compressedStream);
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

        await using var fileStream = systemIoWrapper.FileCreateStream(decompressedFilePath);
        await decompressedStream.CopyToAsync(fileStream);

        return decompressedFilePath;
    }

    public async Task EstablishDbConnection(string filePath, bool isCompressedSaveGame = true)
    {
        var decompressedFilePath = filePath;
        if (isCompressedSaveGame)
        {
            decompressedFilePath = await DecompressSaveGame(filePath, _systemIoWrapper);
        }
        else
        {
            CurrentFilePath = filePath;
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
        var commandText = SqlRunner.GetSqlCommand(SqlFile.DatabaseTables);
        command.CommandText = commandText;
        var reader = await command.ExecuteReaderAsync();

        List<string> tableNames = new();
        while (reader.Read())
        {
            var tableName = reader.GetString(0);
            tableNames.Add(tableName);
        }

        // Using t_stats as a test table since it is an important one for this application
        if (!tableNames.Contains("t_stats"))
            throw new Exception("Invalid save file, missing expected tables");
    }

    public Task Disconnect()
    {
        Connection?.Close();
        Connection?.Dispose();

        Connection = null;
        CurrentFilePath = string.Empty;

        return Task.CompletedTask;
    }
}