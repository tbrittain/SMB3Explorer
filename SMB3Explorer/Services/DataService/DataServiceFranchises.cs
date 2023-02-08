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

    public async IAsyncEnumerable<BattingStatistic> GetFranchiseBattingStatistics()
    {
        var command = Connection!.CreateCommand();
        var commandText = SqlRunner.GetSqlCommand(SqlFile.GetAllFranchiseBatters);
        command.CommandText = commandText;

        command.Parameters.Add(new SqliteParameter("@param1", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });
        
        command.Parameters.Add(new SqliteParameter("@param2", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.FranchiseId.ToBlob()
        });

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var positionPlayer = new BattingStatistic
            {
                PlayerStatsId = int.Parse(reader["statsPlayerID"].ToString()!),
                PlayerId = reader["baseballPlayerGUIDIfKnown"] is not byte[] bytes ? null : bytes.ToGuid(),
                FirstName = reader["firstName"].ToString()!,
                LastName = reader["lastName"].ToString()!,
                PositionNumber = int.Parse(reader["primaryPosition"].ToString()!),
                CurrentTeam = string.IsNullOrEmpty(reader["teamName"].ToString()!)
                    ? null
                    : reader["teamName"].ToString()!,
                PreviousTeam = string.IsNullOrEmpty(reader["previousRecentlyPlayedTeamName"].ToString()!)
                    ? null
                    : reader["previousRecentlyPlayedTeamName"].ToString()!,

                // Counting stats
                GamesBatting = int.Parse(reader["gamesBatting"].ToString()!),
                AtBats = int.Parse(reader["atBats"].ToString()!),
                PlateAppearances = int.Parse(reader["plateAppearances"].ToString()!),
                Runs = int.Parse(reader["runs"].ToString()!),
                Hits = int.Parse(reader["hits"].ToString()!),
                Doubles = int.Parse(reader["doubles"].ToString()!),
                Triples = int.Parse(reader["triples"].ToString()!),
                HomeRuns = int.Parse(reader["homeRuns"].ToString()!),
                ExtraBaseHits = int.Parse(reader["extraBaseHits"].ToString()!),
                RunsBattedIn = int.Parse(reader["rbi"].ToString()!),
                TotalBases = int.Parse(reader["totalBases"].ToString()!),
                StolenBases = int.Parse(reader["stolenBases"].ToString()!),
                CaughtStealing = int.Parse(reader["caughtStealing"].ToString()!),
                Walks = int.Parse(reader["baseOnBalls"].ToString()!),
                Strikeouts = int.Parse(reader["strikeouts"].ToString()!),
                HitByPitch = int.Parse(reader["hitByPitch"].ToString()!),
                SacrificeHits = int.Parse(reader["sacrificeHits"].ToString()!),
                SacrificeFlies = int.Parse(reader["sacrificeFlies"].ToString()!),
                Errors = int.Parse(reader["errors"].ToString()!),
                PassedBalls = int.Parse(reader["passedBalls"].ToString()!),

                // Rate stats
                PlateAppearancesPerGame = SafeParseDouble.Parse(reader["plateAppearancesPerGame"].ToString()!),
                OnBasePercentage = SafeParseDouble.Parse(reader["onBasePct"].ToString()!),
                SluggingPercentage = SafeParseDouble.Parse(reader["sluggingPct"].ToString()!),
                OnBasePlusSlugging = SafeParseDouble.Parse(reader["onBasePlusSlugging"].ToString()!),
                BattingAverage = SafeParseDouble.Parse(reader["battingAverage"].ToString()!),
                BattingAverageOnBallsInPlay = SafeParseDouble.Parse(reader["babip"].ToString()!),
                AtBatsPerHomeRun = SafeParseDouble.Parse(reader["atBatsPerHomeRun"].ToString()!),
                StrikeoutPercentage = SafeParseDouble.Parse(reader["strikeoutPct"].ToString()!),
                WalkPercentage = SafeParseDouble.Parse(reader["baseOnBallsPct"].ToString()!),
                ExtraBaseHitPercentage = SafeParseDouble.Parse(reader["extraBaseHitsPct"].ToString()!),
            };

            yield return positionPlayer;
        }
    }

    public async IAsyncEnumerable<PitchingStatistic> GetFranchisePitchingStatistics()
    {
        var command = Connection!.CreateCommand();
        var commandText = SqlRunner.GetSqlCommand(SqlFile.GetAllFranchisePitchers);
        command.CommandText = commandText;
        
        command.Parameters.Add(new SqliteParameter("@param1", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });
        
        command.Parameters.Add(new SqliteParameter("@param2", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.FranchiseId.ToBlob()
        });

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var pitcher = new PitchingStatistic
            {
                PlayerStatsId = int.Parse(reader["statsPlayerID"].ToString()!),
                PlayerId = reader["baseballPlayerGUIDIfKnown"] is not byte[] bytes ? null : bytes.ToGuid(),
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

                // Counting stats
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
                InningsPitched = SafeParseDouble.Parse(reader["inningsPitched"].ToString()!),
                
                // Rate stats
                EarnedRunAverage = SafeParseDouble.Parse(reader["ERA"].ToString()!),
                Whip = SafeParseDouble.Parse(reader["WHIP"].ToString()!),
                OpponentBattingAverage = SafeParseDouble.Parse(reader["opponent_AVG"].ToString()!),
                WinningPercentage = SafeParseDouble.Parse(reader["winPct"].ToString()!),
                OpponentOnBasePercentage = SafeParseDouble.Parse(reader["opponent_OBP"].ToString()!),
                StrikeoutsPerWalk = SafeParseDouble.Parse(reader["strikeOutsPerWalk"].ToString()!),
                StrikeoutsPerNineInnings = SafeParseDouble.Parse(reader["strikeOuts_9"].ToString()!),
                WalksPerNineInnings = SafeParseDouble.Parse(reader["baseOnBalls_9"].ToString()!),
                HitsPerNineInnings = SafeParseDouble.Parse(reader["hits_9"].ToString()!),
                PitchesPerInning = SafeParseDouble.Parse(reader["pitchesPerInningPitched"].ToString()!),
                InningsPerGame = SafeParseDouble.Parse(reader["inningsPitchedPerGame"].ToString()!),
            };

            yield return pitcher;
        }
    }
}