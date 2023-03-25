using System.Threading.Tasks;
using OneOf;
using OneOf.Types;
using SMB3Explorer.Models.Internal;

namespace SMB3Explorer.Services.HttpClient;

public interface IHttpService
{
    Task<OneOf<AppUpdateResult, None, Error<string>>> CheckForUpdates();
}