using System.Collections.Generic;
using Anvil.API;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.ToolWindows
{
  public sealed class ToolboxWindowButtonView : WindowView<ToolboxWindowButtonView>
  {
    public override string Id => "toolbox";
    public override string Title => "Toolbox";
    public override NuiWindow WindowTemplate { get; }
    public override bool ListInToolbox => false;

    public override IWindowController CreateDefaultController(NwPlayer player)
    {
      return CreateController<ToolboxWindowButtonController>(player);
    }

    // Buttons
    public readonly NuiButton Button;

    // Button/Window Geometry
    public readonly NuiBind<NuiRect> ButtonGeometry = new NuiBind<NuiRect>("btn_geo");

    public ToolboxWindowButtonView()
    {
      NuiRow root = new NuiRow
      {
        Children = new List<NuiElement>
        {
          new NuiButton("Toolbox")
          {
            Id = "btn_open",
            Width = 112f,
            Height = 37f,
          }.Assign(out Button),
        },
      };

      WindowTemplate = new NuiWindow(root, string.Empty)
      {
        Geometry = ButtonGeometry,
        Border = false,
        Closable = false,
        Transparent = true,
        Resizable = false,
        Collapsed = false,
      };
    }
  }
}
