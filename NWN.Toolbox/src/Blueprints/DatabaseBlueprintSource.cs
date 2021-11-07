using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  [ServiceBinding(typeof(IBlueprintSource))]
  internal sealed class DatabaseBlueprintSource : IBlueprintSource
  {
    private readonly DatabaseManager databaseManager;

    public DatabaseBlueprintSource(DatabaseManager databaseManager)
    {
      this.databaseManager = databaseManager;
    }

    public IEnumerable<IBlueprint> GetBlueprints(BlueprintObjectType blueprintType, int start, string search, int count)
    {
      return databaseManager.Database.BlueprintPresets.Where(model => model.Type == blueprintType)
        .Where(model => model.Name.StartsWith(search, StringComparison.InvariantCultureIgnoreCase))
        .Skip(start)
        .Take(count)
        .Select(model => new DatabaseBlueprint(model));
    }
  }
}
