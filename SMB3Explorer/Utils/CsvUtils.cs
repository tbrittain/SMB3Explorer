using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;

namespace SMB3Explorer.Utils;

public static class CsvUtils
{
    private static string GetDefaultFilePath(string fileName) => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SMB3Explorer", fileName);
    
    public static async Task ExportCsv<T>(IAsyncEnumerable<T> records, string fileName, int limit = int.MaxValue)
    {
        var filePath = GetDefaultFilePath(fileName);

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
    }
}