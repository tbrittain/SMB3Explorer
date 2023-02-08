using System.Collections.Generic;
using SMB3Explorer.Models;

// ReSharper disable once CheckNamespace
namespace SMB3Explorer.Services;

public partial class DataService
{
    public IAsyncEnumerable<BattingSeasonStatistic> GetFranchiseSeasonBattingStatistics(bool isRegularSeason = true)
    {
        throw new System.NotImplementedException();
    }

    public IAsyncEnumerable<PitchingSeasonStatistic> GetFranchiseSeasonPitchingStatistics(bool isRegularSeason = true)
    {
        throw new System.NotImplementedException();
    }
}