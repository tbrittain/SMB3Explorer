using System.ComponentModel;
using System.Diagnostics;
using Serilog;
using SMB3Explorer.Services.SystemInteropWrapper;

namespace SMB3Explorer.Utils;

public static class SafeProcess
{
    public static void Start(string fileName, ISystemIoWrapper systemIoWrapper)
    {
        Log.Debug("Starting process {FileName}", fileName);
        var startInfo = new ProcessStartInfo
        {
            FileName = fileName,
            UseShellExecute = true,
            CreateNoWindow = true
        };

        try
        {
            systemIoWrapper.StartProcess(startInfo);
        }
        catch (Win32Exception e)
        {
            Log.Error(e, "Failed to start process {FileName}", fileName);
            DefaultExceptionHandler.HandleException(systemIoWrapper, "Failed to start process.", e);
        }
    }
}