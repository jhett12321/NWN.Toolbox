using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Config;
using Jorteck.Toolbox.Features.ChatCommands;

namespace Jorteck.Toolbox.Features.ServerRestart
{
  [ServiceBinding(typeof(IChatCommand))]
  public class GetResetTimeCommand : IChatCommand
  {
    [Inject]
    private ServerRestartService ServerRestartService { get; init; }

    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Command => "reset";
    public Range ArgCount => ..0;
    public bool DMOnly => false;
    public bool IsAvailable => ServerRestartService.IsEnabled && ConfigService.Config.ServerRestart?.ChatCommandEnable == true;

    public string Description => "Gets the next server reset time.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("Gets the next server reset time. The time is shown in the combat log."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      ServerRestartService.SendRestartTimeMessageToPlayer(caller);
    }
  }
}
