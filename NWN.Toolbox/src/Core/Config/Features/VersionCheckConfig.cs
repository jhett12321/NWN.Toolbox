using System;
using System.ComponentModel;

namespace Jorteck.Toolbox.Config
{
  [Serializable]
  internal sealed class VersionCheckConfig : IFeatureConfig
  {
    [Description("Enable/disable the version check system.")]
    public bool Enabled { get; set; } = false;

    [Description("The minimum version for the server. Clients who are below the version will be kicked when they attempt to connect.")]
    public Version MinimumVersion { get; set; } = new Version
    {
      MajorVersion = 8193,
      MinorVersion = 30,
    };

    [Description("The recommended version for the server. Clients who are below the recommended version will see a warning message after they connect to the server.")]
    public Version RecommendedVersion { get; set; } = new Version
    {
      MajorVersion = 8193,
      MinorVersion = 34,
    };

    public class Version
    {
      public int MajorVersion { get; set; }
      public int MinorVersion { get; set; }

      public System.Version AsVersion()
      {
        return new System.Version(MajorVersion, MinorVersion);
      }
    }
  }
}
