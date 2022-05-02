using System;
using System.ComponentModel;

namespace Jorteck.Toolbox.Config
{
  [Serializable]
  internal sealed class PermissionsConfig : IFeatureConfig
  {
    [Description("Enable/disable the permission system.")]
    public bool Enabled { get; set; } = false;

    [Description("Adds support for managing permissions with chat commands. Requires the ChatCommand module to be enabled.")]
    public bool ChatCommandEnable { get; set; } = true;

    [Description("The base chat command name.")]
    public string ChatCommand { get; set; } = "perms";
  }
}
