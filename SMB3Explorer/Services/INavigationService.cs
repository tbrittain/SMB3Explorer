using System;

namespace SMB3Explorer.Services;

public interface INavigationService
{
    void NavigateToPage(Type pageType, object parameter);
    void NavigateBack();
}