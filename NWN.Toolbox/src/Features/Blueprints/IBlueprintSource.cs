using System.Collections.Generic;

namespace Jorteck.Toolbox
{
  public interface IBlueprintSource
  {
    IEnumerable<IBlueprint> GetBlueprints(BlueprintObjectType blueprintType, int start, string search, int count);
  }
}
