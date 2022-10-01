using System.Collections.Generic;
using Anvil.API;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.ToolWindows
{
  public sealed class PlayerVitalsWindowView : WindowView<PlayerVitalsWindowView>
  {
    public override string Id => "player.vital";
    public override string Title => "Player Properties: Vitals";
    public override NuiWindow WindowTemplate { get; }

    public override IWindowController CreateDefaultController(NwPlayer player)
    {
      return CreateController<PlayerVitalsWindowController>(player);
    }

    // Permission Binds
    public readonly NuiBind<bool> FirstNameEnabled = new NuiBind<bool>("first_name");
    public readonly NuiBind<bool> LastNameEnabled = new NuiBind<bool>("last_name");
    public readonly NuiBind<bool> GenderEnabled = new NuiBind<bool>("gender");
    public readonly NuiBind<bool> RaceEnabled = new NuiBind<bool>("race");
    public readonly NuiBind<bool> SubRaceEnabled = new NuiBind<bool>("subrace");
    public readonly NuiBind<bool> AgeEnabled = new NuiBind<bool>("age");
    public readonly NuiBind<bool> DeityEnabled = new NuiBind<bool>("deity");
    public readonly NuiBind<bool> DescriptionEnabled = new NuiBind<bool>("description");
    public readonly NuiBind<bool> SaveEnabled = new NuiBind<bool>("save");

    // Value Binds
    public readonly NuiBind<string> PlayerName = new NuiBind<string>("player_name_val");
    public readonly NuiBind<string> FirstName = new NuiBind<string>("first_name_val");
    public readonly NuiBind<string> LastName = new NuiBind<string>("last_name_val");
    public readonly NuiBind<int> Gender = new NuiBind<int>("gender_val");
    public readonly NuiBind<string> Race = new NuiBind<string>("race_val");
    public readonly NuiBind<string> SubRace = new NuiBind<string>("subrace_val");
    public readonly NuiBind<string> Age = new NuiBind<string>("age_val");
    public readonly NuiBind<string> Deity = new NuiBind<string>("deity_val");
    public readonly NuiBind<string> Description = new NuiBind<string>("description_val");

    // Buttons
    public readonly NuiButton SelectPlayerButton;
    public readonly NuiButton SaveChangesButton;
    public readonly NuiButton DiscardChangesButton;

    public PlayerVitalsWindowView()
    {
      NuiColumn root = new NuiColumn
      {
        Children = new List<NuiElement>
        {
          new NuiRow
          {
            Margin = 5f,
            Height = 15f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Name - Hover for more info.")
              {
                Tooltip = "Reconnect required to see changes. NOTE: Will destroy any ephemeral data associated with the player (TURD).",
              },
            },
          },
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiTextEdit(string.Empty, FirstName, 255, false)
              {
                Width = 230f,
                Enabled = FirstNameEnabled,
              },
              new NuiSpacer
              {
                Width = 10f,
              },
              new NuiTextEdit(string.Empty, LastName, 255, false)
              {
                Width = 230f,
                Enabled = LastNameEnabled,
              },
            },
          },
          new NuiRow
          {
            Margin = 5f,
            Height = 15f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Gender"),
              new NuiLabel("Race"),
              new NuiLabel("Age"),
            },
          },
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              NuiUtils.CreateComboForEnum<Gender>(Gender).Configure(combo =>
              {
                combo.Enabled = GenderEnabled;
              }),
              new NuiTextEdit(string.Empty, Race, 255, false)
              {
                Width = 157f,
                Enabled = RaceEnabled,
              },
              new NuiTextEdit(string.Empty, Age, 255, false)
              {
                Width = 157f,
                Enabled = AgeEnabled,
              },
            },
          },
          new NuiRow
          {
            Margin = 5f,
            Height = 15f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Subrace"),
              new NuiSpacer
              {
                Width = 10f,
              },
              new NuiLabel("Deity"),
            },
          },
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiTextEdit(string.Empty, SubRace, 255, false)
              {
                Width = 230f,
                Enabled = SubRaceEnabled,
              },
              new NuiSpacer
              {
                Width = 10f,
              },
              new NuiTextEdit(string.Empty, Deity, 255, false)
              {
                Width = 230f,
                Enabled = DeityEnabled,
              },
            },
          },
          new NuiRow
          {
            Margin = 5f,
            Height = 15f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Description"),
            },
          },
          new NuiRow
          {
            Height = 155f,
            Enabled = DescriptionEnabled,
            Children = new List<NuiElement>
            {
              new NuiTextEdit(string.Empty, Description, ushort.MaxValue, true)
              {
                Height = 150f,
              },
            },
          },
          new NuiSpacer(),
          new NuiRow
          {
            Margin = 5f,
            Height = 15f,
            Children = new List<NuiElement>
            {
              new NuiLabel(PlayerName),
            },
          },
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiButton("Select Player")
              {
                Id = "btn_crt_sel",
              }.Assign(out SelectPlayerButton),
              new NuiSpacer(),
              new NuiButton("Save")
              {
                Id = "btn_save",
                Enabled = SaveEnabled,
              }.Assign(out SaveChangesButton),
              new NuiButton("Discard")
              {
                Id = "btn_discard",
                Enabled = SaveEnabled,
              }.Assign(out DiscardChangesButton),
            },
          },
        },
      };

      WindowTemplate = new NuiWindow(root, Title)
      {
        Geometry = new NuiRect(500f, 100f, 500f, 505f),
      };
    }
  }
}
