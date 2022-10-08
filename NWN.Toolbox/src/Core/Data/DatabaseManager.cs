using System;
using System.IO;
using Anvil.Internal;
using Anvil.Services;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Jorteck.Toolbox.Core
{
  [ServiceBinding(typeof(DatabaseManager))]
  internal sealed class DatabaseManager : IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public readonly Database Database;

    public DatabaseManager(PluginStorageService pluginStorageService)
    {
      string pluginStoragePath = pluginStorageService.GetPluginStoragePath(typeof(DatabaseManager).Assembly);
      string storagePath = Path.Combine(pluginStoragePath, "data.db");
      string connectString = $"Data Source={storagePath}";

      Database = new Database(connectString);
      if (!EnvironmentConfig.ReloadEnabled)
      {
        Database.Database.Migrate();
      }
      else
      {
        Log.Warn("Cannot create/update database as hot reload is enabled");
      }
    }

    public void DoTransaction(Action<Database> transaction)
    {
      try
      {
        transaction?.Invoke(Database);
        Database.SaveChanges();
      }
      catch (Exception e)
      {
        Log.Error(e, "DB Operation failed.");
        throw;
      }
    }

    public void Dispose()
    {
      Database?.Dispose();
    }
  }
}
