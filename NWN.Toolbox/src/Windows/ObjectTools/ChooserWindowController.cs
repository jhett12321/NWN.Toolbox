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
      objectSelectionListController = InjectionService.Inject(new ObjectSelectionListController(View.SelectionListView, Token));
      objectSelectionListController.Init(area);
    }

    public override void ProcessEvent(ModuleEvents.OnNuiEvent eventData)
    {
      if (objectSelectionListController.ProcessEvent(eventData))
      {
        return;
      }
    }

    protected override void OnClose() {}
  }
}
