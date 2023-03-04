using System.ComponentModel;
using System.Diagnostics;
using SMB3Explorer.Services.SystemInteropWrapper;

namespace SMB3Explorer.Utils;

public static class SafeProcess
{
    public static void Start(string fileName, ISystemInteropWrapper systemInteropWrapper)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = fileName,
            UseShellExecute = true,
            CreateNoWindow = true
        };

        try
        {
            systemInteropWrapper.StartProcess(startInfo);
        }
        catch (Win32Exception e)
        {
            DefaultExceptionHandler.HandleException(systemInteropWrapper, "Failed to start process.", e);
        }
    }
}