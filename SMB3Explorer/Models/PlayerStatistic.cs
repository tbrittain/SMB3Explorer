using System;
using CsvHelper.Configuration.Attributes;
using SMB3Explorer.Enums;

namespace SMB3Explorer.Models;

public abstract class PlayerStatistic
{
    [Name("player_stats_id"), Index(0)]
    public int PlayerStatsId { get; set; }
    
    [Name("player_id"), Index(1)]
    public Guid? PlayerId { get; set; }
    
    [Name("first_name"), Index(2)]
    public string FirstName { get; set; }
    
    [Name("last_name"), Index(3)]
    public string LastName { get; set; }
    
    [Name("primary_position"), Index(4)]
    public int PositionNumber { get; set; }
    
    [Name("primary_position_name"), Index(5)]
    public string Position => ((BaseballPlayerPosition) PositionNumber).ToString();
    
    [Name("current_team_name"), Index(6)]
    public string? CurrentTeam { get; set; }
    
    [Name("previous_team_name"), Index(7)]
    public string? PreviousTeam { get; set; }
}