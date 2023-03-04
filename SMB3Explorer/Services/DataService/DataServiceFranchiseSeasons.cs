using System.Collections.Generic;
using AutoMapper;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Models;
using SMB3Explorer.Models.Mappings;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Services.DataService;

public partial class DataService
{
    public async IAsyncEnumerable<BattingSeasonStatistic> GetFranchiseSeasonBattingStatistics(
        bool isRegularSeason = true)
    {
        var command = Connection!.CreateCommand();

        var sqlFile = isRegularSeason ? SqlFile.SeasonStatsBatting : SqlFile.PlayoffStatsBatting;

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
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BattingSeasonStatisticMapper>();
        });

        var mapper = new Mapper(config);

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var positionPlayerStatistic = mapper.Map<BattingSeasonStatistic>(reader);
            yield return positionPlayerStatistic;
        }
    }

    public async IAsyncEnumerable<PitchingSeasonStatistic> GetFranchiseSeasonPitchingStatistics(
        bool isRegularSeason = true)
    {
        var command = Connection!.CreateCommand();

        var sqlFile = isRegularSeason ? SqlFile.SeasonStatsPitching : SqlFile.PlayoffStatsPitching;

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
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PitchingSeasonStatisticMapper>();
        });
        
        var mapper = new Mapper(config);

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var pitcherStatistic = mapper.Map<PitchingSeasonStatistic>(reader);
            yield return pitcherStatistic;
        }
    }
}