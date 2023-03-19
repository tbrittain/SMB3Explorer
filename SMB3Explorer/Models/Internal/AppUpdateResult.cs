using System;

namespace SMB3Explorer.Models.Internal;

public readonly record struct AppUpdateResult(string Version, string ReleasePageUrl, DateTime ReleaseDate, string Name)
{
    public int DaysSinceRelease => (DateTime.Now - ReleaseDate).Days;
}