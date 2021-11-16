using Anvil.API;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  [ServiceBinding(typeof(IWindowView))]
  public abstract class WindowView<TView> : IWindowView
    where TView : WindowView<TView>, new()
  {
    public abstract string Id { get; }

    public abstract string Title { get; }

    public abstract NuiWindow WindowTemplate { get; }

    public virtual bool ListInToolbox => true;

    public abstract IWindowController CreateDefaultController(NwPlayer player);

    protected T CreateController<T>(NwPlayer player) where T : WindowController<TView>, new()
    {
      return new T
      {
        View = (TView)this,
        Player = player,
        Token = player.CreateNuiWindow(WindowTemplate),
      };
    }
  }
}
