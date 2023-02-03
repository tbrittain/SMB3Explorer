using System;
using CsvHelper.Configuration.Attributes;
using SMB3Explorer.Enums;

namespace SMB3Explorer.Models;

public abstract class PlayerStatistic
{
    [Name("baseballPlayerGUIDIfKnown")]
    public Guid Id { get; set; }
    
    [Name("firstName")]
    public string FirstName { get; set; }
    
    [Name("lastName")]
    public string LastName { get; set; }
    
    [Name("primaryPosition")]
    public int PositionNumber { get; set; }
    
    public string Position => ((BaseballPlayerPosition) PositionNumber).ToString();
    
    [Name("teamName")]
    public string CurrentTeam { get; set; }
    
    [Name("previousRecentlyPlayedTeamName")]
    public string PreviousTeam { get; set; }
}