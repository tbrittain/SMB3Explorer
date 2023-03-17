using System.ComponentModel;
using System.Diagnostics;
using SMB3Explorer.Services.SystemInteropWrapper;

namespace SMB3Explorer.Utils;

public static class SafeProcess
{
    public static void Start(string fileName, ISystemIoWrapper systemIoWrapper)
    {
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
            DefaultExceptionHandler.HandleException(systemIoWrapper, "Failed to start process.", e);
        }
    }
}