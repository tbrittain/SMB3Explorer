using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Services.ApplicationContext;

namespace SMB3Explorer.Services.DataService;

public sealed partial class DataService : INotifyPropertyChanged, IDataService
{
    private readonly IApplicationContext _applicationContext;

    private SqliteConnection? _connection;
    private string _currentFilePath = string.Empty;

    public DataService(IApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
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