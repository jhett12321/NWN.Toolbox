using System;
using System.Collections.Generic;

namespace Jorteck.Toolbox.Features.Permissions
{
  [Serializable]
  internal sealed class GroupConfig
  {
    public const string ConfigName = "groups.yml";

    public Dictionary<string, GroupEntry> Groups { get; set; } = new Dictionary<string, GroupEntry>
    {
      {
        "default", new GroupEntry
        {
          Default = true,
        }
      },
      {
        "dm", new GroupEntry
        {
          DefaultDm = true,
          Permissions = new List<string>
          {
            "*",
          },
        }
      },
    };
  }

  [Serializable]
  internal sealed class GroupEntry
  {
    public bool Default { get; set; }
    public bool DefaultDm { get; set; }
    public List<string> Permissions { get; set; } = new List<string>();
    public List<string> Inheritance { get; set; } = new List<string>();
    public Dictionary<string, string> Info { get; set; } = new Dictionary<string, string>();
  }
}
