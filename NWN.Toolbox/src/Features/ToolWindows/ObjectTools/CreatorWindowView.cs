using System.Collections.Generic;
using Anvil.API;
using Jorteck.Toolbox.Core;
using Jorteck.Toolbox.Features.Blueprints;

namespace Jorteck.Toolbox.Features.ToolWindows
{
  public sealed class CreatorWindowView : WindowView<CreatorWindowView>
  {
    public readonly string BlueprintRowId = "rows";

    public override string Id => "creator.enhanced";
    public override string Title => "Creator: Enhanced";
    public override NuiWindow WindowTemplate { get; }

    public override IWindowController CreateDefaultController(NwPlayer player)
    {
      return CreateController<CreatorWindowController>(player);
    }

    // Value Binds
    public readonly NuiBind<string> Search = new NuiBind<string>("search_val");
    public readonly NuiBind<int> BlueprintType = new NuiBind<int>("type_val");
    public readonly NuiBind<Color> RowColors = new NuiBind<Color>("row_colors");
    public readonly NuiBind<string> BlueprintNamesAndCategories = new NuiBind<string>("names_val");
    public readonly NuiBind<string> BlueprintCRs = new NuiBind<string>("crs_val");
    public readonly NuiBind<string> BlueprintFactions = new NuiBind<string>("factions_val");
    public readonly NuiBind<int> BlueprintCount = new NuiBind<int>("count");

    //public readonly NuiBind<string> SelectedBlueprint = new NuiBind<string>("selected_blue");

    // Buttons
    public readonly NuiButtonImage SearchButton;
    public readonly NuiButton CreateButton;

    // Button States
    public readonly NuiBind<bool> CreateButtonEnabled = new NuiBind<bool>("create");

    public CreatorWindowView()
    {
      List<NuiListTemplateCell> rowTemplate = new List<NuiListTemplateCell>
      {
        new NuiListTemplateCell(new NuiLabel(BlueprintNamesAndCategories)
        {
          Id = BlueprintRowId,
          Tooltip = BlueprintNamesAndCategories,
          ForegroundColor = RowColors,
        }),
        new NuiListTemplateCell(new NuiLabel(BlueprintCRs)
        {
          Id = BlueprintRowId,
          Tooltip = BlueprintNamesAndCategories,
          ForegroundColor = RowColors,
        })
        {
          Width = 40f,
        },
        new NuiListTemplateCell(new NuiLabel(BlueprintFactions)
        {
          Id = BlueprintRowId,
          Tooltip = BlueprintNamesAndCategories,
          ForegroundColor = RowColors,
        })
        {
          Width = 90f,
        },
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
              new NuiButtonImage("isk_search")
              {
                Id = "btn_search",
                Aspect = 1f,
              }.Assign(out SearchButton),
            },
          },
          new NuiRow
          {
            Children = new List<NuiElement>
            {
              new NuiLabel("Name/Category"),
              new NuiLabel("CR")
              {
                Width = 40f,
              },
              new NuiLabel("Faction")
              {
                Width = 120f,
              },
            },
            Height = 20f,
          },
          new NuiList(rowTemplate, BlueprintCount),
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiSpacer(),
              new NuiButton("Create")
              {
                Id = "btn_create",
                Width = 300f,
                Enabled = CreateButtonEnabled,
              }.Assign(out CreateButton),
              new NuiSpacer(),
            },
          },
        },
      };

      WindowTemplate = new NuiWindow(root, Title)
      {
        Geometry = new NuiRect(500f, 100f, 500f, 720f),
      };
    }
  }
}
