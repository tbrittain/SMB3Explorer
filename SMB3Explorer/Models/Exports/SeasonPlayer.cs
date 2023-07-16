using System;
using System.Linq;
using CsvHelper.Configuration.Attributes;
using SMB3Explorer.Enums;
using SMB3Explorer.Utils;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SMB3Explorer.Models.Exports;

public class SeasonPlayer
{
    private Trait[] _traits = Array.Empty<Trait>();
    private PitchType[] _pitches = Array.Empty<PitchType>();

    [Ignore]
    public Guid PlayerId { get; set; }
    
    [Ignore]
    public int SeasonId { get; set; }
    
    [Name("Season"), Index(0)]
    public int SeasonNum { get; set; }
    
    [Name("First Name"), Index(1)]
    public string FirstName { get; set; } = string.Empty;
    
    [Name("Last Name"), Index(2)]
    public string LastName { get; set; } = string.Empty;
    
    [Ignore]
    public int PrimaryPositionNumber { get; set; }
    
    [Name("Position"), Index(3)]
    // ReSharper disable once UnusedMember.Global
    public string PrimaryPositionDescription => ((BaseballPlayerPosition) PrimaryPositionNumber).GetEnumDescription();
    
    [Ignore]
    public int? SecondaryPositionNumber { get; set; }
    
    [Name("Secondary Position"), Index(4)]
    // ReSharper disable once UnusedMember.Global
    public string? SecondaryPositionDescription => SecondaryPositionNumber is null 
        ? null 
        : ((BaseballPlayerPosition) SecondaryPositionNumber).GetEnumDescription();
    
    [Ignore]
    public int? PitcherRole { get; set; }
    
    [Name("Pitcher Role"), Index(5)]
    // ReSharper disable once UnusedMember.Global
    public string? PitcherRoleDescription => PitcherRole is null 
        ? null 
        : ((PitcherRole) PitcherRole).GetEnumDescription();
    
    [Name("Team"), Index(6)]
    public string? CurrentTeam { get; set; }
    
    [Name("Prev Team"), Index(7)]
    public string? PreviousTeam { get; set; }
    
    [Name("Power"), Index(8)]
    public int Power { get; set; }
    
    [Name("Contact"), Index(9)]
    public int Contact { get; set; }
    
    [Name("Speed"), Index(10)]
    public int Speed { get; set; }
    
    [Name("Fielding"), Index(11)]
    public int Fielding { get; set; }
    
    [Name("Arm"), Index(12)]
    public int? Arm { get; set; }
    
    [Name("Velocity"), Index(13)]
    public int? Velocity { get; set; }
    
    [Name("Junk"), Index(14)]
    public int? Junk { get; set; }
    
    [Name("Accuracy"), Index(15)]
    public int? Accuracy { get; set; }
    
    [Name("Age"), Index(16)]
    public int Age { get; set; }
    
    [Name("Salary"), Index(17)]
    public int Salary { get; set; }

    [Ignore]
    public Trait[] Traits
    {
        set
        {
            _traits = value;
            if (!_traits.Any()) return;
            
            Trait1 = _traits[0].GetEnumDescription();
            
            if (_traits.Length > 1)
                Trait2 = _traits[1].GetEnumDescription();
        }
    }
    
    [Name("Trait 1"), Index(18)]
    public string? Trait1 { get; set; }
    
    [Name("Trait 2"), Index(19)]
    public string? Trait2 { get; set; }
    
    [Name("Chemistry"), Index(20)]
    public string? Chemistry { get; set; }

    [Name("Throw Hand"), Index(21)]
    public string ThrowHand { get; set; } = string.Empty;
    
    [Name("Bat Hand"), Index(22)]
    public string BatHand { get; set; } = string.Empty;

    [Ignore]
    public PitchType[] Pitches
    {
        set
        {
            _pitches = value;
            if (!_pitches.Any()) return;

            Pitch1 = _pitches[0].GetEnumDescription();

            if (_pitches.Length > 1)
                Pitch2 = _pitches[1].GetEnumDescription();
            
            if (_pitches.Length > 2)
                Pitch3 = _pitches[2].GetEnumDescription();
            
            if (_pitches.Length > 3)
                Pitch4 = _pitches[3].GetEnumDescription();
            
            if (_pitches.Length > 4)
                Pitch5 = _pitches[4].GetEnumDescription();
        }
    }

    [Name("Pitch 1"), Index(23)]
    public string? Pitch1 { get; set; }
    
    [Name("Pitch 2"), Index(24)]
    public string? Pitch2 { get; set; }
    
    [Name("Pitch 3"), Index(25)]
    public string? Pitch3 { get; set; }
    
    [Name("Pitch 4"), Index(26)]
    public string? Pitch4 { get; set; }
    
    [Name("Pitch 5"), Index(27)]
    public string? Pitch5 { get; set; }
}