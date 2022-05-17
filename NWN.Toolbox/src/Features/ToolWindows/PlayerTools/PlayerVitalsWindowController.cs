using Anvil.API;
using Anvil.API.Events;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.ToolWindows
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
      if (playerCreature == null)
      {
        return;
      }

      ApplyPermissionBindings(widgetEnabledBinds);
      string value = $"Player: {selectedPlayer.PlayerName}";
      Token.SetBindValue(View.PlayerName, value);
      Token.SetBindValue(View.FirstName, playerCreature.OriginalFirstName);
      Token.SetBindValue(View.LastName, playerCreature.OriginalLastName);
      int value1 = (int)playerCreature.Gender;
      Token.SetBindValue(View.Gender, value1);
      string value2 = ((int)playerCreature.Race.RacialType).ToString();
      Token.SetBindValue(View.Race, value2);
      Token.SetBindValue(View.SubRace, playerCreature.SubRace);
      Token.SetBindValue(View.Age, playerCreature.Age.ToString());
      Token.SetBindValue(View.Deity, playerCreature.Deity);
      Token.SetBindValue(View.Description, playerCreature.Description);
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

      playerCreature.OriginalFirstName = Token.GetBindValue(View.FirstName)!;
      playerCreature.OriginalLastName = Token.GetBindValue(View.LastName)!;

      playerCreature.Name = playerCreature.OriginalFirstName + " " + playerCreature.OriginalLastName;
      playerCreature.Gender = (Gender)Token.GetBindValue(View.Gender);

      if (Token.GetBindValue(View.Race)!.TryParseInt(out int racialType))
      {
        playerCreature.Race = NwRace.FromRaceId(racialType)!;
      }

      playerCreature.SubRace = Token.GetBindValue(View.SubRace)!;

      if (Token.GetBindValue(View.Age)!.TryParseInt(out int age))
      {
        playerCreature.Age = age;
      }

      playerCreature.Deity = Token.GetBindValue(View.Deity)!;
      playerCreature.Description = Token.GetBindValue(View.Description)!;

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
