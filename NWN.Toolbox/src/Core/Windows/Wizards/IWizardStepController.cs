using Anvil.API;
using Anvil.API.Events;

namespace Jorteck.Toolbox.Core
{
  public interface IWizardStepController
  {
    /// <summary>
    /// Gets the associated view.
    /// </summary>
    IWizardStepView View { get; }

    /// <summary>
    /// Gets the parent window token.
    /// </summary>
    NuiWindowToken Token { get; init; }

    /// <summary>
    /// Gets if this step is considered "complete". Determines if the "Next" button should be enabled and clickable.
    /// </summary>
    bool CanCompleteStep { get; }

    /// <summary>
    /// The window title string to show while on this step.
    /// </summary>
    string StepTitle { get; }

    void Init();

    void ProcessEvent(ModuleEvents.OnNuiEvent eventData);

    /// <summary>
    /// Called when leaving this step (next/previous buttons, close button).
    /// </summary>
    void OnClose();
  }

  public interface IWizardStepController<TView> : IWizardStepController where TView : IWizardStepView
  {
    IWizardStepView IWizardStepController.View => View;

    new TView View { get; init; }
  }
}
