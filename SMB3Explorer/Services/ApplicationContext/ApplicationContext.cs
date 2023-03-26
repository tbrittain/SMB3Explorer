using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using SMB3Explorer.Models.Internal;

namespace SMB3Explorer.Services.ApplicationContext;

public sealed class ApplicationContext : IApplicationContext, INotifyPropertyChanged
{
    private FranchiseSelection? _selectedFranchise;
    private bool _franchiseSeasonsLoading;

    /// <summary>
    /// For right now, the application only supports franchise mode.
    /// </summary>
    public FranchiseSelection? SelectedFranchise
    {
        get => _selectedFranchise;
        set
        {
            SetField(ref _selectedFranchise, value);
            OnPropertyChanged(nameof(IsFranchiseSelected));
        }
    }

    public bool IsFranchiseSelected => SelectedFranchise is not null;

    public ConcurrentBag<FranchiseSeason> FranchiseSeasons { get; } = new();

    public FranchiseSeason? MostRecentFranchiseSeason => !FranchiseSeasons.IsEmpty 
        ? FranchiseSeasons.OrderByDescending(_ => _.SeasonNum).Single()
        : null;

    public bool FranchiseSeasonsLoading
    {
        get => _franchiseSeasonsLoading;
        set => SetField(ref _franchiseSeasonsLoading, value);
    }

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