using System;
using Anvil.API;

namespace Jorteck.Toolbox.Features.Blueprints
{
  internal sealed class PaletteBlueprint : IBlueprint
  {
    public required string ResRef { get; init; }
    public required string FullName { get; init; }
    public required string Name { get; init; }
    public required string Category { get; init; }
    public required float? CR { get; init; }
    public required string? Faction { get; init; }
    public required BlueprintObjectType ObjectType { get; init; }

    public NwObject? Create(Location location)
    {
      return ObjectType switch
      {
        BlueprintObjectType.Creature => NwCreature.Create(ResRef, location),
        BlueprintObjectType.Item => NwItem.Create(ResRef, location),
        BlueprintObjectType.Placeable => NwPlaceable.Create(ResRef, location),
        BlueprintObjectType.Store => NwStore.Create(ResRef, location),
        BlueprintObjectType.Waypoint => NwWaypoint.Create(ResRef, location),
        BlueprintObjectType.Door => NwDoor.Create(ResRef, location),
        BlueprintObjectType.Encounter => NwEncounter.Create(ResRef, location),
        BlueprintObjectType.Sound => NwSound.Create(ResRef, location),
        BlueprintObjectType.Trigger => NwTrigger.Create(ResRef, location),
        _ => throw new NotImplementedException($"{ObjectType} blueprints are not supported."),
      };
    }

    public NwItem? Create(NwGameObject? owner)
    {
      if (ObjectType is not BlueprintObjectType.Item || owner == null)
      {
        return null;
      }

      NwItem? item = null;
      if (owner is NwCreature creature)
      {
        item = NwItem.Create(ResRef, owner.Location!);
        if (item != null)
        {
          creature.AcquireItem(item);
        }
      }
      else if (owner is NwPlaceable placeable && placeable.HasInventory)
      {
        item = NwItem.Create(ResRef, owner.Location!);
        if (item != null)
        {
          placeable.AcquireItem(item);
        }
      }

      return item;
    }
  }
}
