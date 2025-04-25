using System.Collections.Frozen;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

// ReSharper disable NotAccessedPositionalProperty.Global

namespace SMB3Explorer.Enums;

public record DatabaseIntOption
{
    [JsonConstructor]
    public DatabaseIntOption()
    {
        // For JSON deserialization only
    }

    public DatabaseIntOption(int optionKey, int optionValue)
    {
        OptionKey = optionKey;
        OptionValue = optionValue;
    }

    [JsonPropertyName("optionKey")]
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public int OptionKey { get; init; }

    [JsonPropertyName("optionValue")]
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public int OptionValue { get; init; }

    override public string ToString() => $"{OptionKey}:{OptionValue}";
}

public class PitchTypes
{
    // ReSharper disable once InconsistentNaming
    private static readonly Dictionary<DatabaseIntOption, PitchType?> _pitches = new()
    {
        {new DatabaseIntOption(58, 0), null},
        {new DatabaseIntOption(58, 1), PitchType.FourSeamFastball},

        {new DatabaseIntOption(59, 0), null},
        {new DatabaseIntOption(59, 1), PitchType.TwoSeamFastball},

        {new DatabaseIntOption(60, 0), null},
        {new DatabaseIntOption(60, 1), PitchType.Screwball},

        {new DatabaseIntOption(61, 0), null},
        {new DatabaseIntOption(61, 1), PitchType.Changeup},

        {new DatabaseIntOption(62, 0), null},
        {new DatabaseIntOption(62, 1), PitchType.Forkball},

        {new DatabaseIntOption(63, 0), null},
        {new DatabaseIntOption(63, 1), PitchType.Curveball},

        {new DatabaseIntOption(64, 0), null},
        {new DatabaseIntOption(64, 1), PitchType.Slider},

        {new DatabaseIntOption(65, 0), null},
        {new DatabaseIntOption(65, 1), PitchType.Cutter}
    };

    public static FrozenDictionary<DatabaseIntOption, PitchType?> Pitches =>
        _pitches.ToFrozenDictionary();
}

public enum PitchType
{
    [Description("4F")] FourSeamFastball,
    [Description("2F")] TwoSeamFastball,
    [Description("SB")] Screwball,
    [Description("CH")] Changeup,
    [Description("FK")] Forkball,
    [Description("CB")] Curveball,
    [Description("SL")] Slider,
    [Description("CF")] Cutter,
}