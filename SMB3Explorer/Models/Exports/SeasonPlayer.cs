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

    [Name("player_id"), Index(0)]
    public Guid PlayerId { get; set; }
    
    [Name("season_id"), Index(1)]
    public int SeasonId { get; set; }
    
    [Name("season_num"), Index(2)]
    public int SeasonNum { get; set; }
    
    [Name("first_name"), Index(3)]
    public string FirstName { get; set; } = string.Empty;
    
    [Name("last_name"), Index(4)]
    public string LastName { get; set; } = string.Empty;
    
    [Name("primary_position"), Index(5)]
    public int PrimaryPositionNumber { get; set; }
    
    [Name("primary_position_name"), Index(6)]
    // ReSharper disable once UnusedMember.Global
    public string PrimaryPositionDescription => ((BaseballPlayerPosition) PrimaryPositionNumber).GetEnumDescription();
    
    [Name("secondary_position"), Index(7)]
    public int? SecondaryPositionNumber { get; set; }
    
    [Name("secondary_position_name"), Index(8)]
    // ReSharper disable once UnusedMember.Global
    public string? SecondaryPositionDescription => SecondaryPositionNumber is null 
        ? null 
        : ((BaseballPlayerPosition) SecondaryPositionNumber).GetEnumDescription();
    
    [Name("pitcher_role"), Index(9)]
    public int? PitcherRole { get; set; }
    
    [Name("pitcher_role_name"), Index(10)]
    // ReSharper disable once UnusedMember.Global
    public string? PitcherRoleDescription => PitcherRole is null 
        ? null 
        : ((PitcherRole) PitcherRole).GetEnumDescription();
    
    [Name("current_team"), Index(11)]
    public string? CurrentTeam { get; set; }
    
    [Name("previous_team"), Index(12)]
    public string? PreviousTeam { get; set; }
    
    [Name("power"), Index(13)]
    public int Power { get; set; }
    
    [Name("contact"), Index(14)]
    public int Contact { get; set; }
    
    [Name("speed"), Index(15)]
    public int Speed { get; set; }
    
    [Name("fielding"), Index(16)]
    public int Fielding { get; set; }
    
    [Name("arm"), Index(17)]
    public int? Arm { get; set; }
    
    [Name("velocity"), Index(18)]
    public int? Velocity { get; set; }
    
    [Name("junk"), Index(19)]
    public int? Junk { get; set; }
    
    [Name("accuracy"), Index(20)]
    public int? Accuracy { get; set; }
    
    [Name("age"), Index(21)]
    public int Age { get; set; }
    
    [Name("salary"), Index(22)]
    public int Salary { get; set; }

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
    
    [Name("trait_1"), Index(23)]
    public string? Trait1 { get; set; }
    
    [Name("trait_2"), Index(24)]
    public string? Trait2 { get; set; }
}