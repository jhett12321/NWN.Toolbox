using System.Globalization;
using System.Numerics;
using Anvil.API;
using Anvil.API.Events;

namespace Jorteck.Toolbox
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
      SetBindWatch(View.TranslationX, enable);
      SetBindWatch(View.TranslationY, enable);
      SetBindWatch(View.TranslationZ, enable);
      SetBindWatch(View.RotationX, enable);
      SetBindWatch(View.RotationY, enable);
      SetBindWatch(View.RotationZ, enable);
      SetBindWatch(View.Scale, enable);
      SetBindWatch(View.AnimSpeed, enable);
      SetBindWatch(View.TranslationXStr, enable);
      SetBindWatch(View.TranslationYStr, enable);
      SetBindWatch(View.TranslationZStr, enable);
      SetBindWatch(View.RotationXStr, enable);
      SetBindWatch(View.RotationYStr, enable);
      SetBindWatch(View.RotationZStr, enable);
      SetBindWatch(View.ScaleStr, enable);
      SetBindWatch(View.AnimSpeedStr, enable);
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
        float scale = GetBindValue(View.Scale);
        selectedObject.VisualTransform.Scale = scale;
        SetBindValue(View.ScaleStr, scale.ToString(CultureInfo.InvariantCulture));
      }
      else if (eventData.ElementId == View.AnimSpeed.Key)
      {
        float animSpeed = GetBindValue(View.AnimSpeed);
        selectedObject.VisualTransform.AnimSpeed = animSpeed;
        SetBindValue(View.AnimSpeedStr, animSpeed.ToString(CultureInfo.InvariantCulture));
      }
      else if (eventData.ElementId == View.TranslationX.Key)
      {
        Vector3 translation = selectedObject.VisualTransform.Translation;
        translation.X = GetBindValue(View.TranslationX);
        selectedObject.VisualTransform.Translation = translation;
        SetBindValue(View.TranslationXStr, translation.X.ToString(CultureInfo.InvariantCulture));
      }
      else if (eventData.ElementId == View.TranslationY.Key)
      {
        Vector3 translation = selectedObject.VisualTransform.Translation;
        translation.Y = GetBindValue(View.TranslationY);
        selectedObject.VisualTransform.Translation = translation;
        SetBindValue(View.TranslationYStr, translation.Y.ToString(CultureInfo.InvariantCulture));
      }
      else if (eventData.ElementId == View.TranslationZ.Key)
      {
        Vector3 translation = selectedObject.VisualTransform.Translation;
        translation.Z = GetBindValue(View.TranslationZ);
        selectedObject.VisualTransform.Translation = translation;
        SetBindValue(View.TranslationZStr, translation.Z.ToString(CultureInfo.InvariantCulture));
      }
      else if (eventData.ElementId == View.RotationX.Key)
      {
        Vector3 rotation = selectedObject.VisualTransform.Rotation;
        rotation.X = GetBindValue(View.RotationX);
        selectedObject.VisualTransform.Rotation = rotation;
        SetBindValue(View.RotationXStr, rotation.X.ToString(CultureInfo.InvariantCulture));
      }
      else if (eventData.ElementId == View.RotationY.Key)
      {
        Vector3 rotation = selectedObject.VisualTransform.Rotation;
        rotation.Y = GetBindValue(View.RotationY);
        selectedObject.VisualTransform.Rotation = rotation;
        SetBindValue(View.RotationYStr, rotation.Y.ToString(CultureInfo.InvariantCulture));
      }
      else if (eventData.ElementId == View.RotationZ.Key)
      {
        Vector3 rotation = selectedObject.VisualTransform.Rotation;
        rotation.Z = GetBindValue(View.RotationZ);
        selectedObject.VisualTransform.Rotation = rotation;
        SetBindValue(View.RotationZStr, rotation.Z.ToString(CultureInfo.InvariantCulture));
      }
      else if (eventData.ElementId == View.ScaleStr.Key)
      {
        if (float.TryParse(GetBindValue(View.ScaleStr), out float scale))
        {
          selectedObject.VisualTransform.Scale = scale;
          SetBindValue(View.Scale, scale);
        }
      }
      else if (eventData.ElementId == View.AnimSpeedStr.Key)
      {
        if (float.TryParse(GetBindValue(View.AnimSpeedStr), out float animSpeed))
        {
          selectedObject.VisualTransform.AnimSpeed = animSpeed;
          SetBindValue(View.AnimSpeed, animSpeed);
        }
      }
      else if (eventData.ElementId == View.TranslationXStr.Key)
      {
        if (float.TryParse(GetBindValue(View.TranslationXStr), out float translationX))
        {
          Vector3 translation = selectedObject.VisualTransform.Translation;
          translation.X = translationX;
          selectedObject.VisualTransform.Translation = translation;
          SetBindValue(View.TranslationX, translationX);
        }
      }
      else if (eventData.ElementId == View.TranslationYStr.Key)
      {
        if (float.TryParse(GetBindValue(View.TranslationYStr), out float translationY))
        {
          Vector3 translation = selectedObject.VisualTransform.Translation;
          translation.Y = translationY;
          selectedObject.VisualTransform.Translation = translation;
          SetBindValue(View.TranslationY, translationY);
        }
      }
      else if (eventData.ElementId == View.TranslationZStr.Key)
      {
        if (float.TryParse(GetBindValue(View.TranslationZStr), out float translationZ))
        {
          Vector3 translation = selectedObject.VisualTransform.Translation;
          translation.Z = translationZ;
          selectedObject.VisualTransform.Translation = translation;
          SetBindValue(View.TranslationZ, translationZ);
        }
      }
      else if (eventData.ElementId == View.RotationXStr.Key)
      {
        if (float.TryParse(GetBindValue(View.RotationXStr), out float rotationX))
        {
          Vector3 rotation = selectedObject.VisualTransform.Rotation;
          rotation.X = rotationX;
          selectedObject.VisualTransform.Rotation = rotation;
          SetBindValue(View.RotationX, rotationX);
        }
      }
      else if (eventData.ElementId == View.RotationYStr.Key)
      {
        if (float.TryParse(GetBindValue(View.RotationYStr), out float rotationY))
        {
          Vector3 rotation = selectedObject.VisualTransform.Rotation;
          rotation.Y = rotationY;
          selectedObject.VisualTransform.Rotation = rotation;
          SetBindValue(View.RotationY, rotationY);
        }
      }
      else if (eventData.ElementId == View.RotationZStr.Key)
      {
        if (float.TryParse(GetBindValue(View.RotationZStr), out float rotationZ))
        {
          Vector3 rotation = selectedObject.VisualTransform.Rotation;
          rotation.Z = rotationZ;
          selectedObject.VisualTransform.Rotation = rotation;
          SetBindValue(View.RotationZ, rotationZ);
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
        SetBindValue(View.ValidObjectSelected, false);
        return;
      }

      SetBindValue(View.ValidObjectSelected, true);

      VisualTransform visualTransform = selectedObject.VisualTransform;
      float scale = visualTransform.Scale;
      float animSpeed = visualTransform.AnimSpeed;
      SetBindValue(View.Scale, scale);
      SetBindValue(View.AnimSpeed, animSpeed);

      Vector3 translation = visualTransform.Translation;
      SetBindValue(View.TranslationX, translation.X);
      SetBindValue(View.TranslationY, translation.Y);
      SetBindValue(View.TranslationZ, translation.Z);

      Vector3 rotation = visualTransform.Rotation;
      SetBindValue(View.RotationX, rotation.X);
      SetBindValue(View.RotationY, rotation.Y);
      SetBindValue(View.RotationZ, rotation.Z);

      SetBindValue(View.ScaleStr, scale.ToString(CultureInfo.InvariantCulture));
      SetBindValue(View.AnimSpeedStr, animSpeed.ToString(CultureInfo.InvariantCulture));
      SetBindValue(View.TranslationXStr, translation.X.ToString(CultureInfo.InvariantCulture));
      SetBindValue(View.TranslationYStr, translation.Y.ToString(CultureInfo.InvariantCulture));
      SetBindValue(View.TranslationZStr, translation.Z.ToString(CultureInfo.InvariantCulture));
      SetBindValue(View.RotationXStr, rotation.X.ToString(CultureInfo.InvariantCulture));
      SetBindValue(View.RotationYStr, rotation.Y.ToString(CultureInfo.InvariantCulture));
      SetBindValue(View.RotationZStr, rotation.Z.ToString(CultureInfo.InvariantCulture));
      ToggleBindWatch(true);
    }

    private void HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.SelectObjectButton.Id)
      {
        Player.TryEnterTargetMode(OnCreatureSelected, ObjectTypes.Creature | ObjectTypes.Placeable | ObjectTypes.Item | ObjectTypes.Door);
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
