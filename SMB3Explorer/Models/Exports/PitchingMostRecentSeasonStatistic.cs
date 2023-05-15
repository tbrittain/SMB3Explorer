using CsvHelper.Configuration.Attributes;

namespace SMB3Explorer.Models.Exports;

public class PitchingMostRecentSeasonStatistic : PitchingSeasonStatistic
{
    public PitchingMostRecentSeasonStatistic(PitchingSeasonStatistic pitchingSeasonStatistic)
    {
        PlayerId = pitchingSeasonStatistic.PlayerId;
        FirstName = pitchingSeasonStatistic.FirstName;
        LastName = pitchingSeasonStatistic.LastName;
        CurrentTeam = pitchingSeasonStatistic.CurrentTeam;
        PreviousTeam = pitchingSeasonStatistic.PreviousTeam;
        PositionNumber = pitchingSeasonStatistic.PositionNumber;
        PitcherRole = pitchingSeasonStatistic.PitcherRole;
        GamesPlayed = pitchingSeasonStatistic.GamesPlayed;
        GamesStarted = pitchingSeasonStatistic.GamesStarted;
        Wins = pitchingSeasonStatistic.Wins;
        Losses = pitchingSeasonStatistic.Losses;
        CompleteGames = pitchingSeasonStatistic.CompleteGames;
        Shutouts = pitchingSeasonStatistic.Shutouts;
        TotalPitches = pitchingSeasonStatistic.TotalPitches;
        Saves = pitchingSeasonStatistic.Saves;
        OutsPitched = pitchingSeasonStatistic.OutsPitched;
        HitsAllowed = pitchingSeasonStatistic.HitsAllowed;
        EarnedRuns = pitchingSeasonStatistic.EarnedRuns;
        HomeRunsAllowed = pitchingSeasonStatistic.HomeRunsAllowed;
        WalksAllowed = pitchingSeasonStatistic.WalksAllowed;
        Strikeouts = pitchingSeasonStatistic.Strikeouts;
        HitByPitch = pitchingSeasonStatistic.HitByPitch;
        BattersFaced = pitchingSeasonStatistic.BattersFaced;
        GamesFinished = pitchingSeasonStatistic.GamesFinished;
        RunsAllowed = pitchingSeasonStatistic.RunsAllowed;
        WildPitches = pitchingSeasonStatistic.WildPitches;
        CompletionDate = pitchingSeasonStatistic.CompletionDate;
        SeasonId = pitchingSeasonStatistic.SeasonId;
        SeasonNum = pitchingSeasonStatistic.SeasonNum;
        Age = pitchingSeasonStatistic.Age;
    }

    [Name("ERA-"), Index(48)]
    public double EarnedRunsAllowedMinus { get; set; }

    [Name("FIP-"), Index(49)]
    public double FieldingIndependentPitchingMinus { get; set; }
}