using System.Collections.Generic;
using Anvil.API;

namespace Jorteck.Toolbox
{
  public sealed class ToolboxWindowView : WindowView<ToolboxWindowView>
  {
    public override string Id => "toolbox";
    public override string Title => "Toolbox";
    public override NuiWindow WindowTemplate { get; }
    public override bool ListInToolbox => false;

    public override IWindowController CreateDefaultController(NwPlayer player)
    {
      return CreateController<ToolboxWindowController>(player);
    }

    // Value Binds
    public readonly NuiBind<string> Search = new NuiBind<string>("search_val");
    public readonly NuiBind<string> WindowNames = new NuiBind<string>("win_names");
    public readonly NuiBind<int> WindowCount = new NuiBind<int>("window_count");

    // Buttons
    public readonly NuiButtonImage SearchButton;
    public readonly NuiButtonImage OpenWindowButton;

    public ToolboxWindowView()
    {
      SearchButton = new NuiButtonImage("isk_search")
      {
        Id = "btn_search",
        Aspect = 1f,
      };

      OpenWindowButton = new NuiButtonImage("dm_goto")
      {
        Id = "btn_openwin",
        Aspect = 1f,
        Tooltip = "Open Window",
      };

      List<NuiListTemplateCell> rowTemplate = new List<NuiListTemplateCell>
      {
        new NuiListTemplateCell(OpenWindowButton)
        {
          VariableSize = false,
          Width = 35f,
        },
        new NuiListTemplateCell(new NuiLabel(WindowNames)
        {
          VerticalAlign = NuiVAlign.Middle,
        }),
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
              new NuiTextEdit("Search for tools...", Search, 255, false),
              SearchButton,
            },
          },
          new NuiList(rowTemplate, WindowCount)
          {
            RowHeight = 35f,
          },
        },
      };

      WindowTemplate = new NuiWindow(root, Title)
      {
        Geometry = new NuiRect(0f, 100f, 400f, 600f),
      };
    }
  }
}
