using Anvil.API.Events;

namespace Jorteck.Toolbox.Core
{
  public interface IWizardStepController
  {
    IWizardStepView View { get; }

    void Init();

    void ProcessEvent(ModuleEvents.OnNuiEvent eventData);

    void OnClose();
  }

  public interface IWizardStepController<T> : IWizardStepController where T : IWizardStepView
  {
    IWizardStepView IWizardStepController.View => View;

    new T View { get; init; }
  }
}
