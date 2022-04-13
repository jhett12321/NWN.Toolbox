using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Toolbox.Features.Permissions
{
  [ServiceBinding(typeof(DMPermissionHandler))]
  internal class DMPermissionHandler
  {
    private readonly PermissionsService permissionsService;

    public DMPermissionHandler(PermissionsService permissionsService)
    {
      this.permissionsService = permissionsService;

      NwModule module = NwModule.Instance;
      module.OnDMGiveXP += eventData => OnDMGive(eventData, DMPermissionConstants.DMGiveXp);
      module.OnDMGiveLevel += eventData => OnDMGive(eventData, DMPermissionConstants.DMGiveLevel);
      module.OnDMGiveGold += eventData => OnDMGive(eventData, DMPermissionConstants.DMGiveGold);
      module.OnDMGiveAlignment += OnDMGiveAlignment;
      module.OnDMGiveItemBefore += OnDMGiveItemBefore;

      module.OnDMPlayerDMLogin += OnPlayerDMLogin;
      module.OnDMPlayerDMLogout += OnPlayerDMLogout;
      module.OnClientEnter += OnClientEnter;

      module.OnDMHeal += eventData => OnDMGroupTarget(eventData, DMPermissionConstants.DMHeal);
      module.OnDMKill += eventData => OnDMGroupTarget(eventData, DMPermissionConstants.DMKill);
      module.OnDMToggleInvulnerable += eventData => OnDMGroupTarget(eventData, DMPermissionConstants.DMInvulnerable);
      module.OnDMForceRest += eventData => OnDMGroupTarget(eventData, DMPermissionConstants.DMForceRest);
      module.OnDMToggleImmortal += eventData => OnDMGroupTarget(eventData, DMPermissionConstants.DMImmortal);
      module.OnDMLimbo += eventData => OnDMGroupTarget(eventData, DMPermissionConstants.DMLimbo);
      module.OnDMToggleAI += eventData => OnDMGroupTarget(eventData, DMPermissionConstants.DMToggleAI);

      module.OnDMGoTo += eventData => OnDMSingleTarget(eventData, DMPermissionConstants.DMGoTo);
      module.OnDMPossess += eventData => OnDMSingleTarget(eventData, DMPermissionConstants.DMPossess);
      module.OnDMPossessFullPower += eventData => OnDMSingleTarget(eventData, DMPermissionConstants.DMPossessFullPower);
      module.OnDMToggleLock += eventData => OnDMSingleTarget(eventData, DMPermissionConstants.DMToggleLock);
      module.OnDMDisableTrap += eventData => OnDMSingleTarget(eventData, DMPermissionConstants.DMDisableTrap);

      module.OnDMAppear += eventData => OnDMStandardEvent(eventData, DMPermissionConstants.DMAppear);
      module.OnDMDisappear += eventData => OnDMStandardEvent(eventData, DMPermissionConstants.DMDisappear);
      module.OnDMSetFaction += eventData => OnDMStandardEvent(eventData, DMPermissionConstants.DMSetFaction);
      module.OnDMGetFactionReputation += eventData => OnDMStandardEvent(eventData, DMPermissionConstants.DMGetFactionReputation);
      module.OnDMSetFactionReputation += eventData => OnDMStandardEvent(eventData, DMPermissionConstants.DMSetFactionReputation);
      module.OnDMTakeItem += eventData => OnDMStandardEvent(eventData, DMPermissionConstants.DMTakeItem);
      module.OnDMSetStat += eventData => OnDMStandardEvent(eventData, DMPermissionConstants.DMSetStat);
      module.OnDMSetTime += eventData => OnDMStandardEvent(eventData, DMPermissionConstants.DMSetTime);
      module.OnDMSetDate += eventData => OnDMStandardEvent(eventData, DMPermissionConstants.DMSetDate);
      module.OnDMGetVariable += eventData => OnDMStandardEvent(eventData, DMPermissionConstants.DMGetLocal);
      module.OnDMSetVariable += eventData => OnDMStandardEvent(eventData, DMPermissionConstants.DMSetLocal);
      module.OnDMDumpLocals += OnDMDumpLocals;

      module.OnDMJumpToPoint += eventData => OnDMTeleport(eventData, DMPermissionConstants.DMJump + DMPermissionConstants.TargetSelf);
      module.OnDMJumpAllPlayersToPoint += eventData => OnDMTeleport(eventData, DMPermissionConstants.DMJumpAllPlayers);
      module.OnDMJumpTargetToPoint += OnDMJumpTargetToPoint;

      module.OnDMChangeDifficulty += OnDMChangeDifficulty;
      module.OnDMViewInventory += OnDMViewInventory;

      module.OnDMSpawnObjectBefore += OnDMSpawnObject;
    }

    private void OnClientEnter(ModuleEvents.OnClientEnter eventData)
    {
      if (permissionsService.HasPermission(eventData.Player, DMPermissionConstants.PlayerDMForceLogin))
      {
        eventData.Player.IsPlayerDM = true;
      }
    }

    private void OnPlayerDMLogin(OnDMPlayerDMLogin eventData)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.PlayerDMLogin))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnPlayerDMLogout(OnDMPlayerDMLogout eventData)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.PlayerDMLogout))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMGiveItemBefore(OnDMGiveItemBefore eventData)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.DMGiveItem))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }

      if (!HasPermissionToTarget(eventData.DungeonMaster, eventData.Target, DMPermissionConstants.DMGiveItem))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMDumpLocals(OnDMDumpLocals eventData)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.DMDumpLocals))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }

      if (!HasPermissionToTarget(eventData.DungeonMaster, eventData.Target, DMPermissionConstants.DMDumpLocals))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMGive(DMGiveEvent eventData, string permission)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, permission))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }

      if (!HasPermissionToTarget(eventData.DungeonMaster, eventData.Target, permission))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMGiveAlignment(OnDMGiveAlignment eventData)
    {
      if (permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.DMGiveAlignment))
      {
        return;
      }

      if (!HasPermissionToTarget(eventData.DungeonMaster, eventData.Target, DMPermissionConstants.DMGiveAlignment))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMGroupTarget(DMGroupTargetEvent eventData, string permissionBase)
    {
      if (permissionsService.HasPermission(eventData.DungeonMaster, permissionBase))
      {
        return;
      }

      foreach (NwObject target in eventData.Targets)
      {
        if (!HasPermissionToTarget(eventData.DungeonMaster, target, permissionBase))
        {
          eventData.Skip = true;
          ShowNoPermissionError(eventData.DungeonMaster);
          break;
        }
      }
    }

    private void OnDMSingleTarget(DMSingleTargetEvent eventData, string permissionBase)
    {
      if (permissionsService.HasPermission(eventData.DungeonMaster, permissionBase))
      {
        return;
      }

      if (!HasPermissionToTarget(eventData.DungeonMaster, eventData.Target, permissionBase))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMStandardEvent(DMStandardEvent eventData, string permission)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, permission))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMTeleport(DMTeleportEvent eventData, string permission)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, permission))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMJumpTargetToPoint(OnDMJumpTargetToPoint eventData)
    {
      if (permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.DMJump))
      {
        return;
      }

      foreach (NwGameObject target in eventData.Targets)
      {
        if (!HasPermissionToTarget(eventData.DungeonMaster, target, DMPermissionConstants.DMJump))
        {
          eventData.Skip = true;
          ShowNoPermissionError(eventData.DungeonMaster);
          break;
        }
      }
    }

    private void OnDMChangeDifficulty(OnDMChangeDifficulty eventData)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.DMChangeDifficulty))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMViewInventory(OnDMViewInventory eventData)
    {
      if (permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.DMViewInventory))
      {
        return;
      }

      if (!HasPermissionToTarget(eventData.DungeonMaster, eventData.Target, DMPermissionConstants.DMViewInventory))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMSpawnObject(OnDMSpawnObjectBefore eventData)
    {
      if (permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.DMSpawn))
      {
        return;
      }

      bool hasPermission = eventData.ObjectType switch
      {
        ObjectTypes.Creature => permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.DMSpawn + DMPermissionConstants.TargetCreature),
        ObjectTypes.Item => permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.DMSpawn + DMPermissionConstants.TargetItem),
        ObjectTypes.Trigger => permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.DMSpawn + DMPermissionConstants.TargetTrigger),
        ObjectTypes.Door => permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.DMSpawn + DMPermissionConstants.TargetDoor),
        ObjectTypes.Waypoint => permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.DMSpawn + DMPermissionConstants.TargetWaypoint),
        ObjectTypes.Placeable => permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.DMSpawn + DMPermissionConstants.TargetPlaceable),
        ObjectTypes.Store => permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.DMSpawn + DMPermissionConstants.TargetStore),
        ObjectTypes.Encounter => permissionsService.HasPermission(eventData.DungeonMaster, DMPermissionConstants.DMSpawn + DMPermissionConstants.TargetEncounter),
        _ => false,
      };

      if (!hasPermission)
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private bool HasPermissionToTarget(NwPlayer dungeonMaster, NwObject target, string permissionBase)
    {
      bool targetSelf = permissionsService.HasPermission(dungeonMaster, permissionBase + DMPermissionConstants.TargetSelf);
      bool targetPlayer = permissionsService.HasPermission(dungeonMaster, permissionBase + DMPermissionConstants.TargetPlayer);

      NwCreature dmCreature = dungeonMaster.ControlledCreature;

      if (targetSelf && dmCreature == target)
      {
        return true;
      }

      if (targetPlayer && target != dmCreature && (target.IsPlayerControlled(out _) || target.IsLoginPlayerCharacter(out _)))
      {
        return true;
      }

      return target switch
      {
        NwCreature => permissionsService.HasPermission(dungeonMaster, permissionBase + DMPermissionConstants.TargetCreature),
        NwItem => permissionsService.HasPermission(dungeonMaster, permissionBase + DMPermissionConstants.TargetItem),
        NwEncounter => permissionsService.HasPermission(dungeonMaster, permissionBase + DMPermissionConstants.TargetEncounter),
        NwWaypoint => permissionsService.HasPermission(dungeonMaster, permissionBase + DMPermissionConstants.TargetWaypoint),
        NwTrigger => permissionsService.HasPermission(dungeonMaster, permissionBase + DMPermissionConstants.TargetTrigger),
        NwPlaceable => permissionsService.HasPermission(dungeonMaster, permissionBase + DMPermissionConstants.TargetPlaceable),
        _ => false,
      };
    }

    private void ShowNoPermissionError(NwPlayer player)
    {
      player.SendServerMessage("You do not have permission to do that.", ColorConstants.Red);
    }
  }
}
