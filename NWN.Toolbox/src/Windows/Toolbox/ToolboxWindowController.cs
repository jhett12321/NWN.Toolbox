using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  public sealed class ToolboxWindowController : WindowController<ToolboxWindowController, ToolboxWindowView>
  {
    [Inject]
    public Lazy<IEnumerable<IWindowView>> AvailableWindows { private get; init; }

    [Inject]
    public Lazy<WindowManager> WindowManager { private get; init; }

    private List<IWindowView> allWindows;
    private readonly Dictionary<string, IWindowView> idToWindowMap = new Dictionary<string, IWindowView>();

    public override void Init()
    {
      allWindows = AvailableWindows.Value.OrderBy(view => view.Title).ToList();
      RefreshWindowList();
    }

    public override void ProcessEvent(ModuleEvents.OnNuiEvent eventData)
    {
      switch (eventData.EventType)
      {
        case NuiEventType.Click:
          HandleButtonClick(eventData);
          break;
      }
    }

    protected override void OnClose()
    {
      idToWindowMap.Clear();
    }

    private void HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.SearchButton.Id)
      {
        RefreshWindowList();
      }
      else if (idToWindowMap.TryGetValue(eventData.ElementId, out IWindowView windowView))
      {
        WindowManager.Value.OpenWindow(Player, windowView);
      }
    }

    private void RefreshWindowList()
    {
      string search = GetBindValue(View.Search);
      List<IWindowView> windows = allWindows.Where(view => view.ListInToolbox && view.Title.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

      NuiColumn subViewRoot = new NuiColumn();
      for (int i = 0; i < windows.Count; i++)
      {
        subViewRoot.Children.Add(CreateWindowButton(windows[i], i));
      }

      Player.NuiSetGroupLayout(Token, View.CreatorListContainer.Id, subViewRoot);
    }

    private NuiElement CreateWindowButton(IWindowView window, int index)
    {
      string buttonId = $"btn_{index}";
      idToWindowMap[buttonId] = window;

      return new NuiButton(window.Title)
      {
        Id = buttonId,
        Height = 30f,
        Width = window.Title.Length * 8f,
      };
    }
  }
}
