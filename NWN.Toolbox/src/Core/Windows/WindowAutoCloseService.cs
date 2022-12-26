using System;
using System.Collections.Generic;
using System.Numerics;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Toolbox.Core
{
  /// <summary>
  /// Automatically closes an open window if the player moves from their original location.<br/>
  /// Windows can be registered automatically by setting the <see cref="WindowController{TView}.AutoClose"/> property, or manually added through this service.
  /// </summary>
  [ServiceBinding(typeof(WindowAutoCloseService))]
  public sealed class WindowAutoCloseService
  {
    private const float CloseDistanceSqr = 3 * 3;

    private readonly List<OpenWindowData> pendingOpenWindows = new List<OpenWindowData>();

    public WindowAutoCloseService(SchedulerService schedulerService)
    {
      NwModule.Instance.OnNuiEvent += OnNuiEvent;
      schedulerService.ScheduleRepeating(CheckForWindowsToClose, TimeSpan.FromSeconds(1));
    }

    public void RegisterWindowForAutoClose(IWindowController windowController)
    {
      Location openLocation = windowController.Token.Player.LoginCreature?.Location;
      if (openLocation == null)
      {
        return;
      }

      pendingOpenWindows.Add(new OpenWindowData()
      {
        WindowController = windowController,
        OpenPosition = openLocation.Position,
        OpenArea = openLocation.Area,
      });
    }

    private void OnNuiEvent(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.EventType != NuiEventType.Close)
      {
        return;
      }

      for (int i = pendingOpenWindows.Count - 1; i >= 0; i--)
      {
        OpenWindowData openWindow = pendingOpenWindows[i];
        if (eventData.Token == openWindow.WindowController.Token)
        {
          pendingOpenWindows.RemoveAt(i);
          break;
        }
      }
    }

    private void CheckForWindowsToClose()
    {
      for (int i = pendingOpenWindows.Count - 1; i >= 0; i--)
      {
        OpenWindowData openWindow = pendingOpenWindows[i];
        if (!openWindow.WindowController.Token.Player.IsValid)
        {
          pendingOpenWindows.RemoveAt(i);
          continue;
        }

        Location location = openWindow.WindowController.Token.Player.ControlledCreature!.Location;
        if (location == null || location.Area != openWindow.OpenArea || Vector3.DistanceSquared(location.Position, openWindow.OpenPosition) > CloseDistanceSqr)
        {
          pendingOpenWindows.RemoveAt(i);
          openWindow.WindowController.Close();
        }
      }
    }

    private sealed class OpenWindowData
    {
      public Vector3 OpenPosition { get; init; }

      public NwArea OpenArea { get; init; }

      public IWindowController WindowController { get; init; }
    }
  }
}
