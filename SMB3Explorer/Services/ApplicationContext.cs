using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SMB3Explorer.Services;

public sealed class ApplicationContext : IApplicationContext, INotifyPropertyChanged
{
    private Guid _selectedFranchiseId;

    /// <summary>
    /// For right now, the application only supports franchise mode.
    /// </summary>
    public Guid SelectedFranchiseId
    {
        get => _selectedFranchiseId;
        set => SetField(ref _selectedFranchiseId, value);
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