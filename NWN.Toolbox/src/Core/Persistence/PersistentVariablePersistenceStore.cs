using Anvil.API;

namespace Jorteck.Toolbox.Core.Persistence
{
  public sealed class PersistentVariablePersistenceStore : IPersistenceStore
  {
    public T? GetState<T>(NwPlayer player, string key)
    {
      PersistentVariableStruct<T>? value = player.ControlledCreature?.GetObjectVariable<PersistentVariableStruct<T>>(key);
      return value?.HasValue == true ? value.Value : default;
    }

    public void UpdateState<T>(NwPlayer player, string key, T value)
    {
      player.ControlledCreature!.GetObjectVariable<PersistentVariableStruct<T>>(key).Value = value;
    }
  }
}
