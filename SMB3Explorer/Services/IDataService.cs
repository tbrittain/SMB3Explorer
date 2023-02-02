using System;
using System.Threading.Tasks;

namespace SMB3Explorer.Services;

public interface IDataService
{
    bool IsConnected { get; }
    Task<(bool, Exception?)> SetupDbConnection(string filePath);

    event EventHandler<EventArgs> ConnectionChanged;
}