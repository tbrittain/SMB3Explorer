using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using SMB3Explorer.Models.Internal;
using SMB3Explorer.Services.SystemIoWrapper;
using static SMB3Explorer.Constants.FileExports;

namespace SMB3Explorer.Utils;

public static class AppData
{
    private static IEnumerable<string> GetApplicationDataFiles(ISystemIoWrapper systemIoWrapper, string ignoreFile)
    {
        return systemIoWrapper.DirectoryGetFiles(TempDirectory, "smb3_explorer_*.sqlite")
            .Where(_ => !_.Equals(ignoreFile, StringComparison.OrdinalIgnoreCase));
    }

    public static AppDataSummary GetApplicationDataSize(ISystemIoWrapper systemIoWrapper, string ignoreFile)
    {
        var files = GetApplicationDataFiles(systemIoWrapper, ignoreFile).ToList();
        Log.Debug("Found {Count} files in application data", files.Count);
        return new AppDataSummary(files.Count, files.Sum(systemIoWrapper.FileGetSize));
    }

    public static Task<List<AppDataFailedPurgeResult>> PurgeApplicationData(ISystemIoWrapper systemIoWrapper,
        string ignoreFile)
    {
        Log.Debug("Purging application data...");
        var files = GetApplicationDataFiles(systemIoWrapper, ignoreFile);

        var results = new List<AppDataFailedPurgeResult>();
        foreach (var file in files)
        {
            var size = systemIoWrapper.FileGetSize(file);
            var ok = systemIoWrapper.FileDelete(file);

            if (!ok) results.Add(new AppDataFailedPurgeResult(file, size));
        }

        if (results.Count > 0)
            Log.Warning("Failed to purge {Count} files from application data", results.Count);
        else
            Log.Debug("Finished purging application data");

        return Task.FromResult(results);
    }
}