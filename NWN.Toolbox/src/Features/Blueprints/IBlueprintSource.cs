using System.Collections.Generic;

namespace Jorteck.Toolbox.Features.Blueprints
{
  public interface IBlueprintSource
  {
    IEnumerable<IBlueprint> GetBlueprints(BlueprintObjectType blueprintType, int start, string search, int count);
  }
}
