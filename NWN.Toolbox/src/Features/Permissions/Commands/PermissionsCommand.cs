using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;
using Jorteck.Toolbox.Features.Chat;

namespace Jorteck.Toolbox.Features.Permissions
{
  internal abstract class PermissionsCommand : IChatCommand
  {
    [Inject]
    protected PermissionsService PermissionsService { get; init; }

    [Inject]
    protected ConfigService ConfigService { get; init; }

    [Inject]
    protected PermissionsConfigService PermissionsConfigService { get; init; }

    public string Command => PermissionsConfigService.GetFullChatCommand(SubCommand);
    public string PermissionKey => $"permissions.{SubCommand.Replace(" ", ".")}";
    public bool DMOnly => true;
    public bool IsAvailable => PermissionsService.IsEnabled && ConfigService.Config.Permissions?.ChatCommandEnable == true;

    public abstract Range ArgCount { get; }
    public abstract string Description { get; }
    public abstract CommandUsage[] Usages { get; }
    public abstract string SubCommand { get; }

    public abstract void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args);
  }
}
