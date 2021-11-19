using System;

namespace Jorteck.Toolbox
{
  [Serializable]
  internal sealed class Config
  {
    public const string ConfigName = "config.yml";

    public int Version { get; set; } = 1;
  }
}
