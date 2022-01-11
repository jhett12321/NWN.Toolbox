using System.Collections.Generic;
using Anvil.API;

namespace Jorteck.Toolbox
{
  public sealed class ChooserWindowView : WindowView<ChooserWindowView>
  {
    public override string Id => "chooser";
    public override string Title => "Chooser: Enhanced";
    public override NuiWindow WindowTemplate { get; }

    public override IWindowController CreateDefaultController(NwPlayer player)
    {
      return CreateController<ChooserWindowController>(player);
    }

    // Sub Views
    public readonly ObjectSelectionListView SelectionListView = new ObjectSelectionListView();

    // Buttons
    public readonly NuiButtonImage GoToButton;
    public readonly NuiButtonImage DestroyButton;
    public readonly NuiButtonImage JumpButton;
    public readonly NuiButtonImage ToggleAIButton;
    public readonly NuiButtonImage HealButton;
    public readonly NuiButtonImage ControlButton;
    public readonly NuiButtonImage RestButton;
    public readonly NuiButtonImage LimboButton;
    public readonly NuiButtonImage ExamineButton;
    public readonly NuiButtonImage PossessButton;
    public readonly NuiButtonImage ToggleImmortalButton;
    public readonly NuiButtonImage TogglePlotModeButton;
    public readonly NuiButtonImage CloneButton;

    // Button States
    public readonly NuiBind<bool> GoToButtonEnabled = new NuiBind<bool>("goto");
    public readonly NuiBind<bool> DestroyButtonEnabled = new NuiBind<bool>("kill");
    public readonly NuiBind<bool> JumpButtonEnabled = new NuiBind<bool>("jump");
    public readonly NuiBind<bool> ToggleAIButtonEnabled = new NuiBind<bool>("ai");
    public readonly NuiBind<bool> HealButtonEnabled = new NuiBind<bool>("heal");
    public readonly NuiBind<bool> ControlButtonEnabled = new NuiBind<bool>("control");
    public readonly NuiBind<bool> RestButtonEnabled = new NuiBind<bool>("rest");
    public readonly NuiBind<bool> LimboButtonEnabled = new NuiBind<bool>("limbo");
    public readonly NuiBind<bool> ExamineButtonEnabled = new NuiBind<bool>("examine");
    public readonly NuiBind<bool> PossessButtonEnabled = new NuiBind<bool>("possess");
    public readonly NuiBind<bool> ToggleImmortalButtonEnabled = new NuiBind<bool>("immortal");
    public readonly NuiBind<bool> TogglePlotButtonEnabled = new NuiBind<bool>("god");
    public readonly NuiBind<bool> CloneButtonEnabled = new NuiBind<bool>("clone");

    public readonly NuiBind<bool>[] AllButtonStates;

    public ChooserWindowView()
    {
      GoToButton = new NuiButtonImage("dm_goto")
      {
        Id = "btn_goto",
        Tooltip = "Go to object",
        Enabled = GoToButtonEnabled,
        Width = 40f,
        Aspect = 1f,
      };
      DestroyButton = new NuiButtonImage("dm_kill")
      {
        Id = "btn_kill",
        Tooltip = "Kill/Destroy Object",
        Enabled = DestroyButtonEnabled,
        Width = 40f,
        Aspect = 1f,
      };
      JumpButton = new NuiButtonImage("dm_jump")
      {
        Id = "btn_jump",
        Tooltip = "Summon/Jump to DM",
        Enabled = JumpButtonEnabled,
        Width = 40f,
        Aspect = 1f,
      };
      ToggleAIButton = new NuiButtonImage("dm_ai")
      {
        Id = "btn_ai",
        Tooltip = "Toggle Creature AI",
        Enabled = ToggleAIButtonEnabled,
        Width = 40f,
        Aspect = 1f,
      };
      HealButton = new NuiButtonImage("dm_heal")
      {
        Id = "btn_heal",
        Tooltip = "Heal",
        Enabled = HealButtonEnabled,
        Width = 40f,
        Aspect = 1f,
      };
      ControlButton = new NuiButtonImage("dm_control")
      {
        Id = "btn_control",
        Tooltip = "Take Control",
        Enabled = ControlButtonEnabled,
        Width = 40f,
        Aspect = 1f,
      };
      RestButton = new NuiButtonImage("dm_rest")
      {
        Id = "btn_rest",
        Tooltip = "Rest",
        Enabled = RestButtonEnabled,
        Width = 40f,
        Aspect = 1f,
      };
      LimboButton = new NuiButtonImage("dm_limbo")
      {
        Id = "btn_limbo",
        Tooltip = "Send to Limbo",
        Enabled = LimboButtonEnabled,
        Width = 40f,
        Aspect = 1f,
      };
      ExamineButton = new NuiButtonImage("dm_examine")
      {
        Id = "btn_examine",
        Tooltip = "Examine",
        Enabled = ExamineButtonEnabled,
        Width = 40f,
        Aspect = 1f,
      };
      PossessButton = new NuiButtonImage("dm_possess")
      {
        Id = "btn_possess",
        Tooltip = "Possess Creature (Full Powers)",
        Enabled = PossessButtonEnabled,
        Width = 40f,
        Aspect = 1f,
      };
      ToggleImmortalButton = new NuiButtonImage("dm_immortal")
      {
        Id = "btn_immortal",
        Tooltip = "Toggle Immortal Mode (Take damage, cannot be killed)",
        Enabled = ToggleImmortalButtonEnabled,
        Width = 40f,
        Aspect = 1f,
      };
      TogglePlotModeButton = new NuiButtonImage("dm_god")
      {
        Id = "btn_god",
        Tooltip = "Toggle Plot/God Mode (Cannot take damage)",
        Enabled = TogglePlotButtonEnabled,
        Width = 40f,
        Aspect = 1f,
      };
      CloneButton = new NuiButtonImage("dm_encounter")
      {
        Id = "btn_clone",
        Tooltip = "Clones the selected object",
        Enabled = CloneButtonEnabled,
        Width = 40f,
        Aspect = 1f,
      };

      AllButtonStates = new[]
      {
        GoToButtonEnabled,
        DestroyButtonEnabled,
        JumpButtonEnabled,
        // ToggleAIButtonEnabled, // TODO - Not implemented
        HealButtonEnabled,
        ControlButtonEnabled,
        RestButtonEnabled,
        LimboButtonEnabled,
        ExamineButtonEnabled,
        PossessButtonEnabled,
        ToggleImmortalButtonEnabled,
        TogglePlotButtonEnabled,
        CloneButtonEnabled,
      };

      NuiColumn root = new NuiColumn
      {
        Children = new List<NuiElement>(SelectionListView.SubView)
        {
          new NuiRow
          {
            Children = new List<NuiElement>
            {
              new NuiSpacer(),
              GoToButton,
              DestroyButton,
              ExamineButton,
              CloneButton,
              JumpButton,
              ToggleAIButton,
              HealButton,
              ControlButton,
              PossessButton,
              RestButton,
              LimboButton,
              ToggleImmortalButton,
              TogglePlotModeButton,
              new NuiSpacer(),
            },
          },
        },
      };

      WindowTemplate = new NuiWindow(root, Title)
      {
        Geometry = new NuiRect(0f, 100f, 700f, 600f),
      };
    }
  }
}
