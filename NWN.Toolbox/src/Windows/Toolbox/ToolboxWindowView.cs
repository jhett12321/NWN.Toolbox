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

    // Sub-views
    public readonly NuiGroup ToolboxListContainer = new NuiGroup
    {
      Id = "toolbox_list",
    };

    // Value Binds
    public readonly NuiBind<string> Search = new NuiBind<string>("search_val");

    // Buttons
    public readonly NuiButtonImage SearchButton;

    public ToolboxWindowView()
    {
      SearchButton = new NuiButtonImage("isk_search")
      {
        Id = "btn_search",
        Aspect = 1f,
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
          ToolboxListContainer,
        },
      };

      WindowTemplate = new NuiWindow(root, Title)
      {
        Geometry = new NuiRect(0f, 100f, 500f, 720f),
      };
    }
  }
}
