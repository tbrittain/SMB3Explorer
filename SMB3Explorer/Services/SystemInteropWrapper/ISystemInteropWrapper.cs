using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using SMB3Explorer.Services.CsvWriterWrapper;

namespace SMB3Explorer.Services.SystemInteropWrapper;

public interface ISystemInteropWrapper
{
    MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon);
    MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button);
    void SetClipboardText(string text);
    string MessageBoxText { get; }
    Process? StartProcess(ProcessStartInfo startInfo);
    bool FileExists(string path);
    bool FileDelete(string path);
    ValueTask FileCreate(string path);
    long FileGetSize(string path);
    bool DirectoryExists(string path);
    void DirectoryCreate(string path);
    string[] DirectoryGetDirectories(string path);
    string[] DirectoryGetFiles(string path, string searchPattern);
    StreamWriter CreateStreamWriter(string path);
    ICsvWriterWrapper CreateCsvWriter();
    bool ShowOpenFileDialog(OpenFileDialog openFileDialog);
}