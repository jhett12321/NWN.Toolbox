using Anvil.API;
using Anvil.API.Events;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.ToolWindows
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

      Token.SetBindWatch(View.Portrait, true);
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
      if (playerCreature == null)
      {
        return;
      }

      ApplyPermissionBindings(widgetEnabledBinds);
      string value = $"Player: {selectedPlayer.PlayerName}";
      Token.SetBindValue(View.PlayerName, value);
      string value1 = $"{playerCreature.OriginalFirstName} {playerCreature.OriginalLastName}";
      Token.SetBindValue(View.CreatureName, value1);
      Token.SetBindValue(View.Portrait, playerCreature.PortraitResRef);
      Token.SetBindValue(View.SoundSet, playerCreature.SoundSet.ToString());
      string value2 = playerCreature.Appearance.RowIndex.ToString();
      Token.SetBindValue(View.Appearance, value2);

      UpdatePortraitPreview();
    }

    private void HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.SelectPlayerButton.Id)
      {
        Token.Player.TryEnterTargetMode(OnCreatureSelected, ObjectTypes.Creature);
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
      string value = Token.GetBindValue(View.Portrait) + "l";
      Token.SetBindValue(View.PortraitPreview, value);
    }

    private void SaveChanges()
    {
      if (selectedPlayer == null || !selectedPlayer.IsValid)
      {
        return;
      }

      NwCreature playerCreature = selectedPlayer.LoginCreature;
      if (playerCreature == null)
      {
        return;
      }

      playerCreature.PortraitResRef = Token.GetBindValue(View.Portrait)!;

      if (Token.GetBindValue(View.Appearance)!.TryParseInt(out int appearanceType))
      {
        if (appearanceType > 0 && appearanceType < NwGameTables.AppearanceTable.Count)
        {
          playerCreature.Appearance = NwGameTables.AppearanceTable[appearanceType];
        }
      }

      if (ushort.TryParse(Token.GetBindValue(View.SoundSet), out ushort soundSet))
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
        Token.SetBindValue(bind, enabled);
      }
    }
  }
}
