using System;
using System.Windows.Controls;

namespace SMB3Explorer.Services;

public class NavigationService : INavigationService
{
    private readonly Frame _frame;

    public NavigationService(Frame frame)
    {
        _frame = frame;
    }

    public void NavigateToPage(Type pageType, object parameter)
    {
        _frame.NavigationService.Navigate(pageType, parameter);
    }

    public void NavigateBack()
    {
        _frame.GoBack();
    }
}