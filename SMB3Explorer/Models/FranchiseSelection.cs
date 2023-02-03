using System;

namespace SMB3Explorer.Models;

public record FranchiseSelection
{
    public Guid LeagueId { get; init; }
    public string LeagueName { get; init; } = string.Empty;
    public string LeagueType { get; init; } = string.Empty;
    public string PlayerTeamName { get; init; } = string.Empty;
    public int NumSeasons { get; init; }

    // ReSharper disable once UnusedMember.Global
    public string DisplayText => $"{LeagueName}: {NumSeasons} seasons as {PlayerTeamName}";
}