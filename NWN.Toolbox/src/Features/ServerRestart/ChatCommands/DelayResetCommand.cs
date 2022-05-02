using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;
using Jorteck.Toolbox.Features.ChatCommands;

namespace Jorteck.Toolbox.Features.ServerRestart
{
  [ServiceBinding(typeof(IChatCommand))]
  public class DelayResetCommand : IChatCommand
  {
    [Inject]
    private ServerRestartService ServerRestartService { get; init; }

    [Inject]
    private ConfigService ConfigService { get; init; }

    [Inject]
    private HelpCommand HelpCommand { get; init; }

    public string Command => "delayreset";
    public Range ArgCount => 1..1;
    public bool DMOnly => true;
    public bool IsAvailable => ServerRestartService.IsEnabled && ConfigService.Config.ServerRestart?.ChatCommandEnable == true;

    public string Description => "Delays the server reset time by the specified amount.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("<time_secs>", "The time in seconds to delay the server reset. Supports negative values."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      if (int.TryParse(args[0], out int adjustSeconds))
      {
        ServerRestartService.TimeUntilRestart += TimeSpan.FromSeconds(adjustSeconds);
        ServerRestartService.SendRestartTimeMessageToAllPlayers();
      }
      else
      {
        HelpCommand.ShowCommandHelpToPlayer(caller, this);
      }
    }
  }
}

