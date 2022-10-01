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

    /// <summary>
    /// Assign a NUI element to an additional parameter.
    /// </summary>
    /// <param name="value">The value to assign.</param>
    /// <param name="assign">The field/variable to assign the value.</param>
    /// <typeparam name="T">The NUI element type.</typeparam>
    /// <returns>The NUI element specified in value for fluent usage.</returns>
    public static T Assign<T>(this T value, out T assign)
    {
      assign = value;
      return value;
    }

    /// <summary>
    /// Configure a NUI element.
    /// </summary>
    /// <param name="value">The value to configure.</param>
    /// <param name="configure">The configuration to change.</param>
    /// <typeparam name="T">The NUI element type.</typeparam>
    /// <returns>The NUI element specified in value for fluent usage.</returns>
    public static T Configure<T>(this T value, Action<T> configure) where T : NuiElement
    {
      configure(value);
      return value;
    }
  }
}
