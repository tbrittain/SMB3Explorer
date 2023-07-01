using System;
using Newtonsoft.Json;

namespace SMB3Explorer.ApplicationConfig.Models;

public class League
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("playerTeam")]
    public string? PlayerTeam { get; set; }
    
    [JsonProperty("numSeasons")]
    public int? NumSeasons { get; set; }
    
    [JsonProperty("numTimesAccessed")]
    public int NumTimesAccessed { get; set; }

    [JsonProperty("firstAccessed")]
    public DateTime FirstAccessed { get; set; }

    [JsonProperty("lastAccessed")]
    public DateTime LastAccessed { get; set; }
}