using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMB3Explorer.Models.Internal;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services.DataService;

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
}