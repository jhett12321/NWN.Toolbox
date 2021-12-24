using System.Collections.Generic;
using Anvil.API;

namespace Jorteck.Toolbox
{
  public sealed class VisualTransformWindowView : WindowView<VisualTransformWindowView>
  {
    private const float TranslationMin = -10f;
    private const float TranslationMax = 10f;
    private const float RotationMax = 360f;
    private const float ScaleMax = 10f;
    private const float SpeedMax = 10f;

    public override string Id => "visual.transform";
    public override string Title => "Visual Transform Editor";
    public override NuiWindow WindowTemplate { get; }

    public override IWindowController CreateDefaultController(NwPlayer player)
    {
      return CreateController<VisualTransformWindowController>(player);
    }

    // State Binds
    public readonly NuiBind<bool> ValidObjectSelected = new NuiBind<bool>("obj_valid");

    // Value Binds
    public readonly NuiBind<float> TranslationX = new NuiBind<float>("vt_trans_x");
    public readonly NuiBind<float> TranslationY = new NuiBind<float>("vt_trans_y");
    public readonly NuiBind<float> TranslationZ = new NuiBind<float>("vt_trans_z");
    public readonly NuiBind<float> RotationX = new NuiBind<float>("vt_rot_x");
    public readonly NuiBind<float> RotationY = new NuiBind<float>("vt_rot_y");
    public readonly NuiBind<float> RotationZ = new NuiBind<float>("vt_rot_z");
    public readonly NuiBind<float> Scale = new NuiBind<float>("vt_scale");
    public readonly NuiBind<float> AnimSpeed = new NuiBind<float>("vt_animspeed");
    public readonly NuiBind<string> TranslationXStr = new NuiBind<string>("vt_trans_x_str");
    public readonly NuiBind<string> TranslationYStr = new NuiBind<string>("vt_trans_y_str");
    public readonly NuiBind<string> TranslationZStr = new NuiBind<string>("vt_trans_z_str");
    public readonly NuiBind<string> RotationXStr = new NuiBind<string>("vt_rot_x_str");
    public readonly NuiBind<string> RotationYStr = new NuiBind<string>("vt_rot_y_str");
    public readonly NuiBind<string> RotationZStr = new NuiBind<string>("vt_rot_z_str");
    public readonly NuiBind<string> ScaleStr = new NuiBind<string>("vt_scale_str");
    public readonly NuiBind<string> AnimSpeedStr = new NuiBind<string>("vt_animspeed_str");

    // Buttons
    public readonly NuiButton SelectObjectButton;

    public VisualTransformWindowView()
    {
      SelectObjectButton = new NuiButton("Select Object")
      {
        Id = "btn_obj_sel",
      };

      NuiColumn root = new NuiColumn
      {
        Children = new List<NuiElement>
        {
          new NuiLabel("Translation")
          {
            Height = 15f,
            VerticalAlign = NuiVAlign.Middle,
          },
          new NuiRow
          {
            Enabled = ValidObjectSelected,
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("X")
              {
                Width = 15f,
                VerticalAlign = NuiVAlign.Middle,
              },
              new NuiTextEdit(string.Empty, TranslationXStr, 10, false) { Width = 70f },
              new NuiSliderFloat(TranslationX, TranslationMin, TranslationMax)
              {
                Width = 385f,
              },
            },
          },
          new NuiRow
          {
            Enabled = ValidObjectSelected,
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Y")
              {
                Width = 15f,
                VerticalAlign = NuiVAlign.Middle,
              },
              new NuiTextEdit(string.Empty, TranslationYStr, 10, false) { Width = 70f },
              new NuiSliderFloat(TranslationY, TranslationMin, TranslationMax)
              {
                Width = 385f,
              },
            },
          },
          new NuiRow
          {
            Enabled = ValidObjectSelected,
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Z")
              {
                Width = 15f,
                VerticalAlign = NuiVAlign.Middle,
              },
              new NuiTextEdit(string.Empty, TranslationZStr, 10, false) { Width = 70f },
              new NuiSliderFloat(TranslationZ, TranslationMin, TranslationMax)
              {
                Width = 385f,
              },
            },
          },
          new NuiLabel("Rotation")
          {
            Height = 15f,
            VerticalAlign = NuiVAlign.Middle,
          },
          new NuiRow
          {
            Enabled = ValidObjectSelected,
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("X")
              {
                Width = 15f,
                VerticalAlign = NuiVAlign.Middle,
              },
              new NuiTextEdit(string.Empty, RotationXStr, 10, false) { Width = 70f },
              new NuiSliderFloat(RotationX, 0f, RotationMax)
              {
                Width = 385f,
              },
            },
          },
          new NuiRow
          {
            Enabled = ValidObjectSelected,
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Y")
              {
                Width = 15f,
                VerticalAlign = NuiVAlign.Middle,
              },
              new NuiTextEdit(string.Empty, RotationYStr, 10, false) { Width = 70f },
              new NuiSliderFloat(RotationY, 0f, RotationMax)
              {
                Width = 385f,
              },
            },
          },
          new NuiRow
          {
            Enabled = ValidObjectSelected,
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Z")
              {
                Width = 15f,
                VerticalAlign = NuiVAlign.Middle,
              },
              new NuiTextEdit(string.Empty, RotationZStr, 10, false) { Width = 70f },
              new NuiSliderFloat(RotationZ, 0f, RotationMax)
              {
                Width = 385f,
              },
            },
          },
          new NuiLabel("Misc")
          {
            Height = 15f,
            VerticalAlign = NuiVAlign.Middle,
          },
          new NuiRow
          {
            Enabled = ValidObjectSelected,
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Object Scale")
              {
                Width = 100f,
                VerticalAlign = NuiVAlign.Middle,
              },
              new NuiTextEdit(string.Empty, ScaleStr, 10, false) { Width = 70f },
              new NuiSliderFloat(Scale, 0f, ScaleMax)
              {
                Width = 300f,
              },
            },
          },
          new NuiRow
          {
            Enabled = ValidObjectSelected,
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiLabel("Anim Speed")
              {
                Width = 100f,
                VerticalAlign = NuiVAlign.Middle,
              },
              new NuiTextEdit(string.Empty, AnimSpeedStr, 10, false) { Width = 70f },
              new NuiSliderFloat(AnimSpeed, 0f, SpeedMax)
              {
                Width = 300f,
              },
            },
          },
          new NuiSpacer(),
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              SelectObjectButton,
            },
          },
        },
      };

      WindowTemplate = new NuiWindow(root, Title)
      {
        Geometry = new NuiRect(500f, 100f, 500f, 505f),
      };
    }
  }
}
