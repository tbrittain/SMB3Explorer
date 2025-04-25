using System;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using OneOf;
using OneOf.Types;
using Serilog;
using SMB3Explorer.Models.Internal;
using static SMB3Explorer.Constants.Github;

namespace SMB3Explorer.Services.HttpService;

public class HttpService(IHttpClientFactory httpClientFactory) : IHttpService
{
    public async Task<OneOf<AppUpdateResult, None, Error<string>>> CheckForUpdates()
    {
        var currentVersion = Assembly.GetEntryAssembly()?.GetName().Version;
        if (currentVersion is null)
        {
            Log.Error("Unable to determine current version");
            return new Error<string>("Unable to determine current version.");
        }

        var httpClient = httpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Add("User-Agent", $"SMB3Explorer/{currentVersion.ToString()}");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        var response = await httpClient.GetAsync(LatestReleaseUrl);

        if (!response.IsSuccessStatusCode)
        {
            Log.Error("Unable to check for updates: {Reason}", response.ReasonPhrase ?? "An unknown error occurred.");
            return new Error<string>(response.ReasonPhrase ?? "An unknown error occurred.");
        }

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GitHubReleaseResponse>(content);

        if (result is null)
        {
            Log.Error("Unable to parse latest release response");
            return new Error<string>("Unable to parse latest release response.");
        }

        var currentVersionWithoutRev =
            new Version(currentVersion.Major, currentVersion.Minor, currentVersion.Build);
        
        var resultVersionWithoutRev =
            new Version(result.Version.Major, result.Version.Minor, result.Version.Build);

        if (currentVersionWithoutRev >= resultVersionWithoutRev) return new None();

        return new AppUpdateResult
        {
            Version = resultVersionWithoutRev,
            ReleasePageUrl = result.HtmlUrl,
            ReleaseDate = result.PublishedAt,
        };
    }
}