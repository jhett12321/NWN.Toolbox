using System;
using System.Text;
using Anvil.API;
using Anvil.Services;
using Cronos;
using Jorteck.Toolbox.Core;
using NLog;

namespace Jorteck.Toolbox.Features.ServerRestart
{
  [ServiceBinding(typeof(ServerRestartService))]
  public sealed class ServerRestartService : IInitializable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    [Inject]
    private ConfigService ConfigService { get; init; }

    [Inject]
    private SchedulerService SchedulerService { get; init; }

    private ScheduledTask restartSchedule;

    void IInitializable.Init()
    {
      if (ConfigService.Config.ServerRestart.IsEnabled())
      {
        restartSchedule = SchedulerService.Schedule(ShutdownServer, GetDelayForRestart());
        if (ConfigService.Config.ServerRestart.RestartWarningSecs != null && ConfigService.Config.ServerRestart.RestartWarningSecs.Count > 0)
        {
          SchedulerService.ScheduleRepeating(CheckForWarning, TimeSpan.FromSeconds(1));
        }
      }
    }

    /// <summary>
    /// Gets or sets the time until the server is shutdown/restarted.
    /// </summary>
    public TimeSpan TimeUntilRestart
    {
      get
      {
        AssertEnabled();
        return restartSchedule.ExecutionTime - Time.TimeSinceStartup;
      }
      set
      {
        AssertEnabled();
        restartSchedule.Cancel();
        restartSchedule = SchedulerService.Schedule(ShutdownServer, value);
      }
    }

    /// <summary>
    /// Gets if automatic server restart is configured.
    /// </summary>
    public bool IsEnabled => ConfigService.Config.ServerRestart.IsEnabled();

    /// <summary>
    /// Broadcasts the restart timer warning message to all players on the server.
    /// </summary>
    public void SendRestartTimeMessageToAllPlayers()
    {
      string message = GetWarningMessage();
      foreach (NwPlayer player in NwModule.Instance.Players)
      {
        if (player.ControlledCreature != null)
        {
          player.SendServerMessage(message);
        }
      }
    }

    /// <summary>
    /// Sends the restart timer warning message to a specific player on the server.
    /// </summary>
    /// <param name="player">The player to send the message to.</param>
    public void SendRestartTimeMessageToPlayer(NwPlayer player)
    {
      string message = GetWarningMessage();
      player.SendServerMessage(message);
    }

    private void AssertEnabled()
    {
      if (!IsEnabled)
      {
        throw new InvalidOperationException("The ServerRestartService is not enabled.");
      }
    }

    private TimeSpan GetDelayForRestart()
    {
      if (!string.IsNullOrEmpty(ConfigService.Config.ServerRestart.RestartTimeCron))
      {
        try
        {
          CronExpression expression = CronExpression.Parse(ConfigService.Config.ServerRestart.RestartTimeCron);
          DateTime? next = expression.GetNextOccurrence(DateTime.UtcNow);
          if (next != null)
          {
            return next.Value - DateTime.UtcNow;
          }
        }
        catch (CronFormatException e)
        {
          Log.Error(e, "Failed to parse cron expression");
        }
      }

      return TimeSpan.FromSeconds(ConfigService.Config.ServerRestart.RestartTimeSecs);
    }

    private void ShutdownServer()
    {
      // Lock the server from new connections.
      NwServer.Instance.PlayerPassword = Guid.NewGuid().ToUUIDString();

      // Boot Players
      string reason = ConfigService.Config.ServerRestart.BootMessage ?? string.Empty;
      foreach (NwPlayer player in NwModule.Instance.Players)
      {
        player.BootPlayer(reason);
      }

      // Shutdown server
      NwServer.Instance.ShutdownServer();
    }

    private void CheckForWarning()
    {
      double remainingSecs = TimeUntilRestart.TotalSeconds;
      if (remainingSecs >= 0 && ConfigService.Config.ServerRestart.RestartWarningSecs.Contains((uint)remainingSecs))
      {
        SendRestartTimeMessageToAllPlayers();
      }
    }

    private string GetWarningMessage()
    {
      if (TimeUntilRestart.TotalSeconds < 1)
      {
        return ConfigService.Config.ServerRestart.WarnMessageNow;
      }

      return ConfigService.Config.ServerRestart.WarnMessage.Replace("<time>", GetFormattedTime(TimeUntilRestart));
    }

    private string GetFormattedTime(TimeSpan timeSpan)
    {
      StringBuilder sb = new StringBuilder();
      if (timeSpan.Days > 0)
      {
        sb.AppendFormat("{0} day{1} ", timeSpan.Days, timeSpan.Days > 1 ? "s" : string.Empty);
      }

      if (timeSpan.Hours > 0)
      {
        sb.AppendFormat("{0} hour{1} ", timeSpan.Hours, timeSpan.Hours > 1 ? "s" : string.Empty);
      }

      if (timeSpan.Minutes > 0)
      {
        sb.AppendFormat("{0} minute{1} ", timeSpan.Minutes, timeSpan.Minutes > 1 ? "s" : string.Empty);
      }

      if (timeSpan.TotalMinutes < 1)
      {
        sb.AppendFormat("{0} second{1} ", timeSpan.Seconds, timeSpan.Seconds > 1 ? "s" : string.Empty);
      }

      return sb.ToString().Trim();
    }
  }
}
