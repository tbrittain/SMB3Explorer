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
        AutomaticallySelectSaveFileCommand?.NotifyCanExecuteChanged();
        ManuallySelectSaveFileCommand?.NotifyCanExecuteChanged();
        UseExistingDatabaseCommand?.NotifyCanExecuteChanged();
    }

    private bool CanSelectSaveFile()
    {
        return !_dataService.IsConnected;
    }

    [RelayCommand(CanExecute = nameof(CanSelectSaveFile))]
    private async Task AutomaticallySelectSaveFile()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var filePath = await SaveFile.GetSaveFilePath();
        if (string.IsNullOrEmpty(filePath))
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        var hasError = await EstablishDbConnection(filePath);
        HandleDatabaseConnection(hasError);
    }

    [RelayCommand(CanExecute = nameof(CanSelectSaveFile))]
    private async Task ManuallySelectSaveFile()
    {
        Mouse.OverrideCursor = Cursors.Wait;
        var filePath = await SaveFile.GetUserProvidedFile(Environment.SpecialFolder.MyDocuments.ToString());
        if (string.IsNullOrEmpty(filePath))
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        var hasError = await EstablishDbConnection(filePath);
        HandleDatabaseConnection(hasError);
    }

    [RelayCommand(CanExecute = nameof(CanSelectSaveFile))]
    private async Task UseExistingDatabase()
    {
        Mouse.OverrideCursor = Cursors.Wait;
        var filePath = await SaveFile.GetUserProvidedFile(Environment.SpecialFolder.MyDocuments.ToString(),
            "SQLite databases (*.db, *.sqlite)|*.db;*.sqlite");
        
        if (string.IsNullOrEmpty(filePath))
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }
        
        var hasError = await EstablishDbConnection(filePath, false);
        HandleDatabaseConnection(hasError);
    }

    private void HandleDatabaseConnection(bool hasError)
    {
        if (hasError) return;

        MessageBox.Show("Successfully connected to SMB3 database at " +
                        $"{Environment.NewLine}{_dataService.CurrentFilePath}");
        _navigationService.NavigateTo<HomeViewModel>();
    }

    private async Task<bool> EstablishDbConnection(string filePath, bool isCompressedSaveGame = true)
    {
        var hasError = false;
        await _dataService.EstablishDbConnection(filePath, isCompressedSaveGame)
            .ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    hasError = true;
                    DefaultExceptionHandler.HandleException("Failed to connect to SMB3 database.", task.Exception);
                }

                Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = Cursors.Arrow);
            });
        return hasError;
    }

    protected override void Dispose(bool disposing)
    {
        _dataService.ConnectionChanged -= DataServiceOnConnectionChanged;
        base.Dispose(disposing);
    }
}