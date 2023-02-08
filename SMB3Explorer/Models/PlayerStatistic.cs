using System;
using CsvHelper.Configuration.Attributes;
using SMB3Explorer.Enums;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Models;

public abstract class PlayerStatistic
{
    [Name("player_id"), Index(0)]
    public Guid? PlayerId { get; set; }

    [Name("first_name"), Index(1)]
    public string FirstName { get; set; } = string.Empty;

    [Name("last_name"), Index(2)]
    public string LastName { get; set; } = string.Empty;
    
    [Name("current_team_name"), Index(3)]
    public string? CurrentTeam { get; set; }
    
    [Name("previous_team_name"), Index(4)]
    public string? PreviousTeam { get; set; }
    
    [Name("primary_position"), Index(5)]
    public int PositionNumber { get; set; }
    
    [Name("primary_position_name"), Index(6)]
    // ReSharper disable once UnusedMember.Global
    public string Position => ((BaseballPlayerPosition) PositionNumber).GetEnumDescription();
}