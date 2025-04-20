using System.Collections.Generic;
using System.Text.Json.Serialization;
using SMB3Explorer.Enums;

namespace SMB3Explorer.ApplicationConfig.Models;

public class ConfigOptions
{
    [JsonPropertyName("gamePreference")]
    public SelectedGame GamePreference { get; set; } = SelectedGame.Smb4;

    [JsonPropertyName("smb4Leagues")]
    public List<League> Leagues { get; set; } = new();
}