using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace SMB3Explorer.Utils;

/// <summary>
/// Enum mapping to SQL file embedded resources in the Resources/Sql folder. The SQL files themselves
/// MUST be included in the solution and set to "Embedded Resource" in the properties, otherwise they
/// will not be included in the compiled executable.
/// </summary>
public enum SqlFile
{
    [Description("DatabaseTables.sql")]
    DatabaseTables,
    
    [Description("Franchises.sql")]
    Franchises,
    
    [Description("FranchiseSeasons.sql")]
    FranchiseSeasons,
    
    [Description("CareerStatsBatting.sql")]
    CareerStatsBatting,
    
    [Description("CareerStatsPitching.sql")]
    CareerStatsPitching,
    
    [Description("PlayoffCareerStatsBatting.sql")]
    PlayoffCareerStatsBatting,
    
    [Description("PlayoffCareerStatsPitching.sql")]
    PlayoffCareerStatsPitching,
    
    [Description("PlayoffStatsBatting.sql")]
    PlayoffStatsBatting,
    
    [Description("PlayoffStatsPitching.sql")]
    PlayoffStatsPitching,
    
    [Description("SeasonStatsBatting.sql")]
    SeasonStatsBatting,
    
    [Description("SeasonStatsPitching.sql")]
    SeasonStatsPitching,
    
    [Description("FranchiseSeasonStandings.sql")]
    FranchiseSeasonStandings,
    
    [Description("FranchisePlayoffStandings.sql")]
    FranchisePlayoffStandings,
    
    [Description("TopPerformersPitching.sql")]
    TopPerformersPitching,
    
    [Description("TopPerformersPitchingPlayoffs.sql")]
    TopPerformersPitchingPlayoffs,
    
    [Description("TopPerformersBatting.sql")]
    TopPerformersBatting,
    
    [Description("TopPerformersBattingPlayoffs.sql")]
    TopPerformersBattingPlayoffs,
    
    [Description("TopPerformersRookiesPitching.sql")]
    TopPerformersRookiesPitching,
    
    [Description("TopPerformersRookiesBatting.sql")]
    TopPerformersRookiesBatting,
    
    [Description("MostRecentSeasonPlayersSmb3.sql")]
    MostRecentSeasonPlayersSmb3,
    
    [Description("MostRecentSeasonPlayersSmb4.sql")]
    MostRecentSeasonPlayersSmb4,
    
    [Description("MostRecentSeasonTeams.sql")]
    MostRecentSeasonTeams,
    
    [Description("SeasonAverageBatterStats.sql")]
    SeasonAverageBatterStats,
    
    [Description("PlayoffsAverageBatterStats.sql")]
    PlayoffsAverageBatterStats,

    [Description("SeasonAveragePitcherStats.sql")]
    SeasonAveragePitcherStats,
    
    [Description("PlayoffsAveragePitcherStats.sql")]
    PlayoffsAveragePitcherStats,
    
    [Description("MostRecentSeasonSchedule.sql")]
    MostRecentSeasonSchedule,
    
    [Description("GetLeaguesForSmb4SaveGame.sql")]
    GetLeaguesForSmb4SaveGame,
}

public static class SqlRunner
{
    /// <summary>
    /// Abstracts the process of getting a SQL file from the embedded resources.
    /// </summary>
    /// <param name="file">A SQL file in the embedded resources mapped by the <see cref="SqlFile"/> enum</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static string GetSqlCommand(SqlFile file)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"SMB3Explorer.Resources.Sql.{file.GetEnumDescription()}";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream ?? throw new InvalidOperationException("Invalid resource name"));
        
        var result = reader.ReadToEnd();
        return result;
    }
}