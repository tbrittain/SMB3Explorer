using CsvHelper.Configuration.Attributes;
using SMB3Explorer.Enums;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Models;

public class BattingStatistic : PlayerStatistic
{
    [Name("secondary_position"), Index(7)]
    public int SecondaryPositionNumber { get; set; }
    
    [Name("secondary_position_name"), Index(8)]
    // ReSharper disable once UnusedMember.Global
    public string SecondaryPosition => ((BaseballPlayerPosition) SecondaryPositionNumber).GetEnumDescription();
    
    [Name("games_batting"), Index(9)]
    public int GamesPlayed { get; set; }
    
    [Name("at_bats"), Index(10)]
    public int AtBats { get; set; }
    
    [Name("plate_appearances"), Index(11)]
    public int PlateAppearances { get; set; }
    
    [Name("runs"), Index(12)]
    public int Runs { get; set; }
    
    [Name("hits"), Index(13)]
    public int Hits { get; set; }
    
    [Name("doubles"), Index(14)]
    public int Doubles { get; set; }
    
    [Name("triples"), Index(15)]
    public int Triples { get; set; }
    
    [Name("home_runs"), Index(16)]
    public int HomeRuns { get; set; }
    
    [Name("rbi"), Index(17)]
    public int RunsBattedIn { get; set; }
    
    [Name("extra_base_hits"), Index(18)]
    public int ExtraBaseHits { get; set; }
    
    [Name("total_bases"), Index(19)]
    public int TotalBases { get; set; }
    
    [Name("stolen_bases"), Index(20)]
    public int StolenBases { get; set; }
    
    [Name("caught_stealing"), Index(21)]
    public int CaughtStealing { get; set; }
    
    [Name("walks"), Index(22)]
    public int Walks { get; set; }
    
    [Name("strikeouts"), Index(23)]
    public int Strikeouts { get; set; }
    
    [Name("hit_by_pitch"), Index(24)]
    public int HitByPitch { get; set; }
    
    [Name("sacrifice_hits"), Index(25)]
    public int SacrificeHits { get; set; }
    
    [Name("sacrifice_flies"), Index(26)]
    public int SacrificeFlies { get; set; }
    
    [Name("errors"), Index(27)]
    public int Errors { get; set; }
    
    [Name("passed_balls"), Index(28)]
    public int PassedBalls { get; set; }
}