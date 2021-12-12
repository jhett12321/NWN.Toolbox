using Anvil.API;
using Anvil.API.Events;

namespace Jorteck.Toolbox
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
      SetBindValue(View.Name, selectedCreature.Name);
      SetBindValue(View.Tag, selectedCreature.Tag);
      SetBindValue(View.Race, ((int)selectedCreature.Race.RacialType).ToString());
      SetBindValue(View.Appearance, ((int)selectedCreature.CreatureAppearanceType).ToString());
      SetBindValue(View.Phenotype, ((int)selectedCreature.Phenotype).ToString());
      SetBindValue(View.Gender, (int)selectedCreature.Gender);
      SetBindValue(View.Description, selectedCreature.Description);
      SetBindValue(View.Dialog, selectedCreature.DialogResRef);
      SetBindValue(View.Portrait, selectedCreature.PortraitResRef);

      UpdatePortraitPreview();
    }

    private void HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.SelectCreatureButton.Id)
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
      if (selectedCreature == null || !selectedCreature.IsValid)
      {
        return;
      }

      selectedCreature.Name = GetBindValue(View.Name);
      selectedCreature.Tag = GetBindValue(View.Tag);
      if (GetBindValue(View.Race).TryParseInt(out int racialType))
      {
        selectedCreature.Race = NwRace.FromRacialType((RacialType)racialType);
      }

      if (GetBindValue(View.Appearance).TryParseInt(out int appearanceType))
      {
        selectedCreature.CreatureAppearanceType = (AppearanceType)appearanceType;
      }

      if (GetBindValue(View.Phenotype).TryParseInt(out int phenotype))
      {
        selectedCreature.Phenotype = (Phenotype)phenotype;
      }

      selectedCreature.Gender = (Gender)GetBindValue(View.Gender);
      selectedCreature.Description = GetBindValue(View.Description);
      selectedCreature.PortraitResRef = GetBindValue(View.Portrait);
      selectedCreature.DialogResRef = GetBindValue(View.Dialog);

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
        SetBindValue(bind, enabled);
      }
    }
  }
}
