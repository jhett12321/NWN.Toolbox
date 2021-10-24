using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  public sealed class CreaturePropertiesBasicWindowController : WindowController<CreaturePropertiesBasicWindowController, CreaturePropertiesBasicWindowView>
  {
    [Inject]
    public CursorTargetService CursorTargetService { private get; init; }

    private NuiBind<bool>[] widgetEnabledBinds;
    private NwCreature selectedCreature;

    public override void Init()
    {
      widgetEnabledBinds = new[]
      {
        View.CreatureNameEnabled,
        View.CreatureTagEnabled,
        View.CreatureRaceEnabled,
        View.CreatureAppearanceEnabled,
        View.CreaturePhenotypeEnabled,
        View.CreatureGenderEnabled,
        View.CreatureDescriptionEnabled,
        View.CreaturePortraitEnabled,
        View.CreatureDialogueEnabled,
        View.SaveEnabled,
      };

      SetBindWatch(View.CreaturePortrait, true);
      Update();
    }

    public override void ProcessEvent(ModuleEvents.OnNuiEvent eventData)
    {
      switch (eventData.EventType)
      {
        case NuiEventType.Unknown:
          break;
        case NuiEventType.Click:
          HandleButtonClick(eventData);
          break;
        case NuiEventType.Watch:
          HandleWatchUpdate(eventData);
          break;
        case NuiEventType.Open:
          Update();
          break;
        case NuiEventType.Focus:
          break;
        case NuiEventType.Blur:
          break;
        case NuiEventType.MouseDown:
          break;
        case NuiEventType.MouseUp:
          break;
      }
    }

    public override void OnClose()
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
      SetBindValue(View.CreatureName, selectedCreature.Name);
      SetBindValue(View.CreatureTag, selectedCreature.Tag);
      SetBindValue(View.CreatureRace, ((int)selectedCreature.RacialType).ToString());
      SetBindValue(View.CreatureAppearance, ((int)selectedCreature.CreatureAppearanceType).ToString());
      SetBindValue(View.CreaturePhenotype, ((int)selectedCreature.Phenotype).ToString());
      SetBindValue(View.CreatureGender, (int)selectedCreature.Gender);
      SetBindValue(View.CreatureDescription, selectedCreature.Description);
      SetBindValue(View.CreatureDialogue, selectedCreature.DialogResRef);
      SetBindValue(View.CreaturePortrait, selectedCreature.PortraitResRef);

      UpdatePortraitPreview();
    }

    private void HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.SelectCreatureButton.Id)
      {
        CursorTargetService.EnterTargetMode(Player, target => OnCreatureSelected(eventData.WindowToken, target), ObjectTypes.Creature);
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
      if (eventData.ElementId == View.CreaturePortrait.Key)
      {
        UpdatePortraitPreview();
      }
    }

    private void UpdatePortraitPreview()
    {
      SetBindValue(View.CreaturePortraitPreview, GetBindValue(View.CreaturePortrait) + "l");
    }

    private void SaveChanges()
    {
      if (selectedCreature == null || !selectedCreature.IsValid)
      {
        return;
      }

      selectedCreature.Name = GetBindValue(View.CreatureName);
      selectedCreature.Tag = GetBindValue(View.CreatureTag);
      if (GetBindValue(View.CreatureRace).TryParseInt(out int racialType))
      {
        selectedCreature.RacialType = (RacialType)racialType;
      }

      if (GetBindValue(View.CreatureAppearance).TryParseInt(out int appearanceType))
      {
        selectedCreature.CreatureAppearanceType = (AppearanceType)appearanceType;
      }

      if (GetBindValue(View.CreaturePhenotype).TryParseInt(out int phenotype))
      {
        selectedCreature.Phenotype = (Phenotype)phenotype;
      }

      selectedCreature.Gender = (Gender)GetBindValue(View.CreatureGender);
      selectedCreature.Description = GetBindValue(View.CreatureDescription);
      selectedCreature.PortraitResRef = GetBindValue(View.CreaturePortrait);
      selectedCreature.DialogResRef = GetBindValue(View.CreatureDialogue);

      Update();
    }

    private void OnCreatureSelected(int token, ModuleEvents.OnPlayerTarget eventData)
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
