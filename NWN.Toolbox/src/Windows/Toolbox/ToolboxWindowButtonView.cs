using System.Collections.Generic;
using Anvil.API;

namespace Jorteck.Toolbox
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

    public ToolboxWindowButtonView()
    {
      Button = new NuiButton("Toolbox")
      {
        Id = "btn_open",
        Width = 112f,
        Height = 37f,
      };

      NuiRow root = new NuiRow
      {
        Children = new List<NuiElement>
        {
          Button,
        },
      };

      WindowTemplate = new NuiWindow(root, null)
      {
        Geometry = new NuiRect(725f, 0f, 160f, 60f),
        Border = false,
        Closable = false,
        Transparent = true,
        Resizable = false,
        Collapsed = false,
      };
    }
  }
}
