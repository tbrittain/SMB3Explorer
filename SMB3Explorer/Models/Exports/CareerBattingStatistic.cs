using CsvHelper.Configuration.Attributes;
using SMB3Explorer.Enums;
using SMB3Explorer.Utils;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SMB3Explorer.Models.Exports;

public class CareerBattingStatistic : CareerStatistic
{
    [Name("primary_position"), Index(10)]
    public int PrimaryPositionNumber { get; set; }
    
    [Name("primary_position_name"), Index(11)]
    // ReSharper disable once UnusedMember.Global
    public string PrimaryPositionDescription => ((BaseballPlayerPosition) PrimaryPositionNumber).GetEnumDescription();
    
    [Name("secondary_position"), Index(12)]
    public int? SecondaryPositionNumber { get; set; }
    
    [Name("secondary_position_name"), Index(13)]
    // ReSharper disable once UnusedMember.Global
    public string? SecondaryPositionDescription => SecondaryPositionNumber is null 
        ? null 
        : ((BaseballPlayerPosition) SecondaryPositionNumber).GetEnumDescription();
    
    [Name("pitcher_role"), Index(14)]
    public int? PitcherRole { get; set; }
    
    [Name("pitcher_role_name"), Index(15)]
    // ReSharper disable once UnusedMember.Global
    public string? PitcherRoleDescription => PitcherRole is null 
        ? null 
        : ((PitcherRole) PitcherRole).GetEnumDescription();
    
    [Name("games_played"), Index(16)]
    public int GamesPlayed { get; set; }
    
    [Name("games_batting"), Index(17)]
    public int GamesBatting { get; set; }
    
    [Name("at_bats"), Index(18)]
    public int AtBats { get; set; }
    
    [Name("plate_appearances"), Index(19)]
    public int PlateAppearances => AtBats + Walks + SacrificeHits + SacrificeFlies + HitByPitch;
    
    [Name("runs"), Index(20)]
    public int Runs { get; set; }
    
    [Name("hits"), Index(21)]
    public int Hits { get; set; }
    
    private int Singles => Hits - Doubles - Triples - HomeRuns;
    
    [Name("doubles"), Index(22)]
    public int Doubles { get; set; }
    
    [Name("triples"), Index(23)]
    public int Triples { get; set; }
    
    [Name("home_runs"), Index(24)]
    public int HomeRuns { get; set; }
    
    [Name("rbi"), Index(25)]
    public int RunsBattedIn { get; set; }
    
    [Name("extra_base_hits"), Index(26)]
    public int ExtraBaseHits => Doubles + Triples + HomeRuns;
    
    [Name("total_bases"), Index(27)]
    public int TotalBases => Singles + (2 * Doubles) + (3 * Triples) + (4 * HomeRuns);
    
    [Name("stolen_bases"), Index(28)]
    public int StolenBases { get; set; }
    
    [Name("caught_stealing"), Index(29)]
    public int CaughtStealing { get; set; }
    
    [Name("walks"), Index(30)]
    public int Walks { get; set; }
    
    [Name("strikeouts"), Index(31)]
    public int Strikeouts { get; set; }
    
    [Name("hit_by_pitch"), Index(32)]
    public int HitByPitch { get; set; }
    
    [Name("sacrifice_hits"), Index(33)]
    public int SacrificeHits { get; set; }
    
    [Name("sacrifice_flies"), Index(34)]
    public int SacrificeFlies { get; set; }
    
    [Name("errors"), Index(35)]
    public int Errors { get; set; }
    
    [Name("passed_balls"), Index(36)]
    public int PassedBalls { get; set; }
    
    [Name("plate_appearances_per_game"), Index(37)]
    public double PlateAppearancesPerGame => PlateAppearances / (double) GamesPlayed;

    [Name("on_base_percentage"), Index(38)]
    public double OnBasePercentage => (Hits + Walks + HitByPitch) /
                                      (double) (AtBats + Walks + HitByPitch + SacrificeFlies);

    [Name("slugging_percentage"), Index(39)]
    public double SluggingPercentage => (Singles + (2 * Doubles) + (3 * Triples) +
                                         (4 * HomeRuns)) / (double) AtBats;

    [Name("on_base_plus_slugging"), Index(40)]
    public double OnBasePlusSlugging => OnBasePercentage + SluggingPercentage;

    [Name("batting_average"), Index(41)]
    public double BattingAverage => Hits / (double) AtBats;

    [Name("babip"), Index(42)]
    public double BattingAverageOnBallsInPlay =>
        (Hits - HomeRuns) / (double) (AtBats - Strikeouts - HomeRuns + SacrificeFlies);

    [Name("at_bats_per_home_run"), Index(43)]
    public double AtBatsPerHomeRun => AtBats / (double) HomeRuns;

    [Name("strikeout_percentage"), Index(44)]
    public double StrikeoutPercentage => Strikeouts / (double) AtBats;

    [Name("walk_percentage"), Index(45)]
    public double WalkPercentage => Walks / (double) PlateAppearances;

    [Name("extra_base_hit_percentage"), Index(46)]
    public double ExtraBaseHitPercentage => ExtraBaseHits / (double) Hits;
}