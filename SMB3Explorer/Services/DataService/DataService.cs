using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Services.ApplicationContext;
using SMB3Explorer.Services.SystemInteropWrapper;

namespace SMB3Explorer.Services.DataService;

public sealed partial class DataService : INotifyPropertyChanged, IDataService
{
    private readonly IApplicationContext _applicationContext;
    private readonly ISystemIoWrapper _systemIoWrapper;

    private SqliteConnection? _connection;
    private string _currentFilePath = string.Empty;

    public DataService(IApplicationContext applicationContext, ISystemIoWrapper systemIoWrapper)
    {
        _applicationContext = applicationContext;
        _systemIoWrapper = systemIoWrapper;
        
        _applicationContext.PropertyChanged += ApplicationContextOnPropertyChanged;
    }

    private void ApplicationContextOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(IApplicationContext.SelectedFranchise):
            {
                if (_applicationContext.SelectedFranchise is null)
                {
                    _applicationContext.FranchiseSeasons.Clear();
                    _applicationContext.MostRecentFranchiseSeason = null;
                    break;
                }

                Mouse.OverrideCursor = Cursors.Wait;
                _applicationContext.FranchiseSeasonsLoading = true;
                Task.Run(async () =>
                {
                    var seasons = await GetFranchiseSeasons();
                    _applicationContext.FranchiseSeasons.Clear();
                    foreach (var season in seasons)
                    {
                        _applicationContext.FranchiseSeasons.Add(season);
                    }
                    
                    _applicationContext.MostRecentFranchiseSeason = seasons.MaxBy(x => x.SeasonNum);
                    _applicationContext.FranchiseSeasonsLoading = false;
                    
                    Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = Cursors.Arrow);
                });

                break;
            }
        }
    }

    private SqliteConnection? Connection
    {
        get => _connection;
        set
        {
            SetField(ref _connection, value);
            ConnectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsConnected => Connection is not null;

    public event EventHandler<EventArgs>? ConnectionChanged;

    public string CurrentFilePath
    {
        get => _currentFilePath;
        private set => SetField(ref _currentFilePath, value);
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