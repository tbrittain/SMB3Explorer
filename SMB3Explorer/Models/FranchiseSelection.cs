﻿using System;
using System.IO;
using System.Linq;

namespace SMB3Explorer.Models;

public record FranchiseSelection
{
    private readonly char[] _invalidChars = new[] {' '}
        .Concat(Path.GetInvalidFileNameChars())
        .ToArray();

    /// <summary>
    /// The League ID is different from a Franchise ID, which is not currently tracked, but will be
    /// and will be used to identify the franchise in the future for various queries that requre
    /// a FranchiseID vs a LeagueID.
    /// </summary>
    public Guid LeagueId { get; init; }
    public string LeagueName { get; init; } = string.Empty;
    public string LeagueType { get; init; } = string.Empty;
    public string PlayerTeamName { get; init; } = string.Empty;
    public int NumSeasons { get; init; }

    // ReSharper disable once UnusedMember.Global
    public string DisplayText => $"{LeagueName}: {NumSeasons} seasons as {PlayerTeamName}";

    public string LeagueNameSafe => new(LeagueName
        .Select(c => _invalidChars
            .Contains(c)
            ? '_'
            : c)
        .ToArray());
}