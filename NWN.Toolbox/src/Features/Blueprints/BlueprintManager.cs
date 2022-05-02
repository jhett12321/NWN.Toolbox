using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  [ServiceBinding(typeof(BlueprintManager))]
  public sealed class BlueprintManager
  {
    private readonly List<IBlueprintSource> blueprintSources;

    public BlueprintManager(IEnumerable<IBlueprintSource> blueprintSources)
    {
      this.blueprintSources = blueprintSources.ToList();
    }

    public List<IBlueprint> GetMatchingBlueprints(BlueprintObjectType objectType, string search, int max)
    {
      List<IBlueprint> results = new List<IBlueprint>();
      // First, try to get equal results from each source
      int each = max / blueprintSources.Count;

      foreach (IBlueprintSource blueprintSource in blueprintSources)
      {
        results.AddRange(blueprintSource.GetBlueprints(objectType, 0, search, each));
      }

      if (results.Count < max)
      {
        foreach (IBlueprintSource blueprintSource in blueprintSources)
        {
          results.AddRange(blueprintSource.GetBlueprints(objectType, each, search, max - results.Count));
        }
      }

      results.Sort((blueprintA, blueprintB) => string.Compare(blueprintA.FullName, blueprintB.FullName, StringComparison.OrdinalIgnoreCase));
      return results;
    }
  }
}
