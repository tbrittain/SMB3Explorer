﻿using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Models;
using SMB3Explorer.Utils;

// ReSharper disable once CheckNamespace
namespace SMB3Explorer.Services;

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
            var positionPlayerStatistic = new BattingSeasonStatistic
            {
                PlayerId = reader["baseballPlayerGUID"] is not byte[] bytes ? null : bytes.ToGuid(),
                FirstName = reader["firstName"].ToString()!,
                LastName = reader["lastName"].ToString()!,
                PositionNumber = int.Parse(reader["primaryPosition"].ToString()!),
                SecondaryPositionNumber = int.Parse(reader["secondaryPosition"].ToString()!),
                CurrentTeam = string.IsNullOrEmpty(reader["teamName"].ToString()!)
                    ? null
                    : reader["currentTeam"].ToString()!,
                PreviousTeam = string.IsNullOrEmpty(reader["previousTeam"].ToString()!)
                    ? null
                    : reader["previousTeam"].ToString()!,

                GamesPlayed = int.Parse(reader["gamesPlayed"].ToString()!),
                GamesBatting = int.Parse(reader["gamesBatting"].ToString()!),
                AtBats = int.Parse(reader["atBats"].ToString()!),
                Runs = int.Parse(reader["runs"].ToString()!),
                Hits = int.Parse(reader["hits"].ToString()!),
                Doubles = int.Parse(reader["doubles"].ToString()!),
                Triples = int.Parse(reader["triples"].ToString()!),
                HomeRuns = int.Parse(reader["homeruns"].ToString()!),
                RunsBattedIn = int.Parse(reader["rbi"].ToString()!),
                StolenBases = int.Parse(reader["stolenBases"].ToString()!),
                CaughtStealing = int.Parse(reader["caughtStealing"].ToString()!),
                Walks = int.Parse(reader["baseOnBalls"].ToString()!),
                Strikeouts = int.Parse(reader["strikeOuts"].ToString()!),
                HitByPitch = int.Parse(reader["hitByPitch"].ToString()!),
                SacrificeHits = int.Parse(reader["sacrificeHits"].ToString()!),
                SacrificeFlies = int.Parse(reader["sacrificeFlies"].ToString()!),
                Errors = int.Parse(reader["errors"].ToString()!),
                PassedBalls = int.Parse(reader["passedBalls"].ToString()!),

                CompletionDate = string.IsNullOrEmpty(reader["completionDate"].ToString()!)
                    ? null
                    : DateTime.Parse(reader["completionDate"].ToString()!),
                SeasonId = int.Parse(reader["seasonId"].ToString()!),
            };

            yield return positionPlayerStatistic;
        }
    }

    public async IAsyncEnumerable<PitchingSeasonStatistic> GetFranchiseSeasonPitchingStatistics(bool isRegularSeason = true)
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
            var pitcherStatistic = new PitchingSeasonStatistic
            {
                                PlayerId = reader["baseballPlayerGUID"] is not byte[] bytes ? null : bytes.ToGuid(),
                FirstName = reader["firstName"].ToString()!,
                LastName = reader["lastName"].ToString()!,
                PositionNumber = int.Parse(reader["primaryPosition"].ToString()!),
                CurrentTeam = string.IsNullOrEmpty(reader["teamName"].ToString()!)
                    ? null
                    : reader["teamName"].ToString()!,
                PreviousTeam = string.IsNullOrEmpty(reader["previousRecentlyPlayedTeamName"].ToString()!)
                    ? null
                    : reader["previousRecentlyPlayedTeamName"].ToString()!,
                
                PitcherRole = int.Parse(reader["pitcherRole"].ToString()!),
                
                GamesPlayed = int.Parse(reader["games"].ToString()!),
                GamesStarted = int.Parse(reader["gamesStarted"].ToString()!),
                Wins = int.Parse(reader["wins"].ToString()!),
                Losses = int.Parse(reader["losses"].ToString()!),
                CompleteGames = int.Parse(reader["completeGames"].ToString()!),
                Shutouts = int.Parse(reader["shutouts"].ToString()!),
                TotalPitches = int.Parse(reader["totalPitches"].ToString()!),
                Saves = int.Parse(reader["saves"].ToString()!),
                OutsPitched = int.Parse(reader["outsPitched"].ToString()!),
                HitsAllowed = int.Parse(reader["hits"].ToString()!),
                EarnedRuns = int.Parse(reader["earnedRuns"].ToString()!),
                HomeRunsAllowed = int.Parse(reader["homeRuns"].ToString()!),
                WalksAllowed = int.Parse(reader["baseOnBalls"].ToString()!),
                Strikeouts = int.Parse(reader["strikeOuts"].ToString()!),
                HitByPitch = int.Parse(reader["battersHitByPitch"].ToString()!),
                BattersFaced = int.Parse(reader["battersFaced"].ToString()!),
                GamesFinished = int.Parse(reader["gamesFinished"].ToString()!),
                RunsAllowed = int.Parse(reader["runsAllowed"].ToString()!),
                WildPitches = int.Parse(reader["wildPitches"].ToString()!),
                
                CompletionDate = string.IsNullOrEmpty(reader["completionDate"].ToString()!)
                    ? null
                    : DateTime.Parse(reader["completionDate"].ToString()!),
                SeasonId = int.Parse(reader["seasonId"].ToString()!),
            };
            
            yield return pitcherStatistic;
        }
    }
}