using System;
using System.Threading.Tasks;

namespace SMB3Explorer.Services;

public interface IDataService
{
    bool IsConnected { get; }
    Task<(bool, Exception?)> EstablishDbConnection(string filePath);

    event EventHandler<EventArgs> ConnectionChanged;
    
    public string CurrentFilePath { get; }
}