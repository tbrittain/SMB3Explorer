using CsvHelper.Configuration.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace SMB3Explorer.Models.Exports;

public class FranchiseSeasonStanding
{
    [Name("Index"), Index(0)]
    public int Index { get; set; }
    
    [Ignore]
    public int SeasonId { get; set; }
    
    [Name("Season"), Index(1)]
    public int SeasonNum { get; set; }
    
    [Name("Team"), Index(2)]
    public string TeamName { get; set; } = string.Empty;
    
    [Name("Division"), Index(3)]
    public string DivisionName { get; set; } = string.Empty;
    
    [Name("Conference"), Index(4)]
    public string ConferenceName { get; set; } = string.Empty;
    
    [Name("W"), Index(5)]
    public int Wins { get; set; }
    
    [Name("L"), Index(6)]
    public int Losses { get; set; }
    
    [Name("Runs For"), Index(7)]
    public int RunsFor { get; set; }
    
    [Name("Runs Against"), Index(8)]
    public int RunsAgainst { get; set; }
    
    [Name("Run Differential"), Index(9)]
    public int RunDifferential { get; set; }
    
    [Name("WPCT"), Index(10)]
    public double WinPercentage { get; set; }
    
    [Name("GB"), Index(11)]
    public double GamesBack { get; set; }
}