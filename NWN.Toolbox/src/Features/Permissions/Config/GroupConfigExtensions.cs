namespace Jorteck.Toolbox.Features.Permissions
{
  internal static class GroupConfigExtensions
  {
    public static bool IsValidGroup(this GroupConfig groupConfig, string groupName)
    {
      return groupConfig?.Groups.ContainsKey(groupName) == true;
    }
  }
}
