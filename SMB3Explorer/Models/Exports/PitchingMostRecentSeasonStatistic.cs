using System;
using CsvHelper.Configuration.Attributes;
using SMB3Explorer.Enums;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Models.Exports;

public class PitchingMostRecentSeasonStatistic : PitchingStatistic
{
    [Ignore]
    public new Guid PlayerId { get; set; }

    [Name("ERA-"), Index(40)]
    public double EarnedRunsAllowedMinus { get; set; }

    [Name("FIP-"), Index(41)]
    public double FieldingIndependentPitchingMinus { get; set; }
}