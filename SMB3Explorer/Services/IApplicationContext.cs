using System;

namespace SMB3Explorer.Services;

public interface IApplicationContext
{
    Guid? SelectedFranchiseId { get; set; }
    bool IsFranchiseSelected { get; }
}