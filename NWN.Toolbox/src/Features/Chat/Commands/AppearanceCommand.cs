using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Toolbox.Features.Chat
{
  [ServiceBinding(typeof(IChatCommand))]
  public class AppearanceCommand : IChatCommand
  {
    [Inject]
    private HelpCommand HelpCommand { get; init; }

    public string Command => "appearance";
    public bool DMOnly => true;
    public Range ArgCount => 1..1;
    public string Description => "Change the appearance of an object/creature.";
    public string[] Aliases => new[] { "appear" };

    public CommandUsage[] Usages => new[]
    {
      new CommandUsage("<#>", "Applies the specified appearance to a placeable/creature."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      if (int.TryParse(args[0], out int appearance) && appearance >= 0)
      {
        caller.EnterTargetMode(eventData => OnSelectObject(eventData, appearance), new TargetModeSettings
        {
          ValidTargets = ObjectTypes.Creature | ObjectTypes.Placeable,
        });
      }
      else
      {
        HelpCommand.ShowCommandHelpToPlayer(caller, this);
      }
    }

    private void OnSelectObject(ModuleEvents.OnPlayerTarget eventData, int appearanceId)
    {
      if (eventData.IsCancelled)
      {
        return;
      }

      if (eventData.TargetObject is NwPlaceable placeable)
      {
        PlaceableTableEntry appearance = NwGameTables.PlaceableTable.ElementAtOrDefault(appearanceId);
        if (appearance != null)
        {
          eventData.Player.SendServerMessage($"Changed '{placeable.Name}' appearance from {placeable.Appearance.RowIndex}: '{placeable.Appearance.Label}' to {appearanceId}: '{appearance.Label}'");
          placeable.Appearance = appearance;
        }
        else
        {
          eventData.Player.SendErrorMessage($"Invalid appearance code {appearanceId}");
        }
      }
      else if (eventData.TargetObject is NwCreature creature)
      {
        AppearanceTableEntry appearance = NwGameTables.AppearanceTable.ElementAtOrDefault(appearanceId);
        if (appearance != null)
        {
          eventData.Player.SendServerMessage($"Changed '{creature.Name}' appearance from {creature.Appearance.RowIndex}: '{creature.Appearance.Label}' to {appearanceId}: '{appearance.Label}'");
          creature.Appearance = appearance;
        }
        else
        {
          eventData.Player.SendErrorMessage($"Invalid appearance code {appearanceId}");
        }
      }
    }
  }
}
