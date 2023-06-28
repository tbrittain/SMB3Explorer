using OneOf;
using OneOf.Types;
using SMB3Explorer.AppConfig.Models;

namespace SMB3Explorer.AppConfig;

public interface IApplicationConfig
{
    OneOf<ConfigOptions, Error<string>> GetConfigOptions();
    OneOf<Success, Error<string>> SaveConfigOptions(ConfigOptions configOptions);
}