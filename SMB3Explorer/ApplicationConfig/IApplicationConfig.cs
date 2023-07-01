using OneOf;
using OneOf.Types;
using SMB3Explorer.ApplicationConfig.Models;

namespace SMB3Explorer.ApplicationConfig;

public interface IApplicationConfig
{
    OneOf<ConfigOptions, Error<string>> GetConfigOptions();
    OneOf<Success, Error<string>> SaveConfigOptions(ConfigOptions configOptions);
}