using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  public sealed class ToolboxWindowButtonController : WindowController<ToolboxWindowButtonController, ToolboxWindowButtonView>
  {
    [Inject]
    public Lazy<WindowManager> WindowManager { private get; init; }

    public override void Init() {}

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
        WindowManager.Value.OpenWindow<ToolboxWindowView>(Player);
      }
    }
  }
}
