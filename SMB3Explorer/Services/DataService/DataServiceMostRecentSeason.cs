using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Models.Exports;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services.DataService;

public partial class DataService
{
    public async IAsyncEnumerable<BattingSeasonStatistic> GetMostRecentSeasonTopBattingStatistics(
        bool isRookies = false)
    {
        var command = Connection!.CreateCommand();

        var sqlFile = isRookies ? SqlFile.TopPerformersRookiesBatting : SqlFile.TopPerformersBatting;

        var commandText = SqlRunner.GetSqlCommand(sqlFile);
        command.CommandText = commandText;

        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });

        command.Parameters.Add(new SqliteParameter("@franchiseId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.FranchiseId.ToBlob()
        });

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var positionPlayerStatistic = GetPositionPlayerSeasonStatistic(true, reader);

            yield return positionPlayerStatistic;
        }
    }

    public async IAsyncEnumerable<PitchingSeasonStatistic> GetMostRecentSeasonTopPitchingStatistics(
        bool isRookies = false)
    {
        var command = Connection!.CreateCommand();

        var sqlFile = isRookies ? SqlFile.TopPerformersRookiesPitching : SqlFile.TopPerformersPitching;

        var commandText = SqlRunner.GetSqlCommand(sqlFile);
        command.CommandText = commandText;

        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });

        command.Parameters.Add(new SqliteParameter("@franchiseId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.FranchiseId.ToBlob()
        });

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var pitcherStatistic = GetPitchingSeasonStatistic(true, reader);

            yield return pitcherStatistic;
        }
    }

    public IAsyncEnumerable<SeasonPlayer> GetMostRecentSeasonPlayers()
    {
        throw new System.NotImplementedException();
    }

    public IAsyncEnumerable<SeasonTeam> GetMostRecentSeasonTeams()
    {
        throw new System.NotImplementedException();
    }
}