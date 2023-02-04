using CsvHelper.Configuration.Attributes;

namespace SMB3Explorer.Models;

/// <summary>
/// Represents a player's career statistics, but could be used for a single season as well
/// </summary>
public class BattingStatistic : PlayerStatistic
{
    [Name("games_batting"), Index(8)]
    public int GamesPlayed { get; set; }
    
    [Name("at_bats"), Index(9)]
    public int AtBats { get; set; }
    
    [Name("plate_appearances"), Index(10)]
    public int PlateAppearances { get; set; }
    
    [Name("runs"), Index(11)]
    public int Runs { get; set; }
    
    [Name("hits"), Index(12)]
    public int Hits { get; set; }
    
    [Name("doubles"), Index(13)]
    public int Doubles { get; set; }
    
    [Name("triples"), Index(14)]
    public int Triples { get; set; }
    
    [Name("home_runs"), Index(15)]
    public int HomeRuns { get; set; }
    
    [Name("rbi"), Index(16)]
    public int RunsBattedIn { get; set; }
    
    [Name("extra_base_hits"), Index(17)]
    public int ExtraBaseHits { get; set; }
    
    [Name("total_bases"), Index(18)]
    public int TotalBases { get; set; }
    
    [Name("stolen_bases"), Index(19)]
    public int StolenBases { get; set; }
    
    [Name("caught_stealing"), Index(20)]
    public int CaughtStealing { get; set; }
    
    [Name("walks"), Index(21)]
    public int Walks { get; set; }
    
    [Name("strikeouts"), Index(22)]
    public int Strikeouts { get; set; }
    
    [Name("hit_by_pitch"), Index(23)]
    public int HitByPitch { get; set; }
    
    [Name("sacrifice_hits"), Index(24)]
    public int SacrificeHits { get; set; }
    
    [Name("sacrifice_flies"), Index(25)]
    public int SacrificeFlies { get; set; }
    
    [Name("errors"), Index(26)]
    public int Errors { get; set; }
    
    [Name("passed_balls"), Index(27)]
    public int PassedBalls { get; set; }
    
    [Name("plate_appearances_per_game"), Index(28)]
    public double PlateAppearancesPerGame { get; set; }
    
    [Name("obp"), Index(29)]
    public double OnBasePercentage { get; set; }
    
    [Name("slugging_percentage"), Index(30)]
    public double SluggingPercentage { get; set; }
    
    [Name("on_base_plus_slugging"), Index(31)]
    public double OnBasePlusSlugging { get; set; }
    
    [Name("batting_average"), Index(32)]
    public double BattingAverage { get; set; }
    
    [Name("babip"), Index(33)]
    public double BattingAverageOnBallsInPlay { get; set; }
    
    [Name("at_bats_per_home_run"), Index(34)]
    public double AtBatsPerHomeRun { get; set; }
    
    [Name("strikeout_percentage"), Index(35)]
    public double StrikeoutPercentage { get; set; }
    
    [Name("walk_percentage"), Index(36)]
    public double WalkPercentage { get; set; }
    
    [Name("extra_base_hit_percentage"), Index(37)]
    public double ExtraBaseHitPercentage { get; set; }
}