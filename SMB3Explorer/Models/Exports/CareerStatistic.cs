using System;
using CsvHelper.Configuration.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace SMB3Explorer.Models.Exports;

public class CareerStatistic
{
    [Name("aggregator_id"), Index(0)]
    public int AggregatorId { get; set; }
    
    [Name("stats_player_id"), Index(1)]
    public int StatsPlayerId { get; set; }
    
    [Name("player_id"), Index(2)]
    public Guid? PlayerId { get; set; }
    
    [Name("current_team"), Index(3)]
    public string? CurrentTeam { get; set; }
    
    [Name("most_recent_team"), Index(4)]
    public string? MostRecentTeam { get; set; }
    
    [Name("second_most_recent_team"), Index(5)]
    public string? SecondMostRecentTeam { get; set; }
    
    [Name("first_name"), Index(6)]
    public string FirstName { get; set; } = string.Empty;
    
    [Name("last_name"), Index(7)]
    public string LastName { get; set; } = string.Empty;
    
    [Name("retirement_season"), Index(8)]
    public int? RetirementSeason { get; set; }
    
    [Name("retirement_age"), Index(9)]
    public int? RetirementAge { get; set; }
}