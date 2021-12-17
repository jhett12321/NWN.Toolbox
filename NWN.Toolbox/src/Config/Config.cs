using System;
using System.ComponentModel;

namespace Jorteck.Toolbox
{
  [Serializable]
  internal sealed class Config
  {
    public const string ConfigName = "config.yml";

    public int Version { get; set; } = 1;

    [Description("ServerRestart is a module that automatically shutdowns the server after a period of time. A custom start script, or service like docker is required to automatically start the server again after shutdown.")]
    public ServerRestartConfig ServerRestart { get; set; } = new ServerRestartConfig();

    public WindowConfig ToolboxWindows { get; set; } = new WindowConfig();
  }
}
