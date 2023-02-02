using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMB3Explorer.Models;
using SMB3Explorer.Utils;

// ReSharper disable once CheckNamespace
namespace SMB3Explorer.Services;

public partial class DataService
{
    public Task<(List<FranchiseSelection>, Exception?)> GetFranchises()
    {
        var command = Connection!.CreateCommand();
        var commandText = SqlRunner.GetSqlCommand(SqlFile.GetFranchises);
        command.CommandText = commandText;
        var reader = command.ExecuteReader();
        
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
        
        return Task.FromResult((franchises, (Exception?)null));
    }
}