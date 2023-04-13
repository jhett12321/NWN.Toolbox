using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Features.Chat;

namespace Jorteck.Toolbox.Features
{
  [ServiceBinding(typeof(IChatCommand))]
  public class SaveThrowCommand : IChatCommand
  {
    [Inject]
    private DiceRollService DiceRollService { get; init; }

    [Inject]
    private HelpCommand HelpCommand { get; init; }

    public string Command => "save";
    public bool DMOnly => false;
    public Range ArgCount => 1..1;

    public string Description => "Roll a saving throw.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("fortitude/fort", "Roll a Fortitude saving throw."),
      new CommandUsage("reflex", "Roll a Reflex saving throw."),
      new CommandUsage("will", "Roll a Will saving throw."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      switch (args[0])
      {
        case "fortitude":
        case "fort":
          DiceRollService.SavingThrowRoll(caller.LoginCreature, SavingThrow.Fortitude, DiceRollService.GetBroadcastMode(caller));
          break;
        case "reflex":
          DiceRollService.SavingThrowRoll(caller.LoginCreature, SavingThrow.Reflex, DiceRollService.GetBroadcastMode(caller));
          break;
        case "will":
          DiceRollService.SavingThrowRoll(caller.LoginCreature, SavingThrow.Will, DiceRollService.GetBroadcastMode(caller));
          break;
        default:
          HelpCommand.ShowCommandHelpToPlayer(caller, this);
          break;
      }
    }
  }
}
