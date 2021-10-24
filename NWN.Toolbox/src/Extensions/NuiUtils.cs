using System;
using System.Collections.Generic;
using Anvil.API;

namespace Jorteck.Toolbox
{
  public static class NuiUtils
  {
    public static NuiCombo CreateComboForEnum<T>(NuiBind<int> selected) where T : struct, Enum
    {
      List<NuiComboEntry> entries = new List<NuiComboEntry>();
      foreach (T value in Enum.GetValues<T>())
      {
        entries.Add(new NuiComboEntry(value.ToString(), (int)(value as object)));
      }

      return new NuiCombo
      {
        Entries = entries,
        Selected = selected,
      };
    }
  }
}
