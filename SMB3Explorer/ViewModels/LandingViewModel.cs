using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using OneOf;
using OneOf.Types;
using Serilog;
using SMB3Explorer.Services.DataService;
using SMB3Explorer.Services.NavigationService;
using SMB3Explorer.Services.SystemInteropWrapper;
using SMB3Explorer.Utils;

namespace SMB3Explorer.ViewModels;

public partial class LandingViewModel : ViewModelBase
{
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
    private readonly ISystemIoWrapper _systemIoWrapper;

    public LandingViewModel(IDataService dataService, INavigationService navigationService, ISystemIoWrapper systemIoWrapper)
    {
        _navigationService = navigationService;
        _systemIoWrapper = systemIoWrapper;
        _dataService = dataService;
        
        Log.Information("Initializing LandingViewModel");

        _dataService.ConnectionChanged += DataServiceOnConnectionChanged;
    }

    private void DataServiceOnConnectionChanged(object? sender, EventArgs e)
    {
        AutomaticallySelectSaveFileCommand.NotifyCanExecuteChanged();
        ManuallySelectSaveFileCommand.NotifyCanExecuteChanged();
        UseExistingDatabaseCommand.NotifyCanExecuteChanged();
    }

    private bool CanSelectSaveFile()
    {
        return !_dataService.IsConnected;
    }

    [RelayCommand(CanExecute = nameof(CanSelectSaveFile))]
    private async Task AutomaticallySelectSaveFile()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var filePathResult =  SaveFile.GetSaveFilePath(_systemIoWrapper);
        if (filePathResult.TryPickT1(out _, out var filePath) || string.IsNullOrEmpty(filePath))
        {
            // TODO: Handle error
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }
        
        var connectionResult = await EstablishDbConnection(filePath);
        HandleDatabaseConnection(connectionResult);
    }

    [RelayCommand(CanExecute = nameof(CanSelectSaveFile))]
    private async Task ManuallySelectSaveFile()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var filePathResult =
            SaveFile.GetUserProvidedFile(Environment.SpecialFolder.MyDocuments.ToString(), _systemIoWrapper);
        if (filePathResult.TryPickT1(out _, out var filePath) || string.IsNullOrEmpty(filePath))
        {
            // TODO: Handle error
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        var connectionResult = await EstablishDbConnection(filePath);
        HandleDatabaseConnection(connectionResult);
    }

    [RelayCommand(CanExecute = nameof(CanSelectSaveFile))]
    private async Task UseExistingDatabase()
    {
        Mouse.OverrideCursor = Cursors.Wait;

        var filePathResult = SaveFile.GetUserProvidedFile(Environment.SpecialFolder.MyDocuments.ToString(),
            _systemIoWrapper,
            "SQLite databases (*.db, *.sqlite)|*.db;*.sqlite");
        if (filePathResult.TryPickT1(out _, out var filePath) || string.IsNullOrEmpty(filePath))
        {
            // TODO: Handle error
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        var connectionResult = await EstablishDbConnection(filePath, false);
        HandleDatabaseConnection(connectionResult);
    }

    private void HandleDatabaseConnection(OneOf<Success, Error> connectionResult)
    {
        if (connectionResult.TryPickT1(out _, out _)) return;

        MessageBox.Show("Successfully connected to SMB3 database at " +
                        $"{Environment.NewLine}{_dataService.CurrentFilePath}");
        _navigationService.NavigateTo<HomeViewModel>();
    }

    private async Task<OneOf<Success, Error>> EstablishDbConnection(string filePath, bool isCompressedSaveGame = true)
    {
        var hasError = false;
        var connectionResult = await _dataService.EstablishDbConnection(filePath, isCompressedSaveGame);
        
        if (connectionResult.TryPickT1(out var error, out _))
        {
            hasError = true;
            DefaultExceptionHandler.HandleException(_systemIoWrapper, "Failed to connect to SMB3 database.",
                new Exception(error.Value));
        }

        Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = Cursors.Arrow);
        
        return !hasError ? new Success() : new Error();
    }

    protected override void Dispose(bool disposing)
    {
        _dataService.ConnectionChanged -= DataServiceOnConnectionChanged;
        base.Dispose(disposing);
    }
}