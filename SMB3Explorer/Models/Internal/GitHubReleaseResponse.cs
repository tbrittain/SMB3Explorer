using System;
using System.Text.Json.Serialization;

namespace SMB3Explorer.Models.Internal;

public class GitHubReleaseResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("tag_name")]
    public string TagName { private get; set; } = string.Empty;
    
    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; } = string.Empty;
    
    public string Version => TagName.StartsWith("v") ? TagName[1..] : TagName;
    
    [JsonPropertyName("published_at")]
    public DateTime PublishedAt { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}