using System.Diagnostics;
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
}