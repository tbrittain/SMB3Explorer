using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace SMB3Explorer.Services;

public class SystemInteropWrapper : ISystemInteropWrapper
{
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
        File.Delete(path);
        return true;
    }

    public async ValueTask FileCreate(string path)
    {
        await File.Create(path).DisposeAsync();
    }

    public bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

    public void DirectoryCreate(string path)
    {
        Directory.CreateDirectory(path);
    }
}