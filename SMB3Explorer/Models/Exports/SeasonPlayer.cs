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
    
    // TODO: Get chemistry for SMB4 players
    [Name("Chemistry"), Index(20)]
    public string? Chemistry { get; set; }
}