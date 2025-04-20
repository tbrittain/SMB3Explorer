using System;
using System.Text.Json.Serialization;

namespace SMB3Explorer.ApplicationConfig.Models;

public class League
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("playerTeam")]
    public string? PlayerTeam { get; set; }
    
    [JsonPropertyName("numSeasons")]
    public int? NumSeasons { get; set; }
    
    [JsonPropertyName("numTimesAccessed")]
    public int NumTimesAccessed { get; set; }

    [JsonPropertyName("firstAccessed")]
    public DateTime FirstAccessed { get; set; }

    [JsonPropertyName("lastAccessed")]
    public DateTime LastAccessed { get; set; }
}