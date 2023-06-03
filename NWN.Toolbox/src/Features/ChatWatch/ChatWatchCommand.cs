using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Features.Chat;

namespace Jorteck.Toolbox.Features
{
  [ServiceBinding(typeof(IChatCommand))]
  public class ChatWatchCommand : IChatCommand
  {
    [Inject]
    private ChatWatchService ChatWatchService { get; init; }

    [Inject]
    private HelpCommand HelpCommand { get; init; }

    public string Command => "chatwatch";
    public Range ArgCount => 1..1;
    public bool DMOnly => true;
    public string Description => "Listen for chat messages sent by players.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("player", "Listen to messages sent by the selected player."),
      new CommandUsage("party", "Listen to messages sent by players in the party of the selected player."),
      new CommandUsage("area", "Listen to messages sent in a certain area."),
      new CommandUsage("module", "Listen to messages sent by all players."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      switch (args[0])
      {
        case "player":
          caller.EnterPlayerTargetMode(eventData => ChatWatchService.ToggleSubscribe(eventData.Caller, eventData.Target));
          break;
        case "area":
          NwArea area = caller.ControlledCreature?.Area;
          if (area != null)
          {
            ChatWatchService.ToggleSubscribe(caller, area);
          }

          break;
        case "module":
          ChatWatchService.ToggleSubscribe(caller);
          break;
        default:
          HelpCommand.ShowCommandHelpToPlayer(caller, this);
          break;
      }
    }
  }
}
