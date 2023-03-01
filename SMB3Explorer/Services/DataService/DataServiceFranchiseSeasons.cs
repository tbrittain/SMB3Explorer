using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Models;
using SMB3Explorer.Models.Mappings;
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
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BattingSeasonStatisticMapper>();
        });

        var mapper = new Mapper(config);

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var positionPlayerStatistic = mapper.Map<BattingSeasonStatistic>(reader);
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