using System;
using CsvHelper.Configuration.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SMB3Explorer.Models.Exports;

public class SeasonTeam
{
    [Name("team_local_id"), Index(0)]
    public int TeamLocalId { get; set; }
    
    [Name("team_id"), Index(1)]
    public Guid TeamId { get; set; }
    
    [Name("team_name"), Index(2)]
    public string TeamName { get; set; } = string.Empty;
    
    [Name("division_name"), Index(3)]
    public string DivisionName { get; set; } = string.Empty;
    
    [Name("conference_name"), Index(4)]
    public string ConferenceName { get; set; } = string.Empty;
    
    [Name("season_id"), Index(5)]
    public int SeasonId { get; set; }
    
    [Name("season_num"), Index(6)]
    public int SeasonNum { get; set; }
    
    [Name("budget"), Index(7)]
    public int Budget { get; set; }
    
    [Name("payroll"), Index(8)]
    public int Payroll { get; set; }
    
    [Name("surplus"), Index(9)]
    public int Surplus { get; set; }
    
    [Name("surplus_per_game"), Index(10)]
    public int SurplusPerGame { get; set; }
    
    [Name("wins"), Index(11)]
    public int Wins { get; set; }
    
    [Name("losses"), Index(12)]
    public int Losses { get; set; }
    
    [Name("run_differential"), Index(13)]
    public int RunDifferential { get; set; }
    
    [Name("win_percentage"), Index(14)]
    public double WinPercentage { get; set; }
    
    [Name("games_back"), Index(15)]
    public double GamesBack { get; set; }
}