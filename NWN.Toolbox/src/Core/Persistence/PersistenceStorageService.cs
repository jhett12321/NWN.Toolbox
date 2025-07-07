using System;
using Anvil.API;
using Anvil.Services;
using NLog;

namespace Jorteck.Toolbox.Core.Persistence
{
  [ServiceBinding(typeof(PersistenceStorageService))]
  public class PersistenceStorageService : IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private IPersistenceStore activeStore;

    public PersistenceStorageService(SchedulerService schedulerService)
    {
      schedulerService.Schedule(AssignDefaultStoreIfUnset, SchedulerService.NextUpdate);
    }

    private void AssignDefaultStoreIfUnset()
    {
      if (activeStore == null)
      {
        SetActiveStore(new PersistentVariablePersistenceStore());
      }
    }

    public void SetActiveStore(IPersistenceStore store)
    {
      activeStore = store;
      Log.Info($"Using '{store.GetType().FullName}' for toolbox persistent settings/data.");
    }

    public T GetState<T>(NwPlayer player, string key)
    {
      return activeStore.GetState<T>(player, key);
    }

    public void UpdateState<T>(NwPlayer player, string key, T value)
    {
      activeStore.UpdateState(player, key, value);
    }

    public void Dispose()
    {
      activeStore = null;
    }
  }
}
