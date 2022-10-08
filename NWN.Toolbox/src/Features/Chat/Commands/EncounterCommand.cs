using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Chat
{
  [ServiceBinding(typeof(IChatCommand))]
  public sealed class EncounterCommand : IChatCommand
  {
    [Inject]
    private DatabaseManager DatabaseManager { get; init; }

    [Inject]
    private HelpCommand HelpCommand { get; init; }

    public string Command => "encounter";
    public string[] Aliases => null;
    public bool DMOnly => true;
    public Range ArgCount => 1..;

    public string Description => "Create and spawn encounter presets.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("Show this help list."),
      new CommandUsage("list", "List available encounters to spawn."),
      new CommandUsage("spawn <name>", "Spawn an encounter with the specified name."),
      new CommandUsage("create <name>", "Creates an encounter preset with the name specified."),
      new CommandUsage("delete <name>", "Deletes an encounter preset with the name specified."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      switch (args[0])
      {
        case "list" when args.Count == 1:
          ListEncounters(caller);
          break;
        case "spawn" when args.Count > 1:
          SpawnEncounterPreset(caller, string.Join(' ', args.Skip(1)));
          break;
        case "create" when args.Count > 1:
          CreateEncounterPreset(caller, string.Join(' ', args.Skip(1)));
          break;
        case "delete" when args.Count > 1:
          DeleteEncounterPreset(caller, string.Join(' ', args.Skip(1)));
          break;
        default:
          HelpCommand.ShowCommandHelpToPlayer(caller, this);
          break;
      }
    }

    private void DeleteEncounterPreset(NwPlayer caller, string presetName)
    {
      DatabaseManager.DoTransaction(database =>
      {
        EncounterModel preset = database.EncounterPresets.FirstOrDefault(model => model.Name == presetName);
        if (preset != null)
        {
          database.Remove(preset);
          caller.SendServerMessage($"Preset \"{presetName}\" deleted.");
        }
        else
        {
          caller.SendErrorMessage($"Unknown preset \"{presetName}\".");
        }
      });
    }

    private void SpawnEncounterPreset(NwPlayer caller, string presetName)
    {
      EncounterModel preset = DatabaseManager.Database.EncounterPresets.FirstOrDefault(model => model.Name == presetName);
      if (preset == null)
      {
        caller.SendErrorMessage($"Unknown encounter preset \"{presetName}\".");
        return;
      }

      caller.EnterTargetMode(eventData =>
      {
        if (eventData.IsCancelled)
        {
          return;
        }

        NwCreature playerCreature = eventData.Player.ControlledCreature!;
        NwArea area = playerCreature.Area!;
        Vector3 direction = Vector3.Normalize(playerCreature.Position - eventData.TargetPosition);
        float orientation = NwMath.VectorToAngle(direction);

        foreach (EncounterModel.EncounterSpawn spawn in preset.Creatures)
        {
          NwCreature creature = NwCreature.Deserialize(spawn.CreatureData);
          if (creature != null)
          {
            Vector3 position = new Vector3(spawn.LocalOffsetX, spawn.LocalOffsetY, spawn.LocalOffsetZ);
            position = eventData.TargetPosition + position;
            creature.Location = Location.Create(area, position, orientation);
          }
        }
      });
    }

    private void CreateEncounterPreset(NwPlayer caller, string presetName)
    {
      if (DatabaseManager.Database.EncounterPresets.Any(model => model.Name == presetName))
      {
        caller.SendErrorMessage($"Preset {presetName} already exists. Delete it first to replace it.");
        return;
      }

      caller.SendServerMessage($"Creating encounter \"{presetName}\".\n" +
        "Please select creatures that should be included in the encounter.\n" +
        "The first creature selected will be the center creature.\n" +
        "Other creatures will spawn relative to this creature's position when spawning the encounter.");

      List<NwCreature> creatures = new List<NwCreature>();
      caller.EnterTargetMode(eventData => SelectCreature(eventData, presetName, creatures), ObjectTypes.Creature);
    }

    private void SelectCreature(ModuleEvents.OnPlayerTarget eventData, string presetName, List<NwCreature> creatures)
    {
      if (eventData.IsCancelled)
      {
        if (creatures.Count > 0)
        {
          SavePreset(eventData.Player, presetName, creatures);
        }
      }
      else
      {
        if (eventData.TargetObject is NwCreature creature)
        {
          creatures.Add(creature);
        }

        eventData.Player.EnterTargetMode(eventData2 => SelectCreature(eventData2, presetName, creatures), ObjectTypes.Creature);
      }
    }

    private void SavePreset(NwPlayer author, string name, List<NwCreature> creatures)
    {
      EncounterModel encounterData = new EncounterModel
      {
        Name = name,
        Author = author.PlayerName,
        Created = DateTime.UtcNow,
        Updated = DateTime.UtcNow,
      };

      NwCreature firstCreature = creatures.First();
      Vector3 firstCreaturePos = firstCreature.Position;

      List<EncounterModel.EncounterSpawn> spawnData = new List<EncounterModel.EncounterSpawn>();
      foreach (NwCreature creature in creatures)
      {
        Vector3 localPosition;
        if (creature == firstCreature)
        {
          localPosition = Vector3.Zero;
        }
        else
        {
          localPosition = creature.Position - firstCreaturePos;
        }

        spawnData.Add(new EncounterModel.EncounterSpawn
        {
          Encounter = encounterData,
          LocalOffsetX = localPosition.X,
          LocalOffsetY = localPosition.Y,
          LocalOffsetZ = localPosition.Z,
          CreatureData = creature.Serialize(),
        });
      }

      encounterData.Creatures = spawnData;
      DatabaseManager.DoTransaction(database =>
      {
        database.EncounterPresets.Add(encounterData);
        author.SendServerMessage($"Encounter preset {name} saved.");
      });
    }

    private void ListEncounters(NwPlayer caller)
    {
      StringBuilder message = new StringBuilder();
      message.AppendLine("===Encounters===");
      message.AppendJoin('\n', DatabaseManager.Database.EncounterPresets.Select(model => $"{model.Name} ({model.Author})"));
      message.AppendLine();
      message.AppendLine("=============");

      caller.SendServerMessage(message.ToString());
    }
  }
}
