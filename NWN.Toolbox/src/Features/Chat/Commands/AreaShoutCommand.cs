using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;

namespace Jorteck.Toolbox.Features.Chat
{
  [ServiceBinding(typeof(IChatCommand))]
  public sealed class AreaShoutCommand : IChatCommand
  {
    [Inject]
    private AreaShoutService AreaShoutService { get; init; }

    public string Command => "areashout";
    public bool DMOnly => false;
    public Range ArgCount => 1..;
    public string Description => "Shouts a message to the current area.";
    public string[] Aliases => new[] { "as", "ashout" };

    public CommandUsage[] Usages => new[]
    {
      new CommandUsage("<message>", "Shouts the specified message to your current area."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      AreaShoutService.SendMessage(caller.ControlledCreature, string.Join(' ', args));
    }
  }
}
