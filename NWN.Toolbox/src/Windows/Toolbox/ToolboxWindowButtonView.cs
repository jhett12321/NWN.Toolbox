using System.Collections.Generic;
using Anvil.API;

namespace Jorteck.Toolbox
{
  public sealed class ToolboxWindowButtonView : WindowView<ToolboxWindowButtonView, ToolboxWindowButtonController>
  {
    public override string Id => "toolbox";
    public override string Title => "Toolbox";

    public override bool ListInToolbox => false;

    public override NuiWindow WindowTemplate { get; }

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
        Geometry = new NuiRect(0f, 38f, 160f, 50f),
        Border = false,
        Closable = false,
        Transparent = true,
        Resizable = false,
        Collapsed = false,
      };
    }
  }
}
