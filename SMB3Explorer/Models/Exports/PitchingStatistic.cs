using System;
using CsvHelper.Configuration.Attributes;
using SMB3Explorer.Enums;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Models.Exports;

public abstract class PitchingStatistic
{
    [Ignore]
    public Guid? PlayerId { get; set; }
    
    [Ignore]
    public int SeasonId { get; set; }

    [Name("Season"), Index(0)]
    public int SeasonNum { get; set; }

    [Name("First Name"), Index(1)]
    public string FirstName { get; set; } = string.Empty;

    [Name("Last Name"), Index(2)]
    public string LastName { get; set; } = string.Empty;

    [Name("Team"), Index(3)]
    public string? TeamName { get; set; }
    
    [Name("Prev Team"), Index(4)]
    public string? MostRecentTeamName { get; set; }
    
    [Name("2nd Prev Team"), Index(5)]
    public string? PreviousTeamName { get; set; }

    [Ignore]
    public int PositionNumber { get; set; }
    
    [Ignore]
    public int PitcherRole { get; set; }

    [Name("Pitcher Role"), Index(6)]
    // ReSharper disable once UnusedMember.Global
    public string PitcherRoleDescription => ((PitcherRole) PitcherRole).GetEnumDescription();
    
    [Name("Age"), Index(7)]
    public int Age { get; set; }

    [Name("W"), Index(8)]
    public int Wins { get; set; }

    [Name("L"), Index(9)]
    public int Losses { get; set; }

    [Name("CG"), Index(10)]
    public int CompleteGames { get; set; }

    [Name("CGSO"), Index(11)]
    public int Shutouts { get; set; }

    [Ignore]
    public int OutsPitched { get; set; }

    [Name("H"), Index(12)]
    public int HitsAllowed { get; set; }
    
    [Name("ER"), Index(13)]
    public int EarnedRuns { get; set; }

    [Name("HR"), Index(14)]
    public int HomeRunsAllowed { get; set; }

    [Name("BB"), Index(15)]
    public int WalksAllowed { get; set; }

    [Name("K"), Index(16)]
    public int Strikeouts { get; set; }
    
    [Name("IP"), Index(17)]
    public double InningsPitched => OutsPitched / 3.0;

    [Name("ERA"), Index(18)]
    public double EarnedRunAverage => EarnedRuns / (InningsPitched / 9.0);
    
    [Name("TP"), Index(19)]
    public int TotalPitches { get; set; }

    [Name("S"), Index(20)]
    public int Saves { get; set; }

    [Name("HBP"), Index(21)]
    public int HitByPitch { get; set; }

    [Name("Batters Faced"), Index(22)]
    public int BattersFaced { get; set; }
    
    [Name("Games Played"), Index(23)]
    public int GamesPlayed { get; set; }

    [Name("Games Started"), Index(24)]
    public int GamesStarted { get; set; }

    [Name("Games Finished"), Index(25)]
    public int GamesFinished { get; set; }

    [Name("Runs Allowed"), Index(26)]
    public int RunsAllowed { get; set; }

    [Name("WP"), Index(27)]
    public int WildPitches { get; set; }

    [Name("BAA"), Index(28)]
    public double BattingAverageAgainst => HitsAllowed / (double) BattersFaced;

    [Name("FIP"), Index(29)]
    public double FieldingIndependentPitching =>
        (((13 * HomeRunsAllowed) + (3 * (WalksAllowed + HitByPitch)) - 
          (2 * Strikeouts)) / (double) OutsPitched) + 3.10;

    [Name("WHIP"), Index(30)]
    public double WalksAndHitsPerInning => (WalksAllowed + HitsAllowed) / InningsPitched;
    
    [Name("WPCT"), Index(31)]
    public double WinPercentage => Wins / (double) (Wins + Losses);
    
    [Name("Opp OBP"), Index(32)]
    public double OpponentOnBasePercentage => (HitsAllowed + WalksAllowed + HitByPitch) / (double) BattersFaced;
    
    [Name("K/BB"), Index(33)]
    public double StrikeoutToWalkRatio => Strikeouts / (double) WalksAllowed;
    
    [Name("K/9"), Index(34)]
    public double StrikeoutsPerNineInnings => Strikeouts / (InningsPitched / 9.0);
    
    [Name("BB/9"), Index(35)]
    public double WalksPerNineInnings => WalksAllowed / (InningsPitched / 9.0);
    
    [Name("H/9"), Index(36)]
    public double HitsPerNineInnings => HitsAllowed / (InningsPitched / 9.0);
    
    [Name("HR/9"), Index(37)]
    public double HomeRunsPerNineInnings => HomeRunsAllowed / (InningsPitched / 9.0);
    
    [Name("Pitches Per Inning"), Index(38)]
    public double PitchesPerInning => TotalPitches / InningsPitched;
    
    [Name("Pitcher Per Game"), Index(39)]
    public double PitchesPerGame => TotalPitches / (double) GamesPlayed;
}