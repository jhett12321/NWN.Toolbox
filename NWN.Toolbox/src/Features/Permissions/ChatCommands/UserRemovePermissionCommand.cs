using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Features.ChatCommands;

namespace Jorteck.Toolbox.Features.Permissions
{
  [ServiceBinding(typeof(IChatCommand))]
  internal class UserRemovePermissionCommand : PermissionsCommand
  {
    public override string SubCommand => "user removepermission";
    public override Range ArgCount => 1..1;

    public override string Description => "Removes a permission from a user.";

    public override CommandUsage[] Usages { get; } =
    {
      new CommandUsage("<permission_name>", "Remove the specified permission from the target user."),
    };

    public override void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      string permission = args[0];
      caller.EnterPlayerTargetMode(selection => RemoveUserPermissionFromTarget(selection, permission));
    }

    private void RemoveUserPermissionFromTarget(NwPlayerExtensions.PlayerTargetPlayerEvent selection, string permission)
    {
      NwPlayer caller = selection.Caller;
      NwPlayer target = selection.Target;

      PermissionsConfigService.UpdateUserConfig(config =>
      {
        if (!config.UsersCd.TryGetValue(target.CDKey, out UserEntry entry))
        {
          entry = new UserEntry();
          config.UsersCd[target.CDKey] = entry;
        }

        if (!entry.Permissions.Contains(permission))
        {
          caller.SendErrorMessage($"No such permission on target: \"{permission}\"");
          return;
        }

        entry.Permissions ??= new List<string>();
        entry.Permissions.Remove(permission);
        caller.SendServerMessage($"Permission removed: \"{permission}\"", ColorConstants.Lime);
      });
    }
  }
}
