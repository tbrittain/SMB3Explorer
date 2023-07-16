using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OneOf;
using OneOf.Types;
using SMB3Explorer.Enums;
using SMB3Explorer.Models.Exports;
using SMB3Explorer.Models.Internal;
using SMB3Explorer.Services.SystemIoWrapper;

namespace SMB3Explorer.Services.DataService;

public interface IDataService
{
    bool IsConnected { get; }
    Task<OneOf<string, Error<string>>> DecompressSaveGame(string filePath, ISystemIoWrapper systemIoWrapper);

    Task<OneOf<List<Smb4LeagueSelection>, Error<string>>> EstablishDbConnection(string filePath,
        bool isCompressedSaveGame = true);

    Task<List<FranchiseSelection>> GetFranchises();
    Task<List<FranchiseSeason>> GetFranchiseSeasons();

    event EventHandler<EventArgs> ConnectionChanged;

    public string CurrentFilePath { get; }
    Task Disconnect();
    IAsyncEnumerable<CareerBattingStatistic> GetFranchiseCareerBattingStatistics(bool isRegularSeason = true);
    IAsyncEnumerable<CareerPitchingStatistic> GetFranchiseCareerPitchingStatistics(bool isRegularSeason = true);
    IAsyncEnumerable<BattingSeasonStatistic> GetFranchiseSeasonBattingStatistics(bool isRegularSeason = true);
    IAsyncEnumerable<PitchingSeasonStatistic> GetFranchiseSeasonPitchingStatistics(bool isRegularSeason = true);

    IAsyncEnumerable<FranchiseSeasonStanding> GetFranchiseSeasonStandings();
    IAsyncEnumerable<FranchisePlayoffStanding> GetFranchisePlayoffStandings();

    IAsyncEnumerable<BattingMostRecentSeasonStatistic> GetMostRecentSeasonTopBattingStatistics(
        MostRecentSeasonFilter filter);

    IAsyncEnumerable<PitchingMostRecentSeasonStatistic> GetMostRecentSeasonTopPitchingStatistics(
        MostRecentSeasonFilter filter);

    IAsyncEnumerable<SeasonPlayer> GetMostRecentSeasonPlayers();
    IAsyncEnumerable<SeasonTeam> GetMostRecentSeasonTeams();
    IAsyncEnumerable<SeasonSchedule> GetMostRecentSeasonSchedule();
    IAsyncEnumerable<SeasonPlayoffSchedule> GetMostRecentSeasonPlayoffSchedule();

    Task<bool> DoesMostRecentSeasonPlayoffExist();
}