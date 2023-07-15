using System;
using CsvHelper.Configuration.Attributes;

namespace SMB3Explorer.Models.Exports;

public class PitchingMostRecentSeasonStatistic : PitchingStatistic
{
    [Name("ERA-"), Index(40)]
    public double EarnedRunsAllowedMinus { get; set; }

    [Name("FIP-"), Index(41)]
    public double FieldingIndependentPitchingMinus { get; set; }
    
    [Name("PlayerId"), Index(42)]
    public new Guid PlayerId { get; set; }
    
    [Name("SeasonId"), Index(43)]
    public new int SeasonId { get; set; }

    [Name("TeamId"), Index(44)]
    public Guid? TeamId { get; set; }
    
    [Name("MostRecentTeamId"), Index(45)]
    public Guid? MostRecentTeamId { get; set; }
    
    [Name("PreviousTeamId"), Index(46)]
    public Guid? PreviousTeamId { get; set; }
}