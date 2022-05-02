using Anvil.API;
using Anvil.Services;

namespace Jorteck.Toolbox.Core
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
      if (player.TryCreateNuiWindow(WindowTemplate, out NuiWindowToken token))
      {
        return new T
        {
          View = (TView)this,
          Token = token,
        };
      }

      return null;
    }
  }
}
