using CsvHelper.Configuration.Attributes;

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
}