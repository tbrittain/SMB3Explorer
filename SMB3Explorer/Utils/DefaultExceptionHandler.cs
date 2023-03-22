using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Serilog;
using SMB3Explorer.Services.SystemInteropWrapper;

namespace SMB3Explorer.Utils;

public static class DefaultExceptionHandler
{
    public const string GithubNewBugUrl =
        "https://github.com/tbrittain/SMB3Explorer/issues/new?labels=bug&template=bug_report.md";

    public static void HandleException(ISystemIoWrapper systemIoWrapper, string userFriendlyMessage,
        Exception exception)
    {
        Log.Warning("Caught exception, attempting to handle gracefully");
        var initialMessage = $"{userFriendlyMessage} " +
                             "The error has been logged to a text file. " +
                             "Press OK to open the log folder and report this issue on GitHub.";

        Log.Error(exception, "Original exception");
        if (exception.InnerException is not null)
        {
            Log.Debug("Exception has inner exception, logging");
            exception = exception.InnerException;
            Log.Error(exception, "Inner exception");
        }

        var formattedMessage = $"{initialMessage}{Environment.NewLine}{exception.Message}";

        var openBrowser = systemIoWrapper.ShowMessageBox(formattedMessage,
            "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);

        if (openBrowser != MessageBoxResult.OK) return;

        Log.Debug("Opening log directory and browser to report bug");
        Logger.FlushAndRestartLogger();

        SafeProcess.Start(Logger.LogDirectory, systemIoWrapper);
        SafeProcess.Start(GithubNewBugUrl, systemIoWrapper);
    }
}