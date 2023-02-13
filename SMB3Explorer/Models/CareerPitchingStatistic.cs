using CsvHelper.Configuration.Attributes;
using SMB3Explorer.Enums;
using SMB3Explorer.Utils;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace SMB3Explorer.Models;

public class CareerPitchingStatistic : CareerStatistic
{
    [Name("pitcher_role"), Index(10)]
    public int PitcherRole { get; set; }
    
    [Name("pitcher_role_name"), Index(11)]
    // ReSharper disable once UnusedMember.Global
    public string PitcherRoleDescription => ((PitcherRole) PitcherRole).GetEnumDescription();
    
    [Name("wins"), Index(12)]
    public int Wins { get; set; }
    
    [Name("losses"), Index(13)]
    public int Losses { get; set; }
    
    [Name("games_played"), Index(14)]
    public int GamesPlayed { get; set; }
    
    [Name("games_started"), Index(15)]
    public int GamesStarted { get; set; }
    
    [Name("total_pitches"), Index(16)]
    public int TotalPitches { get; set; }
    
    [Name("shutouts"), Index(17)]
    public int Shutouts { get; set; }
    
    [Name("saves"), Index(18)]
    public int Saves { get; set; }
    
    [Name("outs_pitched"), Index(19)]
    public int OutsPitched { get; set; }
    
    [Name("hits_allowed"), Index(20)]
    public int HitsAllowed { get; set; }
    
    [Name("earned_runs"), Index(21)]
    public int EarnedRuns { get; set; }
    
    [Name("home_runs_allowed"), Index(22)]
    public int HomeRunsAllowed { get; set; }
    
    [Name("walks_allowed"), Index(23)]
    public int WalksAllowed { get; set; }
    
    [Name("strikeouts"), Index(24)]
    public int Strikeouts { get; set; }
    
    [Name("hit_by_pitch"), Index(25)]
    public int HitByPitch { get; set; }
    
    [Name("batters_faced"), Index(26)]
    public int BattersFaced { get; set; }
    
    [Name("runs_allowed"), Index(27)]
    public int RunsAllowed { get; set; }
    
    [Name("wild_pitches"), Index(28)]
    public int WildPitches { get; set; }
    
    [Name("innings_pitched"), Index(29)]
    public double InningsPitched => OutsPitched / 3.0;

    [Name("era"), Index(30)]
    public double EarnedRunAverage => EarnedRuns / InningsPitched;

    [Name("batting_average_against"), Index(31)]
    public double BattingAverageAgainst => HitsAllowed / (double) BattersFaced;

    [Name("whip"), Index(32)]
    public double WalksAndHitsPerInning => (WalksAllowed + HitsAllowed) / InningsPitched;
    
    [Name("win_percentage"), Index(33)]
    public double WinPercentage => Wins / (double) (Wins + Losses);
    
    [Name("opponent_on_base_percentage"), Index(34)]
    public double OpponentOnBasePercentage => (HitsAllowed + WalksAllowed + HitByPitch) / (double) BattersFaced;
    
    [Name("strikeout_to_walk_ratio"), Index(35)]
    public double StrikeoutToWalkRatio => Strikeouts / (double) WalksAllowed;
    
    [Name("strikeouts_per_nine_innings"), Index(36)]
    public double StrikeoutsPerNineInnings => Strikeouts / (InningsPitched / 9.0);
    
    [Name("walks_per_nine_innings"), Index(37)]
    public double WalksPerNineInnings => WalksAllowed / (InningsPitched / 9.0);
    
    [Name("hits_per_nine_innings"), Index(38)]
    public double HitsPerNineInnings => HitsAllowed / (InningsPitched / 9.0);
    
    [Name("home_runs_per_nine_innings"), Index(39)]
    public double HomeRunsPerNineInnings => HomeRunsAllowed / (InningsPitched / 9.0);
    
    [Name("pitches_per_inning"), Index(40)]
    public double PitchesPerInning => TotalPitches / InningsPitched;
    
    [Name("pitches_per_game"), Index(41)]
    public double PitchesPerGame => TotalPitches / (double) GamesPlayed;
}