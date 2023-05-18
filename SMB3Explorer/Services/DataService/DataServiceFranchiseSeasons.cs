using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Models.Exports;
using SMB3Explorer.Models.Internal;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services.DataService;

public partial class DataService
{
    public async Task<List<FranchiseSeason>> GetFranchiseSeasons()
    {
        var command = Connection!.CreateCommand();
        var commandText = SqlRunner.GetSqlCommand(SqlFile.FranchiseSeasons);

        command.CommandText = commandText;

        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });

        var reader = await command.ExecuteReaderAsync();

        List<FranchiseSeason> seasons = new();
        while (reader.Read())
        {
            var seasonId = int.Parse(reader["seasonId"].ToString()!);
            var seasonNum = int.Parse(reader["seasonNum"].ToString()!);

            var seasonBytes = reader["leagueGUID"] as byte[] ?? Array.Empty<byte>();
            var seasonGuid = seasonBytes.ToGuid();

            var season = new FranchiseSeason(seasonId, seasonNum, seasonGuid);
            seasons.Add(season);
        }

        return seasons;
    }

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

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var positionPlayerStatistic = GetPositionPlayerSeasonStatistic(isRegularSeason, reader);

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

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var pitcherStatistic = GetPitchingSeasonStatistic(isRegularSeason, reader);

            yield return pitcherStatistic;
        }
    }

    private static PitchingSeasonStatistic GetPitchingSeasonStatistic(bool isRegularSeason, IDataRecord reader)
    {
        var pitcherStatistic = new PitchingSeasonStatistic();

        pitcherStatistic.AggregatorId = int.Parse(reader["aggregatorID"].ToString()!);
        pitcherStatistic.StatsPlayerId = int.Parse(reader["statsPlayerID"].ToString()!);
        pitcherStatistic.PlayerId = reader["baseballPlayerGUID"] is not byte[] bytes ? null : bytes.ToGuid();
        
        pitcherStatistic.SeasonId = int.Parse(reader["seasonId"].ToString()!);
        pitcherStatistic.SeasonNum = int.Parse(reader["seasonNum"].ToString()!);
        pitcherStatistic.Age = int.Parse(reader["age"].ToString()!);
        
        pitcherStatistic.FirstName = reader["firstName"].ToString()!;
        pitcherStatistic.LastName = reader["lastName"].ToString()!;
        pitcherStatistic.PositionNumber = int.Parse(reader["primaryPosition"].ToString()!);
        
        pitcherStatistic.TeamName = string.IsNullOrEmpty(reader["teamName"].ToString()!)
            ? null
            : reader["teamName"].ToString()!;
        pitcherStatistic.MostRecentTeamName = string.IsNullOrEmpty(reader["mostRecentlyPlayedTeamName"].ToString()!)
            ? null
            : reader["mostRecentlyPlayedTeamName"].ToString()!;
        pitcherStatistic.PreviousTeam = string.IsNullOrEmpty(reader["previousRecentlyPlayedTeamName"].ToString()!)
            ? null
            : reader["previousRecentlyPlayedTeamName"].ToString()!;
        
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

        return pitcherStatistic;
    }

    private static BattingSeasonStatistic GetPositionPlayerSeasonStatistic(bool isRegularSeason, IDataRecord reader)
    {
        var positionPlayerStatistic = new BattingSeasonStatistic();
        
        positionPlayerStatistic.AggregatorId = int.Parse(reader["aggregatorID"].ToString()!);
        positionPlayerStatistic.StatsPlayerId = int.Parse(reader["statsPlayerID"].ToString()!);
        positionPlayerStatistic.PlayerId = reader["baseballPlayerGUID"] is not byte[] bytes ? null : bytes.ToGuid();
        
        positionPlayerStatistic.SeasonId = int.Parse(reader["seasonId"].ToString()!);
        positionPlayerStatistic.SeasonNum = int.Parse(reader["seasonNum"].ToString()!);
        positionPlayerStatistic.Age = int.Parse(reader["age"].ToString()!);

        positionPlayerStatistic.FirstName = reader["firstName"].ToString()!;
        positionPlayerStatistic.LastName = reader["lastName"].ToString()!;

        var position = reader["primaryPosition"].ToString();
        positionPlayerStatistic.PositionNumber = position is null ? 0 : int.Parse(position);

        positionPlayerStatistic.SecondaryPositionNumber =
            string.IsNullOrEmpty(reader["secondaryPosition"].ToString()!)
                ? null
                : int.Parse(reader["secondaryPosition"].ToString()!);
        positionPlayerStatistic.TeamName = string.IsNullOrEmpty(reader["teamName"].ToString()!)
            ? null
            : reader["teamName"].ToString()!;
        positionPlayerStatistic.MostRecentTeamName = string.IsNullOrEmpty(reader["mostRecentlyPlayedTeamName"].ToString()!)
            ? null
            : reader["mostRecentlyPlayedTeamName"].ToString()!;
        positionPlayerStatistic.PreviousTeam = string.IsNullOrEmpty(reader["previousRecentlyPlayedTeamName"].ToString()!)
            ? null
            : reader["previousRecentlyPlayedTeamName"].ToString()!;
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

        return positionPlayerStatistic;
    }
}