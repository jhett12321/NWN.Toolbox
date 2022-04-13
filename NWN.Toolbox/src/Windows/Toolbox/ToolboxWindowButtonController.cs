using System;
using System.Numerics;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using Jorteck.Toolbox.Config;

namespace Jorteck.Toolbox
{
  public sealed class ToolboxWindowButtonController : WindowController<ToolboxWindowButtonView>
  {
    [Inject]
    public Lazy<WindowManager> WindowManager { private get; init; }

    [Inject]
    private ConfigService ConfigService { get; init; }

    public override void Init()
    {
      Vector2 windowPos = ConfigService?.Config?.ToolboxWindows?.ToolboxButtonPosition ?? new Vector2(725f, 0f);
      Token.SetBindValue(View.ButtonGeometry, new NuiRect(windowPos.X, windowPos.Y, 160f, 60f));
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

    protected override void OnClose() {}

    private void HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.Button.Id)
      {
        WindowManager.Value.OpenWindow<ToolboxWindowView>(Token.Player);
      }
    }
  }
}
