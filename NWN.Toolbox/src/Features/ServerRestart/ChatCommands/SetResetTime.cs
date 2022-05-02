using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;
using Jorteck.Toolbox.Features.ChatCommands;

namespace Jorteck.Toolbox.Features.ServerRestart
{
  [ServiceBinding(typeof(IChatCommand))]
  public class SetResetTime : IChatCommand
  {
    [Inject]
    private ServerRestartService ServerRestartService { get; init; }

    [Inject]
    private ConfigService ConfigService { get; init; }

    [Inject]
    private HelpCommand HelpCommand { get; init; }

    public string Command => "setreset";
    public Range ArgCount => 1..1;
    public bool DMOnly => true;
    public bool IsAvailable => ServerRestartService.IsEnabled && ConfigService.Config.ServerRestart?.ChatCommandEnable == true;

    public string Description => "Sets the next reset time.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("<time_secs>", "The amount of time (in seconds) to schedule a server restart. Must be a positive value."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      if (uint.TryParse(args[0], out uint adjustSeconds))
      {
        ServerRestartService.TimeUntilRestart = TimeSpan.FromSeconds(adjustSeconds);
        ServerRestartService.SendRestartTimeMessageToAllPlayers();
      }
      else
      {
        HelpCommand.ShowCommandHelpToPlayer(caller, this);
      }
    }
  }
}

