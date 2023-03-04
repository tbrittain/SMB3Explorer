using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Models;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services.DataService;

public partial class DataService
{
    public async IAsyncEnumerable<CareerBattingStatistic> GetFranchiseCareerBattingStatistics(
        bool isRegularSeason = true)
    {
        var command = Connection!.CreateCommand();

        var sqlFile = isRegularSeason ? SqlFile.CareerStatsBatting : SqlFile.PlayoffCareerStatsBatting;

        var commandText = SqlRunner.GetSqlCommand(sqlFile);
        command.CommandText = commandText;

        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var positionPlayer = new CareerBattingStatistic();

            positionPlayer.AggregatorId = int.Parse(reader["aggregatorID"].ToString()!);
            positionPlayer.StatsPlayerId = int.Parse(reader["statsPlayerID"].ToString()!);
            positionPlayer.PlayerId = reader["baseballPlayerGUIDIfKnown"] is not byte[] bytes ? null : bytes.ToGuid();
            positionPlayer.CurrentTeam = reader["currentTeamName"].ToString()!;
            positionPlayer.MostRecentTeam = reader["mostRecentTeamName"].ToString()!;
            positionPlayer.SecondMostRecentTeam = reader["secondMostRecentTeamName"].ToString()!;
            positionPlayer.FirstName = reader["firstName"].ToString()!;
            positionPlayer.LastName = reader["lastName"].ToString()!;

            positionPlayer.RetirementSeason = string.IsNullOrEmpty(reader["retirementSeason"].ToString())
                ? null
                : int.Parse(reader["retirementSeason"].ToString()!);

            positionPlayer.RetirementAge = string.IsNullOrEmpty(reader["age"].ToString())
                ? null
                : int.Parse(reader["age"].ToString()!);

            positionPlayer.PrimaryPositionNumber = int.Parse(reader["primaryPosition"].ToString()!);
            positionPlayer.SecondaryPositionNumber = string.IsNullOrEmpty(reader["secondaryPosition"].ToString())
                ? null
                : int.Parse(reader["secondaryPosition"].ToString()!);

            positionPlayer.PitcherRole = string.IsNullOrEmpty(reader["pitcherRole"].ToString())
                ? null
                : int.Parse(reader["pitcherRole"].ToString()!);

            positionPlayer.GamesPlayed = int.Parse(reader["gamesPlayed"].ToString()!);
            positionPlayer.GamesBatting = int.Parse(reader["gamesBatting"].ToString()!);
            positionPlayer.AtBats = int.Parse(reader["atBats"].ToString()!);
            positionPlayer.Runs = int.Parse(reader["runs"].ToString()!);
            positionPlayer.Hits = int.Parse(reader["hits"].ToString()!);
            positionPlayer.Doubles = int.Parse(reader["doubles"].ToString()!);
            positionPlayer.Triples = int.Parse(reader["triples"].ToString()!);
            positionPlayer.HomeRuns = int.Parse(reader["homeruns"].ToString()!);
            positionPlayer.RunsBattedIn = int.Parse(reader["rbi"].ToString()!);
            positionPlayer.StolenBases = int.Parse(reader["stolenBases"].ToString()!);
            positionPlayer.CaughtStealing = int.Parse(reader["caughtStealing"].ToString()!);
            positionPlayer.Walks = int.Parse(reader["baseOnBalls"].ToString()!);
            positionPlayer.Strikeouts = int.Parse(reader["strikeOuts"].ToString()!);
            positionPlayer.HitByPitch = int.Parse(reader["hitByPitch"].ToString()!);
            positionPlayer.SacrificeHits = int.Parse(reader["sacrificeHits"].ToString()!);
            positionPlayer.SacrificeFlies = int.Parse(reader["sacrificeFlies"].ToString()!);
            positionPlayer.Errors = int.Parse(reader["errors"].ToString()!);
            positionPlayer.PassedBalls = int.Parse(reader["passedBalls"].ToString()!);

            yield return positionPlayer;
        }
    }

    public async IAsyncEnumerable<CareerPitchingStatistic> GetFranchiseCareerPitchingStatistics(
        bool isRegularSeason = true)
    {
        var command = Connection!.CreateCommand();

        var sqlFile = isRegularSeason ? SqlFile.CareerStatsPitching : SqlFile.PlayoffCareerStatsPitching;

        var commandText = SqlRunner.GetSqlCommand(sqlFile);
        command.CommandText = commandText;

        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var pitcher = new CareerPitchingStatistic();

            pitcher.AggregatorId = int.Parse(reader["aggregatorID"].ToString()!);
            pitcher.StatsPlayerId = int.Parse(reader["statsPlayerID"].ToString()!);
            pitcher.PlayerId = reader["baseballPlayerGUIDIfKnown"] is not byte[] bytes ? null : bytes.ToGuid();
            pitcher.CurrentTeam = reader["currentTeamName"].ToString()!;
            pitcher.MostRecentTeam = reader["mostRecentTeamName"].ToString()!;
            pitcher.SecondMostRecentTeam = reader["secondMostRecentTeamName"].ToString()!;
            pitcher.FirstName = reader["firstName"].ToString()!;
            pitcher.LastName = reader["lastName"].ToString()!;

            pitcher.RetirementSeason = string.IsNullOrEmpty(reader["retirementSeason"].ToString())
                ? null
                : int.Parse(reader["retirementSeason"].ToString()!);

            pitcher.RetirementAge = string.IsNullOrEmpty(reader["age"].ToString())
                ? null
                : int.Parse(reader["age"].ToString()!);

            pitcher.PitcherRole = int.Parse(reader["pitcherRole"].ToString()!);
            pitcher.Wins = int.Parse(reader["wins"].ToString()!);
            pitcher.Losses = int.Parse(reader["losses"].ToString()!);
            pitcher.GamesPlayed = int.Parse(reader["games"].ToString()!);
            pitcher.GamesStarted = int.Parse(reader["gamesStarted"].ToString()!);
            pitcher.TotalPitches = int.Parse(reader["totalPitches"].ToString()!);
            pitcher.CompleteGames = int.Parse(reader["completeGames"].ToString()!);
            pitcher.Shutouts = int.Parse(reader["shutouts"].ToString()!);
            pitcher.Saves = int.Parse(reader["saves"].ToString()!);
            pitcher.OutsPitched = int.Parse(reader["outsPitched"].ToString()!);
            pitcher.HitsAllowed = int.Parse(reader["hits"].ToString()!);
            pitcher.EarnedRuns = int.Parse(reader["earnedRuns"].ToString()!);
            pitcher.HomeRunsAllowed = int.Parse(reader["homeRuns"].ToString()!);
            pitcher.WalksAllowed = int.Parse(reader["baseOnBalls"].ToString()!);
            pitcher.Strikeouts = int.Parse(reader["strikeOuts"].ToString()!);
            pitcher.HitByPitch = int.Parse(reader["battersHitByPitch"].ToString()!);
            pitcher.BattersFaced = int.Parse(reader["battersFaced"].ToString()!);
            pitcher.GamesFinished = int.Parse(reader["gamesFinished"].ToString()!);
            pitcher.RunsAllowed = int.Parse(reader["runsAllowed"].ToString()!);
            pitcher.WildPitches = int.Parse(reader["wildPitches"].ToString()!);

            yield return pitcher;
        }
    }
}