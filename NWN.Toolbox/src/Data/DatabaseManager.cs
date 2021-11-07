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

    public DatabaseManager(ConfigService configService)
    {
      string configStoragePath = configService.Config.StoragePath;
      if (string.IsNullOrEmpty(configStoragePath))
      {
        configStoragePath = "./data.db";
      }

      string pluginPath = Path.GetDirectoryName(typeof(DatabaseManager).Assembly.Location);
      string storagePath = Path.GetFullPath(configStoragePath, pluginPath!);

      Directory.CreateDirectory(Path.GetDirectoryName(storagePath)!);
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
