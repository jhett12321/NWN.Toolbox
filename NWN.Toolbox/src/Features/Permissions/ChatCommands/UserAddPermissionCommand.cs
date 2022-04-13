using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Features.ChatCommands;

namespace Jorteck.Toolbox.Features.Permissions
{
  [ServiceBinding(typeof(IChatCommand))]
  internal class UserAddPermissionCommand : PermissionsCommand
  {
    public override string SubCommand => "user addpermission";
    public override Range ArgCount => 1..1;

    public override string Description => "Grants a permission to a user.";

    public override CommandUsage[] Usages { get; } =
    {
      new CommandUsage("<permission_name>", "Grant the specified permission to the target user."),
    };

    public override void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      string permission = args[0];
      caller.EnterPlayerTargetMode(selection => AddUserPermissionToTarget(selection, permission));
    }

    private void AddUserPermissionToTarget(NwPlayerExtensions.PlayerTargetPlayerEvent selection, string permission)
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

        if (entry.Permissions.Contains(permission))
        {
          caller.SendErrorMessage($"Target already has permission \"{permission}\"");
          return;
        }

        entry.Permissions ??= new List<string>();
        entry.Permissions.Add(permission);
        caller.SendServerMessage($"Permission granted: \"{permission}\"", ColorConstants.Lime);
      });
    }
  }
}
