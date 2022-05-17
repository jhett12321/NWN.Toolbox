using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Permissions
{
  [ServiceBinding(typeof(PermissionsConfigService))]
  internal class PermissionsConfigService
  {
    private readonly ConfigService configService;

    private readonly Dictionary<string, GroupEntry> defaultGroups = new Dictionary<string, GroupEntry>();
    private readonly Dictionary<string, GroupEntry> defaultDMGroups = new Dictionary<string, GroupEntry>();
    private readonly Dictionary<NwPlayer, PermissionSet> cachedPermissions = new Dictionary<NwPlayer, PermissionSet>();

    internal GroupConfig GroupConfig;
    internal UserConfig UserConfig;

    public PermissionsConfigService(ConfigService configService)
    {
      this.configService = configService;
      if (configService.Config.Permissions.IsEnabled())
      {
        LoadAllConfigsFromDisk();
      }
    }

    internal string GetFullChatCommand(string subCommand)
    {
      if (string.IsNullOrEmpty(configService.Config.Permissions.ChatCommand))
      {
        return subCommand;
      }

      return configService.Config.Permissions.ChatCommand + " " + subCommand;
    }

    internal PermissionSet GetPermissionsForPlayer(NwPlayer player)
    {
      if (!cachedPermissions.TryGetValue(player, out PermissionSet permissionSet))
      {
        permissionSet = CreatePermissionSet(player);
        cachedPermissions[player] = permissionSet;
      }

      return permissionSet;
    }

    internal IEnumerable<string> GetGroupsForPlayer(NwPlayer player, bool includeDefaultEntries)
    {
      UserEntry userEntry = ResolveUserEntry(player);
      List<string> groups = includeDefaultEntries ? new List<string>(player.IsDM ? defaultDMGroups.Keys : defaultGroups.Keys) : new List<string>();

      if (userEntry != null)
      {
        groups.AddRange(userEntry.Groups);
      }

      return groups;
    }

    internal void UpdateGroupConfig(Action<GroupConfig> transaction)
    {
      transaction(GroupConfig);
      configService.SaveConfig(GroupConfig.ConfigName, GroupConfig);
      Refresh();
    }

    internal void UpdateUserConfig(Action<UserConfig> transaction)
    {
      transaction(UserConfig);
      configService.SaveConfig(UserConfig.ConfigName, UserConfig);
      Refresh();
    }

    private PermissionSet CreatePermissionSet(NwPlayer player)
    {
      PermissionSet permissionSet = new PermissionSet();
      UserEntry userEntry = ResolveUserEntry(player);
      IEnumerable<GroupEntry> groupEntries = ResolveGroupEntries(player, userEntry);

      if (userEntry != null)
      {
        ParsePermissionEntries(permissionSet, userEntry.Permissions);
      }

      foreach (GroupEntry groupEntry in groupEntries)
      {
        ParsePermissionEntries(permissionSet, groupEntry.Permissions);
      }

      cachedPermissions[player] = permissionSet;
      return permissionSet;
    }

    private UserEntry ResolveUserEntry(NwPlayer player)
    {
      if (UserConfig.UsersCd.TryGetValue(player.CDKey, out UserEntry userEntry))
      {
        return userEntry;
      }

      if (UserConfig.UsersCharacter.TryGetValue(player.LoginCreature!.UUID.ToUUIDString(), out userEntry))
      {
        return userEntry;
      }

      if (UserConfig.UsersUsername.TryGetValue(player.PlayerName, out userEntry))
      {
        return userEntry;
      }

      return null;
    }

    private IEnumerable<GroupEntry> ResolveGroupEntries(NwPlayer player, UserEntry userEntry)
    {
      if (userEntry == null)
      {
        return !player.IsDM ? defaultGroups.Values : defaultDMGroups.Values;
      }

      List<GroupEntry> groupEntries = !player.IsDM ? new List<GroupEntry>(defaultGroups.Values) : new List<GroupEntry>(defaultDMGroups.Values);
      foreach (string groupName in userEntry.Groups)
      {
        if (GroupConfig.Groups.TryGetValue(groupName, out GroupEntry groupEntry))
        {
          AddInheritedGroups(groupEntries, groupEntry);
          groupEntries.Add(groupEntry);
        }
      }

      return groupEntries;
    }

    private void AddInheritedGroups(List<GroupEntry> destination, GroupEntry groupEntry)
    {
      foreach (string groupName in groupEntry.Inheritance)
      {
        if (GroupConfig.Groups.TryGetValue(groupName, out GroupEntry inheritedGroup))
        {
          AddInheritedGroups(destination, inheritedGroup);
          destination.Add(inheritedGroup);
        }
      }
    }

    private void ParsePermissionEntries(PermissionSet destination, List<string> permissionEntries)
    {
      foreach (string entry in permissionEntries)
      {
        string permission;
        HashSet<string> targetSet;

        // Wildcard
        if (entry.EndsWith(".*"))
        {
          permission = entry[..^2];
          targetSet = destination.WildcardPermissions;
        }
        else if (entry.EndsWith('*'))
        {
          permission = entry[..^1];
          targetSet = destination.WildcardPermissions;
        }
        else
        {
          permission = entry;
          targetSet = destination.Permissions;
        }

        // Negation
        if (permission.StartsWith('-'))
        {
          targetSet.Remove(permission[1..]);
        }
        else
        {
          targetSet.Add(permission);
        }
      }
    }

    private void Refresh()
    {
      cachedPermissions.Clear();
      defaultDMGroups.Clear();

      foreach (KeyValuePair<string, GroupEntry> groupEntry in GroupConfig.Groups)
      {
        if (groupEntry.Value.Default)
        {
          defaultGroups[groupEntry.Key] = groupEntry.Value;
        }

        if (groupEntry.Value.DefaultDm)
        {
          defaultDMGroups[groupEntry.Key] = groupEntry.Value;
        }
      }
    }

    private void LoadAllConfigsFromDisk()
    {
      GroupConfig = configService.LoadConfig<GroupConfig>(GroupConfig.ConfigName);
      UserConfig = configService.LoadConfig<UserConfig>(UserConfig.ConfigName);
      Refresh();
    }
  }
}
