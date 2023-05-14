using CsvHelper.Configuration.Attributes;

namespace SMB3Explorer.Models.Exports;

public class SeasonSchedule
{
    [Name("season_id"), Index(0)]
    public int SeasonId { get; set; }
    
    [Name("season_num"), Index(1)]
    public int SeasonNum { get; set; }
    
    [Name("game_num"), Index(2)]
    public int GameNum { get; set; }
    
    [Name("day"), Index(3)]
    public int Day { get; set; }
    
    [Name("home_team"), Index(4)]
    public string HomeTeam { get; set; } = string.Empty;
    
    [Name("away_team"), Index(5)]
    public string AwayTeam { get; set; } = string.Empty;
}