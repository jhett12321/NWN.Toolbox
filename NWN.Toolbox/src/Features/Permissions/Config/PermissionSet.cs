using System.Collections.Generic;

namespace Jorteck.Toolbox.Features.Permissions
{
  public sealed class PermissionSet
  {
    public HashSet<string> Permissions { get; } = new HashSet<string>();
    public HashSet<string> WildcardPermissions { get; } = new HashSet<string>();
  }
}
