using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Models.Exports;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services.DataService;

public partial class DataService
{
    public async IAsyncEnumerable<FranchiseSeasonStanding> GetFranchiseSeasonStandings()
    {
        var command = Connection!.CreateCommand();
        var commandText = SqlRunner.GetSqlCommand(SqlFile.FranchiseSeasonStandings);
        command.CommandText = commandText;

        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedLeague!.LeagueId.ToBlob()
        });

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var standing = new FranchiseSeasonStanding();

            standing.Index = int.Parse(reader["index"].ToString()!);
            standing.SeasonId = int.Parse(reader["seasonID"].ToString()!);
            standing.SeasonNum = int.Parse(reader["seasonNum"].ToString()!);
            standing.TeamName = reader["teamName"].ToString()!;
            standing.DivisionName = reader["divisionName"].ToString()!;
            standing.ConferenceName = reader["conferenceName"].ToString()!;
            standing.Wins = int.Parse(reader["gamesWon"].ToString()!);
            standing.Losses = int.Parse(reader["gamesLost"].ToString()!);
            standing.RunsFor = int.Parse(reader["runsFor"].ToString()!);
            standing.RunsAgainst = int.Parse(reader["runsAgainst"].ToString()!);
            standing.RunDifferential = int.Parse(reader["runDifferential"].ToString()!);
            standing.WinPercentage = double.Parse(reader["winPct"].ToString()!);
            standing.GamesBack = double.Parse(reader["gamesBack"].ToString()!);

            yield return standing;
        }
    }

    public async IAsyncEnumerable<FranchisePlayoffStanding> GetFranchisePlayoffStandings()
    {
        var command = Connection!.CreateCommand();
        var commandText = SqlRunner.GetSqlCommand(SqlFile.FranchisePlayoffStandings);
        command.CommandText = commandText;

        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedLeague!.LeagueId.ToBlob()
        });

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var standing = new FranchisePlayoffStanding();

            standing.Index = int.Parse(reader["index"].ToString()!);
            standing.SeasonId = int.Parse(reader["seasonID"].ToString()!);
            standing.SeasonNum = int.Parse(reader["seasonNum"].ToString()!);
            standing.TeamName = reader["teamName"].ToString()!;
            standing.DivisionName = reader["divisionName"].ToString()!;
            standing.ConferenceName = reader["conferenceName"].ToString()!;
            standing.Wins = int.Parse(reader["gamesWon"].ToString()!);
            standing.Losses = int.Parse(reader["gamesLost"].ToString()!);
            standing.RunsFor = int.Parse(reader["runsFor"].ToString()!);
            standing.RunsAgainst = int.Parse(reader["runsAgainst"].ToString()!);
            standing.RunDifferential = int.Parse(reader["runDifferential"].ToString()!);

            yield return standing;
        }
    }
}