using System;
using System.Windows;
using Serilog;
using SMB3Explorer.Services.SystemIoWrapper;
using static SMB3Explorer.Constants.FileExports;
using static SMB3Explorer.Constants.Github;

namespace SMB3Explorer.Utils;

public static class DefaultExceptionHandler
{
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

        SafeProcess.Start(LogDirectory, systemIoWrapper);
        SafeProcess.Start(NewBugUrl, systemIoWrapper);
    }
}