using System;
using System.Collections.Generic;

namespace Jorteck.Toolbox
{
  [Serializable]
  internal sealed class WindowConfig
  {
    public ListMode ListMode { get; set; } = ListMode.Blacklist;
    public List<string> Windows { get; set; } = new List<string>();
  }

  internal enum ListMode
  {
    Blacklist,
    Whitelist,
  }
}
