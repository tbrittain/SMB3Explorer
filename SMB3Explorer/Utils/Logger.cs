using System;
using System.IO;
using Serilog;

namespace SMB3Explorer.Utils;

public static class Logger
{
    internal static string LogDirectory { get; private set; } = string.Empty;
    internal static string LogPath { get; private set; } = string.Empty;
    
    public static void InitializeLogger()
    {
        LogDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SMB3Explorer", "Logs");
        LogPath = Path.Combine(LogDirectory, $"log_{DateTime.Now:yyyyMMddHHmmssfff}.txt");

#if RELEASE
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(LogPath)
            .MinimumLevel.Information()
            .CreateLogger();
#else
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(LogPath)
            .MinimumLevel.Debug()
            .CreateLogger();
#endif
    }
    
    internal static void FlushAndRestartLogger()
    {
        Log.Information("Restarting logger");
        
        Log.CloseAndFlush();
        InitializeLogger();
        
        Log.Information("Logger restarted");
    }
}