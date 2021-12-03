using Anvil.API;
using Anvil.API.Events;

namespace Jorteck.Toolbox
{
  public sealed class PlayerVitalsWindowController : WindowController<PlayerVitalsWindowView>
  {
    private NuiBind<bool>[] widgetEnabledBinds;
    private NwPlayer selectedPlayer;

    public override void Init()
    {
      widgetEnabledBinds = new[]
      {
        View.FirstNameEnabled,
        View.LastNameEnabled,
        View.GenderEnabled,
        View.RaceEnabled,
        View.SubRaceEnabled,
        View.AgeEnabled,
        View.DeityEnabled,
        View.DescriptionEnabled,
        View.SaveEnabled,
      };

      Update();
    }

    public override void ProcessEvent(ModuleEvents.OnNuiEvent eventData)
    {
      switch (eventData.EventType)
      {
        case NuiEventType.Click:
          HandleButtonClick(eventData);
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
      SetBindValue(View.FirstName, playerCreature.OriginalFirstName);
      SetBindValue(View.LastName, playerCreature.OriginalLastName);
      SetBindValue(View.Gender, (int)playerCreature.Gender);
      SetBindValue(View.Race, ((int)playerCreature.Race.RacialType).ToString());
      SetBindValue(View.SubRace, playerCreature.SubRace);
      SetBindValue(View.Age, playerCreature.Age.ToString());
      SetBindValue(View.Deity, playerCreature.Deity);
      SetBindValue(View.Description, playerCreature.Description);
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

    private void SaveChanges()
    {
      if (selectedPlayer == null || !selectedPlayer.IsValid)
      {
        return;
      }

      NwCreature playerCreature = selectedPlayer.LoginCreature;
      playerCreature.OriginalFirstName = GetBindValue(View.FirstName);
      playerCreature.OriginalLastName = GetBindValue(View.LastName);

      playerCreature.Name = playerCreature.OriginalFirstName + " " + playerCreature.OriginalLastName;
      playerCreature.Gender = (Gender)GetBindValue(View.Gender);

      if (GetBindValue(View.Race).TryParseInt(out int racialType))
      {
        playerCreature.Race = NwRace.FromRacialType((RacialType)racialType);
      }

      playerCreature.SubRace = GetBindValue(View.SubRace);

      if (GetBindValue(View.Age).TryParseInt(out int age))
      {
        playerCreature.Age = age;
      }

      playerCreature.Deity = GetBindValue(View.Deity);
      playerCreature.Description = GetBindValue(View.Description);

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
