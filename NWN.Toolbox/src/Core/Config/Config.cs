using System;
using System.ComponentModel;

namespace Jorteck.Toolbox.Core
{
  [Serializable]
  internal sealed class Config
  {
    public const string ConfigName = "config.yml";

    public int Version { get; set; } = 1;

    [Description("ChatCommands is a module that provides a system of implementing chat commands in C#. It automatically handles command argument parsing, help lists and more.")]
    public ChatCommandConfig ChatCommands { get; set; } = new ChatCommandConfig();

    [Description("ServerRestart is a module that automatically shutdowns the server after a period of time. A custom start script, or service like docker is required to automatically start the server again after shutdown.")]
    public ServerRestartConfig ServerRestart { get; set; } = new ServerRestartConfig();

    [Description("ToolboxWindows is a module that adds a number of utility windows and tools for improved DM functionality, server administration and more.")]
    public WindowConfig ToolboxWindows { get; set; } = new WindowConfig();

    [Description("Permissions is a module that allows server admins to control what tools and features players/DMs can use by creating groups and assigning permissions.")]
    public PermissionsConfig Permissions { get; set; } = new PermissionsConfig();

    [Description("VersionCheck is a module that defines client version requirements to connect to the server (e.g. for NUI).")]
    public VersionCheckConfig VersionCheck { get; set; } = new VersionCheckConfig();

    [Description("Languages is a module that allows characters to speak in different languages.")]
    public LanguageConfig Languages { get; set; } = new LanguageConfig();
  }
}
