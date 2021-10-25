using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  public abstract class WindowController<TController, TView> : IWindowController
    where TController : WindowController<TController, TView>, new()
    where TView : WindowView<TView, TController>, new()
  {
    [Inject]
    public IPermissionProvider PermissionProvider { private get; init; }

    public TView View { protected get; init; }

    public NwPlayer Player { get; init; }

    public int Token { get; init; }

    public abstract void Init();

    public abstract void ProcessEvent(ModuleEvents.OnNuiEvent eventData);

    public void Close()
    {
      OnClose();
      if (Player != null && Player.IsValid)
      {
        Player.NuiDestroy(Token);
      }
    }

    protected abstract void OnClose();

    protected void ApplyPermissionBindings(params NuiBind<bool>[] binds)
    {
      foreach (NuiBind<bool> nuiBind in binds)
      {
        string permissionKey = string.Format(PermissionKeys.UseWindowFormat, View.Id.ToLowerInvariant(), nuiBind.Key.ToLowerInvariant());
        nuiBind.SetBindValue(Player, Token, PermissionProvider.HasPermission(Player, permissionKey));
      }
    }

    protected T GetBindValue<T>(NuiBind<T> bind)
    {
      return bind.GetBindValue(Player, Token);
    }

    protected void SetBindValue<T>(NuiBind<T> bind, T value)
    {
      bind.SetBindValue(Player, Token, value);
    }

    protected void SetBindWatch<T>(NuiBind<T> bind, bool watch)
    {
      bind.SetBindWatch(Player, Token, watch);
    }
  }
}
