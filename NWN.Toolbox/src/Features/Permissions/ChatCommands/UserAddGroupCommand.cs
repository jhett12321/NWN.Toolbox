using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Features.ChatCommands;

namespace Jorteck.Toolbox.Features.Permissions
{
  [ServiceBinding(typeof(IChatCommand))]
  internal class UserAddGroupCommand : IChatCommand
  {
    [Inject]
    private PermissionsConfigService PermissionsConfigService { get; init; }

    public string Command => PermissionsConfigService.GetFullChatCommand("user addgroup");
    public string[] Aliases => null;
    public bool DMOnly => true;
    public int? ArgCount => 1;
    public string Description => "Adds a group membership to a user.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("<group_name>", "Add a player to the specified group."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
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
