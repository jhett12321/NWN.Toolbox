using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Config;

namespace Jorteck.Toolbox
{
  [ServiceBinding(typeof(ToolboxWindowButtonInitializer))]
  public sealed class ToolboxWindowButtonInitializer : IInitializable
  {
    [Inject]
    private IPermissionProvider PermissionProvider { get; init; }

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
        if (player.LoginCreature != null)
        {
          TryOpenWindow(player);
        }
      }
    }

    private void TryOpenWindow(NwPlayer player)
    {
      if (PermissionProvider.HasPermission(player, PermissionKeys.OpenToolbox))
      {
        WindowManager.OpenWindow<ToolboxWindowButtonView>(player);
      }
    }
  }
}
