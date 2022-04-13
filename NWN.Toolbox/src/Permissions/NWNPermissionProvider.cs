using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Features.Permissions;

namespace Jorteck.Toolbox
{
  [ServiceBinding(typeof(IPermissionProvider))]
  [ServiceBindingOptions(PluginDependencies = new[] { "NWN.Permissions" })]
  internal sealed class NWNPermissionProvider : IPermissionProvider
  {
    // We have to use injection here as EF.Core scans constructors and will throw a missing assembly error if the plugin is not installed.
    [Inject]
    public PermissionsService PermissionsService { private get; init; }

    public bool HasPermission(NwPlayer player, string permissionKey)
    {
      return PermissionsService != null && PermissionsService.HasPermission(player, permissionKey);
    }
  }
}
