using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  public abstract class WindowController<TView> : IWindowController where TView : WindowView<TView>, new()
  {
    [Inject]
    public IPermissionProvider PermissionProvider { private get; init; }

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
        string permissionKey = string.Format(PermissionKeys.UseWindowFormat, View.Id.ToLowerInvariant(), nuiBind.Key.ToLowerInvariant());
        Token.SetBindValue(nuiBind, PermissionProvider.HasPermission(Token.Player, permissionKey));
      }
    }
  }
}
