using System;
using CsvHelper.Configuration.Attributes;

namespace SMB3Explorer.Models.Exports;

public class BattingMostRecentSeasonStatistic : BattingStatistic
{
    [Name("OPS+"), Index(44)]
    public double? OnBasePercentagePlus { get; set; }
    
    [Name("PlayerId"), Index(45)]
    public new Guid PlayerId { get; set; }
}