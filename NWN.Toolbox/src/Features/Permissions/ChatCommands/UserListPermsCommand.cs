using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Features.ChatCommands;

namespace Jorteck.Toolbox.Features.Permissions
{
  [ServiceBinding(typeof(IChatCommand))]
  internal class UserListPermsCommand : IChatCommand
  {
    [Inject]
    private PermissionsConfigService PermissionsConfigService { get; init; }

    public string Command => PermissionsConfigService.GetFullChatCommand("user listpermissions");
    public string[] Aliases => null;
    public bool DMOnly => true;
    public string PermissionKey => PermissionConstants.List;

    public int? ArgCount => 0;
    public string Description => "Lists all user permissions.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("List all permissions for the target user."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
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
