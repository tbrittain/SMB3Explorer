using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.Extensions.DependencyInjection;

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

    public IServiceScope Scope { private get; set; } = null!;

    public async ValueTask DisposeAsync()
    {
        Scope.Dispose();
        await CsvWriter.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
        Scope.Dispose();
        CsvWriter.Dispose();
        GC.SuppressFinalize(this);
    }
}