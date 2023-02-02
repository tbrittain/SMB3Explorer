using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SMB3Explorer.Services;
using SMB3Explorer.Utils;

namespace SMB3Explorer.ViewModels;

public partial class LandingViewModel : ViewModelBase
{
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;

    public LandingViewModel(IDataService dataService, INavigationService navigationService)
    {
        _navigationService = navigationService;
        _dataService = dataService;

        _dataService.ConnectionChanged += DataServiceOnConnectionChanged;
    }

    private void DataServiceOnConnectionChanged(object? sender, EventArgs e)
    {
        SelectSaveFileCommand?.NotifyCanExecuteChanged();
    }

    private bool CanSelectSaveFile()
    {
        return !_dataService.IsConnected;
    }
    
    [RelayCommand]
    private void NavigateToMain()
    {
        _navigationService.NavigateTo<HomeViewModel>();
    }

    [RelayCommand(CanExecute = nameof(CanSelectSaveFile))]
    private async Task SelectSaveFile()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var filePath = await SaveFile.GetSaveFilePath();
        if (string.IsNullOrEmpty(filePath))
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        var (ok, exception) = await _dataService.EstablishDbConnection(filePath);
        Mouse.OverrideCursor = Cursors.Arrow;

        if (!ok)
        {
            DefaultExceptionHandler.HandleException("Failed to connect to SMB3 database.", exception);
            return;
        }

        MessageBox.Show("Successfully connected to SMB3 database at " +
                        $"{Environment.NewLine}{_dataService.CurrentFilePath}");
        NavigateToMainCommand.Execute(null);
    }

    // protected override void Dispose(bool disposing)
    // {
    //     _dataService.ConnectionChanged -= DataServiceOnConnectionChanged;
    //     base.Dispose(disposing);
    // }
}