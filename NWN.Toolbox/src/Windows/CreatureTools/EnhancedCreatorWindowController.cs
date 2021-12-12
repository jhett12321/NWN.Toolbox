using System.Collections.Generic;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  public sealed class EnhancedCreatorWindowController : WindowController<EnhancedCreatorWindowView>
  {
    private const int MaxItems = 200;

    [Inject]
    public BlueprintManager BlueprintManager { private get; init; }

    private readonly Dictionary<string, IBlueprint> idToBlueprintMap = new Dictionary<string, IBlueprint>();
    private IBlueprint selectedBlueprint;

    public override void Init()
    {
      SetBindValue(View.BlueprintType, (int)BlueprintObjectType.Creature);
      RefreshCreatorList();
    }

    public override void ProcessEvent(ModuleEvents.OnNuiEvent eventData)
    {
      switch (eventData.EventType)
      {
        case NuiEventType.Click:
          HandleButtonClick(eventData);
          break;
      }
    }

    protected override void OnClose()
    {
      idToBlueprintMap.Clear();
    }

    private void HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.CreateButton.Id)
      {
        TryCreateBlueprint();
      }
      else if (eventData.ElementId == View.SearchButton.Id)
      {
        RefreshCreatorList();
      }
      else if (idToBlueprintMap.TryGetValue(eventData.ElementId, out IBlueprint blueprint))
      {
        SetBindValue(View.SelectedBlueprint, $"{blueprint.ObjectType}: {blueprint.FullName}");
        selectedBlueprint = blueprint;
      }
    }

    private void TryCreateBlueprint()
    {
      if (selectedBlueprint == null)
      {
        Player.FloatingTextString("Select a blueprint first.", false);
        return;
      }

      Player.TryEnterTargetMode(CreateBlueprintInstance);
    }

    private void CreateBlueprintInstance(ModuleEvents.OnPlayerTarget onPlayerTarget)
    {
      if (selectedBlueprint == null)
      {
        return;
      }

      Location location;
      if (onPlayerTarget.TargetObject is NwArea area)
      {
        location = Location.Create(area, onPlayerTarget.TargetPosition, 0f);
      }
      else if (onPlayerTarget.TargetObject is NwGameObject gameObject)
      {
        location = gameObject.Location;
      }
      else
      {
        return;
      }

      NwObject nwObject = selectedBlueprint.Create(location);
      Player.SendServerMessage($"\"{nwObject.Name}\" Created.");
    }

    private void RefreshCreatorList()
    {
      List<IBlueprint> blueprints = BlueprintManager.GetMatchingBlueprints((BlueprintObjectType)GetBindValue(View.BlueprintType), GetBindValue(View.Search), MaxItems);

      string currentCategory = null;
      NuiColumn subViewRoot = new NuiColumn();

      for (int i = 0; i < blueprints.Count; i++)
      {
        IBlueprint blueprint = blueprints[i];
        if (blueprint.Category != currentCategory)
        {
          subViewRoot.Children.Add(CreateCategoryElement(blueprint.Category));
          currentCategory = blueprint.Category;
        }

        subViewRoot.Children.Add(CreateBlueprintElement(blueprint, i));
      }

      SetGroupLayout(View.CreatorListContainer, subViewRoot);
    }

    private NuiElement CreateCategoryElement(string categoryName)
    {
      return new NuiLabel(categoryName)
      {
        Height = 20f,
        Width = categoryName.Length * 10,
      };
    }

    private NuiElement CreateBlueprintElement(IBlueprint blueprint, int index)
    {
      string buttonId = $"btn_{index}";
      idToBlueprintMap[buttonId] = blueprint;

      return new NuiButton(blueprint.Name)
      {
        Id = buttonId,
        Height = 30f,
        Width = blueprint.Name.Length * 10,
      };
    }
  }
}
