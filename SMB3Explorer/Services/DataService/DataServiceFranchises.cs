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
        var commandText = SqlRunner.GetSqlCommand(SqlFile.GetFranchises);
        command.CommandText = commandText;
        var reader = await command.ExecuteReaderAsync();

        List<FranchiseSelection> franchises = new();
        while (reader.Read())
        {
            var bytes = reader["leagueId"] as byte[] ?? Array.Empty<byte>();
            var leagueId = bytes.FromBlob();

            var franchise = new FranchiseSelection
            {
                LeagueId = leagueId,
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

        var parameter = new SqliteParameter("@param1", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        };

        command.Parameters.Add(parameter);

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var positionPlayer = new BattingStatistic
            {
                PlayerStatsId = int.Parse(reader["statsPlayerID"].ToString()!),
                PlayerId = reader["baseballPlayerGUIDIfKnown"] is not byte[] bytes ? null : bytes.FromBlob(),
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
                GamesPlayed = int.Parse(reader["gamesBatting"].ToString()!),
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

    public IAsyncEnumerable<PitcherStatistic> GetFranchisePitchingStatistics()
    {
        throw new NotImplementedException();
    }
}