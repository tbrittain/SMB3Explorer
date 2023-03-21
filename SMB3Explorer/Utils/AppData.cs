using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SMB3Explorer.Models.Internal;
using SMB3Explorer.Services.SystemInteropWrapper;

namespace SMB3Explorer.Utils;

public static class AppData
{
    private static IEnumerable<string> GetApplicationDataFiles(ISystemIoWrapper systemIoWrapper, string ignoreFile)
    {
        var tempPath = Path.GetTempPath();
        return systemIoWrapper.DirectoryGetFiles(tempPath, "smb3_explorer_*.sqlite")
            .Where(_ => !_.Equals(ignoreFile, StringComparison.OrdinalIgnoreCase));
    }
    
    public static AppDataSummary GetApplicationDataSize(ISystemIoWrapper systemIoWrapper, string ignoreFile)
    {
        var files = GetApplicationDataFiles(systemIoWrapper, ignoreFile).ToList();
        return new AppDataSummary(files.Count, files.Sum(systemIoWrapper.FileGetSize));
    }
    
    public static Task<List<AppDataFailedPurgeResult>> PurgeApplicationData(ISystemIoWrapper systemIoWrapper, string ignoreFile)
    {
        var files = GetApplicationDataFiles(systemIoWrapper, ignoreFile);
        
        var results = new List<AppDataFailedPurgeResult>();
        foreach (var file in files)
        {
            var size = systemIoWrapper.FileGetSize(file);
            var ok = systemIoWrapper.FileDelete(file);
            
            if (!ok) results.Add(new AppDataFailedPurgeResult(file, size));
        }
        
        return Task.FromResult(results);
    }
}