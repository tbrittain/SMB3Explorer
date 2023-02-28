using System;
using System.IO;
using System.Threading.Tasks;

namespace SMB3Explorer.Services.CsvWriterWrapper;

public interface ICsvWriterWrapper : IDisposable, IAsyncDisposable
{
    void Initialize(StreamWriter writer);
    Task WriteHeaderAsync<T>() where T : notnull;
    Task WriteRecordAsync<T>(T record) where T : notnull;
}