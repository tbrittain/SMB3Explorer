using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SMB3Explorer.Models.Internal;

public enum LeagueMode
{
    /// <summary>
    /// This may be the case if no games have been played yet
    /// </summary>
    None,
    Franchise,
    Season,
    Elimination
}

public record LeagueSelection
{
    private readonly char[] _invalidChars = new[] {' '}
        .Concat(Path.GetInvalidFileNameChars())
        .ToArray();
    
    private string? _displayText;

    public required Guid LeagueId { get; init; }
    public required LeagueMode Mode { get; init; }
    public required string LeagueName { get; init; } = string.Empty;
    public required string LeagueType { get; init; } = string.Empty;
    public required string? PlayerTeamName { get; init; } = string.Empty;
    public required int NumSeasons { get; init; }
    
    // ReSharper disable once UnusedMember.Global
    public string DisplayText
    {
        get
        {
            if (!string.IsNullOrEmpty(_displayText))
            {
                return _displayText;
            }

            var sb = new StringBuilder(LeagueName);

            switch (Mode)
            {
                case LeagueMode.Franchise:
                    sb.Append($": Franchise mode ({LeagueType}) with {NumSeasons} seasons");
                    if (!string.IsNullOrEmpty(PlayerTeamName))
                    {
                        sb.Append($" as {PlayerTeamName}");
                    }
                    break;
                case LeagueMode.Season:
                    sb.Append($": Season mode ({LeagueType}) with {NumSeasons} seasons");
                    break;
                case LeagueMode.Elimination:
                    sb.Append($": Elimination mode ({LeagueType})");
                    break;
                case LeagueMode.None:
                    sb.Append($": No games played yet ({LeagueType})");
                    break;
            }

            _displayText = sb.ToString();

            return _displayText;
        }
    }

    public string LeagueNameSafe => new(LeagueName
        .Select(c => _invalidChars
            .Contains(c)
            ? '_'
            : c)
        .ToArray());
}