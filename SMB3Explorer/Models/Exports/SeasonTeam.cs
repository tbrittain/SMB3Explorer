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
    
    [Name("power"), Index(16)]
    public int Power { get; set; }
    
    [Name("contact"), Index(17)]
    public int Contact { get; set; }
    
    [Name("speed"), Index(18)]
    public int Speed { get; set; }
    
    [Name("fielding"), Index(19)]
    public int Fielding { get; set; }
    
    [Name("arm"), Index(20)]
    public int Arm { get; set; }
    
    [Name("velocity"), Index(21)]
    public int Velocity { get; set; }
    
    [Name("junk"), Index(22)]
    public int Junk { get; set; }
    
    [Name("accuracy"), Index(23)]
    public int Accuracy { get; set; }
}