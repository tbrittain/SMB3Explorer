using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Serilog;
using SMB3Explorer.Services.SystemIoWrapper;

namespace SMB3Explorer.Utils;

public static class CsvUtils
{
    public static readonly string DefaultDirectory =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SMB3Explorer");

    private static string GetDefaultFilePath(string fileName) => Path.Combine(DefaultDirectory, fileName);

    public static async Task<(string, int)> ExportCsv<T>(ISystemIoWrapper systemIoWrapper,
        IAsyncEnumerable<T> records, string fileName, int limit = int.MaxValue) where T : notnull
    {
        if (!systemIoWrapper.DirectoryExists(DefaultDirectory))
        {
            Log.Debug("Creating directory {DefaultDirectory}", DefaultDirectory);
            systemIoWrapper.DirectoryCreate(DefaultDirectory);
        }
        
        var filePath = GetDefaultFilePath(fileName);
        
        if (systemIoWrapper.FileExists(filePath)) systemIoWrapper.FileDelete(filePath);
        await systemIoWrapper.FileCreate(filePath);
        Log.Debug("Created file {FilePath}", filePath);

        var rowCount = 1;
        await using var writer = systemIoWrapper.CreateStreamWriter(filePath);
        await using var csv = systemIoWrapper.CreateCsvWriter();
        csv.Initialize(writer);

        await csv.WriteHeaderAsync<T>();

        Log.Debug("Writing records to {FilePath}", filePath);
        var enumerator = records.GetAsyncEnumerator();
        while (await enumerator.MoveNextAsync())
        {
            await csv.WriteRecordAsync(enumerator.Current);

            if (rowCount >= limit)
            {
                break;
            }

            rowCount++;
        }
        
        Log.Debug("Finished writing {RowCount} records to {FilePath}", rowCount, filePath);

        return (filePath, rowCount);
    }
}