using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using SMB3Explorer.ApplicationConfig;
using SMB3Explorer.Services.SystemIoWrapper;
using SMB3Explorer.Utils;

namespace SMB3Explorer.ViewModels;

public partial class LeagueInformationViewModel : ViewModelBase
{
    private readonly ISystemIoWrapper _systemIoWrapper;

    public LeagueInformationViewModel(IApplicationConfig applicationConfig, ISystemIoWrapper systemIoWrapper)
    {
        _systemIoWrapper = systemIoWrapper;
        var configResult = applicationConfig.GetConfigOptions();
        if (configResult.TryPickT1(out var error, out var configOptions))
        {
            MessageBox.Show("Error loading config options: " + error.Value);
            ExistingLeagues = new List<ExistingLeagueViewModel>();
            return;
        }

        ExistingLeagues = configOptions.Leagues
            .Select(x => new ExistingLeagueViewModel
            {
                Name = x.Name,
                Id = x.Id,
                PlayerTeam = x.PlayerTeam,
                NumSeasons = x.NumSeasons,
                NumTimesAccessed = x.NumTimesAccessed,
                FirstAccessed = x.FirstAccessed,
                LastAccessed = x.LastAccessed
            })
            .OrderByDescending(x => x.NumTimesAccessed)
            .ToList();
    }

    [RelayCommand]
    private Task OpenSmb4SaveFileDirectory()
    {
        var directoryResult = SaveFile.GetSmb4SaveFileDirectory(_systemIoWrapper);
        if (directoryResult.TryPickT1(out _, out var directoryPath))
        {
            MessageBox.Show("Could not automatically open save file directory");
            return Task.CompletedTask;
        }

        SafeProcess.Start(directoryPath, _systemIoWrapper);
        return Task.CompletedTask;
    }

    public List<ExistingLeagueViewModel> ExistingLeagues { get; set; }

    public class ExistingLeagueViewModel
    {
        public string Name { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public string SaveFileName => $"league-{Id.ToString().ToUpperInvariant()}.sav";
        public string? PlayerTeam { get; set; }
        public int? NumSeasons { get; set; }
        public int NumTimesAccessed { get; set; }
        public DateTime FirstAccessed { get; set; }
        public DateTime LastAccessed { get; set; }
    }
}