using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Permissions
{
  [ServiceBinding(typeof(PermissionsService))]
  public sealed class PermissionsService
  {
    [Inject]
    private PermissionsConfigService PermissionsConfigService { get; init; }

    [Inject]
    private ConfigService ConfigService { get; init; }

    public bool IsEnabled => ConfigService.Config.Permissions.IsEnabled();

    /// <summary>
    /// Gets if a player has the specified permission.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <param name="permission">The permission to query.</param>
    /// <param name="defaultIfDisabled">The default value to return if the PermissionService is disabled.</param>
    /// <returns>True if the player has the specified permission, otherwise false.</returns>
    public bool HasPermission(NwPlayer player, string permission, bool defaultIfDisabled = false)
    {
      if (!ConfigService.Config.Permissions.IsEnabled())
      {
        return defaultIfDisabled;
      }

      PermissionSet permissionSet = PermissionsConfigService.GetPermissionsForPlayer(player);
      if (permissionSet.Permissions.Contains(permission))
      {
        return true;
      }

      foreach (string wildcardPermission in permissionSet.WildcardPermissions)
      {
        if (permission.StartsWith(wildcardPermission))
        {
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Gets a list of groups that the specified player is a member of.
    /// </summary>
    /// <param name="player">The player to query.</param>
    /// <param name="includeDefault">If true, includes groups that the player is a part of by default.</param>
    /// <returns></returns>
    public IEnumerable<string> GetGroups(NwPlayer player, bool includeDefault = true)
    {
      if (ConfigService.Config.Permissions.IsEnabled())
      {
        return Enumerable.Empty<string>();
      }

      return PermissionsConfigService.GetGroupsForPlayer(player, includeDefault);
    }
  }
}
