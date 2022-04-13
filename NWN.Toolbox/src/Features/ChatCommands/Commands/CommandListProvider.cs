using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Features.Permissions;

namespace Jorteck.Toolbox.Features.ChatCommands
{
  [ServiceBinding(typeof(CommandListProvider))]
  public class CommandListProvider
  {
    [Inject]
    private PermissionsService PermissionsService { get; init; }

    [Inject]
    public IReadOnlyList<IChatCommand> Commands { get; init; }

    public virtual bool CanUseCommand(NwPlayer player, IChatCommand command)
    {
      if (!PermissionsService.IsEnabled)
      {
        return !command.DMOnly || player.IsDM;
      }

      return PermissionsService.HasPermission(player, command.PermissionKey);
    }
  }
}
