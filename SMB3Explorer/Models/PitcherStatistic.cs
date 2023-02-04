using CsvHelper.Configuration.Attributes;
using SMB3Explorer.Enums;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Models;

public class PitcherStatistic : PlayerStatistic
{
    [Name("pitcher_role"), Index(8)]
    public int PitcherRole { get; set; }

    [Name("pitcher_role_name"), Index(9)]
    // ReSharper disable once UnusedMember.Global
    public string PitcherRoleDescription => ((PitcherRole) PitcherRole).GetEnumDescription();
    
    [Name("games_pitching"), Index(10)]
    public int GamesPlayed { get; set; }
    
    [Name("games_started"), Index(11)]
    public int GamesStarted { get; set; }
    
    [Name("wins"), Index(12)]
    public int Wins { get; set; }
    
    [Name("losses"), Index(13)]
    public int Losses { get; set; }
    
    [Name("complete_games"), Index(14)]
    public int CompleteGames { get; set; }
    
    [Name("shutouts"), Index(15)]
    public int Shutouts { get; set; }
    
    [Name("total_pitches"), Index(16)]
    public int TotalPitches { get; set; }
    
    [Name("saves"), Index(17)]
    public int Saves { get; set; }
    
    [Name("outs_pitched"), Index(18)]
    public int OutsPitched { get; set; }
    
    [Name("hits_allowed"), Index(19)]
    public int HitsAllowed { get; set; }
    
    [Name("earned_runs"), Index(20)]
    public int EarnedRuns { get; set; }
    
    [Name("home_runs_allowed"), Index(21)]
    public int HomeRunsAllowed { get; set; }
    
    [Name("walks_allowed"), Index(22)]
    public int WalksAllowed { get; set; }
    
    [Name("strikeouts"), Index(23)]
    public int Strikeouts { get; set; }
    
    [Name("hit_by_pitch"), Index(24)]
    public int HitByPitch { get; set; }
    
    [Name("batters_faced"), Index(25)]
    public int BattersFaced { get; set; }
    
    [Name("games_finished"), Index(26)]
    public int GamesFinished { get; set; }
    
    [Name("runs_allowed"), Index(27)]
    public int RunsAllowed { get; set; }
    
    [Name("wild_pitches"), Index(28)]
    public int WildPitches { get; set; }
    
    [Name("innings_pitched"), Index(29)]
    public double InningsPitched { get; set; }
    
    [Name("earned_run_average"), Index(30)]
    public double EarnedRunAverage { get; set; }
    
    [Name("whip"), Index(31)]
    public double Whip { get; set; }
    
    [Name("opponent_batting_average"), Index(32)]
    public double OpponentBattingAverage { get; set; }
    
    [Name("winning_percentage"), Index(33)]
    public double WinningPercentage { get; set; }
    
    [Name("opponent_on_base_percentage"), Index(34)]
    public double OpponentOnBasePercentage { get; set; }
    
    [Name("strikeouts_per_walk"), Index(35)]
    public double StrikeoutsPerWalk { get; set; }
    
    [Name("strikeouts_per_nine_innings"), Index(36)]
    public double StrikeoutsPerNineInnings { get; set; }
    
    [Name("walks_per_nine_innings"), Index(37)]
    public double WalksPerNineInnings { get; set; }
    
    [Name("hits_per_nine_innings"), Index(38)]
    public double HitsPerNineInnings { get; set; }
    
    [Name("pitches_per_inning"), Index(39)]
    public double PitchesPerInning { get; set; }
    
    [Name("innings_per_game"), Index(40)]
    public double InningsPerGame { get; set; }
}