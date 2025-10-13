using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Features.Chat;

namespace Jorteck.Toolbox.Features
{
  [ServiceBinding(typeof(IChatCommand))]
  public class AbilityCheckCommand : IChatCommand
  {
    [Inject]
    private DiceRollService DiceRollService { get; init; } = null!;

    [Inject]
    private HelpCommand HelpCommand { get; init; } = null!;

    public string Command => "ability";
    public bool DMOnly => false;
    public Range ArgCount => 1..1;

    public string Description => "Perform an ability check.";

    public CommandUsage[] Usages { get; }

    public AbilityCheckCommand()
    {
      string[] abilityNames = Enum.GetNames<Ability>();
      Usages = new CommandUsage[abilityNames.Length];

      for (int i = 0; i < abilityNames.Length; i++)
      {
        string ability = abilityNames[i];
        Usages[i] = new CommandUsage(ability.ToLower(), $"Roll a {ability} check.");
      }
    }

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      NwCreature? callerCreature = caller.ControlledCreature;
      if (callerCreature == null)
      {
        return;
      }

      if (Enum.TryParse(args[0], true, out Ability ability))
      {
        DiceRollService.AbilityRoll(callerCreature, ability, DiceRollService.GetBroadcastMode(caller));
      }
      else
      {
        HelpCommand.ShowCommandHelpToPlayer(caller, this);
      }
    }
  }
}
