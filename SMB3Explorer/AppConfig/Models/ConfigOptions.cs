using System.Collections.Generic;
using Newtonsoft.Json;
using SMB3Explorer.Enums;

namespace SMB3Explorer.AppConfig.Models;

public class ConfigOptions
{
    [JsonProperty("includePlayerIdsInExports")]
    public bool IncludePlayerIdsInExports { get; set; }

    [JsonProperty("gamePreference")]
    public SelectedGame GamePreference { get; set; } = SelectedGame.Smb4;

    [JsonProperty("smb4Leagues")]
    public List<League> Leagues { get; set; } = new();
}