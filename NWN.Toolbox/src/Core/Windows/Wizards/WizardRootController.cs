using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Toolbox.Core
{
  public abstract class WizardRootController<T> : WindowController<T> where T : WindowView<T>, IWizardRootView, new()
  {
    [Inject]
    protected InjectionService InjectionService { get; init; }

    protected readonly List<IWizardStepController> Steps = new List<IWizardStepController>();

    private IWizardStepController currentStep;

    protected IWizardStepController CurrentStep
    {
      get => currentStep;
      set
      {
        currentStep?.OnClose();
        currentStep = value;

        Token.SetGroupLayout(View.ViewContainer, currentStep.View.ViewTemplate);

        currentStep.Init();
        RefreshWizardButtons();

        Token.SetBindValue(View.WindowTitleText, currentStep.StepTitle);
      }
    }

    public sealed override void Init()
    {
      InitWizard();
      CurrentStep = Steps[0];
    }

    public abstract void InitWizard();

    public abstract void OnWizardComplete();

    public override void ProcessEvent(ModuleEvents.OnNuiEvent eventData)
    {
      CurrentStep?.ProcessEvent(eventData);
      RefreshWizardButtons();

      if (eventData.EventType == NuiEventType.Click)
      {
        HandleButtonClick(eventData);
      }
    }

    private void HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.NextButton.Id)
      {
        if (IsFinalStep(CurrentStep))
        {
          Close();
          OnWizardComplete();
        }
        else
        {
          CurrentStep = GetNextStep();
        }
      }
      else if (eventData.ElementId == View.PreviousButton.Id)
      {
        CurrentStep = GetPreviousStep();
      }
    }

    protected virtual bool IsFinalStep(IWizardStepController step)
    {
      int stepIndex = Steps.IndexOf(step);
      return stepIndex >= 0 && stepIndex == Steps.Count - 1;
    }

    protected virtual IWizardStepController GetNextStep()
    {
      return Steps[Steps.IndexOf(CurrentStep) + 1];
    }

    protected virtual IWizardStepController GetPreviousStep()
    {
      return Steps[Steps.IndexOf(CurrentStep) - 1];
    }

    protected virtual TStep GetStep<TStep>() where TStep : IWizardStepController
    {
      return Steps.OfType<TStep>().FirstOrDefault();
    }

    protected TController RegisterStep<TView, TController>()
      where TView : IWizardStepView, new()
      where TController : IWizardStepController<TView>, new()
    {
      TView view = new TView();
      TController controller = InjectionService.Inject(new TController
      {
        View = view,
        Token = Token,
      });

      Steps.Add(controller);
      return controller;
    }

    protected void RegisterStep(IWizardStepController controller)
    {
      Steps.Add(controller);
    }

    protected virtual void RefreshWizardButtons()
    {
      int stepIndex = Steps.IndexOf(CurrentStep);

      Token.SetBindValue(View.NextButtonEnabled, CurrentStep.CanCompleteStep);
      Token.SetBindValue(View.PreviousButtonEnabled, stepIndex > 0 && Steps.Count > 1);
      Token.SetBindValue(View.NextButtonText, !IsFinalStep(CurrentStep) ? View.WizardTexts.NextButton : View.WizardTexts.FinishButton);
    }

    protected override void OnClose()
    {
      CurrentStep?.OnClose();
    }
  }
}
