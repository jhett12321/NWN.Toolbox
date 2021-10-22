using Anvil.API;
using Anvil.API.Events;

namespace Jorteck.Toolbox
{
  internal interface IWindowController
  {
    public NwPlayer Player { get; }

    public int Token { get; }

    public void Init();

    public void ProcessEvent(ModuleEvents.OnNuiEvent eventData);

    public void OnClose();
  }
}
