using System;
using Newtonsoft.Json;

namespace SMB3Explorer.ApplicationConfig.Models;

public class League
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonProperty("id")]
    public Guid Id { get; set; }
}