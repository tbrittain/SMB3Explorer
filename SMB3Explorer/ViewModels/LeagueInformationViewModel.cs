using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SMB3Explorer.ApplicationConfig;
using SMB3Explorer.Models.Internal;

namespace SMB3Explorer.ViewModels;

public class LeagueInformationViewModel : ViewModelBase
{
    public LeagueInformationViewModel(IApplicationConfig applicationConfig)
    {
        var configResult = applicationConfig.GetConfigOptions();
        if (configResult.TryPickT1(out var error, out var configOptions))
        {
            MessageBox.Show("Error loading config options: " + error.Value);
            ExistingLeagues = new List<Smb4LeagueSelection>();
            return;
        }

        ExistingLeagues = configOptions.Leagues
            .Select(x => new Smb4LeagueSelection(x.Name, x.Id, x.PlayerTeam, x.NumSeasons)
            {
                NumTimesAccessed = x.NumTimesAccessed,
                FirstAccessed = x.FirstAccessed,
                LastAccessed = x.LastAccessed
            })
            .OrderByDescending(x => x.NumTimesAccessed)
            .ToList();
    }

    public List<Smb4LeagueSelection> ExistingLeagues { get; set; }
}