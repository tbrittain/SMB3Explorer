using System;
using CsvHelper.Configuration.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace SMB3Explorer.Models.Exports;

public class CareerStatistic
{
    [Ignore]
    public int AggregatorId { get; set; }
    
    [Ignore]
    public int StatsPlayerId { get; set; }
    
    [Ignore]
    public Guid? PlayerId { get; set; }
    
    [Name("First Name"), Index(0)]
    public string FirstName { get; set; } = string.Empty;
    
    [Name("Last Name"), Index(1)]
    public string LastName { get; set; } = string.Empty;
    
    [Name("Team"), Index(2)]
    public string? CurrentTeam { get; set; }
    
    [Name("Prev Team"), Index(3)]
    public string? MostRecentTeam { get; set; }
    
    [Name("2nd Prev Team"), Index(4)]
    public string? SecondMostRecentTeam { get; set; }

    [Name("Retirement Season"), Index(5)]
    public int? RetirementSeason { get; set; }
    
    [Name("Age"), Index(6)]
    public int Age { get; set; }
}