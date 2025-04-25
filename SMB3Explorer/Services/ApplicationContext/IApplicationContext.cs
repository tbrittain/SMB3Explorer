using System.Collections.Concurrent;
using System.ComponentModel;
using SMB3Explorer.Enums;
using SMB3Explorer.Models.Internal;

namespace SMB3Explorer.Services.ApplicationContext;

public interface IApplicationContext
{
    LeagueSelection? SelectedLeague { get; set; }
    ConcurrentBag<FranchiseSeason> LeagueSeasons { get; }
    FranchiseSeason? MostRecentLeagueSeason { get; set; }
    bool LeagueSeasonsLoading { get; set; }
    SelectedGame SelectedGame { get; set; }
    event PropertyChangedEventHandler? PropertyChanged;
}