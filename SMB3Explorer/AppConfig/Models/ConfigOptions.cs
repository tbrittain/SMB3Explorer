using System.Collections.Generic;
using Newtonsoft.Json;

namespace SMB3Explorer.AppConfig.Models;

public class ConfigOptions
{
    [JsonProperty("smb4Leagues")]
    public List<League> Leagues { get; set; } = new();
}