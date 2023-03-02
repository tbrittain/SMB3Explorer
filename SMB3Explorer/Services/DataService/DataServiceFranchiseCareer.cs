using System.Collections.Generic;
using AutoMapper;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Models;
using SMB3Explorer.Models.Mappings;
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
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CareerBattingStatisticMapper>();
        });
        
        var mapper = new Mapper(config);

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var positionPlayer = mapper.Map<CareerBattingStatistic>(reader);
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