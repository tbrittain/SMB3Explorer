using CsvHelper.Configuration.Attributes;

namespace SMB3Explorer.Models.Exports;

public class BattingMostRecentSeasonStatistic
{
    public BattingMostRecentSeasonStatistic(BattingSeasonStatistic battingSeasonStatistic)
    {
        PlayerId = battingSeasonStatistic.PlayerId;
        FirstName = battingSeasonStatistic.FirstName;
        LastName = battingSeasonStatistic.LastName;
        CurrentTeam = battingSeasonStatistic.CurrentTeam;
        PreviousTeam = battingSeasonStatistic.PreviousTeam;
        PositionNumber = battingSeasonStatistic.PositionNumber;
        SecondaryPositionNumber = battingSeasonStatistic.SecondaryPositionNumber;
        GamesBatting = battingSeasonStatistic.GamesBatting;
        GamesPlayed = battingSeasonStatistic.GamesPlayed;
        AtBats = battingSeasonStatistic.AtBats;
        Runs = battingSeasonStatistic.Runs;
        Hits = battingSeasonStatistic.Hits;
        Doubles = battingSeasonStatistic.Doubles;
        Triples = battingSeasonStatistic.Triples;
        HomeRuns = battingSeasonStatistic.HomeRuns;
        RunsBattedIn = battingSeasonStatistic.RunsBattedIn;
        StolenBases = battingSeasonStatistic.StolenBases;
        CaughtStealing = battingSeasonStatistic.CaughtStealing;
        Walks = battingSeasonStatistic.Walks;
        Strikeouts = battingSeasonStatistic.Strikeouts;
        HitByPitch = battingSeasonStatistic.HitByPitch;
        SacrificeHits = battingSeasonStatistic.SacrificeHits;
        SacrificeFlies = battingSeasonStatistic.SacrificeFlies;
        PassedBalls = battingSeasonStatistic.PassedBalls;
        CompletionDate = battingSeasonStatistic.CompletionDate;
        SeasonId = battingSeasonStatistic.SeasonId;
        SeasonNum = battingSeasonStatistic.SeasonNum;
        Age = battingSeasonStatistic.Age;
    }
    
    [Name("OPS+"), Index(47)]
    public double OnBasePercentagePlus { get; set; }
}