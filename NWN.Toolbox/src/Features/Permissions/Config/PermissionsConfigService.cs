using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

    internal readonly GroupConfig? GroupConfig;
    internal readonly UserConfig? UserConfig;

    public PermissionsConfigService(ConfigService configService)
    {
      this.configService = configService;
      if (configService.Config.Permissions.IsEnabled())
      {
        GroupConfig = configService.LoadConfig<GroupConfig>(GroupConfig.ConfigName);
        UserConfig = configService.LoadConfig<UserConfig>(UserConfig.ConfigName);
        Refresh();
      }
    }

    internal string GetFullChatCommand(string subCommand)
    {
      if (string.IsNullOrEmpty(configService.Config.Permissions?.ChatCommand))
      {
        return subCommand;
      }

      return configService.Config.Permissions.ChatCommand + " " + subCommand;
    }

    internal PermissionSet? GetPermissionsForPlayer(NwPlayer player)
    {
      if (!IsPermissionsEnabled())
      {
        return null;
      }

      if (!cachedPermissions.TryGetValue(player, out PermissionSet? permissionSet))
      {
        permissionSet = CreatePermissionSet(GroupConfig, UserConfig, player);
        cachedPermissions[player] = permissionSet;
      }

      return permissionSet;
    }

    internal IEnumerable<string> GetGroupsForPlayer(NwPlayer player, bool includeDefaultEntries)
    {
      if (!IsPermissionsEnabled())
      {
        return [];
      }

      UserEntry? userEntry = ResolveUserEntry(UserConfig, player);
      List<string> groups = includeDefaultEntries ? [..player.IsDM ? defaultDMGroups.Keys : defaultGroups.Keys] : [];

      if (userEntry?.Groups != null)
      {
        groups.AddRange(userEntry.Groups);
      }

      return groups;
    }

    internal void UpdateGroupConfig(Action<GroupConfig> transaction)
    {
      if (!IsPermissionsEnabled())
      {
        return;
      }

      transaction(GroupConfig);
      configService.SaveConfig(GroupConfig.ConfigName, GroupConfig);
      Refresh();
    }

    internal void UpdateUserConfig(Action<UserConfig> transaction)
    {
      if (!IsPermissionsEnabled())
      {
        return;
      }

      transaction(UserConfig);
      configService.SaveConfig(UserConfig.ConfigName, UserConfig);
      Refresh();
    }

    private PermissionSet CreatePermissionSet(GroupConfig groupConfig, UserConfig userConfig, NwPlayer player)
    {
      PermissionSet permissionSet = new PermissionSet();
      UserEntry? userEntry = ResolveUserEntry(userConfig, player);
      IEnumerable<GroupEntry> groupEntries = ResolveGroupEntries(groupConfig, player, userEntry);

      if (userEntry?.Permissions != null)
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

    private UserEntry? ResolveUserEntry(UserConfig userConfig, NwPlayer player)
    {
      if (userConfig.UsersCd.TryGetValue(player.CDKey, out UserEntry? userEntry))
      {
        return userEntry;
      }

      if (userConfig.UsersCharacter.TryGetValue(player.LoginCreature!.UUID.ToUUIDString(), out userEntry))
      {
        return userEntry;
      }

      if (userConfig.UsersUsername.TryGetValue(player.PlayerName, out userEntry))
      {
        return userEntry;
      }

      return null;
    }

    private IEnumerable<GroupEntry> ResolveGroupEntries(GroupConfig groupConfig, NwPlayer player, UserEntry? userEntry)
    {
      if (userEntry?.Groups == null)
      {
        return !player.IsDM ? defaultGroups.Values : defaultDMGroups.Values;
      }

      List<GroupEntry> groupEntries = !player.IsDM ? [..defaultGroups.Values] : [..defaultDMGroups.Values];
      foreach (string groupName in userEntry.Groups)
      {
        if (groupConfig.Groups.TryGetValue(groupName, out GroupEntry? groupEntry))
        {
          AddInheritedGroups(groupConfig, groupEntries, groupEntry);
          groupEntries.Add(groupEntry);
        }
      }

      return groupEntries;
    }

    private void AddInheritedGroups(GroupConfig groupConfig, List<GroupEntry> destination, GroupEntry groupEntry)
    {
      foreach (string groupName in groupEntry.Inheritance)
      {
        if (groupConfig.Groups.TryGetValue(groupName, out GroupEntry? inheritedGroup))
        {
          AddInheritedGroups(groupConfig, destination, inheritedGroup);
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
      if (GroupConfig == null)
      {
        return;
      }

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

    [MemberNotNullWhen(true, nameof(GroupConfig), nameof(UserConfig))]
    private bool IsPermissionsEnabled()
    {
      return UserConfig != null && GroupConfig != null;
    }
  }
}
