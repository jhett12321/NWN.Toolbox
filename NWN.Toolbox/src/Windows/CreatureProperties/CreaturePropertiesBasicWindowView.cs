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
    public readonly NuiBind<bool> NameEnabled = new NuiBind<bool>("name");
    public readonly NuiBind<bool> TagEnabled = new NuiBind<bool>("tag");
    public readonly NuiBind<bool> RaceEnabled = new NuiBind<bool>("race");
    public readonly NuiBind<bool> AppearanceEnabled = new NuiBind<bool>("appearance");
    public readonly NuiBind<bool> PhenotypeEnabled = new NuiBind<bool>("phenotype");
    public readonly NuiBind<bool> GenderEnabled = new NuiBind<bool>("gender");
    public readonly NuiBind<bool> DescriptionEnabled = new NuiBind<bool>("description");
    public readonly NuiBind<bool> PortraitEnabled = new NuiBind<bool>("portrait");
    public readonly NuiBind<bool> DialogEnabled = new NuiBind<bool>("dialog");
    public readonly NuiBind<bool> SaveEnabled = new NuiBind<bool>("save");

    // Value Binds
    public readonly NuiBind<string> Name = new NuiBind<string>("name_val");
    public readonly NuiBind<string> Tag = new NuiBind<string>("tag_val");
    public readonly NuiBind<string> Race = new NuiBind<string>("race_val");
    public readonly NuiBind<string> Appearance = new NuiBind<string>("appearance_val");
    public readonly NuiBind<string> Phenotype = new NuiBind<string>("phenotype_val");
    public readonly NuiBind<int> Gender = new NuiBind<int>("gender_val");
    public readonly NuiBind<string> Description = new NuiBind<string>("description_val");
    public readonly NuiBind<string> Portrait = new NuiBind<string>("portrait_val");
    public readonly NuiBind<string> PortraitPreview = new NuiBind<string>("portrait_prev");
    public readonly NuiBind<string> Dialog = new NuiBind<string>("dialog_val");

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
              new NuiLabel(Name)
              {
                VerticalAlign = NuiVAlign.Middle,
                HorizontalAlign = NuiHAlign.Center,
              },
            },
          },
          new NuiRow
          {
            Height = 40f,
            Enabled = NameEnabled,
            Children = new List<NuiElement>
            {
              new NuiLabel("Name")
              {
                Width = 80f,
                Margin = 10f,
              },
              new NuiTextEdit(string.Empty, Name, 255, false),
            },
          },
          new NuiRow
          {
            Height = 40f,
            Enabled = TagEnabled,
            Children = new List<NuiElement>
            {
              new NuiLabel("Tag")
              {
                Width = 80f,
                Margin = 10f,
              },
              new NuiTextEdit(string.Empty, Tag, 255, false),
            },
          },
          new NuiRow
          {
            Height = 40f,
            Enabled = RaceEnabled,
            Children = new List<NuiElement>
            {
              new NuiLabel("Race")
              {
                Width = 80f,
                Margin = 10f,
              },
              new NuiTextEdit(string.Empty, Race, 255, false),
            },
          },
          new NuiRow
          {
            Height = 40f,
            Enabled = AppearanceEnabled,
            Children = new List<NuiElement>
            {
              new NuiLabel("Appearance")
              {
                Width = 80f,
                Margin = 10f,
              },
              new NuiTextEdit(string.Empty, Appearance, 255, false),
            },
          },
          new NuiRow
          {
            Height = 40f,
            Enabled = PhenotypeEnabled,
            Children = new List<NuiElement>
            {
              new NuiLabel("Phenotype")
              {
                Width = 80f,
                Margin = 10f,
              },
              new NuiTextEdit(string.Empty, Phenotype, 255, false),
            },
          },
          new NuiRow
          {
            Height = 40f,
            Enabled = GenderEnabled,
            Children = new List<NuiElement>
            {
              new NuiLabel("Gender")
              {
                Width = 80f,
                Margin = 10f,
              },
              NuiUtils.CreateComboForEnum<Gender>(Gender),
            },
          },
          new NuiRow
          {
            Height = 160f,
            Enabled = DescriptionEnabled,
            Children = new List<NuiElement>
            {
              new NuiLabel("Description")
              {
                Width = 80f,
                Margin = 10f,
              },
              new NuiTextEdit(string.Empty, Description, ushort.MaxValue, true)
              {
                Height = 150f,
              },
            },
          },
          new NuiRow
          {
            Height = 105f,
            Enabled = PortraitEnabled,
            Children = new List<NuiElement>
            {
              new NuiLabel("Portrait")
              {
                Width = 80f,
                Margin = 10f,
              },
              new NuiImage(PortraitPreview)
              {
                Width = 64,
                Height = 100,
                ImageAspect = NuiAspect.Fit100,
                Visible = PortraitEnabled,
              },
              new NuiTextEdit(string.Empty, Portrait, 255, false),
            },
          },
          new NuiRow
          {
            Height = 40f,
            Enabled = DialogEnabled,
            Children = new List<NuiElement>
            {
              new NuiLabel("Conversation")
              {
                Width = 85f,
                Margin = 10f,
              },
              new NuiTextEdit(string.Empty, Dialog, 255, false),
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
        Geometry = new NuiRect(0, 0, 500, 720),
      };
    }
  }
}
