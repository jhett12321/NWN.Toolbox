using Anvil.API;

namespace Jorteck.Toolbox.Core.Persistence
{
  public sealed class PersistentVariablePersistenceStore : IPersistenceStore
  {
    public T GetState<T>(NwPlayer player, string key)
    {
      return player.ControlledCreature?.GetObjectVariable<PersistentVariableStruct<T>>(key);
    }

    public void UpdateState<T>(NwPlayer player, string key, T value)
    {
      player.ControlledCreature!.GetObjectVariable<PersistentVariableStruct<T>>(key).Value = value;
    }
  }
}
