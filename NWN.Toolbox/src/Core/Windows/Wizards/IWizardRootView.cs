using Anvil.API;

namespace Jorteck.Toolbox.Core
{
  public interface IWizardRootView
  {
    NuiBind<string> WindowTitleText { get; }

    NuiGroup ViewContainer { get; }

    WizardTexts WizardTexts => WizardTexts.Default;

    NuiButton NextButton { get; }

    NuiBind<bool> NextButtonEnabled { get; }

    NuiBind<string> NextButtonText { get; }

    NuiButton PreviousButton { get; }

    NuiBind<bool> PreviousButtonEnabled { get; }
  }
}
