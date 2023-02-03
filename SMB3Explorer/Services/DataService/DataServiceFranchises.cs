using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public Task<List<PositionPlayerStatistic>> GetFranchisePositionPlayers()
    {
        throw new NotImplementedException();
    }
}