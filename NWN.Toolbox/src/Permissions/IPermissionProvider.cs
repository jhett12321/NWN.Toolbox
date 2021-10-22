using Anvil.API;

namespace Jorteck.Toolbox
{
  public interface IPermissionProvider
  {
    bool HasPermission(NwPlayer player, string permissionKey);
  }
}
