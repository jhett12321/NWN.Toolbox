using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using Jorteck.Toolbox.Features.Permissions;

namespace Jorteck.Toolbox
{
  public abstract class WindowController<TView> : IWindowController where TView : WindowView<TView>, new()
  {
    private const string UseWindowPermissionKeyFormat = "toolbox.window.use.{0}.{1}";

    [Inject]
    public PermissionsService PermissionsService { get; init; }

    public TView View { protected get; init; }

    public WindowToken Token { get; init; }

    public abstract void Init();

    public abstract void ProcessEvent(ModuleEvents.OnNuiEvent eventData);

    public void Close()
    {
      OnClose();
      Token?.Dispose();
    }

    protected abstract void OnClose();

    protected void ApplyPermissionBindings(params NuiBind<bool>[] binds)
    {
      foreach (NuiBind<bool> nuiBind in binds)
      {
        string permissionKey = string.Format(UseWindowPermissionKeyFormat, View.Id.ToLowerInvariant(), nuiBind.Key.ToLowerInvariant());
        Token.SetBindValue(nuiBind, !PermissionsService.IsEnabled || PermissionsService.HasPermission(Token.Player, permissionKey));
      }
    }
  }
}
