using System;
using System.Collections.Generic;
using Anvil.API;

namespace Jorteck.Toolbox
{
  public sealed class WindowToken : IDisposable
  {
    public NwPlayer Player { get; }
    public int WindowId { get; }

    public WindowToken(NwPlayer player, int windowId)
    {
      Player = player;
      WindowId = windowId;
    }

    public T GetBindValue<T>(NuiBind<T> bind)
    {
      return bind.GetBindValue(Player, WindowId);
    }

    public List<T> GetBindValues<T>(NuiBind<T> bind)
    {
      return bind.GetBindValues(Player, WindowId);
    }

    public void SetBindValue<T>(NuiBind<T> bind, T value)
    {
      bind.SetBindValue(Player, WindowId, value);
    }

    public void SetBindValues<T>(NuiBind<T> bind, IEnumerable<T> values)
    {
      bind.SetBindValues(Player, WindowId, values);
    }

    public void SetBindWatch<T>(NuiBind<T> bind, bool watch)
    {
      bind.SetBindWatch(Player, WindowId, watch);
    }

    public void SetGroupLayout(NuiGroup group, NuiLayout newLayout)
    {
      group.SetLayout(Player, WindowId, newLayout);
    }

    public void Dispose()
    {
      if (Player != null && Player.IsValid)
      {
        Player.NuiDestroy(WindowId);
      }
    }
  }
}
