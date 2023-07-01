using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using OneOf;
using OneOf.Types;
using Serilog;
using SMB3Explorer.Models.Internal;
using SMB3Explorer.Services.SystemIoWrapper;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services.DataService;

public partial class DataService
{
    public async Task<OneOf<string, Error<string>>> DecompressSaveGame(string filePath,
        ISystemIoWrapper systemIoWrapper)
    {
        Log.Information("Decompressing save game at {FilePath}", filePath);
        await using var compressedStream = systemIoWrapper.FileOpenRead(filePath);
        if (compressedStream is null)
        {
            Log.Error("Failed to open file, the file stream was null");
            return new Error<string>("Could not open file.");
        }

        await using var zlibStream = systemIoWrapper.GetZlibDecompressionStream(compressedStream);
        using var decompressedStream = new MemoryStream();

        var buffer = new byte[4096];
        int count;

        Log.Debug("Writing decompressed data to memory stream");
        while ((count = zlibStream.Read(buffer, 0, buffer.Length)) != 0)
        {
            decompressedStream.Write(buffer, 0, count);
        }

        decompressedStream.Position = 0;

        var decompressedFileName = $"smb3_explorer_{DateTime.Now:yyyyMMddHHmmssfff}.sqlite";
        var decompressedFilePath = Path.Combine(Path.GetTempPath(), decompressedFileName);

        CurrentFilePath = decompressedFilePath;

        Log.Debug("Writing decompressed data to file at {FilePath}", decompressedFilePath);
        await using var fileStream = systemIoWrapper.FileCreateStream(decompressedFilePath);
        await decompressedStream.CopyToAsync(fileStream);

        Log.Information("Finished decompressing save game");
        return decompressedFilePath;
    }

    public async Task<OneOf<List<Smb4LeagueSelection>, Error<string>>> EstablishDbConnection(string filePath,
        bool isCompressedSaveGame = true)
    {
        Log.Information(
            "Establishing database connection to {FilePath}, with isCompressedSaveGame={IsCompressedSaveGame}",
            filePath, isCompressedSaveGame);
        var decompressedFilePath = filePath;
        if (isCompressedSaveGame)
        {
            var decompressResult = await DecompressSaveGame(filePath, _systemIoWrapper);
            if (decompressResult.TryPickT1(out var error, out decompressedFilePath))
            {
                return new Error<string>(error.Value);
            }
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

        Log.Debug("Opening connection to database at {FilePath}", decompressedFilePath);

        Connection = new SqliteConnection(connectionStringBuilder.ToString());
        Connection.Open();

        Log.Debug("Testing connection to database");
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

        if (!tableNames.Contains("t_stats") || !tableNames.Contains("t_leagues"))
        {
            Log.Error("Invalid save file, missing expected tables");
            return new Error<string>("Invalid save file, missing expected tables");
        }
        
        if (_applicationContext.MostRecentSelectedSaveFilePath is null) return new List<Smb4LeagueSelection>();

        List<Smb4LeagueSelection> leagues = new();
        var command2 = Connection.CreateCommand();
        commandText = SqlRunner.GetSqlCommand(SqlFile.GetLeagues);
        command2.CommandText = commandText;
        var reader2 = await command2.ExecuteReaderAsync();

        var smb4LeagueId = Guid.Empty;
        var smb4LeagueFilePath = _applicationContext.MostRecentSelectedSaveFilePath;

        if (smb4LeagueFilePath is not null)
        {
            var smb4LeagueFileName = Path.GetFileName(smb4LeagueFilePath);
            var smb4LeagueName = Path.GetFileNameWithoutExtension(smb4LeagueFileName);
            smb4LeagueName = smb4LeagueName[7..];
            var ok = Guid.TryParse(smb4LeagueName, out smb4LeagueId);
            if (!ok)
            {
                Log.Error("Failed to parse GUID from file name {FileName}. " +
                          "This occurs when we are attempting to cache the SMB4 league in the " +
                          "config for later on", smb4LeagueFileName);
                return new Error<string>("Failed to parse GUID from file name.");
            }
        }

        while (reader2.Read())
        {
            var leagueName = reader2.GetString(0);
            var league = new Smb4LeagueSelection(leagueName, smb4LeagueId);
            leagues.Add(league);
        }

        Log.Information("Successfully established database connection");
        return leagues;
    }

    public async Task Disconnect()
    {
        Log.Information("Disconnecting from database");
        if (Connection is not null)
        {
            try
            {
                Log.Debug("Committing all transactions");
                var command = Connection.CreateCommand();
                command.CommandText = "COMMIT";
                await command.ExecuteNonQueryAsync();
            }
            catch (SqliteException)
            {
                Log.Debug("No transactions to commit");
                // no-op, may throw if there are no transactions to commit
            }
            finally
            {
                Log.Debug("Closing and disposing of connection");
                // Remove reference to connection object
                var conn = Connection;
                Connection = null;

                // Close and dispose of the connection object
                conn.Close();
                conn.Dispose();

                // Clear all connections from the connection pool
                SqliteConnection.ClearAllPools();
            }
        }

        CurrentFilePath = string.Empty;
        Log.Debug("Connection to database closed");
    }
}