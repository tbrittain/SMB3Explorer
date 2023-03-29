using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using SMB3Explorer.Enums;
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

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var pitcherStatistic = GetPitchingSeasonStatistic(true, reader);

            yield return pitcherStatistic;
        }
    }

    public async IAsyncEnumerable<SeasonPlayer> GetMostRecentSeasonPlayers()
    {
        var command = Connection!.CreateCommand();

        var commandText = SqlRunner.GetSqlCommand(SqlFile.MostRecentSeasonPlayers);
        command.CommandText = commandText;

        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var seasonPlayer = new SeasonPlayer();

            seasonPlayer.PlayerId = reader.GetGuid(0);
            seasonPlayer.SeasonId = reader.GetInt32(1);
            seasonPlayer.SeasonNum = reader.GetInt32(2);
            seasonPlayer.FirstName = reader.GetString(3);
            seasonPlayer.LastName = reader.GetString(4);
            seasonPlayer.PrimaryPositionNumber = reader.GetInt32(5);
            seasonPlayer.PitcherRole = reader.IsDBNull(6) ? null : reader.GetInt32(6);
            seasonPlayer.SecondaryPositionNumber = reader.IsDBNull(7) ? null : reader.GetInt32(7);
            seasonPlayer.CurrentTeam = reader.IsDBNull(8) ? null : reader.GetString(8);
            seasonPlayer.PreviousTeam = reader.IsDBNull(9) ? null : reader.GetString(9);

            seasonPlayer.Power = reader.GetInt32(10);
            seasonPlayer.Contact = reader.GetInt32(11);
            seasonPlayer.Speed = reader.GetInt32(12);
            seasonPlayer.Fielding = reader.GetInt32(13);
            seasonPlayer.Arm = reader.IsDBNull(14) ? null : reader.GetInt32(14);
            seasonPlayer.Velocity = reader.IsDBNull(15) ? null : reader.GetInt32(15);
            seasonPlayer.Junk = reader.IsDBNull(16) ? null : reader.GetInt32(16);
            seasonPlayer.Accuracy = reader.IsDBNull(17) ? null : reader.GetInt32(17);
            seasonPlayer.Age = reader.GetInt32(18);
            seasonPlayer.Salary = reader.GetInt32(19);

            var traitsSerialized = reader.IsDBNull(20) ? null : reader.GetString(20);
            if (!string.IsNullOrEmpty(traitsSerialized))
            {
                var traits = JsonConvert
                    .DeserializeObject<PlayerTrait.DatabaseTraitSubtypePair[]>(traitsSerialized);

                seasonPlayer.Traits = (traits ?? Array.Empty<PlayerTrait.DatabaseTraitSubtypePair>())
                    .Select(_ => PlayerTrait.TraitMap[_])
                    .ToArray();
            }

            yield return seasonPlayer;
        }
    }

    public IAsyncEnumerable<SeasonTeam> GetMostRecentSeasonTeams()
    {
        throw new System.NotImplementedException();
    }
}