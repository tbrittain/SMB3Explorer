using System;
using CsvHelper.Configuration.Attributes;
using SMB3Explorer.Enums;
using SMB3Explorer.Utils;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace SMB3Explorer.Models.Exports;

public class PitchingSeasonStatistic
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
    
    [Name("pitcher_role"), Index(7)]
    public int PitcherRole { get; set; }

    [Name("pitcher_role_name"), Index(8)]
    // ReSharper disable once UnusedMember.Global
    public string PitcherRoleDescription => ((PitcherRole) PitcherRole).GetEnumDescription();

    [Name("games_played"), Index(9)]
    public int GamesPlayed { get; set; }

    [Name("games_started"), Index(10)]
    public int GamesStarted { get; set; }

    [Name("wins"), Index(11)]
    public int Wins { get; set; }

    [Name("losses"), Index(12)]
    public int Losses { get; set; }

    [Name("complete_games"), Index(13)]
    public int CompleteGames { get; set; }

    [Name("shutouts"), Index(14)]
    public int Shutouts { get; set; }

    [Name("total_pitches"), Index(15)]
    public int TotalPitches { get; set; }

    [Name("saves"), Index(16)]
    public int Saves { get; set; }

    [Name("outs_pitched"), Index(17)]
    public int OutsPitched { get; set; }

    [Name("hits_allowed"), Index(18)]
    public int HitsAllowed { get; set; }
    
    [Name("earned_runs"), Index(19)]
    public int EarnedRuns { get; set; }

    [Name("home_runs_allowed"), Index(20)]
    public int HomeRunsAllowed { get; set; }

    [Name("walks_allowed"), Index(21)]
    public int WalksAllowed { get; set; }

    [Name("strikeouts"), Index(22)]
    public int Strikeouts { get; set; }

    [Name("hit_by_pitch"), Index(23)]
    public int HitByPitch { get; set; }

    [Name("batters_faced"), Index(24)]
    public int BattersFaced { get; set; }

    [Name("games_finished"), Index(25)]
    public int GamesFinished { get; set; }

    [Name("runs_allowed"), Index(26)]
    public int RunsAllowed { get; set; }

    [Name("wild_pitches"), Index(27)]
    public int WildPitches { get; set; }

    [Name("innings_pitched"), Index(30)]
    public double InningsPitched => OutsPitched / 3.0;

    [Name("era"), Index(31)]
    public double EarnedRunAverage => EarnedRuns / InningsPitched;

    [Name("batting_average_against"), Index(32)]
    public double BattingAverageAgainst => HitsAllowed / (double) BattersFaced;

    [Name("whip"), Index(33)]
    public double WalksAndHitsPerInning => (WalksAllowed + HitsAllowed) / InningsPitched;
    
    [Name("win_percentage"), Index(34)]
    public double WinPercentage => Wins / (double) (Wins + Losses);
    
    [Name("opponent_on_base_percentage"), Index(35)]
    public double OpponentOnBasePercentage => (HitsAllowed + WalksAllowed + HitByPitch) / (double) BattersFaced;
    
    [Name("strikeout_to_walk_ratio"), Index(36)]
    public double StrikeoutToWalkRatio => Strikeouts / (double) WalksAllowed;
    
    [Name("strikeouts_per_nine_innings"), Index(37)]
    public double StrikeoutsPerNineInnings => Strikeouts / (InningsPitched / 9.0);
    
    [Name("walks_per_nine_innings"), Index(38)]
    public double WalksPerNineInnings => WalksAllowed / (InningsPitched / 9.0);
    
    [Name("hits_per_nine_innings"), Index(39)]
    public double HitsPerNineInnings => HitsAllowed / (InningsPitched / 9.0);
    
    [Name("home_runs_per_nine_innings"), Index(40)]
    public double HomeRunsPerNineInnings => HomeRunsAllowed / (InningsPitched / 9.0);
    
    [Name("pitches_per_inning"), Index(41)]
    public double PitchesPerInning => TotalPitches / InningsPitched;
    
    [Name("pitches_per_game"), Index(42)]
    public double PitchesPerGame => TotalPitches / (double) GamesPlayed;
    
    [Name("season_completion_date"), Index(43)]
    public DateTime? CompletionDate { get; set; }

    [Name("season_id"), Index(44)]
    public int SeasonId { get; set; }
    
    [Name("season_num"), Index(45)]
    public int SeasonNum { get; set; }
    
    [Name("age"), Index(46)]
    public int Age { get; set; }
}