using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMB3Explorer.Models;

namespace SMB3Explorer.Services;

public interface IDataService
{
    bool IsConnected { get; }
    Task<string> DecompressSaveGame(string filePath);
    Task EstablishDbConnection(string filePath, bool isCompressedSaveGame = true);
    Task<List<FranchiseSelection>> GetFranchises();

    event EventHandler<EventArgs> ConnectionChanged;
    
    public string CurrentFilePath { get; }
    Task Disconnect();
    IAsyncEnumerable<BattingStatistic> GetFranchiseCareerBattingStatistics(bool isRegularSeason = true);
    IAsyncEnumerable<PitchingStatistic> GetFranchiseCareerPitchingStatistics(bool isRegularSeason = true);
}