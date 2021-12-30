using Anvil.API;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  [ServiceBinding(typeof(ToolboxWindowButtonInitializer))]
  public sealed class ToolboxWindowButtonInitializer
  {
    private readonly IPermissionProvider permissionProvider;
    private readonly WindowManager windowManager;

    public ToolboxWindowButtonInitializer(IPermissionProvider permissionProvider, WindowManager windowManager)
    {
      this.permissionProvider = permissionProvider;
      this.windowManager = windowManager;

      NwModule.Instance.OnClientEnter += eventData => TryOpenWindow(eventData.Player);
      foreach (NwPlayer player in NwModule.Instance.Players)
      {
        if (player.LoginCreature != null)
        {
          TryOpenWindow(player);
        }
      }
    }

    private void TryOpenWindow(NwPlayer player)
    {
      if (permissionProvider.HasPermission(player, PermissionKeys.OpenToolbox))
      {
        windowManager.OpenWindow<ToolboxWindowButtonView>(player);
      }
    }
  }
}
