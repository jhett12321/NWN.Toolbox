using Anvil.API;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  [ServiceBinding(typeof(IPermissionProvider))]
  [ServiceBindingOptions(BindingPriority = BindingPriority.Lowest)]
  internal sealed class FallbackPermissionProvider : IPermissionProvider
  {
    public bool HasPermission(NwPlayer player, string permissionKey)
    {
      return player.IsDM;
    }
  }
}
