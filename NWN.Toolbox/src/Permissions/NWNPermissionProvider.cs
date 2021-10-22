using Anvil.API;
using Anvil.Services;
using Jorteck.Permissions;

namespace Jorteck.Toolbox
{
  [ServiceBinding(typeof(IPermissionProvider))]
  [ServiceBindingOptions(PluginDependencies = new[] { "NWN.Permissions" })]
  public sealed class NWNPermissionProvider : IPermissionProvider
  {
    private readonly PermissionsService permissionsService;

    public NWNPermissionProvider(PermissionsService permissionsService)
    {
      this.permissionsService = permissionsService;
    }

    public bool HasPermission(NwPlayer player, string permissionKey)
    {
      return permissionsService.HasPermission(player, permissionKey);
    }
  }
}
