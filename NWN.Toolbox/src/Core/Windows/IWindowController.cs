using Anvil.API;
using Anvil.API.Events;

namespace Jorteck.Toolbox.Core
{
  /// <summary>
  /// Internal interface - implement <see cref="WindowController{TView}"/> instead.
  /// </summary>
  public interface IWindowController
  {
    public NuiWindowToken Token { get; init; }

    public void Init();

    public void ProcessEvent(ModuleEvents.OnNuiEvent eventData);

    public void Close(bool destroyWindow = true);
  }
}
