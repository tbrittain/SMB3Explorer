using OneOf;
using OneOf.Types;
using SMB3Explorer.AppConfig.Models;

namespace SMB3Explorer.AppConfig;

public interface IAppConfig
{
    OneOf<ConfigOptions, Error<string>> GetConfigOptions();
    OneOf<Success, Error<string>> SaveConfigOptions(ConfigOptions configOptions);
}