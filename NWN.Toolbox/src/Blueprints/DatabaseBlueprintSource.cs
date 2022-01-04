using System.Collections.Generic;
using System.Linq;

namespace Jorteck.Toolbox
{
  //[ServiceBinding(typeof(IBlueprintSource))] // Disabled for now...
  internal sealed class DatabaseBlueprintSource : IBlueprintSource
  {
    private readonly DatabaseManager databaseManager;

    public DatabaseBlueprintSource(DatabaseManager databaseManager)
    {
      this.databaseManager = databaseManager;
    }

    public IEnumerable<IBlueprint> GetBlueprints(BlueprintObjectType blueprintType, int start, string search, int count)
    {
      if (string.IsNullOrEmpty(search))
      {
        return databaseManager.Database.BlueprintPresets.Where(model => model.Type == blueprintType)
          .Skip(start)
          .Take(count)
          .Select(model => new DatabaseBlueprint(model));
      }

      return databaseManager.Database.BlueprintPresets.Where(model => model.Type == blueprintType && model.Name.StartsWith(search))
        .Skip(start)
        .Take(count)
        .Select(model => new DatabaseBlueprint(model));
    }
  }
}
