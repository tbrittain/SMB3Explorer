using System;
using System.IO;
using Serilog;
using static SMB3Explorer.Constants.FileExports;

namespace SMB3Explorer.Utils;

public static class Logger
{
    private static readonly string LogPath = Path.Combine(LogDirectory, $"log_{DateTime.Now:yyyyMMddHHmmssfff}.txt");
    
    public static void InitializeLogger()
    {
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