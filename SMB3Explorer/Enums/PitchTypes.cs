using System.Collections.Generic;
using System.Collections.Immutable;

namespace SMB3Explorer.Enums;

public record struct DatabaseIntOption(int OptionKey, int OptionValue, int OptionType);

public class PitchTypes
{
    // ReSharper disable once InconsistentNaming
    private static readonly Dictionary<DatabaseIntOption, PitchType> _pitches = new()
    {
        
    };

    public static ImmutableDictionary<DatabaseIntOption, PitchType> Pitches =>
        _pitches.ToImmutableDictionary();
}

public enum PitchType
{
    
}