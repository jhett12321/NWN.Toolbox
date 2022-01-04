using System;
using System.Collections.Generic;
using System.Diagnostics;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  public sealed class EnhancedCreatorWindowController : WindowController<EnhancedCreatorWindowView>
  {
    private const int MaxItems = 1000;

    [Inject]
    public BlueprintManager BlueprintManager { private get; init; }

    private List<IBlueprint> blueprintRowMapping;
    private List<NuiColor> rowColors;

    //private readonly Dictionary<string, IBlueprint> idToBlueprintMap = new Dictionary<string, IBlueprint>();
    private IBlueprint selectedBlueprint;

    private TimeSpan lastSelectionClick;

    public override void Init()
    {
      Token.SetBindValue(View.BlueprintType, (int)BlueprintObjectType.Creature);
      Token.SetBindValue(View.CreateButtonEnabled, false);
      RefreshCreatorList();
    }

    public override void ProcessEvent(ModuleEvents.OnNuiEvent eventData)
    {
      switch (eventData.EventType)
      {
        case NuiEventType.Click:
          HandleButtonClick(eventData);
          break;
        case NuiEventType.MouseDown:
          HandleMouseDown(eventData);
          break;
      }
    }

    protected override void OnClose() {}

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
    }

    private void HandleMouseDown(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.BlueprintRowId)
      {
        SelectBlueprint(eventData.ArrayIndex);
      }
    }

    private void SelectBlueprint(int index)
    {
      if (index >= 0 && index < blueprintRowMapping.Count)
      {
        IBlueprint newSelection = blueprintRowMapping[index];
        if (newSelection == null) // Category
        {
          return;
        }

        if (newSelection != selectedBlueprint)
        {
          UpdateSelection(index);
        }
        else if (Time.TimeSinceStartup - lastSelectionClick < UXConstants.DoubleClickThreshold)
        {
          TryCreateBlueprint();
        }

        lastSelectionClick = Time.TimeSinceStartup;
      }
    }

    private void UpdateSelection(int index)
    {
      ResetExistingSelection();

      rowColors[index] = UXConstants.SelectedColor;
      Token.SetBindValues(View.RowColors, rowColors);

      selectedBlueprint = blueprintRowMapping[index];
      Token.SetBindValue(View.CreateButtonEnabled, true);
    }

    private void ResetExistingSelection()
    {
      if (selectedBlueprint != null)
      {
        int existingSelection = blueprintRowMapping.IndexOf(selectedBlueprint);
        if (existingSelection >= 0)
        {
          rowColors[existingSelection] = UXConstants.DefaultColor2;
        }
      }
    }

    private void TryCreateBlueprint()
    {
      if (selectedBlueprint == null)
      {
        Token.Player.FloatingTextString("Select a blueprint first.", false);
        return;
      }

      Token.Player.TryEnterTargetMode(CreateBlueprintInstance);
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
      Token.Player.SendServerMessage($"\"{nwObject.Name}\" Created.");
    }

    private void RefreshCreatorList()
    {
      List<IBlueprint> blueprints = LoadSearchResults();
      blueprintRowMapping = new List<IBlueprint>(blueprints.Count * 2);

      int listCapacity = blueprints.Count * 2;
      rowColors = new List<NuiColor>(listCapacity);
      List<string> blueprintNamesAndCategories = new List<string>(listCapacity);
      List<string> blueprintCRs = new List<string>(listCapacity);
      List<string> blueprintFactions = new List<string>(listCapacity);

      string currentCategory = null;
      foreach (IBlueprint blueprint in blueprints)
      {
        if (blueprint.Category != currentCategory)
        {
          rowColors.Add(UXConstants.DefaultColor);
          blueprintRowMapping.Add(null);
          blueprintNamesAndCategories.Add(blueprint.Category);
          blueprintCRs.Add(null);
          blueprintFactions.Add(null);
          currentCategory = blueprint.Category;
        }

        rowColors.Add(blueprint == selectedBlueprint ? UXConstants.SelectedColor : UXConstants.DefaultColor2);
        blueprintRowMapping.Add(blueprint);
        blueprintNamesAndCategories.Add(blueprint.Name);
        blueprintCRs.Add(blueprint.CR?.ToString("0.##"));
        blueprintFactions.Add(blueprint.Faction);
      }

      Stopwatch stopwatch = Stopwatch.StartNew();
      Token.SetBindValues(View.RowColors, rowColors);
      Token.SetBindValues(View.BlueprintNamesAndCategories, blueprintNamesAndCategories);
      Token.SetBindValues(View.BlueprintCRs, blueprintCRs);
      Token.SetBindValues(View.BlueprintFactions, blueprintFactions);
      Token.SetBindValue(View.BlueprintCount, blueprintRowMapping.Count);
      Console.WriteLine($"Result writing took {stopwatch.Elapsed.TotalSeconds}s to complete.");
    }

    private List<IBlueprint> LoadSearchResults()
    {
      return BlueprintManager.GetMatchingBlueprints((BlueprintObjectType)Token.GetBindValue(View.BlueprintType), Token.GetBindValue(View.Search), MaxItems);
    }
  }
}
