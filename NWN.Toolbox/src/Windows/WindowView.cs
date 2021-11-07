using Anvil.API;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  [ServiceBinding(typeof(IWindowView))]
  public abstract class WindowView<TView, TController> : IWindowView
    where TView : WindowView<TView, TController>, new()
    where TController : WindowController<TController, TView>, new()
  {
    public abstract string Id { get; }

    public abstract string Title { get; }

    public abstract NuiWindow WindowTemplate { get; }

    public virtual bool ListInToolbox => true;

    IWindowController IWindowView.CreateController(NwPlayer player)
    {
      return new TController
      {
        View = (TView)this,
        Player = player,
        Token = player.CreateNuiWindow(WindowTemplate),
      };
    }
  }
}
