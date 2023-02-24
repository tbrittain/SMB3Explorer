using System.ComponentModel;
using System.Diagnostics;
using SMB3Explorer.Services;

namespace SMB3Explorer.Utils;

public static class SafeProcess
{
    public static void Start(string fileName, ISystemInteropWrapper systemInteropWrapper)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                UseShellExecute = true,
                CreateNoWindow = true
            }
        };

        try
        {
            process.Start();
        }
        catch (Win32Exception e)
        {
            DefaultExceptionHandler.HandleException("Failed to start process.", e, systemInteropWrapper);
        }
    }
}