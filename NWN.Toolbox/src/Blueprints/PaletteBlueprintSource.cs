using System.Collections.Generic;
using System.Linq;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  [ServiceBinding(typeof(IBlueprintSource))]
  internal sealed class PaletteBlueprintSource : IBlueprintSource
  {
    private const string CreaturePaletteResRef = "creaturepal";
    private const string DoorPaletteResRef = "doorpal";
    private const string EncounterPaletteResRef = "encounterpal";
    private const string ItemPaletteResRef = "itempal";
    private const string PlaceablePaletteResRef = "placeablepal";
    private const string SoundPaletteResRef = "soundpal";
    private const string StorePaletteResRef = "storepal";
    private const string TriggerPaletteResRef = "triggerpal";
    private const string WaypointPaletteResRef = "waypointpal";

    private readonly List<IBlueprint> blueprints = new List<IBlueprint>();

    public PaletteBlueprintSource(InjectionService injectionService)
    {
      blueprints.AddRange(injectionService.Inject(new Palette(CreaturePaletteResRef, BlueprintObjectType.Creature)).GetBlueprints());
      blueprints.AddRange(injectionService.Inject(new Palette(DoorPaletteResRef, BlueprintObjectType.Door)).GetBlueprints());
      blueprints.AddRange(injectionService.Inject(new Palette(EncounterPaletteResRef, BlueprintObjectType.Encounter)).GetBlueprints());
      blueprints.AddRange(injectionService.Inject(new Palette(ItemPaletteResRef, BlueprintObjectType.Item)).GetBlueprints());
      blueprints.AddRange(injectionService.Inject(new Palette(PlaceablePaletteResRef, BlueprintObjectType.Placeable)).GetBlueprints());
      blueprints.AddRange(injectionService.Inject(new Palette(SoundPaletteResRef, BlueprintObjectType.Sound)).GetBlueprints());
      blueprints.AddRange(injectionService.Inject(new Palette(StorePaletteResRef, BlueprintObjectType.Store)).GetBlueprints());
      blueprints.AddRange(injectionService.Inject(new Palette(TriggerPaletteResRef, BlueprintObjectType.Trigger)).GetBlueprints());
      blueprints.AddRange(injectionService.Inject(new Palette(WaypointPaletteResRef, BlueprintObjectType.Waypoint)).GetBlueprints());
    }

    public IEnumerable<IBlueprint> GetBlueprints(BlueprintObjectType blueprintType, int start, string search, int count)
    {
      return blueprints.Where(blueprint => blueprint.Name.Contains(search)).Skip(start).Take(count);
    }
  }
}
