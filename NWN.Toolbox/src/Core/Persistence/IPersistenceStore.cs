using Anvil.API;

namespace Jorteck.Toolbox.Core.Persistence
{
  public interface IPersistenceStore
  {
    T GetState<T>(NwPlayer player, string key);

    void UpdateState<T>(NwPlayer player, string key, T value);
  }
}
