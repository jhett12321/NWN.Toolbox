using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using NLog;

namespace Jorteck.Toolbox.Core
{
  [ServiceBinding(typeof(WindowManager))]
  public sealed class WindowManager : IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly InjectionService injectionService;
    private readonly WindowAutoCloseService windowAutoCloseService;

    private readonly List<IWindowView> windowViews;
    private readonly Dictionary<NwPlayer, List<IWindowController>> windowControllers = new Dictionary<NwPlayer, List<IWindowController>>();

    public WindowManager(InjectionService injectionService, WindowAutoCloseService windowAutoCloseService, IEnumerable<IWindowView> windowViews)
    {
      this.injectionService = injectionService;
      this.windowAutoCloseService = windowAutoCloseService;
      this.windowViews = windowViews.OrderBy(view => view.Title).ToList();

      NwModule.Instance.OnNuiEvent += OnNuiEvent;
      NwModule.Instance.OnClientLeave += OnClientLeave;
    }

    /// <summary>
    /// Opens a window view using the specified controller.
    /// </summary>
    /// <param name="player">The player to show the window.</param>
    /// <param name="configure">Additional configuration for the controller before it is initialized by the <see cref="WindowManager"/>.</param>
    /// <typeparam name="TView">The type of view to open.</typeparam>
    /// <typeparam name="TController">The type of controller for the view.</typeparam>
    /// <returns>The created controller. Null if the client cannot render windows.</returns>
    public TController OpenWindow<TView, TController>(NwPlayer player, Action<TController> configure = null)
      where TView : WindowView<TView>, new()
      where TController : WindowController<TView>, new()
    {
      TView view = (TView)GetWindowFromType(typeof(TView));
      if (view != null && player.TryCreateNuiWindow(view.WindowTemplate, out NuiWindowToken token))
      {
        TController controller = injectionService.Inject(new TController
        {
          View = view,
          Token = token,
        });

        configure?.Invoke(controller);

        InitController(controller, player);
        return controller;
      }

      return null;
    }

    /// <summary>
    /// Opens a window view using the view's default controller.
    /// </summary>
    /// <param name="player">The player opening the window.</param>
    /// <typeparam name="T">The type of view to open.</typeparam>
    public void OpenWindow<T>(NwPlayer player) where T : WindowView<T>, new()
    {
      IWindowView view = GetWindowFromType(typeof(T));
      if (view != null)
      {
        OpenWindow(player, view);
      }
    }

    /// <summary>
    /// Opens a window view using the view's default controller.
    /// </summary>
    /// <param name="player">The player opening the window.</param>
    /// <param name="view">The view to open.</param>
    public void OpenWindow(NwPlayer player, IWindowView view)
    {
      IWindowController controller = view.CreateDefaultController(player);
      if (controller == null)
      {
        return;
      }

      injectionService.Inject(controller);
      InitController(controller, player);
    }

    private IWindowView GetWindowFromType(Type windowType)
    {
      foreach (IWindowView view in windowViews)
      {
        if (view.GetType() == windowType)
        {
          return view;
        }
      }

      Log.Error("Failed to find window of type {Type}", windowType.FullName);
      return null;
    }

    private void InitController(IWindowController controller, NwPlayer player)
    {
      if (controller.AutoClose)
      {
        windowAutoCloseService.RegisterWindowForAutoClose(controller);
      }

      controller.Init();
      windowControllers.AddElement(player, controller);
    }

    private void OnNuiEvent(ModuleEvents.OnNuiEvent eventData)
    {
      if (windowControllers.TryGetValue(eventData.Player, out List<IWindowController> playerControllers))
      {
        IWindowController controller = null;
        int index;

        for (index = 0; index < playerControllers.Count; index++)
        {
          IWindowController playerController = playerControllers[index];
          if (eventData.Token == playerController.Token)
          {
            controller = playerController;
            break;
          }
        }

        if (controller == null)
        {
          return;
        }

        controller.ProcessEvent(eventData);
        if (eventData.EventType == NuiEventType.Close)
        {
          controller.Close(false);
          playerControllers.RemoveAt(index);
        }
      }
    }

    private void OnClientLeave(ModuleEvents.OnClientLeave eventData)
    {
      if (windowControllers.TryGetValue(eventData.Player, out List<IWindowController> playerControllers))
      {
        foreach (IWindowController controller in playerControllers)
        {
          controller.Close();
        }

        windowControllers.Remove(eventData.Player);
      }
    }

    void IDisposable.Dispose()
    {
      foreach (List<IWindowController> controllers in windowControllers.Values)
      {
        foreach (IWindowController controller in controllers)
        {
          controller.Close();
        }
      }

      windowControllers.Clear();
    }
  }
}
