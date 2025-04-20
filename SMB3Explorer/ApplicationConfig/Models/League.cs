using System;
using System.Text.Json.Serialization;

namespace SMB3Explorer.ApplicationConfig.Models;

public class League
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("playerTeam")]
    public string? PlayerTeam { get; init; }
    
    [JsonPropertyName("numSeasons")]
    public int? NumSeasons { get; init; }
    
    [JsonPropertyName("numTimesAccessed")]
    public int NumTimesAccessed { get; init; }

    [JsonPropertyName("firstAccessed")]
    public DateTime FirstAccessed { get; init; }

    [JsonPropertyName("lastAccessed")]
    public DateTime LastAccessed { get; init; }
}