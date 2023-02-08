using System;
using CsvHelper.Configuration.Attributes;

namespace SMB3Explorer.Models;

public class BattingSeasonStatistic : BattingStatistic
{
    [Name("season_completion_date"), Index(40)]
    public DateTime CompletionDate { get; set; }
    
    /// <summary>
    /// This is an auto-incrementing integer cross-leagues. It does not necessarily correspond to the
    /// season number relative to a given league.
    /// </summary>
    [Name("season_id"), Index(41)]
    public int SeasonId { get; set; }
}