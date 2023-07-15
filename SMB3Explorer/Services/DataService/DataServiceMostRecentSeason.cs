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
        MostRecentSeasonFilter filter)
    {
        var seasonAverageOps = await GetAverageSeasonOps(filter);

        var command = Connection!.CreateCommand();

        // TODO: fix the mappings for the top performers queries
        var sqlFile = filter switch
        {
            MostRecentSeasonFilter.RegularSeason => SqlFile.TopPerformersBatting,
            MostRecentSeasonFilter.Rookies => SqlFile.TopPerformersRookiesBatting,
            MostRecentSeasonFilter.Playoffs => SqlFile.TopPerformersBattingPlayoffs,
            _ => throw new ArgumentOutOfRangeException(nameof(filter), filter, null)
        };

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
            var positionPlayerStatistic = new BattingMostRecentSeasonStatistic();

            positionPlayerStatistic.PlayerId = reader.GetGuid(0);
            positionPlayerStatistic.SeasonId = reader.GetInt32(1);
            positionPlayerStatistic.SeasonNum = reader.GetInt32(2);
            positionPlayerStatistic.FirstName = reader.GetString(3);
            positionPlayerStatistic.LastName = reader.GetString(4);
            positionPlayerStatistic.PositionNumber = reader.GetInt32(5);
            positionPlayerStatistic.PitcherRole = reader.IsDBNull(6) ? null : reader.GetInt32(6);
            positionPlayerStatistic.SecondaryPositionNumber = reader.IsDBNull(7) ? null : reader.GetInt32(7);
            positionPlayerStatistic.GamesBatting = reader.GetInt32(9);
            positionPlayerStatistic.GamesPlayed = reader.GetInt32(10);
            positionPlayerStatistic.AtBats = reader.GetInt32(11);
            positionPlayerStatistic.Runs = reader.GetInt32(12);
            positionPlayerStatistic.Hits = reader.GetInt32(13);
            positionPlayerStatistic.Doubles = reader.GetInt32(14);
            positionPlayerStatistic.Triples = reader.GetInt32(15);
            positionPlayerStatistic.HomeRuns = reader.GetInt32(16);
            positionPlayerStatistic.RunsBattedIn = reader.GetInt32(17);
            positionPlayerStatistic.StolenBases = reader.GetInt32(18);
            positionPlayerStatistic.CaughtStealing = reader.GetInt32(19);
            positionPlayerStatistic.Walks = reader.GetInt32(20);
            positionPlayerStatistic.Strikeouts = reader.GetInt32(21);
            positionPlayerStatistic.HitByPitch = reader.GetInt32(22);
            positionPlayerStatistic.SacrificeHits = reader.GetInt32(23);
            positionPlayerStatistic.SacrificeFlies = reader.GetInt32(24);
            positionPlayerStatistic.Errors = reader.GetInt32(25);
            positionPlayerStatistic.PassedBalls = reader.GetInt32(26);
            positionPlayerStatistic.OnBasePercentagePlus = reader.IsDBNull(27) ? null : reader.GetDouble(27);
            positionPlayerStatistic.TeamName = reader.IsDBNull(29) ? null : reader.GetString(29);
            positionPlayerStatistic.MostRecentTeamName = reader.IsDBNull(30) ? null : reader.GetString(30);
            positionPlayerStatistic.PreviousTeamName = reader.IsDBNull(31) ? null : reader.GetString(31);
            positionPlayerStatistic.Age = reader.GetInt32(32);

            yield return positionPlayerStatistic;
        }
    }

    public async IAsyncEnumerable<PitchingMostRecentSeasonStatistic> GetMostRecentSeasonTopPitchingStatistics(
        MostRecentSeasonFilter filter)
    {
        var seasonAveragePitcherStats = await GetAverageSeasonPitcherStats(filter);

        var command = Connection!.CreateCommand();

        // TODO: fix the mappings for the top performers queries
        var sqlFile = filter switch
        {
            MostRecentSeasonFilter.RegularSeason => SqlFile.TopPerformersPitching,
            MostRecentSeasonFilter.Rookies => SqlFile.TopPerformersRookiesPitching,
            MostRecentSeasonFilter.Playoffs => SqlFile.TopPerformersPitchingPlayoffs,
            _ => throw new ArgumentOutOfRangeException(nameof(filter), filter, null)
        };

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
            var mostRecentSeasonStatistic = new PitchingMostRecentSeasonStatistic();

            mostRecentSeasonStatistic.PlayerId = reader.GetGuid(0);
            mostRecentSeasonStatistic.SeasonId = reader.GetInt32(1);
            mostRecentSeasonStatistic.SeasonNum = reader.GetInt32(2);
            mostRecentSeasonStatistic.FirstName = reader.GetString(3);
            mostRecentSeasonStatistic.LastName = reader.GetString(4);
            mostRecentSeasonStatistic.PositionNumber = reader.GetInt32(5);
            mostRecentSeasonStatistic.PitcherRole = reader.IsDBNull(6) ? null : reader.GetInt32(6);
            mostRecentSeasonStatistic.Wins = reader.GetInt32(8);
            mostRecentSeasonStatistic.Losses = reader.GetInt32(9);
            mostRecentSeasonStatistic.GamesStarted = reader.GetInt32(11);
            mostRecentSeasonStatistic.CompleteGames = reader.GetInt32(12);
            mostRecentSeasonStatistic.TotalPitches = reader.GetInt32(13);
            mostRecentSeasonStatistic.GamesPlayed = reader.GetInt32(14);
            mostRecentSeasonStatistic.Shutouts = reader.GetInt32(15);
            mostRecentSeasonStatistic.Saves = reader.GetInt32(16);
            mostRecentSeasonStatistic.OutsPitched = reader.GetInt32(17);
            mostRecentSeasonStatistic.HitsAllowed = reader.GetInt32(18);
            mostRecentSeasonStatistic.EarnedRuns = reader.GetInt32(19);
            mostRecentSeasonStatistic.HomeRunsAllowed = reader.GetInt32(20);
            mostRecentSeasonStatistic.WalksAllowed = reader.GetInt32(21);
            mostRecentSeasonStatistic.Strikeouts = reader.GetInt32(22);
            mostRecentSeasonStatistic.HitByPitch = reader.GetInt32(23);
            mostRecentSeasonStatistic.BattersFaced = reader.GetInt32(24);
            mostRecentSeasonStatistic.RunsAllowed = reader.GetInt32(25);
            mostRecentSeasonStatistic.WildPitches = reader.GetInt32(27);
            mostRecentSeasonStatistic.EarnedRunsAllowedMinus = reader.GetDouble(28);
            mostRecentSeasonStatistic.FieldingIndependentPitchingMinus = reader.GetDouble(29);
            mostRecentSeasonStatistic.TeamName = reader.IsDBNull(31) ? null : reader.GetString(31);
            mostRecentSeasonStatistic.MostRecentTeamName = reader.IsDBNull(32) ? null : reader.GetString(32);
            mostRecentSeasonStatistic.PreviousTeamName = reader.IsDBNull(33) ? null : reader.GetString(33);
            mostRecentSeasonStatistic.Age = reader.GetInt32(34);

            yield return mostRecentSeasonStatistic;
        }
    }

    public async IAsyncEnumerable<SeasonPlayer> GetMostRecentSeasonPlayers()
    {
        var command = Connection!.CreateCommand();

        var commandText = _applicationContext.SelectedGame switch
        {
            SelectedGame.Smb3 => SqlRunner.GetSqlCommand(SqlFile.MostRecentSeasonPlayersSmb3),
            SelectedGame.Smb4 => SqlRunner.GetSqlCommand(SqlFile.MostRecentSeasonPlayersSmb4),
            _ => throw new ArgumentOutOfRangeException()
        };

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
                var traits =
                    JsonConvert.DeserializeObject<PlayerTrait.DatabaseTraitSubtypePair[]>(traitsSerialized) ??
                    Array.Empty<PlayerTrait.DatabaseTraitSubtypePair>();

                seasonPlayer.Traits = _applicationContext.SelectedGame switch
                {
                    SelectedGame.Smb3 => traits
                        .Select(x => PlayerTrait.Smb3TraitMap[x])
                        .Distinct()
                        .ToArray(),
                    SelectedGame.Smb4 => traits
                        .Select(x => PlayerTrait.Smb4TraitMap[x])
                        .Distinct()
                        .ToArray(),
                    _ => throw new ArgumentException()
                };
            }

            if (_applicationContext.SelectedGame is SelectedGame.Smb4)
            {
                var chemistry = reader.GetString(21);
                seasonPlayer.Chemistry = chemistry;

                seasonPlayer.ThrowHand = reader.GetString(22);
                seasonPlayer.BatHand = reader.GetString(23);

                var pitchesSerialized = reader.IsDBNull(24) ? null : reader.GetString(24);
                if (!string.IsNullOrEmpty(pitchesSerialized))
                {
                    var pitches = JsonConvert.DeserializeObject<DatabaseIntOption[]>(pitchesSerialized) ??
                                  Array.Empty<DatabaseIntOption>();

                    seasonPlayer.Pitches = pitches
                        .Select(x => PitchTypes.Pitches[x])
                        .Where(x => x is not null)
                        .Cast<PitchType>()
                        .Distinct()
                        .ToArray();
                }
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
            seasonTeam.RunsFor = reader.GetInt32(18);
            seasonTeam.RunsAgainst = reader.GetInt32(19);
            seasonTeam.Power = reader.GetInt32(20);
            seasonTeam.Contact = reader.GetInt32(21);
            seasonTeam.Speed = reader.GetInt32(22);
            seasonTeam.Fielding = reader.GetInt32(23);
            seasonTeam.Arm = reader.GetInt32(24);
            seasonTeam.Velocity = reader.GetInt32(25);
            seasonTeam.Junk = reader.GetInt32(26);
            seasonTeam.Accuracy = reader.GetInt32(27);

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
            seasonSchedule.HomeTeam = reader.GetString(5);
            seasonSchedule.HomeTeamId = reader.GetGuid(6);
            seasonSchedule.AwayTeam = reader.GetString(8);
            seasonSchedule.AwayTeamId = reader.GetGuid(9);
            seasonSchedule.HomeRunsScored = reader.IsDBNull(10) ? null : reader.GetInt32(10);
            seasonSchedule.AwayRunsScored = reader.IsDBNull(11) ? null : reader.GetInt32(11);
            seasonSchedule.HomePitcherId = reader.IsDBNull(13) ? null : reader.GetGuid(13);
            seasonSchedule.HomePitcherName = reader.IsDBNull(14) ? null : reader.GetString(14);
            seasonSchedule.AwayPitcherId = reader.IsDBNull(16) ? null : reader.GetGuid(16);
            seasonSchedule.AwayPitcherName = reader.IsDBNull(17) ? null : reader.GetString(17);

            yield return seasonSchedule;
        }
    }

    private async Task<double> GetAverageSeasonOps(MostRecentSeasonFilter filter)
    {
        var command = Connection!.CreateCommand();

        var sqlFile = filter switch
        {
            MostRecentSeasonFilter.RegularSeason => SqlFile.SeasonAverageBatterStats,
            MostRecentSeasonFilter.Rookies => SqlFile.SeasonAverageBatterStats,
            MostRecentSeasonFilter.Playoffs => SqlFile.PlayoffsAverageBatterStats,
            _ => throw new ArgumentOutOfRangeException(nameof(filter), filter, null)
        };

        var commandText = SqlRunner.GetSqlCommand(sqlFile);
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

    private async Task<AverageSeasonPitcherStats> GetAverageSeasonPitcherStats(MostRecentSeasonFilter filter)
    {
        var command = Connection!.CreateCommand();

        var sqlFile = filter switch
        {
            MostRecentSeasonFilter.RegularSeason => SqlFile.SeasonAveragePitcherStats,
            MostRecentSeasonFilter.Rookies => SqlFile.SeasonAveragePitcherStats,
            MostRecentSeasonFilter.Playoffs => SqlFile.PlayoffsAveragePitcherStats,
            _ => throw new ArgumentOutOfRangeException(nameof(filter), filter, null)
        };

        var commandText = SqlRunner.GetSqlCommand(sqlFile);
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