using Anvil.API;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox
{
  public static class ObjectExtensions
  {
    public static string GetTypeName(this NwObject gameObject)
    {
      return gameObject switch
      {
        NwArea => "Area",
        NwAreaOfEffect => "Area of Effect",
        NwCreature creature when creature.IsPlayerControlled => "Player",
        NwCreature => "Creature",
        NwDoor => "Door",
        NwEncounter => "Encounter",
        NwItem => "Item",
        NwPlaceable => "Placeable",
        NwSound => "Sound",
        NwStore => "Store",
        NwTrigger => "Trigger",
        NwWaypoint => "Waypoint",
        NwModule => "Module",
        _ => "Unknown",
      };
    }

    public static ObjectSelectionTypes GetSelectionType(this NwObject gameObject)
    {
      return gameObject switch
      {
        NwArea => ObjectSelectionTypes.All,
        NwAreaOfEffect => ObjectSelectionTypes.AreaOfEffect,
        NwCreature => ObjectSelectionTypes.Creature,
        NwDoor => ObjectSelectionTypes.Door,
        NwEncounter => ObjectSelectionTypes.Encounter,
        NwItem => ObjectSelectionTypes.Item,
        NwPlaceable => ObjectSelectionTypes.Placeable,
        NwSound => ObjectSelectionTypes.Sound,
        NwStore => ObjectSelectionTypes.Store,
        NwTrigger => ObjectSelectionTypes.Trigger,
        NwWaypoint => ObjectSelectionTypes.Waypoint,
        NwModule => ObjectSelectionTypes.All,
        _ => ObjectSelectionTypes.All,
      };
    }
  }
}
