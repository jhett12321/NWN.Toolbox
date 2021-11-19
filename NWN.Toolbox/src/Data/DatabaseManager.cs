using System;
using System.IO;
using Anvil.Services;
using Microsoft.EntityFrameworkCore;

namespace Jorteck.Toolbox
{
  [ServiceBinding(typeof(DatabaseManager))]
  internal sealed class DatabaseManager : IDisposable
  {
    public readonly Database Database;

    public DatabaseManager(PluginStorageService pluginStorageService)
    {
      string pluginStoragePath = pluginStorageService.GetPluginStoragePath(typeof(DatabaseManager).Assembly);
      string storagePath = Path.Combine(pluginStoragePath, "data.db");
      string connectString = $"Data Source={storagePath}";

      Database = new Database(connectString);
      Database.Database.Migrate();
    }

    public void Dispose()
    {
      Database?.Dispose();
    }
  }
}
