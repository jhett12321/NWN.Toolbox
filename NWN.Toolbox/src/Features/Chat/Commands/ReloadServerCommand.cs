using System;
using System.Collections.Generic;
using Anvil;
using Anvil.API;
using Anvil.Internal;
using Anvil.Services;

namespace Jorteck.Toolbox.Features.Chat
{
  [ServiceBinding(typeof(IChatCommand))]
  public sealed class ReloadServerCommand : IChatCommand
  {
    public string Command => "reload";
    public Range ArgCount => ..0;
    public bool DMOnly => true;
    public string Description => "Reload anvil plugins and services.";
    public bool IsAvailable => EnvironmentConfig.ReloadEnabled;

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("Triggers a reload of all anvil plugins and services. Requires hot reload to be enabled."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      AnvilCore.Reload();
    }
  }
}
