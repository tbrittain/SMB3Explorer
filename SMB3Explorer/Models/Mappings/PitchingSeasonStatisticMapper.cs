using System;
using System.Text;
using AutoMapper;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Models.Mappings;

public class PitchingSeasonStatisticMapper : Profile
{
    public PitchingSeasonStatisticMapper()
    {
        CreateMap<SqliteDataReader, PitchingSeasonStatistic>()
            .ForMember(dest => dest.PlayerId,
                opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src["baseballPlayerGUID"].ToString())
                        ? (Guid?) null
                        : Encoding.UTF8.GetBytes(src["baseballPlayerGUID"].ToString()!).ToGuid()))
            .ForMember(dest => dest.FirstName,
                opt => opt.MapFrom(src => src["firstName"].ToString()))
            .ForMember(dest => dest.LastName,
                opt => opt.MapFrom(src => src["lastName"].ToString()))
            .ForMember(dest => dest.PositionNumber,
                opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src["primaryPosition"].ToString())
                        ? 0
                        : int.Parse(src["primaryPosition"].ToString()!)))
            .ForMember(dest => dest.CurrentTeam,
                opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src["currentTeam"].ToString())
                        ? null
                        : src["currentTeam"].ToString()))
            .ForMember(dest => dest.PreviousTeam,
                opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src["previousTeam"].ToString())
                        ? null
                        : src["previousTeam"].ToString()))
            .ForMember(dest => dest.PitcherRole,
                opt => opt.MapFrom(src => int.Parse(src["pitcherRole"].ToString()!)))
            .ForMember(dest => dest.GamesPlayed,
                opt => opt.MapFrom(src => int.Parse(src["games"].ToString()!)))
            .ForMember(dest => dest.GamesStarted,
                opt => opt.MapFrom(src => int.Parse(src["gamesStarted"].ToString()!)))
            .ForMember(dest => dest.Wins,
                opt => opt.MapFrom(src => int.Parse(src["wins"].ToString()!)))
            .ForMember(dest => dest.Losses,
                opt => opt.MapFrom(src => int.Parse(src["losses"].ToString()!)))
            .ForMember(dest => dest.CompleteGames,
                opt => opt.MapFrom(src => int.Parse(src["completeGames"].ToString()!)))
            .ForMember(dest => dest.Shutouts,
                opt => opt.MapFrom(src => int.Parse(src["shutouts"].ToString()!)))
            .ForMember(dest => dest.TotalPitches,
                opt => opt.MapFrom(src => int.Parse(src["totalPitches"].ToString()!)))
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
                opt => opt.MapFrom(src => int.Parse(src["baseOnBalls"].ToString()!)))
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
                opt => opt.MapFrom(src => int.Parse(src["wildPitches"].ToString()!)))
            .ForMember(dest => dest.CompletionDate,
                opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src["completionDate"].ToString())
                        ? (DateTime?) null
                        : DateTime.Parse(src["completionDate"].ToString()!)))
            .ForMember(dest => dest.SeasonId,
                opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src["seasonId"].ToString())
                        ? (int?) null
                        : int.Parse(src["seasonId"].ToString()!)));
    }
}