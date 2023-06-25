using System;
using System.IO;
using Newtonsoft.Json;
using OneOf;
using OneOf.Types;
using Serilog;
using SMB3Explorer.AppConfig.Models;
using SMB3Explorer.Constants;

namespace SMB3Explorer.AppConfig;

public class AppConfig : IAppConfig
{
    private const string ConfigFileName = "config.json";

    public OneOf<ConfigOptions, Error<string>> GetConfigOptions()
    {
        var configFilePath = Path.Combine(FileExports.ConfigDirectory, ConfigFileName);
        if (File.Exists(configFilePath))
        {
            var configJson = File.ReadAllText(configFilePath);
            var config = JsonConvert.DeserializeObject<ConfigOptions>(configJson);
            if (config is not null) return config;

            Log.Error("Failed to deserialize config file");
            return new Error<string>("Failed to deserialize config file.");
        }

        var configOptions = new ConfigOptions();
        SaveConfigOptions(configOptions);
        return configOptions;
    }

    public OneOf<Success, Error<string>> SaveConfigOptions(ConfigOptions configOptions)
    {
        var configFilePath = Path.Combine(FileExports.ConfigDirectory, ConfigFileName);
        var configJson = JsonConvert.SerializeObject(configOptions, Formatting.Indented);
        try
        {
            File.WriteAllText(configFilePath, configJson);
        }
        catch (Exception e)
        {
            Log.Error(e, "Failed to save config file");
            return new Error<string>("Failed to save config file.");
        }

        return new Success();
    }
}