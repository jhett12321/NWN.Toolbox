using Anvil.API;

namespace Jorteck.Toolbox
{
  /// <summary>
  /// Internal interface - implement <see cref="WindowView{TView}"/> instead.
  /// </summary>
  public interface IWindowView
  {
    public string Id { get; }

    public string Title { get; }

    public bool ListInToolbox { get; }

    public IWindowController CreateDefaultController(NwPlayer player);
  }
}
