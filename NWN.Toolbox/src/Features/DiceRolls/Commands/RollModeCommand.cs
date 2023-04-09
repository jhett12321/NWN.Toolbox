using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Features.Chat;

namespace Jorteck.Toolbox.Features
{
  [ServiceBinding(typeof(IChatCommand))]
  public sealed class RollModeCommand : IChatCommand
  {
    [Inject]
    private DiceRollService DiceRollService { get; init; }

    public string Command => "rollmode";
    public bool DMOnly => false;
    public Range ArgCount => 1..1;

    public string Description => "Configure the privacy of your dice rolls.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("private", "Sets your dice roll visibility to private. Visible to yourself and DMs."),
      new CommandUsage("local", "Sets your dice roll visibility to local. Visible to nearby players in chat range."),
      new CommandUsage("dm", "Sets your dice roll visibility to DM. Visible only to DMs."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      switch (args[0])
      {
        case "private":
          DiceRollService.SetBroadcastMode(caller, RollBroadcastTargets.PrivateChat | RollBroadcastTargets.DM);
          caller.SendServerMessage("Set dice roll broadcast mode to private.");
          break;
        case "local":
          DiceRollService.SetBroadcastMode(caller, RollBroadcastTargets.LocalTalk | RollBroadcastTargets.DM);
          caller.SendServerMessage("Set dice roll broadcast mode to local.");
          break;
        case "dm":
          DiceRollService.SetBroadcastMode(caller, RollBroadcastTargets.DM);
          caller.SendServerMessage("Set dice roll broadcast mode to dm.");
          break;
      }
    }
  }
}
