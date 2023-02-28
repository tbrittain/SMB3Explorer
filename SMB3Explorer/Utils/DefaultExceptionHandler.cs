using System;
using System.Diagnostics;
using System.Windows;
using SMB3Explorer.Services;

namespace SMB3Explorer.Utils;

public static class DefaultExceptionHandler
{
    public const string GithubNewIssueUrl = "https://github.com/tbrittain/SMB3Explorer/issues/new";

    public static void HandleException(ISystemInteropWrapper systemInteropWrapper, string userFriendlyMessage,
        Exception exception)
    {
        var initialMessage = $"{userFriendlyMessage} " +
                             "A full stack trace has been copied to your clipboard. " +
                             "Press OK to report this issue on GitHub.";

        var formattedMessage = $"{initialMessage}{Environment.NewLine}{exception.Message}";

        var openBrowser = systemInteropWrapper.ShowMessageBox(formattedMessage,
            "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        
        var stackTrace = exception.StackTrace ?? "Unknown error";
        systemInteropWrapper.SetClipboardText($"{exception.Message}{stackTrace}");

        if (openBrowser != MessageBoxResult.OK) return;

        systemInteropWrapper.StartProcess(new ProcessStartInfo("cmd", $"/c start {GithubNewIssueUrl}"));
    }
}