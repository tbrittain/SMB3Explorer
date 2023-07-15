using System;
using CsvHelper.Configuration.Attributes;

namespace SMB3Explorer.Models.Exports;

public class BattingMostRecentSeasonStatistic : BattingStatistic
{
    [Name("OPS+"), Index(44)]
    public double? OnBasePercentagePlus { get; set; }
    
    [Name("PlayerId"), Index(45)]
    public new Guid PlayerId { get; set; }
    
    [Name("SeasonId"), Index(46)]
    public new int SeasonId { get; set; }

    [Name("TeamId"), Index(47)]
    public Guid? TeamId { get; set; }
    
    [Name("MostRecentTeamId"), Index(48)]
    public Guid? MostRecentTeamId { get; set; }
    
    [Name("PreviousTeamId"), Index(49)]
    public Guid? PreviousTeamId { get; set; }
}