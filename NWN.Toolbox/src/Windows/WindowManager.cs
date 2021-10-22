using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using NLog;

namespace Jorteck.Toolbox
{
  [ServiceBinding(typeof(WindowManager))]
  public sealed class WindowManager
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly InjectionService injectionService;

    private readonly List<IWindowView> windowViews;
    private readonly Dictionary<NwPlayer, List<IWindowController>> windowControllers = new Dictionary<NwPlayer, List<IWindowController>>();

    public WindowManager(InjectionService injectionService, IEnumerable<IWindowView> views)
    {
      this.injectionService = injectionService;
      this.windowViews = views.OrderBy(view => view.Title).ToList();

      NwModule.Instance.OnNuiEvent += OnNuiEvent;
      NwModule.Instance.OnClientLeave += OnClientLeave;
    }

    public void OpenWindow<T>(NwPlayer player) where T : IWindowView
    {
      Type windowType = typeof(T);
      foreach (IWindowView view in windowViews)
      {
        if (view.GetType() == windowType)
        {
          OpenWindow(player, view);
          return;
        }
      }

      Log.Error("Failed to create window of type {Type}", windowType.FullName);
    }

    public void OpenWindow(NwPlayer player, IWindowView view)
    {
      IWindowController controller = injectionService.Inject(view.CreateController(player));
      controller.Init();
      windowControllers.AddElement(player, controller);
    }

    private void OnNuiEvent(ModuleEvents.OnNuiEvent eventData)
    {
      if (windowControllers.TryGetValue(eventData.Player, out List<IWindowController> playerControllers))
      {
        IWindowController controller = playerControllers.FirstOrDefault(windowController => windowController.Token == eventData.WindowToken);
        controller?.ProcessEvent(eventData);
      }
    }

    private void OnClientLeave(ModuleEvents.OnClientLeave eventData)
    {
      if (windowControllers.TryGetValue(eventData.Player, out List<IWindowController> playerControllers))
      {
        foreach (IWindowController controller in playerControllers)
        {
          controller.OnClose();
        }

        windowControllers.Remove(eventData.Player);
      }
    }
  }
}
