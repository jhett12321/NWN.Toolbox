using Anvil.API.Events;

namespace Jorteck.Toolbox
{
  /// <summary>
  /// Internal interface - implement <see cref="WindowController{TView}"/> instead.
  /// </summary>
  public interface IWindowController
  {
    public WindowToken Token { get; init; }

    public void Init();

    public void ProcessEvent(ModuleEvents.OnNuiEvent eventData);

    public void Close();
  }
}
