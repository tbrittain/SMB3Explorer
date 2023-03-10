using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Models;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services.DataService;

public partial class DataService
{
    public async IAsyncEnumerable<BattingSeasonStatistic> GetFranchiseSeasonBattingStatistics(
        bool isRegularSeason = true)
    {
        var command = Connection!.CreateCommand();

        var sqlFile = isRegularSeason ? SqlFile.SeasonStatsBatting : SqlFile.PlayoffStatsBatting;

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
            var positionPlayerStatistic = new BattingSeasonStatistic();

            positionPlayerStatistic.FirstName = reader["firstName"].ToString()!;
            positionPlayerStatistic.LastName = reader["lastName"].ToString()!;

            var position = reader["primaryPosition"].ToString();
            positionPlayerStatistic.PositionNumber = position is null ? 0 : int.Parse(position);

            positionPlayerStatistic.SecondaryPositionNumber =
                string.IsNullOrEmpty(reader["secondaryPosition"].ToString()!)
                    ? null
                    : int.Parse(reader["secondaryPosition"].ToString()!);
            positionPlayerStatistic.CurrentTeam = string.IsNullOrEmpty(reader["currentTeam"].ToString()!)
                ? null
                : reader["currentTeam"].ToString()!;
            positionPlayerStatistic.GamesPlayed = int.Parse(reader["gamesPlayed"].ToString()!);
            positionPlayerStatistic.GamesBatting = int.Parse(reader["gamesBatting"].ToString()!);
            positionPlayerStatistic.AtBats = int.Parse(reader["atBats"].ToString()!);
            positionPlayerStatistic.Runs = int.Parse(reader["runs"].ToString()!);
            positionPlayerStatistic.Hits = int.Parse(reader["hits"].ToString()!);
            positionPlayerStatistic.Doubles = int.Parse(reader["doubles"].ToString()!);
            positionPlayerStatistic.Triples = int.Parse(reader["triples"].ToString()!);
            positionPlayerStatistic.HomeRuns = int.Parse(reader["homeruns"].ToString()!);
            positionPlayerStatistic.RunsBattedIn = int.Parse(reader["rbi"].ToString()!);
            positionPlayerStatistic.StolenBases = int.Parse(reader["stolenBases"].ToString()!);
            positionPlayerStatistic.CaughtStealing = int.Parse(reader["caughtStealing"].ToString()!);
            positionPlayerStatistic.Walks = int.Parse(reader["baseOnBalls"].ToString()!);
            positionPlayerStatistic.Strikeouts = int.Parse(reader["strikeOuts"].ToString()!);
            positionPlayerStatistic.HitByPitch = int.Parse(reader["hitByPitch"].ToString()!);
            positionPlayerStatistic.SacrificeHits = int.Parse(reader["sacrificeHits"].ToString()!);
            positionPlayerStatistic.SacrificeFlies = int.Parse(reader["sacrificeFlies"].ToString()!);
            positionPlayerStatistic.Errors = int.Parse(reader["errors"].ToString()!);
            positionPlayerStatistic.PassedBalls = int.Parse(reader["passedBalls"].ToString()!);
            positionPlayerStatistic.CompletionDate = string.IsNullOrEmpty(reader["completionDate"].ToString()!)
                ? null
                : DateTime.Parse(reader["completionDate"].ToString()!);
            positionPlayerStatistic.SeasonId = int.Parse(reader["seasonId"].ToString()!);
            positionPlayerStatistic.PlayerId = reader["baseballPlayerGUID"] is not byte[] bytes ? null : bytes.ToGuid();

            if (isRegularSeason)
            {
                positionPlayerStatistic.PreviousTeam = string.IsNullOrEmpty(reader["previousTeam"].ToString()!)
                    ? null
                    : reader["previousTeam"].ToString()!;
            }

            yield return positionPlayerStatistic;
        }
    }

    public async IAsyncEnumerable<PitchingSeasonStatistic> GetFranchiseSeasonPitchingStatistics(
        bool isRegularSeason = true)
    {
        var command = Connection!.CreateCommand();

        var sqlFile = isRegularSeason ? SqlFile.SeasonStatsPitching : SqlFile.PlayoffStatsPitching;

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
            var pitcherStatistic = new PitchingSeasonStatistic();

            pitcherStatistic.PlayerId = reader["baseballPlayerGUID"] is not byte[] bytes ? null : bytes.ToGuid();
            pitcherStatistic.FirstName = reader["firstName"].ToString()!;
            pitcherStatistic.LastName = reader["lastName"].ToString()!;
            pitcherStatistic.PositionNumber = int.Parse(reader["primaryPosition"].ToString()!);
            pitcherStatistic.CurrentTeam = string.IsNullOrEmpty(reader["currentTeam"].ToString()!)
                ? null
                : reader["currentTeam"].ToString()!;
            pitcherStatistic.PitcherRole = int.Parse(reader["pitcherRole"].ToString()!);
            pitcherStatistic.GamesPlayed = int.Parse(reader["games"].ToString()!);
            pitcherStatistic.GamesStarted = int.Parse(reader["gamesStarted"].ToString()!);
            pitcherStatistic.Wins = int.Parse(reader["wins"].ToString()!);
            pitcherStatistic.Losses = int.Parse(reader["losses"].ToString()!);
            pitcherStatistic.CompleteGames = int.Parse(reader["completeGames"].ToString()!);
            pitcherStatistic.Shutouts = int.Parse(reader["shutouts"].ToString()!);
            pitcherStatistic.TotalPitches = int.Parse(reader["totalPitches"].ToString()!);
            pitcherStatistic.Saves = int.Parse(reader["saves"].ToString()!);
            pitcherStatistic.OutsPitched = int.Parse(reader["outsPitched"].ToString()!);
            pitcherStatistic.HitsAllowed = int.Parse(reader["hits"].ToString()!);
            pitcherStatistic.EarnedRuns = int.Parse(reader["earnedRuns"].ToString()!);
            pitcherStatistic.HomeRunsAllowed = int.Parse(reader["homeRuns"].ToString()!);
            pitcherStatistic.WalksAllowed = int.Parse(reader["baseOnBalls"].ToString()!);
            pitcherStatistic.Strikeouts = int.Parse(reader["strikeOuts"].ToString()!);
            pitcherStatistic.HitByPitch = int.Parse(reader["battersHitByPitch"].ToString()!);
            pitcherStatistic.BattersFaced = int.Parse(reader["battersFaced"].ToString()!);
            pitcherStatistic.GamesFinished = int.Parse(reader["gamesFinished"].ToString()!);
            pitcherStatistic.RunsAllowed = int.Parse(reader["runsAllowed"].ToString()!);
            pitcherStatistic.WildPitches = int.Parse(reader["wildPitches"].ToString()!);
            pitcherStatistic.CompletionDate = string.IsNullOrEmpty(reader["completionDate"].ToString()!)
                ? null
                : DateTime.Parse(reader["completionDate"].ToString()!);
            pitcherStatistic.SeasonId = int.Parse(reader["seasonId"].ToString()!);

            if (isRegularSeason)
            {
                pitcherStatistic.PreviousTeam = string.IsNullOrEmpty(reader["previousTeam"].ToString()!)
                    ? null
                    : reader["previousTeam"].ToString()!;
            }

            yield return pitcherStatistic;
        }
    }
}