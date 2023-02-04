using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SMB3Explorer.Enums;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum PitcherRole
{
    [Description("SP")]
    Starter = 1,
    
    [Description("SP/RP")]
    StarterReliever = 2,
    
    [Description("RP")]
    Reliever = 3,
    
    [Description("CL")]
    Closer = 4
}