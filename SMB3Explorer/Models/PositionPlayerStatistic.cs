using CsvHelper.Configuration.Attributes;

namespace SMB3Explorer.Models;

public class PositionPlayerStatistic : PlayerStatistic
{
    [Name("gamesBatting")]
    public int GamesPlayed { get; set; }
    
    [Name("atBats")]
    public int AtBats { get; set; }
    
    [Name("plateAppearances")]
    public int PlateAppearances { get; set; }
    
    [Name("runs")]
    public int Runs { get; set; }
    
    [Name("hits")]
    public int Hits { get; set; }
    
    [Name("doubles")]
    public int Doubles { get; set; }
    
    [Name("triples")]
    public int Triples { get; set; }
    
    [Name("homeRuns")]
    public int HomeRuns { get; set; }
    
    [Name("rbi")]
    public int RunsBattedIn { get; set; }
    
    [Name("stolenBases")]
    public int StolenBases { get; set; }
    
    [Name("caughtStealing")]
    public int CaughtStealing { get; set; }
    
    [Name("baseOnBalls")]
    public int Walks { get; set; }
    
    [Name("strikeouts")]
    public int Strikeouts { get; set; }
    
    [Name("hitByPitch")]
    public int HitByPitch { get; set; }
    
    [Name("sacrificeHits")]
    public int SacrificeHits { get; set; }
    
    [Name("sacrificeFlies")]
    public int SacrificeFlies { get; set; }
    
    [Name("errors")]
    public int Errors { get; set; }
    
    [Name("passedBalls")]
    public int PassedBalls { get; set; }
    
    // TODO: Rate stats here
}