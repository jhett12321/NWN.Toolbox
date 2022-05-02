using System;

namespace Jorteck.Toolbox.Core
{
  [Flags]
  public enum ObjectSelectionTypes
  {
    Creature = 1 << 0,
    Item = 1 << 1,
    Trigger = 1 << 2,
    Door = 1 << 3,
    AreaOfEffect = 1 << 4,
    Waypoint = 1 << 5,
    Placeable = 1 << 6,
    Store = 1 << 7,
    Encounter = 1 << 8,
    Sound = 1 << 9,
    Player = 1 << 10,
    All = ~0,
  }
}
