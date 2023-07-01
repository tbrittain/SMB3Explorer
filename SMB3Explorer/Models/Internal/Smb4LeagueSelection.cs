using System;
using System.Text;

namespace SMB3Explorer.Models.Internal;

public record Smb4LeagueSelection(string LeagueName, Guid SaveGameLeagueId, string? PlayerTeam, int? NumSeasons)
{
    public int NumTimesAccessed { get; set; }
    public DateTime FirstAccessed { get; set; }
    public DateTime LastAccessed { get; set; }
    
    // ReSharper disable once UnusedMember.Global
    public string DisplayName
    {
        get
        {
            var sb = new StringBuilder(LeagueName);

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

    public override int GetHashCode()
    {
        return HashCode.Combine(LeagueName, SaveGameLeagueId, PlayerTeam);
    }
}