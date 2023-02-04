using System;
using SMB3Explorer.Models;

namespace SMB3Explorer.Services;

public interface IApplicationContext
{
    FranchiseSelection? SelectedFranchise { get; set; }
    bool IsFranchiseSelected { get; }
}