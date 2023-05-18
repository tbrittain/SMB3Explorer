using System;
using CsvHelper.Configuration.Attributes;
using SMB3Explorer.Enums;
using SMB3Explorer.Utils;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace SMB3Explorer.Models.Exports;

public class BattingSeasonStatistic
{
    public int AggregatorId { get; set; }

    public int StatsPlayerId { get; set; }

    public Guid? PlayerId { get; set; }

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
    public string? PreviousTeam { get; set; }

    public int PositionNumber { get; set; }

    [Name("Position"), Index(6)]
    // ReSharper disable once UnusedMember.Global
    public string Position => ((BaseballPlayerPosition) PositionNumber).GetEnumDescription();

    public int? SecondaryPositionNumber { get; set; }

    [Name("Secondary Position"), Index(7)]
    // ReSharper disable once UnusedMember.Global
    public string? SecondaryPosition => SecondaryPositionNumber.HasValue
        ? ((BaseballPlayerPosition) SecondaryPositionNumber).GetEnumDescription()
        : null;

    public int PitcherRole { get; set; }

    [Name("Pitcher Role"), Index(8)]
    // ReSharper disable once UnusedMember.Global
    public string PitcherRoleDescription => ((PitcherRole) PitcherRole).GetEnumDescription();
    
    [Name("Age"), Index(9)]
    public int Age { get; set; }

    [Name("Games Batting"), Index(10)]
    public int GamesBatting { get; set; }

    [Name("Games Played"), Index(11)]
    public int GamesPlayed { get; set; }

    [Name("AB"), Index(12)]
    public int AtBats { get; set; }

    [Name("PA"), Index(13)]
    public int PlateAppearances => AtBats + Walks + SacrificeHits + SacrificeFlies + HitByPitch;

    [Name("R"), Index(14)]
    public int Runs { get; set; }

    [Name("H"), Index(15)]
    public int Hits { get; set; }
    
    [Name("BA"), Index(36)]
    public double BattingAverage => Hits / (double) AtBats;
    
    [Name("1B"), Index(17)]
    public int Singles => Hits - Doubles - Triples - HomeRuns;

    [Name("2B"), Index(18)]
    public int Doubles { get; set; }

    [Name("3B"), Index(19)]
    public int Triples { get; set; }

    [Name("HR"), Index(20)]
    public int HomeRuns { get; set; }

    [Name("RBI"), Index(21)]
    public int RunsBattedIn { get; set; }

    [Name("XBH"), Index(22)]
    public int ExtraBaseHits => Doubles + Triples + HomeRuns;

    [Name("TB"), Index(23)]
    public int TotalBases => Singles + (2 * Doubles) + (3 * Triples) + (4 * HomeRuns);

    [Name("SB"), Index(24)]
    public int StolenBases { get; set; }

    [Name("CS"), Index(25)]
    public int CaughtStealing { get; set; }

    [Name("BB"), Index(26)]
    public int Walks { get; set; }

    [Name("K"), Index(27)]
    public int Strikeouts { get; set; }

    [Name("HBP"), Index(28)]
    public int HitByPitch { get; set; }
    
    [Name("OBP"), Index(29)]
    public double OnBasePercentage => (Hits + Walks + HitByPitch) /
                                      (double) (AtBats + Walks + HitByPitch + SacrificeFlies);

    [Name("SLG"), Index(30)]
    public double SluggingPercentage => (Singles + (2 * Doubles) + (3 * Triples) +
                                         (4 * HomeRuns)) / (double) AtBats;

    [Name("OPS"), Index(31)]
    public double OnBasePlusSlugging => OnBasePercentage + SluggingPercentage;
    
    // Caveat with this, the denominator should be subtracting intentional walks, but that data is not available
    [Name("wOBA"), Index(32)]
    public double WeightedOnBaseAverage => ((0.69 * Walks) + (0.72 * HitByPitch) + (0.89 * Singles) + (1.27 * Doubles) +
                                            (1.62 * Triples) + (2.10 * HomeRuns)) / (AtBats + Walks + SacrificeFlies + HitByPitch);

    [Name("ISO"), Index(33)]
    public double IsolatedPower => SluggingPercentage - BattingAverage;
    
    [Name("BABIP"), Index(34)]
    public double BattingAverageOnBallsInPlay =>
        (Hits - HomeRuns) / (double) (AtBats - Strikeouts - HomeRuns + SacrificeFlies);

    [Name("Sac Hits"), Index(35)]
    public int SacrificeHits { get; set; }

    [Name("Sac Flies"), Index(36)]
    public int SacrificeFlies { get; set; }

    [Name("Errors"), Index(37)]
    public int Errors { get; set; }

    [Name("Passed Balls"), Index(38)]
    public int PassedBalls { get; set; }

    [Name("PA/Game"), Index(39)]
    public double PlateAppearancesPerGame => PlateAppearances / (double) GamesPlayed;

    [Name("AB/HR"), Index(40)]
    public double AtBatsPerHomeRun => AtBats / (double) HomeRuns;

    [Name("K%"), Index(41)]
    public double StrikeoutPercentage => Strikeouts / (double) AtBats;

    [Name("BB%"), Index(42)]
    public double WalkPercentage => Walks / (double) PlateAppearances;

    [Name("XBH%"), Index(43)]
    public double ExtraBaseHitPercentage => ExtraBaseHits / (double) Hits;
}
