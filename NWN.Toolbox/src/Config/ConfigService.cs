using System.IO;
using Anvil.Services;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Jorteck.Toolbox
{
  [ServiceBinding(typeof(ConfigService))]
  internal sealed class ConfigService
  {
    private readonly string pluginStoragePath;

    private readonly IDeserializer deserializer = new DeserializerBuilder()
      .WithNamingConvention(UnderscoredNamingConvention.Instance)
      .Build();

    private readonly ISerializer serializer = new SerializerBuilder()
      .WithNamingConvention(UnderscoredNamingConvention.Instance)
      .Build();

    // ReSharper disable once NotAccessedField.Global
    internal readonly Config Config;

    public ConfigService(PluginStorageService pluginStorageService)
    {
      pluginStoragePath = pluginStorageService.GetPluginStoragePath(typeof(ConfigService).Assembly);
      Config = LoadConfig<Config>(Config.ConfigName);
    }

    private T LoadConfig<T>(string fileName) where T : new()
    {
      string configPath = GetConfigPath(fileName);
      T retVal;

      if (!File.Exists(configPath))
      {
        retVal = new T();
        SaveConfig(fileName, retVal);
      }
      else
      {
        retVal = deserializer.Deserialize<T>(File.ReadAllText(configPath));
      }

      return retVal;
    }

    private void SaveConfig<T>(string fileName, T instance)
    {
      string yaml = serializer.Serialize(instance);
      File.WriteAllText(GetConfigPath(fileName), yaml);
    }

    private string GetConfigPath(string fileName)
    {
      return Path.Combine(pluginStoragePath, fileName);
    }
  }
}
