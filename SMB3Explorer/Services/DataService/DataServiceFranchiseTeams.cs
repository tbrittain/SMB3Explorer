using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Models;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services.DataService;

public partial class DataService
{
    public async IAsyncEnumerable<FranchiseSeasonStanding> GetFranchiseSeasonStandings()
    {
        var command = Connection!.CreateCommand();
        var commandText = SqlRunner.GetSqlCommand(SqlFile.FranchiseSeasonStandings);
        command.CommandText = commandText;
        var reader = await command.ExecuteReaderAsync();
        
        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });

        
        while (reader.Read())
        {
            // TODO: Implement this
            yield return new FranchiseSeasonStanding();
        }
    }

    public async IAsyncEnumerable<FranchisePlayoffStanding> GetFranchisePlayoffStandings()
    {
        var command = Connection!.CreateCommand();
        var commandText = SqlRunner.GetSqlCommand(SqlFile.FranchisePlayoffStandings);
        command.CommandText = commandText;
        var reader = await command.ExecuteReaderAsync();
        
        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });


        while (reader.Read())
        {
            // TODO: Implement this
            yield return new FranchisePlayoffStanding();
        }
    }
}