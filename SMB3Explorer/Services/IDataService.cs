using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMB3Explorer.Models;

namespace SMB3Explorer.Services;

public interface IDataService
{
    bool IsConnected { get; }
    Task EstablishDbConnection(string filePath);
    Task<List<FranchiseSelection>> GetFranchises();

    event EventHandler<EventArgs> ConnectionChanged;
    
    public string CurrentFilePath { get; }
    Task Disconnect();
    IAsyncEnumerable<BattingStatistic> GetFranchiseBattingStatistics();
    IAsyncEnumerable<PitcherStatistic> GetFranchisePitchingStatistics();
}