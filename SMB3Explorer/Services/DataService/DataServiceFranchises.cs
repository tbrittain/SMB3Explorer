using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Models;
using SMB3Explorer.Utils;

// ReSharper disable once CheckNamespace
namespace SMB3Explorer.Services;

public partial class DataService
{
    public async Task<List<FranchiseSelection>> GetFranchises()
    {
        var command = Connection!.CreateCommand();
        var commandText = SqlRunner.GetSqlCommand(SqlFile.Franchises);
        command.CommandText = commandText;
        var reader = await command.ExecuteReaderAsync();

        List<FranchiseSelection> franchises = new();
        while (reader.Read())
        {
            var leagueBytes = reader["leagueId"] as byte[] ?? Array.Empty<byte>();
            var leagueId = leagueBytes.ToGuid();
            
            var franchiseBytes = reader["franchiseId"] as byte[] ?? Array.Empty<byte>();
            var franchiseId = franchiseBytes.ToGuid();

            var franchise = new FranchiseSelection
            {
                LeagueId = leagueId,
                FranchiseId = franchiseId,
                LeagueName = reader["leagueName"].ToString()!,
                LeagueType = reader["leagueTypeName"].ToString()!,
                PlayerTeamName = reader["playerTeamName"].ToString()!,
                NumSeasons = int.Parse(reader["numSeasons"].ToString()!)
            };
            franchises.Add(franchise);
        }

        return franchises;
    }

    public async IAsyncEnumerable<BattingStatistic> GetFranchiseCareerBattingStatistics(bool isRegularSeason = true)
    {
        var command = Connection!.CreateCommand();
        
        var sqlFile = isRegularSeason ? SqlFile.CareerStatsBatting : SqlFile.PlayoffCareerStatsBatting;
        
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
            var positionPlayer = new BattingStatistic
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
            };

            yield return positionPlayer;
        }
    }

    public async IAsyncEnumerable<PitchingStatistic> GetFranchiseCareerPitchingStatistics(bool isRegularSeason = true)
    {
        var command = Connection!.CreateCommand();
        
        var sqlFile = isRegularSeason ? SqlFile.CareerStatsPitching : SqlFile.PlayoffCareerStatsPitching;
        
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
            var pitcher = new PitchingStatistic
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
            };

            yield return pitcher;
        }
    }
}