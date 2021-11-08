using System.Collections.Generic;
using Anvil.API;

namespace Jorteck.Toolbox
{
  public sealed class ToolboxWindowView : WindowView<ToolboxWindowView, ToolboxWindowController>
  {
    public override string Id => "toolbox";
    public override string Title => "Toolbox";

    public override bool ListInToolbox => false;

    public override NuiWindow WindowTemplate { get; }

    // Sub-views
    public readonly NuiGroup CreatorListContainer = new NuiGroup
    {
      Id = "creator_list",
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
          CreatorListContainer,
        },
      };

      WindowTemplate = new NuiWindow(root, Title)
      {
        Geometry = new NuiRect(0f, 100f, 500f, 720f),
      };
    }
  }
}
