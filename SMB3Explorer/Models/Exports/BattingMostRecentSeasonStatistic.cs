using System;
using CsvHelper.Configuration.Attributes;

namespace SMB3Explorer.Models.Exports;

public class BattingMostRecentSeasonStatistic : BattingStatistic
{
    [Ignore]
    public new Guid PlayerId { get; set; }
    
    [Name("OPS+"), Index(44)]
    public double OnBasePercentagePlus { get; set; }
}