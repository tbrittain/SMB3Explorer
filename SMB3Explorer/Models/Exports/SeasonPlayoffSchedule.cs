using System;
using CsvHelper.Configuration.Attributes;

namespace SMB3Explorer.Models.Exports;

public class SeasonPlayoffSchedule
{
    [Name("Season"), Index(0)]
    public int SeasonNum { get; set; }
    
    [Name("Series"), Index(1)]
    public int SeriesNum { get; set; }

    [Name("Team 1"), Index(2)]
    public string Team1Name { get; set; } = string.Empty;
    
    [Name("Team 1 Seed"), Index(3)]
    public int Team1Seed { get; set; }

    [Name("Team 2"), Index(4)]
    public string Team2Name { get; set; } = string.Empty;
    
    [Name("Team 2 Seed"), Index(5)]
    public int Team2Seed { get; set; }
    
    [Name("Game Number"), Index(6)]
    public int GameNum { get; set; }

    [Name("Home Team"), Index(7)]
    public string HomeTeamName { get; set; } = string.Empty;

    [Name("Away Team"), Index(8)]
    public string AwayTeamName { get; set; } = string.Empty;
    
    [Name("Home Score"), Index(9)]
    public int? HomeRunsScored { get; set; }
    
    [Name("Away Score"), Index(10)]
    public int? AwayRunsScored { get; set; }

    [Name("Home Pitcher"), Index(11)]
    public string? HomePitcherName { get; set; } = string.Empty;

    [Name("Away Pitcher"), Index(12)]
    public string? AwayPitcherName { get; set; } = string.Empty;

    [Name("Team1Id"), Index(13)]
    public Guid Team1Guid { get; set; }
    
    [Name("Team2Id"), Index(14)]
    public Guid Team2Guid { get; set; }
    
    [Name("HomeTeamId"), Index(15)]
    public Guid HomeTeamId { get; set; }
    
    [Name("AwayTeamId"), Index(16)]
    public Guid AwayTeamId { get; set; }
    
    [Name("HomePitcherId"), Index(17)]
    public Guid? HomePitcherId { get; set; }
    
    [Name("AwayPitcherId"), Index(18)]
    public Guid? AwayPitcherId { get; set; }
    
    [Name("SeasonId"), Index(19)]
    public int SeasonId { get; set; }
}