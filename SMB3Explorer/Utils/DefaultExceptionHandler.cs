using System;
using System.Diagnostics;
using System.Windows;

namespace SMB3Explorer.Utils;

public static class DefaultExceptionHandler
{
    public static void HandleException(string userFriendlyMessage, Exception? exception)
    {
        var initialMessage = $"{userFriendlyMessage} " +
                             "A full stack trace has been copied to your clipboard. " +
                             "Press OK to report this issue on GitHub.";

        var formattedMessage = $"{initialMessage}{Environment.NewLine}{exception?.Message ?? "Unknown error"}";

        var openBrowser = MessageBox.Show(formattedMessage,
            "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);

        Application.Current.Dispatcher.Invoke(() => Clipboard.SetText(exception?.StackTrace ?? "Unknown error"));
        
        if (openBrowser != MessageBoxResult.OK) return;

        const string url = "https://github.com/tbrittain/SMB3Explorer/issues/new";
        Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));
    }
}