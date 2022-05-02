using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Design;

namespace Jorteck.Toolbox.Core
{
  internal sealed class DatabaseDesignFactory : IDesignTimeDbContextFactory<Database>
  {
    public Database CreateDbContext(string[] args)
    {
      string directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      string path = Path.Combine(directoryPath!, @"toolbox-design-data.db");
      return new Database($"Data Source={path}");
    }
  }
}
