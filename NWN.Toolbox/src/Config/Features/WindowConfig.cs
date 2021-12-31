using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;

namespace Jorteck.Toolbox
{
  [Serializable]
  internal sealed class WindowConfig
  {
    [Description("Should the toolbox button be shown to players who have permissions?")]
    public bool ShowToolboxButton { get; set; } = true;

    [Description("The position of the toolbox button.")]
    public Vector2 ToolboxButtonPosition { get; set; } = new Vector2(725f, 0f);

    [Description("How the \"windows\" list should be interpreted. \"Whitelist\" and \"Blacklist\" are supported as values. Whitelist indicates that only the windows listed will be available, while Blacklist indicates that all windows are available except the ones listed.")]
    public ListMode ListMode { get; set; } = ListMode.Blacklist;

    [Description("The list of windows to use as the whitelist/blacklist. Dependant on list_mode.")]
    public List<string> Windows { get; set; } = new List<string>();
  }

  internal enum ListMode
  {
    Blacklist,
    Whitelist,
  }
}
