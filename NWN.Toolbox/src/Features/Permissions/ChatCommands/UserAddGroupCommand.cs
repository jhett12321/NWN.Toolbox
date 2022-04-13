using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Features.ChatCommands;

namespace Jorteck.Toolbox.Features.Permissions
{
  [ServiceBinding(typeof(IChatCommand))]
  internal class UserAddGroupCommand : PermissionsCommand
  {
    public override string SubCommand => "user addgroup";
    public override Range ArgCount => 1..1;

    public override string Description => "Adds a group membership to a user.";

    public override CommandUsage[] Usages { get; } =
    {
      new CommandUsage("<group_name>", "Add a player to the specified group."),
    };

    public override void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      string group = args[0];
      if (!PermissionsConfigService.GroupConfig.IsValidGroup(group))
      {
        caller.SendErrorMessage($"Invalid group \"{group}\".");
        return;
      }

      caller.EnterPlayerTargetMode(selection => AddUserGroupToTarget(selection, group));
    }

    private void AddUserGroupToTarget(NwPlayerExtensions.PlayerTargetPlayerEvent selection, string group)
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

        if (entry.Groups.Contains(group))
        {
          caller.SendErrorMessage($"Target is already in group \"{group}\"");
          return;
        }

        entry.Groups ??= new List<string>();
        entry.Groups.Add(group);
        caller.SendErrorMessage($"Permission group granted: \"{group}\"");
      });
    }
  }
}
