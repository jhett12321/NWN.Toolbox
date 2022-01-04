using System;
using Anvil.API;

namespace Jorteck.Toolbox
{
  internal static class UXConstants
  {
    public static readonly TimeSpan DoubleClickThreshold = TimeSpan.FromMilliseconds(500);

    public static readonly NuiColor DefaultColor = new NuiColor(255, 255, 255);
    public static readonly NuiColor DefaultColor2 = new NuiColor(128, 128, 128);
    public static readonly NuiColor SelectedColor = new NuiColor(255, 255, 0);
  }
}
