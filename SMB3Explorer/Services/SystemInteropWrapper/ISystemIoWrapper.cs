using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Ionic.Zlib;
using Microsoft.Win32;
using SMB3Explorer.Services.CsvWriterWrapper;

namespace SMB3Explorer.Services.SystemInteropWrapper;

public interface ISystemIoWrapper
{
    MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button,
        MessageBoxImage icon);

    MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button);

    void SetClipboardText(string text);
    string MessageBoxText { get; }
    Process? StartProcess(ProcessStartInfo startInfo);

    bool FileExists(string path);
    bool FileDelete(string path);
    ValueTask FileCreate(string path);
    Stream FileCreateStream(string path);
    Stream? FileOpenRead(string path);
    long FileGetSize(string path);
    
    ZlibStream GetZlibDecompressionStream(Stream stream);
    
    bool DirectoryExists(string path);
    void DirectoryCreate(string path);
    string[] DirectoryGetDirectories(string path);
    string[] DirectoryGetFiles(string path, string searchPattern);
    
    StreamWriter CreateStreamWriter(string path);
    ICsvWriterWrapper CreateCsvWriter();
    bool ShowOpenFileDialog(OpenFileDialog openFileDialog);
}