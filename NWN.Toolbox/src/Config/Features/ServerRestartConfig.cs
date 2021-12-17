using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Jorteck.Toolbox
{
  [Serializable]
  internal sealed class ServerRestartConfig
  {
    [Description("Enable/disable automatic server restarts.")]
    public bool Enabled { get; set; } = false;

    [Description("The amount of time in seconds to trigger a restart after the server has started.")]
    public uint RestartTimeSecs { get; set; } = 44 * 60 * 60;

    [Description("Defines a restart schedule using a cron expression: https://crontab.guru/\n  # If defined, overrides restart_time_secs.")]
    public string RestartTimeCron { get; set; } = "";

    [Description("The periods at which restart timer warnings are broadcasted to all players.")]
    public List<uint> RestartWarningSecs { get; set; } = new List<uint> { 0, 60, 300, 1800, 3600, 14400 };

    [Description("The boot message shown to players when the server is restarting.")]
    public string BootMessage { get; set; } = "The server is restarting.";

    [Description("The warning message shown to players at each restart_warning_secs period.")]
    public string WarnMessage { get; set; } = "The server will restart in <time>.";

    [Description("The warning message shown to players when server restart is imminent.")]
    public string WarnMessageNow { get; set; } = "The server is restarting now!";
  }
}
