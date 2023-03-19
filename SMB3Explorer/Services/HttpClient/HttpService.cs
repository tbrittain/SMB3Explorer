using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OneOf;
using OneOf.Types;
using SMB3Explorer.Models.Internal;

namespace SMB3Explorer.Services.HttpClient;

public class HttpService : IHttpService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<OneOf<AppUpdateResult, None, Error<string>>> CheckForUpdates()
    {
        const string url = "https://api.github.com/repos/tbrittain/SMB3Explorer/releases/latest";
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return new Error<string>(response.ReasonPhrase ?? "An unknown error occurred.");

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GitHubReleaseResponse>(content);

        if (result is null)
        {
            return new Error<string>("Unable to parse latest release response.");
        }

        var currentVersion = Assembly.GetEntryAssembly()?.GetName().Version;
        if (currentVersion is null)
        {
            return new Error<string>("Unable to determine current version.");
        }

        var currentVersionWithoutRev =
            new Version(currentVersion.Major, currentVersion.Minor, currentVersion.Build).ToString();

        if (result.Version == currentVersionWithoutRev) return new None();

        return new AppUpdateResult
        {
            Version = result.Version,
            ReleasePageUrl = result.HtmlUrl,
            ReleaseDate = result.PublishedAt,
        };
    }
}