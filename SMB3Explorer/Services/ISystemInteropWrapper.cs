using System.Diagnostics;
using System.Windows;

namespace SMB3Explorer.Services;

public interface ISystemInteropWrapper
{
    MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon);
    void SetClipboardText(string text);
    string MessageBoxText { get; }
    Process? StartProcess(ProcessStartInfo startInfo);
}