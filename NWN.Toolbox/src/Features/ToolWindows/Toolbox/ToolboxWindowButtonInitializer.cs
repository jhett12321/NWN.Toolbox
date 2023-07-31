using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;
using Jorteck.Toolbox.Features.Permissions;

namespace Jorteck.Toolbox.Features.ToolWindows
{
  [ServiceBinding(typeof(ToolboxWindowButtonInitializer))]
  public sealed class ToolboxWindowButtonInitializer : IInitializable
  {
    private const string OpenToolboxPermissionKey = "toolbox.window.list";

    [Inject]
    private PermissionsService PermissionsService { get; init; }

    [Inject]
    private WindowManager WindowManager { get; init; }

    [Inject]
    private ConfigService ConfigService { get; init; }

    public void Init()
    {
      if (ConfigService?.Config?.ToolboxWindows?.ShowToolboxButton != true)
      {
        return;
      }

      NwModule.Instance.OnClientEnter += eventData => TryOpenWindow(eventData.Player);
      foreach (NwPlayer player in NwModule.Instance.Players)
      {
        if (player.IsValid && player.IsConnected)
        {
          TryOpenWindow(player);
        }
      }
    }

    private void TryOpenWindow(NwPlayer player)
    {
      if (PermissionsService.HasPermission(player, OpenToolboxPermissionKey, player.IsDM))
      {
        WindowManager.OpenWindow<ToolboxWindowButtonView>(player);
      }
    }
  }
}
