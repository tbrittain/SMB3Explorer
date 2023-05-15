using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using SMB3Explorer.Enums;
using SMB3Explorer.Models.Exports;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services.DataService;

public partial class DataService
{
    public async IAsyncEnumerable<BattingMostRecentSeasonStatistic> GetMostRecentSeasonTopBattingStatistics(
        bool isRookies = false)
    {
        var seasonAverageOps = await GetAverageSeasonOps();
        
        var command = Connection!.CreateCommand();

        var sqlFile = isRookies ? SqlFile.TopPerformersRookiesBatting : SqlFile.TopPerformersBatting;

        var commandText = SqlRunner.GetSqlCommand(sqlFile);
        command.CommandText = commandText;

        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });
        
        command.Parameters.Add(new SqliteParameter("@leagueOps", SqliteType.Real)
        {
            Value = seasonAverageOps
        });

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var positionPlayerStatistic = GetPositionPlayerSeasonStatistic(true, reader);

            var mostRecentSeasonStatistic = new BattingMostRecentSeasonStatistic(positionPlayerStatistic);

            var opsPlus = reader["opsPlus"].ToString()!;
            mostRecentSeasonStatistic.OnBasePercentagePlus = string.IsNullOrEmpty(opsPlus) ? 0 : double.Parse(opsPlus);
            
            yield return mostRecentSeasonStatistic;
        }
    }

    public async IAsyncEnumerable<PitchingMostRecentSeasonStatistic> GetMostRecentSeasonTopPitchingStatistics(
        bool isRookies = false)
    {
        var seasonAveragePitcherStats = await GetAverageSeasonPitcherStats();
        
        var command = Connection!.CreateCommand();

        var sqlFile = isRookies ? SqlFile.TopPerformersRookiesPitching : SqlFile.TopPerformersPitching;

        var commandText = SqlRunner.GetSqlCommand(sqlFile);
        command.CommandText = commandText;

        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });
        
        command.Parameters.Add(new SqliteParameter("@leagueEra", SqliteType.Real)
        {
            Value = seasonAveragePitcherStats.Era
        });
        
        command.Parameters.Add(new SqliteParameter("@leagueFip", SqliteType.Real)
        {
            Value = seasonAveragePitcherStats.Fip
        });

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var pitcherStatistic = GetPitchingSeasonStatistic(true, reader);

            var mostRecentSeasonStatistic = new PitchingMostRecentSeasonStatistic(pitcherStatistic);

            var eraMinus = reader["eraMinus"].ToString()!;
            mostRecentSeasonStatistic.EarnedRunsAllowedMinus =
                string.IsNullOrEmpty(eraMinus) ? 0 : double.Parse(eraMinus);

            var fipMinus = reader["fipMinus"].ToString()!;
            mostRecentSeasonStatistic.FieldingIndependentPitchingMinus =
                string.IsNullOrEmpty(fipMinus) ? 0 : double.Parse(fipMinus);
            
            yield return mostRecentSeasonStatistic;
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

    public async IAsyncEnumerable<SeasonTeam> GetMostRecentSeasonTeams()
    {
        var command = Connection!.CreateCommand();
        
        var commandText = SqlRunner.GetSqlCommand(SqlFile.MostRecentSeasonTeams);
        command.CommandText = commandText;
        
        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });
        
        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var seasonTeam = new SeasonTeam();

            seasonTeam.TeamLocalId = reader.GetInt32(0);
            seasonTeam.TeamId = reader.GetGuid(1);
            seasonTeam.TeamName = reader.GetString(2);
            seasonTeam.DivisionName = reader.GetString(4);
            seasonTeam.ConferenceName = reader.GetString(6);
            seasonTeam.SeasonId = reader.GetInt32(7);
            seasonTeam.SeasonNum = reader.GetInt32(8);
            seasonTeam.Budget = reader.GetInt32(9);
            seasonTeam.Payroll = reader.GetInt32(10);
            seasonTeam.Surplus = reader.GetInt32(11);
            seasonTeam.SurplusPerGame = reader.GetInt32(12);
            seasonTeam.Wins = reader.GetInt32(13);
            seasonTeam.Losses = reader.GetInt32(14);
            seasonTeam.GamesBack = reader.GetDouble(15);
            seasonTeam.WinPercentage = reader.GetDouble(16);
            seasonTeam.RunDifferential = reader.GetInt32(17);
            seasonTeam.Power = reader.GetInt32(18);
            seasonTeam.Contact = reader.GetInt32(19);
            seasonTeam.Speed = reader.GetInt32(20);
            seasonTeam.Fielding = reader.GetInt32(21);
            seasonTeam.Arm = reader.GetInt32(22);
            seasonTeam.Velocity = reader.GetInt32(23);
            seasonTeam.Junk = reader.GetInt32(24);
            seasonTeam.Accuracy = reader.GetInt32(25);
            
            yield return seasonTeam;
        }
    }

    public async IAsyncEnumerable<SeasonSchedule> GetMostRecentSeasonSchedule()
    {
        var command = Connection!.CreateCommand();
        
        var commandText = SqlRunner.GetSqlCommand(SqlFile.MostRecentSeasonSchedule);
        command.CommandText = commandText;
        
        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });
        
        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var seasonSchedule = new SeasonSchedule();
            
            seasonSchedule.SeasonId = reader.GetInt32(0);
            seasonSchedule.SeasonNum = reader.GetInt32(1);
            seasonSchedule.GameNum = reader.GetInt32(2);
            seasonSchedule.Day = reader.GetInt32(3);
            seasonSchedule.HomeTeam = reader.GetString(4);
            seasonSchedule.AwayTeam = reader.GetString(5);
            
            yield return seasonSchedule;
        }
    }

    private async Task<double> GetAverageSeasonOps()
    {
        var command = Connection!.CreateCommand();

        var commandText = SqlRunner.GetSqlCommand(SqlFile.SeasonAverageBatterStats);
        command.CommandText = commandText;
        
        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });
        
        var reader = await command.ExecuteReaderAsync();
        reader.Read();
        
        var opsOrdinal = reader.GetDouble(0);
        return opsOrdinal;
    }

    private async Task<AverageSeasonPitcherStats> GetAverageSeasonPitcherStats()
    {
        var command = Connection!.CreateCommand();

        var commandText = SqlRunner.GetSqlCommand(SqlFile.SeasonAveragePitcherStats);
        command.CommandText = commandText;
        
        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });
        
        var reader = await command.ExecuteReaderAsync();
        reader.Read();
        
        var eraOrdinal = reader.GetDouble(0);
        var fipOrdinal = reader.GetDouble(1);
        return new AverageSeasonPitcherStats(eraOrdinal, fipOrdinal);
    }

    private record AverageSeasonPitcherStats(double Era, double Fip);
}