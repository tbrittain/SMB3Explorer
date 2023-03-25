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
    [Name("player_id"), Index(0)]
    public Guid? PlayerId { get; set; }

    [Name("first_name"), Index(1)]
    public string FirstName { get; set; } = string.Empty;

    [Name("last_name"), Index(2)]
    public string LastName { get; set; } = string.Empty;

    [Name("current_team_name"), Index(3)]
    public string? CurrentTeam { get; set; }

    [Name("previous_team_name"), Index(4)]
    public string? PreviousTeam { get; set; }

    [Name("primary_position"), Index(5)]
    public int PositionNumber { get; set; }

    [Name("primary_position_name"), Index(6)]
    // ReSharper disable once UnusedMember.Global
    public string Position => ((BaseballPlayerPosition) PositionNumber).GetEnumDescription();
    
    [Name("secondary_position"), Index(7)]
    public int? SecondaryPositionNumber { get; set; }

    [Name("secondary_position_name"), Index(8)]
    // ReSharper disable once UnusedMember.Global
    public string? SecondaryPosition => SecondaryPositionNumber.HasValue
        ? ((BaseballPlayerPosition) SecondaryPositionNumber).GetEnumDescription()
        : null;

    [Name("games_batting"), Index(9)]
    public int GamesBatting { get; set; }

    [Name("games_played"), Index(10)]
    public int GamesPlayed { get; set; }

    [Name("at_bats"), Index(11)]
    public int AtBats { get; set; }

    [Name("plate_appearances"), Index(12)]
    public int PlateAppearances => AtBats + Walks + SacrificeHits + SacrificeFlies + HitByPitch;

    [Name("runs"), Index(13)]
    public int Runs { get; set; }

    [Name("hits"), Index(14)]
    public int Hits { get; set; }
    
    private int Singles => Hits - Doubles - Triples - HomeRuns;

    [Name("doubles"), Index(15)]
    public int Doubles { get; set; }

    [Name("triples"), Index(16)]
    public int Triples { get; set; }

    [Name("home_runs"), Index(17)]
    public int HomeRuns { get; set; }

    [Name("rbi"), Index(18)]
    public int RunsBattedIn { get; set; }

    [Name("extra_base_hits"), Index(19)]
    public int ExtraBaseHits => Doubles + Triples + HomeRuns;

    [Name("total_bases"), Index(20)]
    public int TotalBases => Hits + 2 * Doubles + 3 * Triples + 4 * HomeRuns;

    [Name("stolen_bases"), Index(21)]
    public int StolenBases { get; set; }

    [Name("caught_stealing"), Index(22)]
    public int CaughtStealing { get; set; }

    [Name("walks"), Index(23)]
    public int Walks { get; set; }

    [Name("strikeouts"), Index(24)]
    public int Strikeouts { get; set; }

    [Name("hit_by_pitch"), Index(25)]
    public int HitByPitch { get; set; }

    [Name("sacrifice_hits"), Index(26)]
    public int SacrificeHits { get; set; }

    [Name("sacrifice_flies"), Index(27)]
    public int SacrificeFlies { get; set; }

    [Name("errors"), Index(28)]
    public int Errors { get; set; }

    [Name("passed_balls"), Index(29)]
    public int PassedBalls { get; set; }

    [Name("plate_appearances_per_game"), Index(30)]
    public double PlateAppearancesPerGame => PlateAppearances / (double) GamesPlayed;

    [Name("on_base_percentage"), Index(31)]
    public double OnBasePercentage => (Hits + Walks + HitByPitch) /
                                     (double) (AtBats + Walks + HitByPitch + SacrificeFlies);

    [Name("slugging_percentage"), Index(32)]
    public double SluggingPercentage => (Singles + (2 * Doubles) + (3 * Triples) +
                                         (4 * HomeRuns)) / (double) AtBats;

    [Name("on_base_plus_slugging"), Index(33)]
    public double OnBasePlusSlugging => OnBasePercentage + SluggingPercentage;

    [Name("batting_average"), Index(34)]
    public double BattingAverage => Hits / (double) AtBats;

    [Name("babip"), Index(35)]
    public double BattingAverageOnBallsInPlay =>
        (Hits - HomeRuns) / (double) (AtBats - Strikeouts - HomeRuns + SacrificeFlies);

    [Name("at_bats_per_home_run"), Index(36)]
    public double AtBatsPerHomeRun => AtBats / (double) HomeRuns;

    [Name("strikeout_percentage"), Index(37)]
    public double StrikeoutPercentage => Strikeouts / (double) AtBats;

    [Name("walk_percentage"), Index(38)]
    public double WalkPercentage => Walks / (double) PlateAppearances;

    [Name("extra_base_hit_percentage"), Index(39)]
    public double ExtraBaseHitPercentage => ExtraBaseHits / (double) Hits;
    
    [Name("season_completion_date"), Index(40)]
    public DateTime? CompletionDate { get; set; }

    [Name("season_id"), Index(41)]
    public int SeasonId { get; set; }
    
    [Name("season_num"), Index(42)]
    public int SeasonNum { get; set; }
}