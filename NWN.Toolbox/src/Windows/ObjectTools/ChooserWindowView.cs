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
    public NuiButton SelectAreaButton { get; }
    public NuiButton SelectModuleButton { get; }

    public readonly NuiBind<bool> SelectAreaButtonEnabled = new NuiBind<bool>("sel_area");
    public readonly NuiBind<bool> SelectModuleButtonEnabled = new NuiBind<bool>("sel_module");
    public readonly NuiBind<bool> OkButtonEnabled = new NuiBind<bool>("ok");
    public readonly NuiBind<bool> CancelButtonEnabled = new NuiBind<bool>("cancel");

    public NuiButton OkButton { get; }
    public NuiButton CancelButton { get; }

    public ChooserWindowView()
    {
      SelectAreaButton = new NuiButton("Select Current Area")
      {
        Id = "sel_area_btn",
        Enabled = SelectAreaButtonEnabled,
      };

      SelectModuleButton = new NuiButton("Select Module")
      {
        Id = "sel_mod_btn",
        Enabled = SelectModuleButtonEnabled,
      };

      OkButton = new NuiButton("OK")
      {
        Id = "ok_btn",
        Enabled = OkButtonEnabled,
      };

      CancelButton = new NuiButton("Cancel")
      {
        Id = "cancel_btn",
        Enabled = CancelButtonEnabled,
      };

      NuiColumn root = new NuiColumn
      {
        Children = new List<NuiElement>(SelectionListView.SubView),
      };

      WindowTemplate = new NuiWindow(root, Title)
      {
        Geometry = new NuiRect(0f, 100f, 700f, 600f),
      };
    }
  }
}
