using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;

namespace SMB3Explorer.Services.CsvWriterWrapper;

public class CsvWriterWrapper : ICsvWriterWrapper
{
    private CsvWriter CsvWriter { get; set; } = null!;

    public void Initialize(StreamWriter streamWriter)
    {
        CsvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
    }

    public async Task WriteHeaderAsync<T>() where T : notnull
    {
        CsvWriter.WriteHeader<T>();
        await CsvWriter.NextRecordAsync();
    }

    public async Task WriteRecordAsync<T>(T record) where T : notnull
    {
        CsvWriter.WriteRecord(record);
        await CsvWriter.NextRecordAsync();
    }

    public void Dispose()
    {
        CsvWriter.Dispose();
        GC.SuppressFinalize(this);
    }

    public ValueTask DisposeAsync()
    {
        CsvWriter.Dispose();
        GC.SuppressFinalize(this);
        return default;
    }
}