using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SMB3Explorer.ViewModels;

namespace SMB3Explorer.Services;

public sealed class NavigationService : INavigationService, INotifyPropertyChanged
{
    private readonly Func<Type, ViewModelBase> _viewModelFactory;

    private ViewModelBase _currentViewModel;

    public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public ViewModelBase CurrentView
    {
        get => _currentViewModel;
        private set => SetField(ref _currentViewModel, value);
    }

    public void NavigateTo<TViewModelBase>() where TViewModelBase : ViewModelBase
    {
        var viewModelBase = _viewModelFactory.Invoke(typeof(TViewModelBase));
        CurrentView = viewModelBase;
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