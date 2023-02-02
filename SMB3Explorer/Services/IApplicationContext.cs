using System;

namespace SMB3Explorer.Services;

public interface IApplicationContext
{
    Guid? SelectedLeagueId { get; set; }
    bool IsLeagueSelected { get; }
}