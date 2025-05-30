﻿using System.Collections.Frozen;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

// ReSharper disable NotAccessedPositionalProperty.Global

namespace SMB3Explorer.Enums;

public static class PlayerTrait
{
    public record DatabaseTraitSubtypePair
    {
        [JsonConstructor]
        public DatabaseTraitSubtypePair()
        {
            // For JSON deserialization only
        }

        public DatabaseTraitSubtypePair(int traitId, int? subtypeId)
        {
            TraitId = traitId;
            SubtypeId = subtypeId;
        }

        [JsonPropertyName("traitId")]
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public int TraitId { get; init; }

        [JsonPropertyName("subtypeId")]
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public int? SubtypeId { get; init; }
    }

    // ReSharper disable once InconsistentNaming
    private static Dictionary<DatabaseTraitSubtypePair, Trait> _smb3TraitMap { get; } = new()
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

    public static FrozenDictionary<DatabaseTraitSubtypePair, Trait> Smb3TraitMap { get; } =
        _smb3TraitMap.ToFrozenDictionary();

    // ReSharper disable once InconsistentNaming
    private static Dictionary<DatabaseTraitSubtypePair, Trait> _smb4TraitMap { get; } = new()
    {
        {new DatabaseTraitSubtypePair(0, 0), Trait.BatterPowerVsRight},
        {new DatabaseTraitSubtypePair(0, 1), Trait.BatterPowerVsLeft},
        {new DatabaseTraitSubtypePair(1, 0), Trait.BatterContactVsRight},
        {new DatabaseTraitSubtypePair(1, 1), Trait.BatterContactVsLeft},
        {new DatabaseTraitSubtypePair(2, 6), Trait.BatterRbiHero},
        {new DatabaseTraitSubtypePair(2, 7), Trait.BatterRbiZero},
        {new DatabaseTraitSubtypePair(3, 2), Trait.BatterHighPitch},
        {new DatabaseTraitSubtypePair(3, 3), Trait.BatterLowPitch},
        {new DatabaseTraitSubtypePair(3, 4), Trait.BatterInsidePitch},
        {new DatabaseTraitSubtypePair(3, 5), Trait.BatterOutsidePitch},
        {new DatabaseTraitSubtypePair(4, 6), Trait.BatterToughOut},
        {new DatabaseTraitSubtypePair(4, 7), Trait.BatterWhiffer},
        {new DatabaseTraitSubtypePair(5, 12), Trait.PitcherSpecialist},
        {new DatabaseTraitSubtypePair(5, 13), Trait.PitcherReverseSplits},
        {new DatabaseTraitSubtypePair(6, 6), Trait.PitcherComposed},
        {new DatabaseTraitSubtypePair(6, 7), Trait.PitcherWalkProne},
        {new DatabaseTraitSubtypePair(7, 6), Trait.PitcherKCollector},
        {new DatabaseTraitSubtypePair(7, 7), Trait.PitcherKNeglector},
        {new DatabaseTraitSubtypePair(8, 6), Trait.BatterStealer},
        {new DatabaseTraitSubtypePair(8, 7), Trait.BatterBadJumps},
        {new DatabaseTraitSubtypePair(9, 6), Trait.BatterUtility},
        {new DatabaseTraitSubtypePair(10, 8), Trait.BatterFastballHitter},
        {new DatabaseTraitSubtypePair(10, 9), Trait.BatterOffSpeedHitter},
        {new DatabaseTraitSubtypePair(11, 6), Trait.BatterBadBallHitter},
        {new DatabaseTraitSubtypePair(12, 10), Trait.BatterBigHack},
        {new DatabaseTraitSubtypePair(12, 11), Trait.BatterLittleHack},
        {new DatabaseTraitSubtypePair(13, 6), Trait.BatterRallyStarter},
        {new DatabaseTraitSubtypePair(14, 6), Trait.BatterFirstPitchSlayer},
        {new DatabaseTraitSubtypePair(14, 7), Trait.BatterFirstPitchPrayer},
        {new DatabaseTraitSubtypePair(15, 6), Trait.BatterPinchPerfect},
        {new DatabaseTraitSubtypePair(16, 6), Trait.BatterAceExterminator},
        {new DatabaseTraitSubtypePair(17, 6), Trait.BatterMindGamer},
        {new DatabaseTraitSubtypePair(17, 7), Trait.BatterEasyTarget},
        {new DatabaseTraitSubtypePair(18, 6), Trait.PitcherPickOfficer},
        {new DatabaseTraitSubtypePair(18, 7), Trait.PitcherEasyJumps},
        {new DatabaseTraitSubtypePair(19, 6), Trait.PitcherGetsAhead},
        {new DatabaseTraitSubtypePair(19, 7), Trait.PitcherFallsBehind},
        {new DatabaseTraitSubtypePair(20, 6), Trait.PitcherRallyStopper},
        {new DatabaseTraitSubtypePair(20, 7), Trait.PitcherSurrounded},
        {new DatabaseTraitSubtypePair(21, 7), Trait.PitcherCrossedUp},
        {new DatabaseTraitSubtypePair(22, 14), Trait.PitcherElite4SeamFastball},
        {new DatabaseTraitSubtypePair(22, 15), Trait.PitcherElite2SeamFastball},
        {new DatabaseTraitSubtypePair(22, 16), Trait.PitcherEliteCutter},
        {new DatabaseTraitSubtypePair(22, 17), Trait.PitcherEliteCurveball},
        {new DatabaseTraitSubtypePair(22, 18), Trait.PitcherEliteSlider},
        {new DatabaseTraitSubtypePair(22, 19), Trait.PitcherEliteChangeUp},
        {new DatabaseTraitSubtypePair(22, 20), Trait.PitcherEliteScrewball},
        {new DatabaseTraitSubtypePair(22, 21), Trait.PitcherEliteForkball},
        {new DatabaseTraitSubtypePair(23, 6), Trait.PitcherWorkhorse},
        {new DatabaseTraitSubtypePair(24, 22), Trait.PitcherTwoWayOutfielder},
        {new DatabaseTraitSubtypePair(24, 23), Trait.PitcherTwoWayInfielder},
        {new DatabaseTraitSubtypePair(24, 24), Trait.PitcherTwoWayCatcher},
        {new DatabaseTraitSubtypePair(25, 6), Trait.PitcherMetalHead},
        {new DatabaseTraitSubtypePair(26, 6), Trait.BatterSprinter},
        {new DatabaseTraitSubtypePair(26, 7), Trait.BatterSlowPoke},
        {new DatabaseTraitSubtypePair(27, 6), Trait.BatterBaseRounder},
        {new DatabaseTraitSubtypePair(27, 7), Trait.BatterBaseJogger},
        {new DatabaseTraitSubtypePair(28, 6), Trait.BatterDistractor},
        {new DatabaseTraitSubtypePair(29, 6), Trait.MagicHands},
        {new DatabaseTraitSubtypePair(29, 7), Trait.ButterFingers},
        {new DatabaseTraitSubtypePair(30, 7), Trait.WildThrower},
        {new DatabaseTraitSubtypePair(31, 7), Trait.PitcherWildThing},
        {new DatabaseTraitSubtypePair(32, 6), Trait.Clutch},
        {new DatabaseTraitSubtypePair(32, 7), Trait.Choker},
        {new DatabaseTraitSubtypePair(33, 25), Trait.Consistent},
        {new DatabaseTraitSubtypePair(33, 26), Trait.Volatile},
        {new DatabaseTraitSubtypePair(34, 6), Trait.Durable},
        {new DatabaseTraitSubtypePair(34, 7), Trait.InjuryProne},
        {new DatabaseTraitSubtypePair(35, 6), Trait.Stimulated},
        {new DatabaseTraitSubtypePair(36, 6), Trait.BatterCannonArm},
        {new DatabaseTraitSubtypePair(36, 7), Trait.BatterNoodleArm},
        {new DatabaseTraitSubtypePair(37, 6), Trait.BatterDiveWizard},
        {new DatabaseTraitSubtypePair(38, 6), Trait.BatterSignStealer},
        {new DatabaseTraitSubtypePair(39, 7), Trait.PitcherMeltdown},
        {new DatabaseTraitSubtypePair(40, 6), Trait.BatterBunter},
    };

