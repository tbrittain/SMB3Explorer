using System;
using System.Text;
using AutoMapper;
using Microsoft.Data.Sqlite;
using SMB3Explorer.Utils;

namespace SMB3Explorer.Models.Mappings;

public class CareerBattingStatisticMapper : Profile
{
    public CareerBattingStatisticMapper()
    {
        CreateMap<SqliteDataReader, CareerBattingStatistic>()
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
            .ForMember(dest => dest.PrimaryPositionNumber,
                opt => opt.MapFrom(src => int.Parse(src["primaryPosition"].ToString()!)))
            .ForMember(dest => dest.SecondaryPositionNumber,
                opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src["secondaryPosition"].ToString())
                        ? (int?) null
                        : int.Parse(src["secondaryPosition"].ToString()!)))
            .ForMember(dest => dest.PitcherRole,
                opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src["pitcherRole"].ToString())
                        ? (int?) null
                        : int.Parse(src["pitcherRole"].ToString()!)))
            .ForMember(dest => dest.GamesPlayed,
                opt => opt.MapFrom(src => int.Parse(src["gamesPlayed"].ToString()!)))
            .ForMember(dest => dest.GamesBatting,
                opt => opt.MapFrom(src => int.Parse(src["gamesBatting"].ToString()!)))
            .ForMember(dest => dest.AtBats,
                opt => opt.MapFrom(src => int.Parse(src["atBats"].ToString()!)))
            .ForMember(dest => dest.Runs,
                opt => opt.MapFrom(src => int.Parse(src["runs"].ToString()!)))
            .ForMember(dest => dest.Hits,
                opt => opt.MapFrom(src => int.Parse(src["hits"].ToString()!)))
            .ForMember(dest => dest.Doubles,
                opt => opt.MapFrom(src => int.Parse(src["doubles"].ToString()!)))
            .ForMember(dest => dest.Triples,
                opt => opt.MapFrom(src => int.Parse(src["triples"].ToString()!)))
            .ForMember(dest => dest.HomeRuns,
                opt => opt.MapFrom(src => int.Parse(src["homeruns"].ToString()!)))
            .ForMember(dest => dest.RunsBattedIn,
                opt => opt.MapFrom(src => int.Parse(src["rbi"].ToString()!)))
            .ForMember(dest => dest.StolenBases,
                opt => opt.MapFrom(src => int.Parse(src["stolenBases"].ToString()!)))
            .ForMember(dest => dest.CaughtStealing,
                opt => opt.MapFrom(src => int.Parse(src["caughtStealing"].ToString()!)))
            .ForMember(dest => dest.Walks,
                opt => opt.MapFrom(src => int.Parse(src["baseOnBalls"].ToString()!)))
            .ForMember(dest => dest.Strikeouts,
                opt => opt.MapFrom(src => int.Parse(src["strikeOuts"].ToString()!)))
            .ForMember(dest => dest.HitByPitch,
                opt => opt.MapFrom(src => int.Parse(src["hitByPitch"].ToString()!)))
            .ForMember(dest => dest.SacrificeHits,
                opt => opt.MapFrom(src => int.Parse(src["sacrificeHits"].ToString()!)))
            .ForMember(dest => dest.SacrificeFlies,
                opt => opt.MapFrom(src => int.Parse(src["sacrificeFlies"].ToString()!)))
            .ForMember(dest => dest.Errors,
                opt => opt.MapFrom(src => int.Parse(src["errors"].ToString()!)))
            .ForMember(dest => dest.PassedBalls,
                opt => opt.MapFrom(src => int.Parse(src["passedBalls"].ToString()!)));
    }
}