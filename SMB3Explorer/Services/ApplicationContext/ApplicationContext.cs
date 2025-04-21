using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SMB3Explorer.Enums;
using SMB3Explorer.Models.Internal;

namespace SMB3Explorer.Services.ApplicationContext;

public sealed class ApplicationContext : IApplicationContext, INotifyPropertyChanged
{
    private LeagueSelection? _selectedFranchise;
    private bool _franchiseSeasonsLoading;
    private FranchiseSeason? _mostRecentFranchiseSeason;

    public LeagueSelection? SelectedLeague
    {
        get => _selectedFranchise;
        set => SetField(ref _selectedFranchise, value);
    }

    public ConcurrentBag<FranchiseSeason> LeagueSeasons { get; } = [];

    public FranchiseSeason? MostRecentLeagueSeason
    {
        get => _mostRecentFranchiseSeason;
        set => SetField(ref _mostRecentFranchiseSeason, value);
    }

    public bool LeagueSeasonsLoading
    {
        get => _franchiseSeasonsLoading;
        set => SetField(ref _franchiseSeasonsLoading, value);
    }

    public SelectedGame SelectedGame { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }
}