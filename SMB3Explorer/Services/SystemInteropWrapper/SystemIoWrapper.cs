using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Ionic.Zlib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Serilog;
using SMB3Explorer.Services.CsvWriterWrapper;

namespace SMB3Explorer.Services.SystemInteropWrapper;

public class SystemIoWrapper : ISystemIoWrapper
{
    private readonly IServiceProvider  _serviceProvider;
    
    public SystemIoWrapper(IServiceProvider  serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
    {
        MessageBoxText = messageBoxText;
        
        return MessageBox.Show(messageBoxText, caption, button, icon);
    }

    public MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button)
    {
        MessageBoxText = messageBoxText;
        
        return MessageBox.Show(messageBoxText, caption, button);
    }

    public void SetClipboardText(string text)
    {
        Application.Current.Dispatcher.Invoke(() => Clipboard.SetText(text));
    }

    public string MessageBoxText { get; private set; } = string.Empty;

    public Process? StartProcess(ProcessStartInfo startInfo)
    {
        return Process.Start(startInfo);
    }

    public bool FileExists(string path)
    {
        return File.Exists(path);
    }

    public bool FileDelete(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (Exception e)
        {
            Log.Error(e, "Failed to delete file {Path}", path);
            return false;
        }

        return true;
    }

    public async ValueTask FileCreate(string path)
    {
        await File.Create(path).DisposeAsync();
    }

    public Stream FileCreateStream(string path)
    {
        return File.Create(path);
    }

    public Stream? FileOpenRead(string path)
    {
        return File.OpenRead(path);
    }

    public ZlibStream GetZlibDecompressionStream(Stream stream)
    {
        return new ZlibStream(stream, CompressionMode.Decompress);
    }

    public long FileGetSize(string path)
    {
        return new FileInfo(path).Length;
    }

    public bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

    public void DirectoryCreate(string path)
    {
        Directory.CreateDirectory(path);
    }

    public string[] DirectoryGetDirectories(string path)
    {
        return Directory.GetDirectories(path);
    }

    public string[] DirectoryGetFiles(string path, string searchPattern)
    {
        return Directory.GetFiles(path, searchPattern);
    }

    public StreamWriter CreateStreamWriter(string path)
    {
        return new StreamWriter(path);
    }

    public ICsvWriterWrapper CreateCsvWriter()
    {
        var scope = _serviceProvider.CreateScope();
        var csvWriterWrapper = scope.ServiceProvider.GetRequiredService<ICsvWriterWrapper>();
        csvWriterWrapper.Scope = scope;
        return csvWriterWrapper;
    }

    public bool ShowOpenFileDialog(OpenFileDialog openFileDialog)
    {
        return openFileDialog.ShowDialog() == true;
    }
}