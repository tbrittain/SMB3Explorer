using CsvHelper.Configuration.Attributes;
using SMB3Explorer.Enums;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Models;

public class PitcherStatistic : PlayerStatistic
{
    [Name("pitcher_role"), Index(9)]
    public int PitcherRole { get; set; }

    [Name("pitcher_role_name"), Index(10)]
    // ReSharper disable once UnusedMember.Global
    public string PitcherRoleDescription => ((PitcherRole) PitcherRole).GetEnumDescription();
    
    [Name("games_pitching"), Index(11)]
    public int GamesPlayed { get; set; }
    
    [Name("games_started"), Index(12)]
    public int GamesStarted { get; set; }
    
    [Name("wins"), Index(13)]
    public int Wins { get; set; }
    
    [Name("losses"), Index(14)]
    public int Losses { get; set; }
    
    [Name("complete_games"), Index(15)]
    public int CompleteGames { get; set; }
    
    [Name("shutouts"), Index(16)]
    public int Shutouts { get; set; }
    
    [Name("total_pitches"), Index(17)]
    public int TotalPitches { get; set; }
    
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
    
    [Name("games_finished"), Index(27)]
    public int GamesFinished { get; set; }
    
    [Name("runs_allowed"), Index(28)]
    public int RunsAllowed { get; set; }
    
    [Name("wild_pitches"), Index(29)]
    public int WildPitches { get; set; }
    
    [Name("innings_pitched"), Index(30)]
    public double InningsPitched { get; set; }
}