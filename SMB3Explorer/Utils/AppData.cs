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
    private static IEnumerable<string> GetApplicationDataFiles(ISystemInteropWrapper systemInteropWrapper, string ignoreFile)
    {
        var tempPath = Path.GetTempPath();
        return systemInteropWrapper.DirectoryGetFiles(tempPath, "smb3_explorer_*.sqlite")
            .Where(_ => !_.Equals(ignoreFile, StringComparison.OrdinalIgnoreCase));
    }
    
    public static AppDataSummary GetApplicationDataSize(ISystemInteropWrapper systemInteropWrapper, string ignoreFile)
    {
        var files = GetApplicationDataFiles(systemInteropWrapper, ignoreFile).ToList();
        return new AppDataSummary(files.Count, files.Sum(systemInteropWrapper.FileGetSize));
    }
    
    public static Task<List<AppDataFailedPurgeResult>> PurgeApplicationData(ISystemInteropWrapper systemInteropWrapper, string ignoreFile)
    {
        var files = GetApplicationDataFiles(systemInteropWrapper, ignoreFile);
        
        var results = new List<AppDataFailedPurgeResult>();
        foreach (var file in files)
        {
            var size = systemInteropWrapper.FileGetSize(file);
            var ok = systemInteropWrapper.FileDelete(file);
            
            if (!ok) results.Add(new AppDataFailedPurgeResult(file, size));
        }
        
        return Task.FromResult(results);
    }
}