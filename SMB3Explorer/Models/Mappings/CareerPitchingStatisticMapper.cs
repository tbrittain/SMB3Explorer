using System;
using System.Text;
using AutoMapper;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Models.Mappings;

public class CareerPitchingStatisticMapper : Profile
{
    public CareerPitchingStatisticMapper()
    {
        CreateMap<SqliteDataReader, CareerPitchingStatistic>()
            .ForMember(dest => dest.AggregatorId, opt => opt.MapFrom(src => src["aggregatorID"].ToString()))
            .ForMember(dest => dest.StatsPlayerId, opt => opt.MapFrom(src => src["statsPlayerID"].ToString()))
            .ForMember(dest => dest.PlayerId,
                opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src["baseballPlayerGUIDIfKnown"].ToString())
                        ? (Guid?) null
                        : Encoding.UTF8.GetBytes(src["baseballPlayerGUIDIfKnown"].ToString()!).ToGuid()))
            .ForMember(dest => dest.CurrentTeam,
                opt => opt.MapFrom(src => src["currentTeamName"].ToString()))
            .ForMember(dest => dest.MostRecentTeam,
                opt => opt.MapFrom(src => src["mostRecentTeamName"].ToString()))
            .ForMember(dest => dest.SecondMostRecentTeam,
                opt => opt.MapFrom(src => src["secondMostRecentTeamName"].ToString()))
            .ForMember(dest => dest.FirstName,
                opt => opt.MapFrom(src => src["firstName"].ToString()))
            .ForMember(dest => dest.LastName,
                opt => opt.MapFrom(src => src["lastName"].ToString()))
            .ForMember(dest => dest.RetirementSeason,
                opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src["retirementSeason"].ToString())
                        ? (int?) null
                        : int.Parse(src["retirementSeason"].ToString()!)))
            .ForMember(dest => dest.RetirementAge,
                opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src["age"].ToString())
                        ? (int?) null
                        : int.Parse(src["age"].ToString()!)))
            .ForMember(dest => dest.PitcherRole,
                opt => opt.MapFrom(src => int.Parse(src["pitcherRole"].ToString()!)))
            .ForMember(dest => dest.Wins,
                opt => opt.MapFrom(src => int.Parse(src["wins"].ToString()!)))
            .ForMember(dest => dest.Losses,
                opt => opt.MapFrom(src => int.Parse(src["losses"].ToString()!)))
            .ForMember(dest => dest.GamesPlayed,
                opt => opt.MapFrom(src => int.Parse(src["games"].ToString()!)))
            .ForMember(dest => dest.GamesStarted,
                opt => opt.MapFrom(src => int.Parse(src["gamesStarted"].ToString()!)))
            .ForMember(dest => dest.TotalPitches,
                opt => opt.MapFrom(src => int.Parse(src["totalPitches"].ToString()!)))
            .ForMember(dest => dest.CompleteGames,
                opt => opt.MapFrom(src => int.Parse(src["completeGames"].ToString()!)))
            .ForMember(dest => dest.Shutouts,
                opt => opt.MapFrom(src => int.Parse(src["shutouts"].ToString()!)))
            .ForMember(dest => dest.Saves,
                opt => opt.MapFrom(src => int.Parse(src["saves"].ToString()!)))
            .ForMember(dest => dest.OutsPitched,
                opt => opt.MapFrom(src => int.Parse(src["outsPitched"].ToString()!)))
            .ForMember(dest => dest.HitsAllowed,
                opt => opt.MapFrom(src => int.Parse(src["hits"].ToString()!)))
            .ForMember(dest => dest.EarnedRuns,
                opt => opt.MapFrom(src => int.Parse(src["earnedRuns"].ToString()!)))
            .ForMember(dest => dest.HomeRunsAllowed,
                opt => opt.MapFrom(src => int.Parse(src["homeRuns"].ToString()!)))
            .ForMember(dest => dest.WalksAllowed,
                opt => opt.MapFrom(src => int.Parse(src["basesOnBalls"].ToString()!)))
            .ForMember(dest => dest.Strikeouts,
                opt => opt.MapFrom(src => int.Parse(src["strikeOuts"].ToString()!)))
            .ForMember(dest => dest.HitByPitch,
                opt => opt.MapFrom(src => int.Parse(src["battersHitByPitch"].ToString()!)))
            .ForMember(dest => dest.BattersFaced,
                opt => opt.MapFrom(src => int.Parse(src["battersFaced"].ToString()!)))
            .ForMember(dest => dest.GamesFinished,
                opt => opt.MapFrom(src => int.Parse(src["gamesFinished"].ToString()!)))
            .ForMember(dest => dest.RunsAllowed,
                opt => opt.MapFrom(src => int.Parse(src["runsAllowed"].ToString()!)))
            .ForMember(dest => dest.WildPitches,
                opt => opt.MapFrom(src => int.Parse(src["wildPitches"].ToString()!)));
    }
}