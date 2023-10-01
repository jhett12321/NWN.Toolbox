using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Features.Chat;

namespace Jorteck.Toolbox.Features
{
  [ServiceBinding(typeof(IChatCommand))]
  public sealed class RollDiceCommand : IChatCommand
  {
    [Inject]
    private DiceRollService DiceRollService { get; init; }

    [Inject]
    private HelpCommand HelpCommand { get; init; }

    public string Command => "roll";
    public bool DMOnly => false;
    public Range ArgCount => 1..1;

    public string Description => "Perform a dice roll.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("<n>d<n>", "Perform the specified dice roll. E.g. '2d6', '1d20'"),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      string[] rollValues = args[0].Split('d', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

      try
      {
        if (rollValues.Length == 2 && int.TryParse(rollValues[0], out int diceCount) && int.TryParse(rollValues[1], out int numSides) && diceCount > 0 && numSides > 0)
        {
          DiceRollService.RollDice(caller.ControlledCreature, numSides, diceCount, DiceRollService.GetBroadcastMode(caller));
          return;
        }

        if (rollValues.Length == 1 && int.TryParse(rollValues[0], out numSides))
        {
          DiceRollService.RollDice(caller.ControlledCreature, numSides, 1, DiceRollService.GetBroadcastMode(caller));
          return;
        }
      }
      catch (OverflowException) {}
      catch (ArgumentOutOfRangeException) {}

      HelpCommand.ShowCommandHelpToPlayer(caller, this);
    }
  }
}
