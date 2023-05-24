using System.Globalization;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.ToolWindows
{
  public sealed class CreaturePropertiesBasicWindowController : WindowController<CreaturePropertiesBasicWindowView>
  {
    private NuiBind<bool>[] widgetEnabledBinds;
    private NwCreature selectedCreature;

    public override void Init()
    {
      widgetEnabledBinds = new[]
      {
        View.NameEnabled,
        View.TagEnabled,
        View.RaceEnabled,
        View.AppearanceEnabled,
        View.PhenotypeEnabled,
        View.GenderEnabled,
        View.DescriptionEnabled,
        View.PortraitEnabled,
        View.DialogEnabled,
        View.CREnabled,
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
      selectedCreature = null;
    }

    private void Update()
    {
      if (selectedCreature == null)
      {
        SetElementsEnabled(false);
        return;
      }

      ApplyPermissionBindings(widgetEnabledBinds);
      Token.SetBindValue(View.Name, selectedCreature.Name);
      Token.SetBindValue(View.Tag, selectedCreature.Tag);
      string value = ((int)selectedCreature.Race.RacialType).ToString();
      Token.SetBindValue(View.Race, value);
      string value1 = selectedCreature.Appearance.RowIndex.ToString();
      Token.SetBindValue(View.Appearance, value1);
      string value2 = ((int)selectedCreature.Phenotype).ToString();
      Token.SetBindValue(View.Phenotype, value2);
      int value3 = (int)selectedCreature.Gender;
      Token.SetBindValue(View.Gender, value3);
      Token.SetBindValue(View.Description, selectedCreature.Description);
      Token.SetBindValue(View.CR, selectedCreature.ChallengeRating.ToString(CultureInfo.InvariantCulture));
      Token.SetBindValue(View.Dialog, selectedCreature.DialogResRef);
      Token.SetBindValue(View.Portrait, selectedCreature.PortraitResRef);

      UpdatePortraitPreview();
    }

    private void HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.SelectCreatureButton.Id)
      {
        Token.Player.TryEnterTargetMode(OnCreatureSelected, new TargetModeSettings
        {
          ValidTargets = ObjectTypes.Creature,
        });
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
      if (selectedCreature == null || !selectedCreature.IsValid)
      {
        return;
      }

      selectedCreature.Name = Token.GetBindValue(View.Name)!;
      selectedCreature.Tag = Token.GetBindValue(View.Tag)!;
      if (Token.GetBindValue(View.Race)!.TryParseInt(out int racialType))
      {
        selectedCreature.Race = NwRace.FromRaceId(racialType)!;
      }

      if (Token.GetBindValue(View.Appearance)!.TryParseInt(out int appearanceType))
      {
        if (appearanceType > 0 && appearanceType < NwGameTables.AppearanceTable.Count)
        {
          selectedCreature.Appearance = NwGameTables.AppearanceTable[appearanceType];
        }
      }

      if (Token.GetBindValue(View.Phenotype)!.TryParseInt(out int phenotype))
      {
        selectedCreature.Phenotype = (Phenotype)phenotype;
      }

      selectedCreature.Gender = (Gender)Token.GetBindValue(View.Gender);
      selectedCreature.Description = Token.GetBindValue(View.Description)!;

      if (Token.GetBindValue(View.CR)!.TryParseFloat(out float challengeRating))
      {
        selectedCreature.ChallengeRating = challengeRating;
      }

      selectedCreature.PortraitResRef = Token.GetBindValue(View.Portrait)!;
      selectedCreature.DialogResRef = Token.GetBindValue(View.Dialog)!;

      Update();
    }

    private void OnCreatureSelected(ModuleEvents.OnPlayerTarget eventData)
    {
      if (eventData.TargetObject == null || eventData.TargetObject is not NwCreature creature)
      {
        return;
      }

      if (creature.IsLoginPlayerCharacter)
      {
        eventData.Player.SendServerMessage("You may not select players.");
        return;
      }

      selectedCreature = creature;
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
