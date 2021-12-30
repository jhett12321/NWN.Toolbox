using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Anvil.API;
using Anvil.API.Events;

namespace Jorteck.Toolbox
{
  public sealed class ObjectSelectionListController
  {
    private static readonly TimeSpan DoubleClickThreshold = TimeSpan.FromMilliseconds(500);

    private readonly NuiColor defaultColor = new NuiColor(255, 255, 255);
    private readonly NuiColor selectedColor = new NuiColor(255, 255, 0);

    private List<NwGameObject> objectList;
    private NuiColor[] rowColors;

    private NwObject selectedObject;
    private NwArea selectedArea;

    private readonly ObjectSelectionListView view;
    private readonly WindowToken windowToken;
    private TimeSpan lastSelectionClick;

    public ObjectSelectionListController(ObjectSelectionListView view, WindowToken windowToken)
    {
      this.view = view;
      this.windowToken = windowToken;
    }

    public void Init(NwArea area)
    {
      windowToken.SetBindValue(view.SearchObjectType, (int)ObjectSelectionTypes.Creature);
      UpdateAreaSelection(area);
      RefreshObjectList();
    }

    public bool ProcessEvent(ModuleEvents.OnNuiEvent eventData)
    {
      switch (eventData.EventType)
      {
        case NuiEventType.Click:
          return HandleButtonClick(eventData);
        case NuiEventType.MouseDown:
          return HandleMouseDown(eventData);
        default:
          return false;
      }
    }

    private bool HandleMouseDown(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == view.ObjectRowId)
      {
        SelectObject(eventData.ArrayIndex);
        return true;
      }

      return false;
    }

    private bool HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == view.SearchButton.Id)
      {
        RefreshObjectList();
        return true;
      }

      return false;
    }

    private void RefreshObjectList()
    {
      if (selectedArea == null)
      {
        return;
      }

      objectList = LoadSearchResults();
      UpdateListViewBinds(objectList);
    }

    private List<NwGameObject> LoadSearchResults()
    {
      string search = windowToken.GetBindValue(view.Search);
      ObjectSelectionTypes searchTypes = (ObjectSelectionTypes)windowToken.GetBindValue(view.SearchObjectType);

      List<NwGameObject> results = new List<NwGameObject>();
      if (searchTypes.HasFlag(ObjectSelectionTypes.Player))
      {
        foreach (NwPlayer player in NwModule.Instance.Players)
        {
          NwCreature controlledCreature = player.ControlledCreature;
          if (controlledCreature != null && player.ControlledCreature.Area == selectedArea)
          {
            results.Add(player.ControlledCreature);
          }
        }
      }

      float distance = windowToken.GetBindValue(view.SearchDistance).ParseFloat(-1f);
      if (distance <= 0f)
      {
        results.AddRange(selectedArea.Objects.Where(gameObject => searchTypes.HasFlag(gameObject.GetSelectionType())
          && gameObject.Name.Contains(search, StringComparison.OrdinalIgnoreCase)));
      }
      else
      {
        NwCreature playerCreature = windowToken.Player.ControlledCreature;
        if (selectedArea == playerCreature.Area)
        {
          Vector3 playerPos = playerCreature.Position;
          float distanceSqr = distance * distance;

          results.AddRange(selectedArea.Objects.Where(gameObject => searchTypes.HasFlag(gameObject.GetSelectionType())
            && gameObject.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
            && Vector3.DistanceSquared(playerPos, gameObject.Position) < distanceSqr));
        }
      }

      return results;
    }

    private void UpdateListViewBinds(List<NwGameObject> gameObjects)
    {
      rowColors = new NuiColor[gameObjects.Count];

      string[] objectTypes = new string[gameObjects.Count];
      string[] objectNames = new string[gameObjects.Count];
      string[] objectResRefs = new string[gameObjects.Count];
      string[] objectTags = new string[gameObjects.Count];
      string[] objectHPs = new string[gameObjects.Count];
      string[] objectCRs = new string[gameObjects.Count];

      for (int i = 0; i < gameObjects.Count; i++)
      {
        NwGameObject gameObject = gameObjects[i];
        rowColors[i] = gameObject != selectedObject ? defaultColor : selectedColor;
        objectTypes[i] = gameObject.GetTypeName();
        objectNames[i] = gameObject.Name;
        objectResRefs[i] = gameObject.ResRef;
        objectTags[i] = gameObject.Tag;
        objectHPs[i] = $"{gameObject.HP}/{gameObject.MaxHP}";
        objectCRs[i] = gameObject is NwCreature creature ? creature.ChallengeRating.ToString(CultureInfo.InvariantCulture) : "";
      }

      windowToken.SetBindValues(view.RowColors, rowColors);
      windowToken.SetBindValues(view.ObjectTypes, objectTypes);
      windowToken.SetBindValues(view.ObjectNames, objectNames);
      windowToken.SetBindValues(view.ObjectResRefs, objectResRefs);
      windowToken.SetBindValues(view.ObjectTags, objectTags);
      windowToken.SetBindValues(view.ObjectHPs, objectHPs);
      windowToken.SetBindValues(view.ObjectCRs, objectCRs);
      windowToken.SetBindValue(view.ObjectCount, gameObjects.Count);
    }

    private void SelectObject(int index)
    {
      if (index >= 0 && index < objectList.Count)
      {
        NwGameObject newSelection = objectList[index];
        if (newSelection != selectedObject)
        {
          UpdateObjectSelection(index);
        }
        else if (Time.TimeSinceStartup - lastSelectionClick < DoubleClickThreshold)
        {
          JumpToObject(newSelection);
        }

        lastSelectionClick = Time.TimeSinceStartup;
      }
    }

    private void JumpToObject(NwGameObject gameObject)
    {
      windowToken.Player.ControlledCreature.JumpToObject(gameObject);
    }

    private void UpdateObjectSelection(int index)
    {
      if (selectedObject != null && selectedObject is NwGameObject gameObject)
      {
        int existingSelection = objectList.IndexOf(gameObject);
        if (existingSelection >= 0)
        {
          rowColors[existingSelection] = defaultColor;
        }
      }

      selectedObject = objectList[index];
      rowColors[index] = selectedColor;
      windowToken.SetBindValues(view.RowColors, rowColors);
    }

    private void UpdateAreaSelection(NwArea area)
    {
      selectedArea = area;
      windowToken.SetBindValue(view.CurrentArea, area.Name);
    }
  }
}
