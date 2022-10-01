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

    /// <summary>
    /// Assigns the created value to an additional variable. Useful for adding field references to nested NUI elements.
    /// </summary>
    /// <param name="assign">The field/variable to assign.</param>
    /// <param name="value">The value to assign</param>
    /// <typeparam name="T">The type of value.</typeparam>
    /// <returns>The input value provided in the value parameter.</returns>
    protected T Assign<T>(out T assign, T value)
    {
      assign = value;
      return value;
    }
  }
}
