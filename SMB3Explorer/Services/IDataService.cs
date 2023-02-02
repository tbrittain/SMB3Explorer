using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMB3Explorer.Models;

namespace SMB3Explorer.Services;

public interface IDataService
{
    bool IsConnected { get; }
    Task<(bool, Exception?)> EstablishDbConnection(string filePath);
    Task<(List<FranchiseSelection>, Exception?)> GetFranchises();

    event EventHandler<EventArgs> ConnectionChanged;
    
    public string CurrentFilePath { get; }
}