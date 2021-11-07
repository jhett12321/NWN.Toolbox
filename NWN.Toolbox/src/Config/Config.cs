using System;

namespace Jorteck.Toolbox
{
  [Serializable]
  internal sealed class Config
  {
    public const string ConfigName = "config.yml";

    public string StoragePath { get; set; } = "./data/data.db";
    public int Version { get; set; } = 1;
  }
}
