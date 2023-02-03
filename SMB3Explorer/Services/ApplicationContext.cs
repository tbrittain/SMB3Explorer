using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SMB3Explorer.Models;

namespace SMB3Explorer.Services;

public sealed class ApplicationContext : IApplicationContext, INotifyPropertyChanged
{
    private FranchiseSelection? _selectedFranchise;

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