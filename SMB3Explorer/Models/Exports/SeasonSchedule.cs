﻿using CsvHelper.Configuration.Attributes;

namespace SMB3Explorer.Models.Exports;

public class SeasonSchedule
{
    [Ignore]
    public int SeasonId { get; set; }
    
    [Name("Season"), Index(0)]
    public int SeasonNum { get; set; }
    
    [Name("Game Number"), Index(1)]
    public int GameNum { get; set; }
    
    [Name("Day"), Index(2)]
    public int Day { get; set; }
    
    [Name("Home Team"), Index(3)]
    public string HomeTeam { get; set; } = string.Empty;
    
    [Name("Away Team"), Index(4)]
    public string AwayTeam { get; set; } = string.Empty;
    
    [Name("Home Score"), Index(5)]
    public int? HomeRunsScored { get; set; }
    
    [Name("Away Score"), Index(6)]
    public int? AwayRunsScored { get; set; }
    
    [Name("Home Pitcher"), Index(7)]
    public string? HomePitcherName { get; set; } = string.Empty;
    
    [Name("Away Pitcher"), Index(8)]
    public string? AwayPitcherName { get; set; } = string.Empty;
}