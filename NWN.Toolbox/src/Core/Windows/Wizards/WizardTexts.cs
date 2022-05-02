namespace Jorteck.Toolbox.Core
{
  public sealed class WizardTexts
  {
    public string NextButton { get; init; }
    public string PreviousButton { get; init; }
    public string FinishButton { get; init; }

    public static readonly WizardTexts Default = new WizardTexts
    {
      NextButton = "Next",
      PreviousButton = "Previous",
      FinishButton = "Finish",
    };
  }
}
