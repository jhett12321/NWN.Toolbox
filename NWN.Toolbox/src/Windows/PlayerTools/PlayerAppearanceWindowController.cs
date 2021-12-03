using Anvil.API;
using Anvil.API.Events;

namespace Jorteck.Toolbox
{
  public sealed class PlayerAppearanceWindowController : WindowController<PlayerAppearanceWindowView>
  {
    private NuiBind<bool>[] widgetEnabledBinds;
    private NwPlayer selectedPlayer;

    public override void Init()
    {
      widgetEnabledBinds = new[]
      {
        View.PortraitEnabled,
        View.SoundSetEnabled,
        View.AppearanceEnabled,
        View.SaveEnabled,
      };

      SetBindWatch(View.Portrait, true);
      Update();
    }

    public override void ProcessEvent(ModuleEvents.OnNuiEvent eventData)
    {
      switch (eventData.EventType)
      {
        case NuiEventType.Click:
          HandleButtonClick(eventData);
          break;
        case NuiEventType.Watch:
          HandleWatchUpdate(eventData);
          break;
        case NuiEventType.Open:
          Update();
          break;
      }
    }

    protected override void OnClose()
    {
      selectedPlayer = null;
    }

    private void Update()
    {
      if (selectedPlayer == null || !selectedPlayer.IsValid)
      {
        SetElementsEnabled(false);
        return;
      }

      NwCreature playerCreature = selectedPlayer.LoginCreature;

      ApplyPermissionBindings(widgetEnabledBinds);
      SetBindValue(View.PlayerName, $"Player: {selectedPlayer.PlayerName}");
      SetBindValue(View.CreatureName, $"{playerCreature.OriginalFirstName} {playerCreature.OriginalLastName}");
      SetBindValue(View.Portrait, playerCreature.PortraitResRef);
      SetBindValue(View.SoundSet, playerCreature.SoundSet.ToString());
      SetBindValue(View.Appearance, ((int)playerCreature.CreatureAppearanceType).ToString());

      UpdatePortraitPreview();
    }

    private void HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.SelectPlayerButton.Id)
      {
        Player.TryEnterTargetMode(OnCreatureSelected, ObjectTypes.Creature);
      }
      else if (eventData.ElementId == View.SaveChangesButton.Id)
      {
        SaveChanges();
      }
      else if (eventData.ElementId == View.DiscardChangesButton.Id)
      {
        Update();
      }
    }

    private void HandleWatchUpdate(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.Portrait.Key)
      {
        UpdatePortraitPreview();
      }
    }

    private void UpdatePortraitPreview()
    {
      SetBindValue(View.PortraitPreview, GetBindValue(View.Portrait) + "l");
    }

    private void SaveChanges()
    {
      if (selectedPlayer == null || !selectedPlayer.IsValid)
      {
        return;
      }

      NwCreature playerCreature = selectedPlayer.LoginCreature;
      playerCreature.PortraitResRef = GetBindValue(View.Portrait);

      if (GetBindValue(View.Appearance).TryParseInt(out int appearanceType))
      {
        playerCreature.CreatureAppearanceType = (AppearanceType)appearanceType;
      }

      if (ushort.TryParse(GetBindValue(View.SoundSet), out ushort soundSet))
      {
        playerCreature.SoundSet = soundSet;
      }

      Update();
    }

    private void OnCreatureSelected(ModuleEvents.OnPlayerTarget eventData)
    {
      if (eventData.TargetObject == null || eventData.TargetObject is not NwCreature creature)
      {
        return;
      }

      if (!creature.IsLoginPlayerCharacter(out NwPlayer player))
      {
        eventData.Player.SendServerMessage("You may only select player characters.");
        return;
      }

      selectedPlayer = player;
      Update();
    }

    private void SetElementsEnabled(bool enabled)
    {
      foreach (NuiBind<bool> bind in widgetEnabledBinds)
      {
        SetBindValue(bind, enabled);
      }
    }
  }
}
