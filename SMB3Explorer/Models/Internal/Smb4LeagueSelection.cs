using System;
using System.Text;

namespace SMB3Explorer.Models.Internal;

public record Smb4LeagueSelection(string LeagueName, Guid SaveGameLeagueId, string? PlayerTeam, int? NumSeasons)
{
    public int NumTimesAccessed { get; set; }
    public DateTime FirstAccessed { get; init; }
    public DateTime LastAccessed { get; set; }
    public LeagueMode? Mode { get; init; }
    
    // ReSharper disable once UnusedMember.Global
    public string DisplayName
    {
        get
        {
            var sb = new StringBuilder(LeagueName);

            if (Mode is not null)
            {
                sb.Append($" ({Mode} mode)");
            }

            if (!string.IsNullOrEmpty(PlayerTeam) && NumSeasons is not null)
            {
                sb.Append($" ({NumSeasons} seasons as {PlayerTeam})");
            }

            sb.Append($", first accessed {FirstAccessed.ToShortDateString()}");
            return sb.ToString();
        }
    }

    public virtual bool Equals(Smb4LeagueSelection? other)
    {
        return other is not null &&
               LeagueName == other.LeagueName &&
               SaveGameLeagueId == other.SaveGameLeagueId &&
               PlayerTeam == other.PlayerTeam;
    }

    override public int GetHashCode()
    {
        return HashCode.Combine(LeagueName, SaveGameLeagueId, PlayerTeam);
    }
}