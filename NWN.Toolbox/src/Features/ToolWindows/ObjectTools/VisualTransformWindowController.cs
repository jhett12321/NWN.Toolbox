using System.Globalization;
using System.Numerics;
using Anvil.API;
using Anvil.API.Events;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.ToolWindows
{
  public sealed class VisualTransformWindowController : WindowController<VisualTransformWindowView>
  {
    private NwGameObject selectedObject;

    public override void Init()
    {
      Update();
    }

    public override void ProcessEvent(ModuleEvents.OnNuiEvent eventData)
    {
      switch (eventData.EventType)
      {
        case NuiEventType.Click:
          HandleButtonClick(eventData);
          break;
        case NuiEventType.Watch:
          ToggleBindWatch(false);
          HandleWatchUpdate(eventData);
          ToggleBindWatch(true);
          break;
        case NuiEventType.Open:
          Update();
          break;
      }
    }

    private void ToggleBindWatch(bool enable)
    {
      Token.SetBindWatch(View.TranslationX, enable);
      Token.SetBindWatch(View.TranslationY, enable);
      Token.SetBindWatch(View.TranslationZ, enable);
      Token.SetBindWatch(View.RotationX, enable);
      Token.SetBindWatch(View.RotationY, enable);
      Token.SetBindWatch(View.RotationZ, enable);
      Token.SetBindWatch(View.Scale, enable);
      Token.SetBindWatch(View.AnimSpeed, enable);
      Token.SetBindWatch(View.TranslationXStr, enable);
      Token.SetBindWatch(View.TranslationYStr, enable);
      Token.SetBindWatch(View.TranslationZStr, enable);
      Token.SetBindWatch(View.RotationXStr, enable);
      Token.SetBindWatch(View.RotationYStr, enable);
      Token.SetBindWatch(View.RotationZStr, enable);
      Token.SetBindWatch(View.ScaleStr, enable);
      Token.SetBindWatch(View.AnimSpeedStr, enable);
    }

    // ReSharper disable once FunctionComplexityOverflow
    private void HandleWatchUpdate(ModuleEvents.OnNuiEvent eventData)
    {
      if (selectedObject == null || !selectedObject.IsValid)
      {
        Update();
        return;
      }

      if (eventData.ElementId == View.Scale.Key)
      {
        float scale = Token.GetBindValue(View.Scale);
        selectedObject.VisualTransform.Scale = scale;
        Token.SetBindValue(View.ScaleStr, scale.ToString(CultureInfo.InvariantCulture));
      }
      else if (eventData.ElementId == View.AnimSpeed.Key)
      {
        float animSpeed = Token.GetBindValue(View.AnimSpeed);
        selectedObject.VisualTransform.AnimSpeed = animSpeed;
        Token.SetBindValue(View.AnimSpeedStr, animSpeed.ToString(CultureInfo.InvariantCulture));
      }
      else if (eventData.ElementId == View.TranslationX.Key)
      {
        Vector3 translation = selectedObject.VisualTransform.Translation;
        translation.X = Token.GetBindValue(View.TranslationX);
        selectedObject.VisualTransform.Translation = translation;
        string value = translation.X.ToString(CultureInfo.InvariantCulture);
        Token.SetBindValue(View.TranslationXStr, value);
      }
      else if (eventData.ElementId == View.TranslationY.Key)
      {
        Vector3 translation = selectedObject.VisualTransform.Translation;
        translation.Y = Token.GetBindValue(View.TranslationY);
        selectedObject.VisualTransform.Translation = translation;
        string value = translation.Y.ToString(CultureInfo.InvariantCulture);
        Token.SetBindValue(View.TranslationYStr, value);
      }
      else if (eventData.ElementId == View.TranslationZ.Key)
      {
        Vector3 translation = selectedObject.VisualTransform.Translation;
        translation.Z = Token.GetBindValue(View.TranslationZ);
        selectedObject.VisualTransform.Translation = translation;
        string value = translation.Z.ToString(CultureInfo.InvariantCulture);
        Token.SetBindValue(View.TranslationZStr, value);
      }
      else if (eventData.ElementId == View.RotationX.Key)
      {
        Vector3 rotation = selectedObject.VisualTransform.Rotation;
        rotation.X = Token.GetBindValue(View.RotationX);
        selectedObject.VisualTransform.Rotation = rotation;
        string value = rotation.X.ToString(CultureInfo.InvariantCulture);
        Token.SetBindValue(View.RotationXStr, value);
      }
      else if (eventData.ElementId == View.RotationY.Key)
      {
        Vector3 rotation = selectedObject.VisualTransform.Rotation;
        rotation.Y = Token.GetBindValue(View.RotationY);
        selectedObject.VisualTransform.Rotation = rotation;
        string value = rotation.Y.ToString(CultureInfo.InvariantCulture);
        Token.SetBindValue(View.RotationYStr, value);
      }
      else if (eventData.ElementId == View.RotationZ.Key)
      {
        Vector3 rotation = selectedObject.VisualTransform.Rotation;
        rotation.Z = Token.GetBindValue(View.RotationZ);
        selectedObject.VisualTransform.Rotation = rotation;
        string value = rotation.Z.ToString(CultureInfo.InvariantCulture);
        Token.SetBindValue(View.RotationZStr, value);
      }
      else if (eventData.ElementId == View.ScaleStr.Key)
      {
        if (float.TryParse(Token.GetBindValue(View.ScaleStr), out float scale))
        {
          selectedObject.VisualTransform.Scale = scale;
          Token.SetBindValue(View.Scale, scale);
        }
      }
      else if (eventData.ElementId == View.AnimSpeedStr.Key)
      {
        if (float.TryParse(Token.GetBindValue(View.AnimSpeedStr), out float animSpeed))
        {
          selectedObject.VisualTransform.AnimSpeed = animSpeed;
          Token.SetBindValue(View.AnimSpeed, animSpeed);
        }
      }
      else if (eventData.ElementId == View.TranslationXStr.Key)
      {
        if (float.TryParse(Token.GetBindValue(View.TranslationXStr), out float translationX))
        {
          Vector3 translation = selectedObject.VisualTransform.Translation;
          translation.X = translationX;
          selectedObject.VisualTransform.Translation = translation;
          Token.SetBindValue(View.TranslationX, translationX);
        }
      }
      else if (eventData.ElementId == View.TranslationYStr.Key)
      {
        if (float.TryParse(Token.GetBindValue(View.TranslationYStr), out float translationY))
        {
          Vector3 translation = selectedObject.VisualTransform.Translation;
          translation.Y = translationY;
          selectedObject.VisualTransform.Translation = translation;
          Token.SetBindValue(View.TranslationY, translationY);
        }
      }
      else if (eventData.ElementId == View.TranslationZStr.Key)
      {
        if (float.TryParse(Token.GetBindValue(View.TranslationZStr), out float translationZ))
        {
          Vector3 translation = selectedObject.VisualTransform.Translation;
          translation.Z = translationZ;
          selectedObject.VisualTransform.Translation = translation;
          Token.SetBindValue(View.TranslationZ, translationZ);
        }
      }
      else if (eventData.ElementId == View.RotationXStr.Key)
      {
        if (float.TryParse(Token.GetBindValue(View.RotationXStr), out float rotationX))
        {
          Vector3 rotation = selectedObject.VisualTransform.Rotation;
          rotation.X = rotationX;
          selectedObject.VisualTransform.Rotation = rotation;
          Token.SetBindValue(View.RotationX, rotationX);
        }
      }
      else if (eventData.ElementId == View.RotationYStr.Key)
      {
        if (float.TryParse(Token.GetBindValue(View.RotationYStr), out float rotationY))
        {
          Vector3 rotation = selectedObject.VisualTransform.Rotation;
          rotation.Y = rotationY;
          selectedObject.VisualTransform.Rotation = rotation;
          Token.SetBindValue(View.RotationY, rotationY);
        }
      }
      else if (eventData.ElementId == View.RotationZStr.Key)
      {
        if (float.TryParse(Token.GetBindValue(View.RotationZStr), out float rotationZ))
        {
          Vector3 rotation = selectedObject.VisualTransform.Rotation;
          rotation.Z = rotationZ;
          selectedObject.VisualTransform.Rotation = rotation;
          Token.SetBindValue(View.RotationZ, rotationZ);
        }
      }
    }

    protected override void OnClose()
    {
      selectedObject = null;
    }

    private void Update()
    {
      ToggleBindWatch(false);
      if (selectedObject == null || !selectedObject.IsValid)
      {
        Token.SetBindValue(View.ValidObjectSelected, false);
        return;
      }

      Token.SetBindValue(View.ValidObjectSelected, true);

      VisualTransform visualTransform = selectedObject.VisualTransform;
      float scale = visualTransform.Scale;
      float animSpeed = visualTransform.AnimSpeed;
      Token.SetBindValue(View.Scale, scale);
      Token.SetBindValue(View.AnimSpeed, animSpeed);

      Vector3 translation = visualTransform.Translation;
      Token.SetBindValue(View.TranslationX, translation.X);
      Token.SetBindValue(View.TranslationY, translation.Y);
      Token.SetBindValue(View.TranslationZ, translation.Z);

      Vector3 rotation = visualTransform.Rotation;
      Token.SetBindValue(View.RotationX, rotation.X);
      Token.SetBindValue(View.RotationY, rotation.Y);
      Token.SetBindValue(View.RotationZ, rotation.Z);

      string value = scale.ToString(CultureInfo.InvariantCulture);
      Token.SetBindValue(View.ScaleStr, value);
      string value1 = animSpeed.ToString(CultureInfo.InvariantCulture);
      Token.SetBindValue(View.AnimSpeedStr, value1);
      string value2 = translation.X.ToString(CultureInfo.InvariantCulture);
      Token.SetBindValue(View.TranslationXStr, value2);
      string value3 = translation.Y.ToString(CultureInfo.InvariantCulture);
      Token.SetBindValue(View.TranslationYStr, value3);
      string value4 = translation.Z.ToString(CultureInfo.InvariantCulture);
      Token.SetBindValue(View.TranslationZStr, value4);
      string value5 = rotation.X.ToString(CultureInfo.InvariantCulture);
      Token.SetBindValue(View.RotationXStr, value5);
      string value6 = rotation.Y.ToString(CultureInfo.InvariantCulture);
      Token.SetBindValue(View.RotationYStr, value6);
      string value7 = rotation.Z.ToString(CultureInfo.InvariantCulture);
      Token.SetBindValue(View.RotationZStr, value7);
      ToggleBindWatch(true);
    }

    private void HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.SelectObjectButton.Id)
      {
        Token.Player.TryEnterTargetMode(OnCreatureSelected, ObjectTypes.Creature | ObjectTypes.Placeable | ObjectTypes.Item | ObjectTypes.Door);
      }
    }

    private void OnCreatureSelected(ModuleEvents.OnPlayerTarget eventData)
    {
      if (eventData.TargetObject == null || eventData.TargetObject is not NwGameObject gameObject)
      {
        return;
      }

      selectedObject = gameObject;
      Update();
    }
  }
}
