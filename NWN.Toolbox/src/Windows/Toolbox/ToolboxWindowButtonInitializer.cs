using Anvil.API;
using Anvil.API.Events;
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

      NwModule.Instance.OnClientEnter += OnClientEnter;
    }

    private void OnClientEnter(ModuleEvents.OnClientEnter onClientEnter)
    {
      if (permissionProvider.HasPermission(onClientEnter.Player, PermissionKeys.OpenToolbox))
      {
        windowManager.OpenWindow<ToolboxWindowButtonView>(onClientEnter.Player);
      }
    }
  }
}
