using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMB3Explorer.Models;
using SMB3Explorer.Services.SystemInteropWrapper;

namespace SMB3Explorer.Services.DataService;

public interface IDataService
{
    bool IsConnected { get; }
    Task<string> DecompressSaveGame(string filePath, ISystemIoWrapper systemIoWrapper);
    Task EstablishDbConnection(string filePath, bool isCompressedSaveGame = true);
    Task<List<FranchiseSelection>> GetFranchises();

    event EventHandler<EventArgs> ConnectionChanged;
    
    public string CurrentFilePath { get; }
    Task Disconnect();
    IAsyncEnumerable<CareerBattingStatistic> GetFranchiseCareerBattingStatistics(bool isRegularSeason = true);
    IAsyncEnumerable<CareerPitchingStatistic> GetFranchiseCareerPitchingStatistics(bool isRegularSeason = true);
    IAsyncEnumerable<BattingSeasonStatistic> GetFranchiseSeasonBattingStatistics(bool isRegularSeason = true);
    IAsyncEnumerable<PitchingSeasonStatistic> GetFranchiseSeasonPitchingStatistics(bool isRegularSeason = true);
}