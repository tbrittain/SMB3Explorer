using System;
using Newtonsoft.Json;

namespace SMB3Explorer.Models.Internal;

public class GitHubReleaseResponse
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("tag_name")]
    public string TagName { private get; set; } = string.Empty;
    
    [JsonProperty("html_url")]
    public string HtmlUrl { get; set; } = string.Empty;
    
    public Version Version => Version.Parse(TagName.TrimStart('v'));
    
    [JsonProperty("published_at")]
    public DateTime PublishedAt { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}