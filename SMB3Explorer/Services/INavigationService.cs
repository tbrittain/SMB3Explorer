using System;
using SMB3Explorer.ViewModels;

namespace SMB3Explorer.Services;

public interface INavigationService
{
    ViewModelBase CurrentView { get; }
    void NavigateTo<T> () where T : ViewModelBase;
}