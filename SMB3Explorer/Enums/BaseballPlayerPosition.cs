﻿using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SMB3Explorer.Enums;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum BaseballPlayerPosition
{
    [Description("")]
    None = 0,
    
    [Description("P")]
    Pitcher = 1,
    
    [Description("C")]
    Catcher = 2,
    
    [Description("1B")]
    FirstBase = 3,
    
    [Description("2B")]
    SecondBase = 4,
    
    [Description("3B")]
    ThirdBase = 5,
    
    [Description("SS")]
    ShortStop = 6,
    
    [Description("LF")]
    LeftField = 7,
    
    [Description("CF")]
    CenterField = 8,
    
    [Description("RF")]
    RightField = 9,
    
    // The following are only used as Secondary Positions
    
    [Description("IF")]
    Infield = 10,
    
    [Description("OF")]
    Outfield = 11,
    
    [Description("1B/OF")]
    FirstBaseAndOutfield = 12,
    
    [Description("IF/OF")]
    InfieldAndOutfield = 13,
}