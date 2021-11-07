using System.Collections.Generic;
using Anvil.API;

namespace Jorteck.Toolbox
{
  public sealed class EnhancedCreatorWindowView : WindowView<EnhancedCreatorWindowView, EnhancedCreatorWindowController>
  {
    public override string Id => "creator.enhanced";
    public override string Title => "Creator: Enhanced";

    public override NuiWindow WindowTemplate { get; }

    // Sub-views
    public readonly NuiGroup CreatorListContainer = new NuiGroup
    {
      Id = "creator_list",
    };

    // Value Binds
    public readonly NuiBind<string> Search = new NuiBind<string>("search_val");
    public readonly NuiBind<int> BlueprintType = new NuiBind<int>("type_val");

    // Buttons
    public readonly NuiButtonImage SearchButton;
    public readonly NuiButton CreateButton;

    public EnhancedCreatorWindowView()
    {
      SearchButton = new NuiButtonImage("isk_search")
      {
        Id = "btn_search",
        Aspect = 1f,
      };

      CreateButton = new NuiButton("Create")
      {
        Id = "btn_create",
        Width = 300f,
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
              NuiUtils.CreateComboForEnum<BlueprintObjectType>(BlueprintType),
              new NuiTextEdit("Search...", Search, 255, false),
              SearchButton,
            },
          },
          CreatorListContainer,
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiSpacer(),
              CreateButton,
              new NuiSpacer(),
            }
          }
        },
      };

      WindowTemplate = new NuiWindow(root, Title)
      {
        Geometry = new NuiRect(0, 0, 500, 720f),
      };
    }
  }
}
