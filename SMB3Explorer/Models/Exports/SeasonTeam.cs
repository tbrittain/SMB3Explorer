using System;
using CsvHelper.Configuration.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SMB3Explorer.Models.Exports;

public class SeasonTeam
{
    [Ignore]
    public int TeamLocalId { get; set; }

    [Name("Team"), Index(0)]
    public string TeamName { get; set; } = string.Empty;
    
    [Name("Division"), Index(1)]
    public string DivisionName { get; set; } = string.Empty;
    
    [Name("Conference"), Index(2)]
    public string ConferenceName { get; set; } = string.Empty;
    
    [Ignore]
    public int SeasonId { get; set; }
    
    [Name("Season"), Index(3)]
    public int SeasonNum { get; set; }
    
    [Name("Budget"), Index(4)]
    public int Budget { get; set; }
    
    [Name("Payroll"), Index(5)]
    public int Payroll { get; set; }
    
    [Name("Surplus"), Index(6)]
    public int Surplus { get; set; }
    
    [Name("Surplus/Game"), Index(7)]
    public int SurplusPerGame { get; set; }
    
    [Name("W"), Index(8)]
    public int Wins { get; set; }
    
    [Name("L"), Index(9)]
    public int Losses { get; set; }
    
    [Name("Run Differential"), Index(10)]
    public int RunDifferential { get; set; }

    [Name("Runs For"), Index(11)]
    public int RunsFor { get; set; }
    
    [Name("Runs Against"), Index(12)]
    public int RunsAgainst { get; set; }
    
    [Name("GB"), Index(13)]
    public double GamesBack { get; set; }
    
    [Name("WPCT"), Index(14)]
    public double WinPercentage { get; set; }

    [Name("Pythagorean WPCT"), Index(15)]
    public double PythagoreanWinPercentage => 
        Math.Pow(RunsFor, 1.82) / (Math.Pow(RunsFor, 1.82) + Math.Pow(RunsAgainst, 1.82));
    
    private int GamesPlayed => Wins + Losses;
    
    [Name("Expected W"), Index(16)]
    public int ExpectedWins => (int) Math.Round(GamesPlayed * PythagoreanWinPercentage);
    
    [Name("Expected L"), Index(17)]
    public int ExpectedLosses => (int) Math.Round(GamesPlayed * (1 - PythagoreanWinPercentage));

    [Name("Power"), Index(18)]
    public int Power { get; set; }
    
    [Name("Contact"), Index(19)]
    public int Contact { get; set; }
    
    [Name("Speed"), Index(20)]
    public int Speed { get; set; }
    
    [Name("Fielding"), Index(21)]
    public int Fielding { get; set; }
    
    [Name("Arm"), Index(22)]
    public int Arm { get; set; }
    
    [Name("Velocity"), Index(23)]
    public int Velocity { get; set; }
    
    [Name("Junk"), Index(24)]
    public int Junk { get; set; }
    
    [Name("Accuracy"), Index(25)]
    public int Accuracy { get; set; }

    [Name("TeamId"), Index(26)]
    public Guid TeamId { get; set; }
}