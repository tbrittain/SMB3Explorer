using SMB3Explorer.Models.Internal;

namespace SMB3Explorer.Services.ApplicationContext;

public interface IApplicationContext
{
    FranchiseSelection? SelectedFranchise { get; set; }
    bool IsFranchiseSelected { get; }
}