using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;

namespace SMB3Explorer.Utils;

public static class CsvUtils
{
    private static readonly string DefaultDirectory =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SMB3Explorer");

    private static string GetDefaultFilePath(string fileName) => Path.Combine(DefaultDirectory, fileName);

    public static async Task<string> ExportCsv<T>(IAsyncEnumerable<T> records, string fileName,
        int limit = int.MaxValue)
    {
        if (!Directory.Exists(DefaultDirectory))
        {
            Directory.CreateDirectory(DefaultDirectory);
        }
        
        var filePath = GetDefaultFilePath(fileName);
        
        if (File.Exists(filePath)) File.Delete(filePath);
        await File.Create(filePath).DisposeAsync();

        await using var writer = new StreamWriter(filePath);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteHeader<T>();
        await csv.NextRecordAsync();

        var row = 0;
        var enumerator = records.GetAsyncEnumerator();
        while (await enumerator.MoveNextAsync())
        {
            csv.WriteRecord(enumerator.Current);
            await csv.NextRecordAsync();

            if (row++ >= limit)
            {
                break;
            }
        }

        await writer.FlushAsync();

        return filePath;
    }
}