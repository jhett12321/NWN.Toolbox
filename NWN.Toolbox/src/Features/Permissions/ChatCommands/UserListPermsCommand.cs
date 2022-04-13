using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Features.ChatCommands;

namespace Jorteck.Toolbox.Features.Permissions
{
  [ServiceBinding(typeof(IChatCommand))]
  internal class UserListPermsCommand : PermissionsCommand
  {
    public override string SubCommand => "user listpermissions";
    public override Range ArgCount => 0..0;

    public override string Description => "Lists all user permissions.";

    public override CommandUsage[] Usages { get; } =
    {
      new CommandUsage("List all permissions for the target user."),
    };

    public override void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      caller.EnterPlayerTargetMode(ListPermissionsOfTarget);
    }

    private void ListPermissionsOfTarget(NwPlayerExtensions.PlayerTargetPlayerEvent selection)
    {
      NwPlayer caller = selection.Caller;
      NwPlayer target = selection.Target;

      PermissionSet userPermissions = PermissionsConfigService.GetPermissionsForPlayer(target);
      caller.SendServerMessage($"Target has {(userPermissions.Permissions.Count == 0 ? "no permissions." : "the following permissions:")}", ColorConstants.Orange);
      caller.SendServerMessage(string.Join("\n", userPermissions.Permissions), ColorConstants.Lime);
    }
  }
}
