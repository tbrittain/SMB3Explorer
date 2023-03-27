using System.Collections.Generic;
using System.ComponentModel;

namespace SMB3Explorer.Enums;

public static class PlayerTrait
{
    public record struct DatabaseTraitSubtypePair(int TraitId, int? SubtypeId);

    public static Dictionary<DatabaseTraitSubtypePair, Trait> TraitMap { get; } = new()
    {
        {new DatabaseTraitSubtypePair(0, 0), Trait.BatterPowerVsRight},
        {new DatabaseTraitSubtypePair(0, 1), Trait.BatterPowerVsLeft},
        {new DatabaseTraitSubtypePair(1, 0), Trait.BatterContactVsRight},
        {new DatabaseTraitSubtypePair(1, 1), Trait.BatterContactVsLeft},
        {new DatabaseTraitSubtypePair(2, 6), Trait.BatterRbiMan},
        {new DatabaseTraitSubtypePair(2, 7), Trait.BatterRbiDud},
        {new DatabaseTraitSubtypePair(3, 2), Trait.BatterHighPitch},
        {new DatabaseTraitSubtypePair(3, 3), Trait.BatterLowPitch},
        {new DatabaseTraitSubtypePair(3, 4), Trait.BatterInsidePitch},
        {new DatabaseTraitSubtypePair(3, 5), Trait.BatterOutsidePitch},
        {new DatabaseTraitSubtypePair(4, 6), Trait.BatterToughOut},
        {new DatabaseTraitSubtypePair(4, 7), Trait.BatterWhiffer},
        {new DatabaseTraitSubtypePair(5, null), Trait.PitcherSpecialist},
        {new DatabaseTraitSubtypePair(6, 6), Trait.PitcherComposed},
        {new DatabaseTraitSubtypePair(6, 7), Trait.PitcherWalkProne},
        {new DatabaseTraitSubtypePair(7, 6), Trait.PitcherStrikeoutMan},
        {new DatabaseTraitSubtypePair(7, 7), Trait.PitcherStrikeoutDud},
        {new DatabaseTraitSubtypePair(8, 6), Trait.BatterStealer},
        {new DatabaseTraitSubtypePair(8, 7), Trait.BatterBadJumps},
        {new DatabaseTraitSubtypePair(9, null), Trait.BatterUtility},
    };
}

public enum Trait
{
    [Description("POW vs RHP")]
    BatterPowerVsRight,
    
    [Description("POW vs LHP")]
    BatterPowerVsLeft,
    
    [Description("CON vs RHP")]
    BatterContactVsRight,
    
    [Description("CON vs LHP")]
    BatterContactVsLeft,
    
    [Description("RBI Man")]
    BatterRbiMan,
    
    [Description("RBI Dud")]
    BatterRbiDud,
    
    [Description("High Pitch")]
    BatterHighPitch,
    
    [Description("Low Pitch")]
    BatterLowPitch,
    
    [Description("Inside Pitch")]
    BatterInsidePitch,
    
    [Description("Outside Pitch")]
    BatterOutsidePitch,
    
    [Description("Tough Out")]
    BatterToughOut,
    
    [Description("Whiffer")]
    BatterWhiffer,
    
    [Description("Stealer")]
    BatterStealer,
    
    [Description("Bad Jumps")]
    BatterBadJumps,
    
    [Description("Utility")]
    BatterUtility,
    
    [Description("Composed")]
    PitcherComposed,
    
    [Description("BB Prone")]
    PitcherWalkProne,
    
    [Description("Specialist")]
    PitcherSpecialist,
    
    [Description("K Man")]
    PitcherStrikeoutMan,
    
    [Description("K Dud")]
    PitcherStrikeoutDud,
}