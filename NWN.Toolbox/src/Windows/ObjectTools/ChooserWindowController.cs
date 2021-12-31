using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  public sealed class ChooserWindowController : WindowController<ChooserWindowView>
  {
    [Inject]
    private InjectionService InjectionService { get; init; }

    private ObjectSelectionListController objectSelectionListController;

    public override void Init()
    {
      Init(Token.Player.ControlledCreature.Area);
    }

    public void Init(NwArea area)
    {
      UpdateAvailableChooserButtons();
      objectSelectionListController = InjectionService.Inject(new ObjectSelectionListController(View.SelectionListView, Token));
      objectSelectionListController.Init(area);
      objectSelectionListController.OnObjectSelectChange += OnObjectSelectChange;
    }

    private void OnObjectSelectChange()
    {
      if (objectSelectionListController.SelectedObject is NwArea)
      {
        UpdateAvailableChooserButtons();
      }
      else if (objectSelectionListController.SelectedObject is NwAreaOfEffect)
      {
        UpdateAvailableChooserButtons(View.GoToButtonEnabled, View.DestroyButtonEnabled);
      }
      else if (objectSelectionListController.SelectedObject is NwCreature)
      {
        UpdateAvailableChooserButtons(View.AllButtonStates);
      }
      else if (objectSelectionListController.SelectedObject is NwDoor)
      {
        UpdateAvailableChooserButtons(View.GoToButtonEnabled, View.DestroyButtonEnabled, View.ExamineButtonEnabled, View.TogglePlotButtonEnabled);
      }
      else if (objectSelectionListController.SelectedObject is NwEncounter)
      {
        UpdateAvailableChooserButtons(View.GoToButtonEnabled, View.DestroyButtonEnabled, View.TogglePlotButtonEnabled);
      }
      else if (objectSelectionListController.SelectedObject is NwItem)
      {
        UpdateAvailableChooserButtons(View.GoToButtonEnabled, View.DestroyButtonEnabled, View.ExamineButtonEnabled, View.TogglePlotButtonEnabled);
      }
      else if (objectSelectionListController.SelectedObject is NwPlaceable)
      {
        UpdateAvailableChooserButtons(View.GoToButtonEnabled, View.JumpButtonEnabled, View.DestroyButtonEnabled, View.ExamineButtonEnabled, View.TogglePlotButtonEnabled, View.HealButtonEnabled);
      }
      else if (objectSelectionListController.SelectedObject is NwSound)
      {
        UpdateAvailableChooserButtons(View.GoToButtonEnabled, View.DestroyButtonEnabled, View.TogglePlotButtonEnabled);
      }
      else if (objectSelectionListController.SelectedObject is NwStore)
      {
        UpdateAvailableChooserButtons(View.GoToButtonEnabled, View.DestroyButtonEnabled, View.TogglePlotButtonEnabled);
      }
      else if (objectSelectionListController.SelectedObject is NwTrigger)
      {
        UpdateAvailableChooserButtons(View.GoToButtonEnabled, View.DestroyButtonEnabled, View.ExamineButtonEnabled, View.TogglePlotButtonEnabled);
      }
      else if (objectSelectionListController.SelectedObject is NwWaypoint)
      {
        UpdateAvailableChooserButtons(View.GoToButtonEnabled, View.DestroyButtonEnabled, View.TogglePlotButtonEnabled);
      }
      else if (objectSelectionListController.SelectedObject is NwModule)
      {
        UpdateAvailableChooserButtons();
      }
      else
      {
        UpdateAvailableChooserButtons();
      }
    }

    private void UpdateAvailableChooserButtons(params NuiBind<bool>[] enabledButtons)
    {
      foreach (NuiBind<bool> buttonState in View.AllButtonStates)
      {
        Token.SetBindValue(buttonState, enabledButtons.Contains(buttonState));
      }
    }

    public override void ProcessEvent(ModuleEvents.OnNuiEvent eventData)
    {
      if (objectSelectionListController.ProcessEvent(eventData))
      {
        return;
      }

      if (eventData.EventType == NuiEventType.Click)
      {
        HandleButtonClick(eventData);
      }
    }

    private void HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      NwObject targetObject = objectSelectionListController.SelectedObject;
      if (targetObject is not NwGameObject gameObject)
      {
        return;
      }

      NwPlayer player = Token.Player;
      string elementId = eventData.ElementId;
      if (elementId == View.GoToButton.Id)
      {
        objectSelectionListController.JumpToObject(gameObject);
      }
      else if (elementId == View.DestroyButton.Id)
      {
        DestroyObject(player, gameObject);
      }
      else if (elementId == View.JumpButton.Id)
      {
        JumpToDM(player, gameObject);
      }
      else if (elementId == View.ToggleAIButton.Id)
      {
        ToggleAI(player, gameObject);
      }
      else if (elementId == View.HealButton.Id)
      {
        Heal(player, gameObject);
      }
      else if (elementId == View.ControlButton.Id)
      {
        ControlObject(player, gameObject);
      }
      else if (elementId == View.RestButton.Id)
      {
        ForceRest(player, gameObject);
      }
      else if (elementId == View.LimboButton.Id)
      {
        SendToLimbo(player, gameObject);
      }
      else if (elementId == View.ExamineButton.Id)
      {
        ExamineObject(player, gameObject);
      }
      else if (elementId == View.PossessButton.Id)
      {
        PossessObject(player, gameObject);
      }
      else if (elementId == View.ToggleImmortalButton.Id)
      {
        ToggleImmortal(player, gameObject);
      }
      else if (elementId == View.TogglePlotMode.Id)
      {
        TogglePlotMode(player, gameObject);
      }
    }

    private void DestroyObject(NwPlayer player, NwGameObject gameObject)
    {
      string objectName = gameObject.Name;
      if (gameObject.PlotFlag)
      {
        player.SendErrorMessage($"{objectName}: Cannot destroy as god/plot mode is enabled. Disable god mode first before destroying the object.");
        return;
      }

      bool isStatic = gameObject is NwPlaceable placeable && placeable.IsStatic;

      gameObject.Destroy();

      player.SendServerMessage($"{objectName}: Destroyed");
      if (isStatic)
      {
        player.SendServerMessage($"NOTE! Players need to re-enter the area to see the updated change.");
      }
    }

    private void JumpToDM(NwPlayer player, NwGameObject gameObject)
    {
      gameObject.Location = player.ControlledCreature.Location;
    }

    private void ToggleAI(NwPlayer player, NwGameObject gameObject)
    {
      // TODO implement.
      player.SendErrorMessage("Not implemented.");
    }

    private void Heal(NwPlayer player, NwGameObject gameObject)
    {
      string objectName = gameObject.Name;
      int damage = gameObject.MaxHP - gameObject.HP;

      if (damage > 0)
      {
        gameObject.ApplyEffect(EffectDuration.Instant, Effect.Heal(damage));
        player.SendServerMessage($"{objectName}: Healed {damage} hitpoints");
      }
      else
      {
        player.SendErrorMessage($"{objectName}: Already at full hitpoints");
      }
    }

    private void ControlObject(NwPlayer player, NwGameObject gameObject)
    {
      if (gameObject is NwCreature creature && player.IsDM && creature.Master == null)
      {
        player.DMPossessCreature(creature, false);
      }
    }

    private void ForceRest(NwPlayer player, NwGameObject gameObject)
    {
      if (gameObject is NwCreature creature)
      {
        creature.ForceRest();
        player.SendServerMessage($"{gameObject.Name}: Force Rest triggered");
      }
    }

    private void SendToLimbo(NwPlayer player, NwGameObject gameObject)
    {
      NwModule.Instance.MoveObjectToLimbo(gameObject);
      player.SendServerMessage($"{gameObject.Name}: Moved to limbo");
    }

    private void ExamineObject(NwPlayer player, NwGameObject gameObject)
    {
      player.ForceExamine(gameObject);
    }

    private void PossessObject(NwPlayer player, NwGameObject gameObject)
    {
      if (gameObject is NwCreature creature && player.IsDM && creature.Master == null)
      {
        player.DMPossessCreature(creature, true);
      }
    }

    private void ToggleImmortal(NwPlayer player, NwGameObject gameObject)
    {
      if (gameObject is NwCreature creature)
      {
        creature.Immortal = !creature.Immortal;
        player.SendServerMessage($"{gameObject.Name}: Toggled immortal mode to {creature.Immortal}");
      }
    }

    private void TogglePlotMode(NwPlayer player, NwGameObject gameObject)
    {
      gameObject.PlotFlag = !gameObject.PlotFlag;
      player.SendServerMessage($"{gameObject.Name}: Toggled plot/god mode to {gameObject.PlotFlag}");
    }

    protected override void OnClose() {}
  }
}
