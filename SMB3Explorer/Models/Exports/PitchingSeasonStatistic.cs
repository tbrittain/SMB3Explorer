using System;
using CsvHelper.Configuration.Attributes;
using SMB3Explorer.Enums;
using SMB3Explorer.Utils;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace SMB3Explorer.Models.Exports;

public class PitchingSeasonStatistic : PitchingStatistic
{
    public int AggregatorId { get; set; }

    public int StatsPlayerId { get; set; }
}
