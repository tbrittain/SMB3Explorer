using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SMB3Explorer.Services.CsvWriterWrapper;

public interface ICsvWriterWrapper : IAsyncDisposable, IDisposable
{
    void Initialize(StreamWriter writer);
    Task WriteHeaderAsync<T>() where T : notnull;
    Task WriteRecordAsync<T>(T record) where T : notnull;
    IServiceScope Scope { set; }
}