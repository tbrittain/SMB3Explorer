using CsvHelper.Configuration.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace SMB3Explorer.Models;

public class FranchiseSeasonStanding
{
    [Name("index"), Index(0)]
    public int Index { get; set; }
    
    [Name("season_id"), Index(1)]
    public int SeasonId { get; set; }
    
    [Name("season_num"), Index(2)]
    public int SeasonNum { get; set; }
    
    [Name("team_name"), Index(3)]
    public string TeamName { get; set; } = string.Empty;
    
    [Name("division_name"), Index(4)]
    public string DivisionName { get; set; } = string.Empty;
    
    [Name("conference_name"), Index(5)]
    public string ConferenceName { get; set; } = string.Empty;
    
    [Name("wins"), Index(6)]
    public int Wins { get; set; }
    
    [Name("losses"), Index(7)]
    public int Losses { get; set; }
    
    [Name("runs_for"), Index(8)]
    public int RunsFor { get; set; }
    
    [Name("runs_against"), Index(9)]
    public int RunsAgainst { get; set; }
    
    [Name("run_differential"), Index(10)]
    public int RunDifferential { get; set; }
    
    [Name("win_percentage"), Index(11)]
    public double WinPercentage { get; set; }
    
    [Name("games_back"), Index(12)]
    public int GamesBack { get; set; }
}