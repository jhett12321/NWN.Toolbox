using System.Collections.Generic;
using Anvil.API;

namespace Jorteck.Toolbox
{
  public sealed class PlayerAppearanceWindowView : WindowView<PlayerAppearanceWindowView, PlayerAppearanceWindowController>
  {
    public override string Id => "player.appearance";
    public override string Title => "Player Properties: Appearance";

    public override NuiWindow WindowTemplate { get; }

    // Permission Binds
    public readonly NuiBind<bool> PortraitEnabled = new NuiBind<bool>("portrait");
    public readonly NuiBind<bool> SoundSetEnabled = new NuiBind<bool>("voice_set");
    public readonly NuiBind<bool> AppearanceEnabled = new NuiBind<bool>("appearance");
    public readonly NuiBind<bool> SaveEnabled = new NuiBind<bool>("save");

    // Value Binds
    public readonly NuiBind<string> PlayerName = new NuiBind<string>("player_name_val");
    public readonly NuiBind<string> CreatureName = new NuiBind<string>("creature_name_val");
    public readonly NuiBind<string> Portrait = new NuiBind<string>("portrait_val");
    public readonly NuiBind<string> PortraitPreview = new NuiBind<string>("portrait_prev");
    public readonly NuiBind<string> SoundSet = new NuiBind<string>("sound_set");
    public readonly NuiBind<string> Appearance = new NuiBind<string>("appearance_val");

    // Buttons
    public readonly NuiButton SelectPlayerButton;
    public readonly NuiButton SaveChangesButton;
    public readonly NuiButton DiscardChangesButton;

    public PlayerAppearanceWindowView()
    {
      SelectPlayerButton = new NuiButton("Select Player")
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
            Height = 250f,
            Children = new List<NuiElement>
            {
              new NuiColumn
              {
                Width = 132f,
                Children = new List<NuiElement>
                {
                  new NuiLabel(CreatureName)
                  {
                    Margin = 5f,
                    Height = 15f,
                    HorizontalAlign = NuiHAlign.Center,
                  },
                  new NuiImage(PortraitPreview)
                  {
                    Width = 128f,
                    Height = 200f,
                    ImageAspect = NuiAspect.Fill,
                    HorizontalAlign = NuiHAlign.Center,
                    Visible = PortraitEnabled,
                  },
                  new NuiLabel(PlayerName)
                  {
                    Height = 15f,
                    HorizontalAlign = NuiHAlign.Center,
                  },
                },
              },
              new NuiColumn
              {
                Width = 300f,
                Children = new List<NuiElement>
                {
                  new NuiLabel("Portrait")
                  {
                    Margin = 5f,
                    Height = 15f,
                    Enabled = PortraitEnabled,
                  },
                  new NuiRow
                  {
                    Height = 40f,
                    Enabled = PortraitEnabled,
                    Children = new List<NuiElement>
                    {
                      new NuiTextEdit(string.Empty, Portrait, 255, false)
                      {
                        Width = 230f,
                      },
                    },
                  },
                  new NuiLabel("Sound Set")
                  {
                    Margin = 5f,
                    Height = 15f,
                    Enabled = SoundSetEnabled,
                  },
                  new NuiRow
                  {
                    Height = 40f,
                    Enabled = SoundSetEnabled,
                    Children = new List<NuiElement>
                    {
                      new NuiTextEdit(string.Empty, SoundSet, 255, false)
                      {
                        Width = 230f,
                      },
                    },
                  },
                  new NuiLabel("Appearance")
                  {
                    Margin = 5f,
                    Height = 15f,
                    Enabled = AppearanceEnabled,
                  },
                  new NuiRow
                  {
                    Height = 40f,
                    Enabled = AppearanceEnabled,
                    Children = new List<NuiElement>
                    {
                      new NuiTextEdit(string.Empty, Appearance, 255, false)
                      {
                        Width = 230f,
                      },
                    },
                  },
                },
              },
            },
          },
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              SelectPlayerButton,
              new NuiSpacer(),
              SaveChangesButton,
              DiscardChangesButton,
            },
          },
        },
      };

      WindowTemplate = new NuiWindow(root, Title)
      {
        Geometry = new NuiRect(500f, 100f, 480f, 345f),
      };
    }
  }
}
