using System;
using Anvil.API;

namespace Jorteck.Toolbox
{
  public static class CreatureSizeExtensions
  {
    public static int ACModifier(this CreatureSize size)
    {
      return size switch
      {
        CreatureSize.Huge => -2,
        CreatureSize.Large => -1,
        CreatureSize.Medium => 0,
        CreatureSize.Small => 1,
        CreatureSize.Tiny => 2,
        CreatureSize.Invalid => 0,
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, null),
      };
    }
  }
}
