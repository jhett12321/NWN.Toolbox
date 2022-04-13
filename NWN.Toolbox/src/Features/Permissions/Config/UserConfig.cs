using System;
using System.Collections.Generic;

namespace Jorteck.Toolbox.Features.Permissions
{
  [Serializable]
  internal sealed class UserConfig
  {
    public const string ConfigName = "users.yml";

    public Dictionary<string, UserEntry> UsersCd { get; set; } = new Dictionary<string, UserEntry>();
    public Dictionary<string, UserEntry> UsersCharacter { get; set; } = new Dictionary<string, UserEntry>();
    public Dictionary<string, UserEntry> UsersUsername { get; set; } = new Dictionary<string, UserEntry>();
  }

  [Serializable]
  internal sealed class UserEntry
  {
    public string LastUsername { get; set; }
    public string LastCharacter { get; set; }
    public List<string> Groups { get; set; } = new List<string>();
    public List<string> Permissions { get; set; } = new List<string>();
    public Dictionary<string, string> Info { get; set; } = new Dictionary<string, string>();
  }
}
