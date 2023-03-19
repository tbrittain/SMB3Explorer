using System;
using System.Net.Http;
using System.Threading.Tasks;
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

    public Task<OneOf<AppUpdateResult, None>> CheckForUpdates()
    {
        throw new NotImplementedException();
    }
}