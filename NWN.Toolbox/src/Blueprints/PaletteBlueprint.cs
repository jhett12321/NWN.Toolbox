using System;
using Anvil.API;

namespace Jorteck.Toolbox
{
  internal sealed class PaletteBlueprint : IBlueprint
  {
    public string ResRef { get; init; }
    public string Name { get; init; }
    public string Category { get; init; }
    public BlueprintObjectType ObjectType { get; init; }

    public NwObject Create(Location location)
    {
      return ObjectType switch
      {
        BlueprintObjectType.Creature => NwCreature.Create(ResRef, location),
        BlueprintObjectType.Item => NwItem.Create(ResRef, location),
        BlueprintObjectType.Placeable => NwPlaceable.Create(ResRef, location),
        BlueprintObjectType.Store => NwStore.Create(ResRef, location),
        BlueprintObjectType.Waypoint => NwWaypoint.Create(ResRef, location),
        _ => throw new NotImplementedException($"{ObjectType} blueprints are not supported.")
      };
    }
  }
}
