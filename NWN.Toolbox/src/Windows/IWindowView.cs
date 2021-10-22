using Anvil.API;

namespace Jorteck.Toolbox
{
  /// <summary>
  /// Internal interface - implement <see cref="WindowView{TView,TController}"/> instead.
  /// </summary>
  public interface IWindowView
  {
    public string Title { get; }

    internal IWindowController CreateController(NwPlayer player);
  }
}
