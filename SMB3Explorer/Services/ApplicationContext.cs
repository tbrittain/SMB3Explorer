using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SMB3Explorer.Services;

public sealed class ApplicationContext : IApplicationContext, INotifyPropertyChanged
{
    private Guid? _selectedLeagueId;

    /// <summary>
    /// For right now, the application only supports franchise mode.
    /// </summary>
    public Guid? SelectedLeagueId
    {
        get => _selectedLeagueId;
        set
        {
            SetField(ref _selectedLeagueId, value);
            OnPropertyChanged(nameof(IsLeagueSelected));
        }
    }

    public bool IsLeagueSelected => SelectedLeagueId.HasValue;

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