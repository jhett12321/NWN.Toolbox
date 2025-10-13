namespace Jorteck.Toolbox.Core
{
  public sealed class WizardTexts
  {
    public required string NextButton { get; init; }
    public required string PreviousButton { get; init; }
    public required string FinishButton { get; init; }

    public static readonly WizardTexts Default = new WizardTexts
    {
      NextButton = "Next",
      PreviousButton = "Previous",
      FinishButton = "Finish",
    };
  }
}
