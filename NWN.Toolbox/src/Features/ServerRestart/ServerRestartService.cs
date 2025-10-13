using System;
using System.Text;
using Anvil.API;
using Anvil.API.Events;
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
    private ConfigService ConfigService { get; init; } = null!;

    [Inject]
    private SchedulerService SchedulerService { get; init; } = null!;

    private ScheduledTask? restartSchedule;

    private ServerRestartConfig RestartConfig
    {
      get
      {
        if (!ConfigService.Config.ServerRestart.IsEnabled())
        {
          throw new InvalidOperationException($"The {nameof(ServerRestartService)} is not enabled.");
        }

        return ConfigService.Config.ServerRestart;
      }
    }

    void IInitializable.Init()
    {
      if (ConfigService.Config.ServerRestart.IsEnabled())
      {
        restartSchedule = SchedulerService.Schedule(ShutdownServer, GetDelayForRestart());
        if (ConfigService.Config.ServerRestart.RestartWarningSecs != null && ConfigService.Config.ServerRestart.RestartWarningSecs.Count > 0)
        {
          SchedulerService.ScheduleRepeating(CheckForWarning, TimeSpan.FromSeconds(1));
        }

        NwModule.Instance.OnClientEnter += OnClientEnter;
      }
    }

    /// <summary>
    /// Gets or sets the time until the server is shutdown/restarted.
    /// </summary>
    public TimeSpan? TimeUntilRestart
    {
      get => restartSchedule != null ? restartSchedule.ExecutionTime - Time.TimeSinceStartup : null;
      set
      {
        restartSchedule?.Cancel();
        restartSchedule = null;

        if (value != null)
        {
          restartSchedule = SchedulerService.Schedule(ShutdownServer, value.Value);
        }
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
      string? message = GetWarningMessage();
      if (message == null)
      {
        return;
      }

      foreach (NwPlayer player in NwModule.Instance.Players)
      {
        if (player.IsValid && player.IsConnected)
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
      string? message = GetWarningMessage();
      if (message == null)
      {
        return;
      }

      player.SendServerMessage(message);
    }

    private void OnClientEnter(ModuleEvents.OnClientEnter eventData)
    {
      SendRestartTimeMessageToPlayer(eventData.Player);
    }

    private TimeSpan GetDelayForRestart()
    {
      ServerRestartConfig config = RestartConfig;

      if (!string.IsNullOrEmpty(config.RestartTimeCron))
      {
        try
        {
          CronExpression expression = CronExpression.Parse(config.RestartTimeCron);
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

      return TimeSpan.FromSeconds(config.RestartTimeSecs);
    }

    private void ShutdownServer()
    {
      // Lock the server from new connections.
      NwServer.Instance.PlayerPassword = Guid.NewGuid().ToUUIDString();

      // Boot Players
      string reason = RestartConfig.BootMessage ?? string.Empty;
      foreach (NwPlayer player in NwModule.Instance.Players)
      {
        if (player.IsValid)
        {
          player.BootPlayer(reason);
        }
      }

      // Shutdown server
      NwServer.Instance.ShutdownServer();
    }

    private void CheckForWarning()
    {
      if (TimeUntilRestart == null)
      {
        return;
      }

      TimeSpan timeUntilRestart = TimeUntilRestart.Value;
      ServerRestartConfig config = RestartConfig;

      double remainingSecs = timeUntilRestart.TotalSeconds;
      if (remainingSecs >= 0 && config.RestartWarningSecs?.Contains((uint)remainingSecs) == true)
      {
        SendRestartTimeMessageToAllPlayers();
      }
    }

    private string? GetWarningMessage()
    {
      if (TimeUntilRestart == null)
      {
        return null;
      }

      TimeSpan timeUntilRestart = TimeUntilRestart.Value;
      ServerRestartConfig config = RestartConfig;

      if (timeUntilRestart.TotalSeconds < 1)
      {
        return config.WarnMessageNow;
      }

      return config.WarnMessage?.Replace("<time>", GetFormattedTime(timeUntilRestart));
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
