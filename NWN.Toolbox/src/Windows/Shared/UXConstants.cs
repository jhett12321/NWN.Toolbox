using System;
using Anvil.API;

namespace Jorteck.Toolbox
{
  internal static class UXConstants
  {
    public static readonly TimeSpan DoubleClickThreshold = TimeSpan.FromMilliseconds(500);

    public static readonly Color DefaultColor = new Color(255, 255, 255);
    public static readonly Color DefaultColor2 = new Color(128, 128, 128);
    public static readonly Color SelectedColor = new Color(255, 255, 0);
  }
}
