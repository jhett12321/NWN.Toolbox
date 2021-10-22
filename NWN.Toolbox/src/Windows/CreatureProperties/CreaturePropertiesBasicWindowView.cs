using System.Collections.Generic;
using Anvil.API;

namespace Jorteck.Toolbox
{
  public sealed class CreaturePropertiesBasicWindowView : WindowView<CreaturePropertiesBasicWindowView, CreaturePropertiesBasicWindowController>
  {
    public override string Id => "creature.basic";
    public override string Title => "Creature Properties: Basic";

    public override NuiWindow WindowTemplate { get; }

    // Permission Binds
    public readonly NuiBind<bool> CreatureNameEnabled = new NuiBind<bool>("name");
    public readonly NuiBind<bool> CreatureTagEnabled = new NuiBind<bool>("tag");
    public readonly NuiBind<bool> CreatureRaceEnabled = new NuiBind<bool>("race");
    public readonly NuiBind<bool> CreatureAppearanceEnabled = new NuiBind<bool>("appearance");
    public readonly NuiBind<bool> CreaturePhenotypeEnabled = new NuiBind<bool>("phenotype");
    public readonly NuiBind<bool> CreatureGenderEnabled = new NuiBind<bool>("gender");
    public readonly NuiBind<bool> CreatureDescriptionEnabled = new NuiBind<bool>("description");
    public readonly NuiBind<bool> CreaturePortraitEnabled = new NuiBind<bool>("portrait");
    public readonly NuiBind<bool> CreatureDialogueEnabled = new NuiBind<bool>("dialog");
    public readonly NuiBind<bool> SaveEnabled = new NuiBind<bool>("save");

    // Value Binds
    public readonly NuiBind<string> CreatureName = new NuiBind<string>("name_val");
    public readonly NuiBind<string> CreatureTag = new NuiBind<string>("tag_val");
    public readonly NuiBind<string> CreatureRace = new NuiBind<string>("race_val");
    public readonly NuiBind<string> CreatureAppearance = new NuiBind<string>("appearance_val");
    public readonly NuiBind<string> CreaturePhenotype = new NuiBind<string>("phenotype_val");
    public readonly NuiBind<int> CreatureGender = new NuiBind<int>("gender_val");
    public readonly NuiBind<string> CreatureDescription = new NuiBind<string>("description_val");
    public readonly NuiBind<string> CreaturePortrait = new NuiBind<string>("portrait_val");
    public readonly NuiBind<string> CreatureDialogue = new NuiBind<string>("dialog_val");

    // Buttons
    public readonly NuiButton SelectCreatureButton;
    public readonly NuiButton SaveChangesButton;
    public readonly NuiButton DiscardChangesButton;

    public CreaturePropertiesBasicWindowView()
    {
      SelectCreatureButton = new NuiButton("Select Creature")
      {
        Id = "btn_crt_sel",
      };

      SaveChangesButton = new NuiButton("Save")
      {
        Id = "btn_save",
        Enabled = SaveEnabled,
      };

      DiscardChangesButton = new NuiButton("Discard")
      {
        Id = "btn_discard",
        Enabled = SaveEnabled,
      };

      NuiColumn root = new NuiColumn
      {
        Children = new List<NuiElement>
        {
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel(CreatureName)
              {
                VerticalAlign = NuiVAlign.Middle,
                HorizontalAlign = NuiHAlign.Center,
              },
            },
          },
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Name")
              {
                Width = 80f,
                Margin = 10f,
              },
              new NuiTextEdit(string.Empty, CreatureName, 255, false)
              {
                Enabled = CreatureNameEnabled,
              },
            },
          },
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Tag")
              {
                Width = 80f,
                Margin = 10f,
              },
              new NuiTextEdit(string.Empty, CreatureTag, 255, false)
              {
                Enabled = CreatureTagEnabled,
              },
            },
          },
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Race")
              {
                Width = 80f,
                Margin = 10f,
              },
              new NuiTextEdit(string.Empty, CreatureRace, 255, false)
              {
                Enabled = CreatureRaceEnabled,
              },
            },
          },
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Appearance")
              {
                Width = 80f,
                Margin = 10f,
              },
              new NuiTextEdit(string.Empty, CreatureAppearance, 255, false)
              {
                Enabled = CreatureAppearanceEnabled,
              },
            },
          },
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Phenotype")
              {
                Width = 80f,
                Margin = 10f,
              },
              new NuiTextEdit(string.Empty, CreaturePhenotype, 255, false)
              {
                Enabled = CreaturePhenotypeEnabled,
              },
            },
          },
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Gender")
              {
                Width = 80f,
                Margin = 10f,
              },
              NuiUtils.CreateComboForEnum<Gender>(CreatureGender, CreatureGenderEnabled),
            },
          },
          new NuiRow
          {
            Height = 160f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Description")
              {
                Width = 80f,
                Margin = 10f,
              },
              new NuiTextEdit(string.Empty, CreatureDescription, 255, true)
              {
                Height = 150f,
                Enabled = CreatureDescriptionEnabled,
              },
            },
          },
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Portrait")
              {
                Width = 80f,
                Margin = 10f,
              },
              new NuiTextEdit(string.Empty, CreaturePortrait, 255, false)
              {
                Enabled = CreaturePortraitEnabled,
              },
            },
          },
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Dialogue")
              {
                Width = 80f,
                Margin = 10f,
              },
              new NuiTextEdit(string.Empty, CreatureDialogue, 255, false)
              {
                Enabled = CreatureDialogueEnabled,
              },
            },
          },
          new NuiSpacer(),
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              SelectCreatureButton,
              new NuiSpacer(),
              SaveChangesButton,
              DiscardChangesButton,
            },
          },
        },
      };

      WindowTemplate = new NuiWindow(root, Title)
      {
        Geometry = new NuiRect(0, 0, 500, 700),
      };
    }
  }
}
