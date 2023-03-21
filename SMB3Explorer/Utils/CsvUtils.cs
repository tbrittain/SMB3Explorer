using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SMB3Explorer.Services.CsvWriterWrapper;
using SMB3Explorer.Services.SystemInteropWrapper;

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
            systemIoWrapper.DirectoryCreate(DefaultDirectory);
        }
        
        var filePath = GetDefaultFilePath(fileName);
        
        if (systemIoWrapper.FileExists(filePath)) systemIoWrapper.FileDelete(filePath);
        await systemIoWrapper.FileCreate(filePath);

        var rowCount = 1;
        await using var writer = systemIoWrapper.CreateStreamWriter(filePath);
        await using var csv = systemIoWrapper.CreateCsvWriter();
        csv.Initialize(writer);

        await csv.WriteHeaderAsync<T>();

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

        return (filePath, rowCount);
    }
}