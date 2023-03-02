using System.Collections.Generic;
using AutoMapper;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Models;
using SMB3Explorer.Models.Mappings;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services.DataService;

public partial class DataService
{
    public async IAsyncEnumerable<CareerBattingStatistic> GetFranchiseCareerBattingStatistics(
        bool isRegularSeason = true)
    {
        var command = Connection!.CreateCommand();

        var sqlFile = isRegularSeason ? SqlFile.CareerStatsBatting : SqlFile.PlayoffCareerStatsBatting;

        var commandText = SqlRunner.GetSqlCommand(sqlFile);
        command.CommandText = commandText;

        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CareerBattingStatisticMapper>();
        });
        
        var mapper = new Mapper(config);

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var positionPlayer = mapper.Map<CareerBattingStatistic>(reader);
            yield return positionPlayer;
        }
    }

    public async IAsyncEnumerable<CareerPitchingStatistic> GetFranchiseCareerPitchingStatistics(
        bool isRegularSeason = true)
    {
        var command = Connection!.CreateCommand();

        var sqlFile = isRegularSeason ? SqlFile.CareerStatsPitching : SqlFile.PlayoffCareerStatsPitching;

        var commandText = SqlRunner.GetSqlCommand(sqlFile);
        command.CommandText = commandText;

        command.Parameters.Add(new SqliteParameter("@leagueId", SqliteType.Blob)
        {
            Value = _applicationContext.SelectedFranchise!.LeagueId.ToBlob()
        });
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CareerPitchingStatisticMapper>();
        });
        
        var mapper = new Mapper(config);

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var pitcher = mapper.Map<CareerPitchingStatistic>(reader);
            yield return pitcher;
        }
    }
}