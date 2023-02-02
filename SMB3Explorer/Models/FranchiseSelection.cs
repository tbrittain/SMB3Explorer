using System;

namespace SMB3Explorer.Models;

public class FranchiseSelection
{
    public Guid LeagueId { get; set; }
    public string LeagueName { get; set; } = string.Empty;
    public string LeagueType { get; set; } = string.Empty;
    public string PlayerTeamName { get; set; } = string.Empty;
    public int NumSeasons { get; set; }

    // ReSharper disable once UnusedMember.Global
    public string DisplayText => $"{LeagueName}: {NumSeasons} seasons as {PlayerTeamName}";
}