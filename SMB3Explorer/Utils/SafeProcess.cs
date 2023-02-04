using System.ComponentModel;

namespace SMB3Explorer.Utils;

public static class SafeProcess
{
    public static void Start(string fileName)
    {
        var process = new System.Diagnostics.Process
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo
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
            DefaultExceptionHandler.HandleException("Failed to start process.", e);
        }
    }
}