    public static FrozenDictionary<DatabaseTraitSubtypePair, Trait> Smb4TraitMap { get; } =
        _smb4TraitMap.ToFrozenDictionary();
}

public enum Trait
{
    [Description("POW vs RHP")] BatterPowerVsRight,
    [Description("POW vs LHP")] BatterPowerVsLeft,
    [Description("CON vs RHP")] BatterContactVsRight,
    [Description("CON vs LHP")] BatterContactVsLeft,
    [Description("RBI Man")] BatterRbiMan,
    [Description("RBI Dud")] BatterRbiDud,
    [Description("High Pitch")] BatterHighPitch,
    [Description("Low Pitch")] BatterLowPitch,
    [Description("Inside Pitch")] BatterInsidePitch,
    [Description("Outside Pitch")] BatterOutsidePitch,
    [Description("Tough Out")] BatterToughOut,
    [Description("Whiffer")] BatterWhiffer,
    [Description("Stealer")] BatterStealer,
    [Description("Bad Jumps")] BatterBadJumps,
    [Description("Utility")] BatterUtility,
    [Description("Composed")] PitcherComposed,
    [Description("BB Prone")] PitcherWalkProne,
    [Description("Specialist")] PitcherSpecialist,
    [Description("K Man")] PitcherStrikeoutMan,
    [Description("K Dud")] PitcherStrikeoutDud,
    [Description("Big Hack")] BatterBigHack,
    [Description("Magic Hands")] MagicHands,
    [Description("Little Hack")] BatterLittleHack,
    [Description("Elite 4F")] PitcherElite4SeamFastball,
    [Description("K Collector")] PitcherKCollector,
    [Description("Falls Behind")] PitcherFallsBehind,
    [Description("Mind Gamer")] BatterMindGamer,
    [Description("Elite CH")] PitcherEliteChangeUp,
    [Description("Meltdown")] PitcherMeltdown,
    [Description("Gets Ahead")] PitcherGetsAhead,
    [Description("K Neglector")] PitcherKNeglector,
    [Description("Choker")] Choker,
    [Description("Consistent")] Consistent,
    [Description("RBI Zero")] BatterRbiZero,
    [Description("First Pitch Prayer")] BatterFirstPitchPrayer,
    [Description("First Pitch Slayer")] BatterFirstPitchSlayer,
    [Description("Sprinter")] BatterSprinter,
    [Description("Pick Officer")] PitcherPickOfficer,
    [Description("Fastball Hitter")] BatterFastballHitter,
    [Description("Workhorse")] PitcherWorkhorse,
    [Description("Clutch")] Clutch,
    [Description("Elite 2F")] PitcherElite2SeamFastball,
    [Description("Crossed Up")] PitcherCrossedUp,
    [Description("Volatile")] Volatile,
    [Description("Rally Stopper")] PitcherRallyStopper,
    [Description("Butter Fingers")] ButterFingers,
    [Description("Off-speed Hitter")] BatterOffSpeedHitter,
    [Description("Ace Exterminator")] BatterAceExterminator,
    [Description("Metal Head")] PitcherMetalHead,
    [Description("Stimulated")] Stimulated,
    [Description("Reverse Splits")] PitcherReverseSplits,
    [Description("Rally Starter")] BatterRallyStarter,
    [Description("Pinch Perfect")] BatterPinchPerfect,
    [Description("Base Jogger")] BatterBaseJogger,
    [Description("Cannon Arm")] BatterCannonArm,
    [Description("Easy Target")] BatterEasyTarget,
    [Description("Elite CB")] PitcherEliteCurveball,
    [Description("Surrounded")] PitcherSurrounded,
    [Description("Easy Jumps")] PitcherEasyJumps,
    [Description("Slow Poke")] BatterSlowPoke,
    [Description("Wild Thrower")] WildThrower,
    [Description("Sign Stealer")] BatterSignStealer,
    [Description("Noodle Arm")] BatterNoodleArm,
    [Description("Elite CF")] PitcherEliteCutter,
    [Description("Wild Thing")] PitcherWildThing,
    [Description("Elite SL")] PitcherEliteSlider,
    [Description("Dive Wizard")] BatterDiveWizard,
    [Description("Injury Prone")] InjuryProne,
    [Description("Distractor")] BatterDistractor,
    [Description("Durable")] Durable,
    [Description("Bunter")] BatterBunter,
    [Description("Base Rounder")] BatterBaseRounder,
    [Description("Elite FK")] PitcherEliteForkball,
    [Description("Elite SB")] PitcherEliteScrewball,
    [Description("Two Way (IF)")] PitcherTwoWayInfielder,
    [Description("Two Way (OF)")] PitcherTwoWayOutfielder,
    [Description("Two Way (C)")] PitcherTwoWayCatcher,
    [Description("Bad Ball Hitter")] BatterBadBallHitter,
    [Description("RBI Hero")] BatterRbiHero
}