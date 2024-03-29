﻿using System;

namespace SMB3Explorer.Models.Internal;

public readonly record struct AppUpdateResult(Version Version, string ReleasePageUrl, DateTime ReleaseDate)
{
    public int DaysSinceRelease => (DateTime.Now - ReleaseDate).Days;
}