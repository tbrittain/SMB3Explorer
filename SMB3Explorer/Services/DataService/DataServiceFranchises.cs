﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SMB3Explorer.Models.Internal;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services.DataService;

public partial class DataService
{
    public async Task<List<LeagueSelection>> GetLeagues()
    {
        var command = Connection!.CreateCommand();
        var commandText = SqlRunner.GetSqlCommand(SqlFile.Franchises);
        command.CommandText = commandText;
        var reader = await command.ExecuteReaderAsync();

        List<LeagueSelection> franchises = [];
        while (reader.Read())
        {
            var leagueBytes = reader["leagueId"] as byte[] ?? [];
            var leagueId = leagueBytes.ToGuid();

            var franchiseBytes = reader["franchiseId"] as byte[];
            var franchiseId = franchiseBytes?.ToGuid();

            var isEliminationRaw = reader.IsDBNull(8) ? null : (bool?)reader.GetBoolean(8);

            var franchise = new LeagueSelection
            {
                LeagueId = leagueId,
                Mode = LeagueModeExtensions.Parse(franchiseId is not null, isEliminationRaw),
                LeagueName = reader["leagueName"].ToString()!,
                LeagueType = reader["leagueTypeName"].ToString()!,
                PlayerTeamName = reader["playerTeamName"].ToString(),
                NumSeasons = int.Parse(reader["numSeasons"].ToString()!)
            };
            franchises.Add(franchise);
        }

        return franchises;
    }
}