using System;

namespace SMB3Explorer.Utils;

public static class FormattedStringBytes
{
    public static string BytesToString(long byteCount)
    {
        return byteCount switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(byteCount)),
            < 1024 => $"{byteCount}B",
            < 1048576 => $"{byteCount / 1024.0:0.##}KB",
            < 1073741824 => $"{byteCount / 1048576.0:0.##}MB",
            _ => $"{byteCount / 1073741824.0:0.##}GB"
        };
    }
}
