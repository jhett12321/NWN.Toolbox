namespace Jorteck.Toolbox.Features.Languages
{
  public readonly struct LanguageOutput
  {
    public readonly ILanguage Language;
    public readonly string Output;
    public readonly string Interpretation;

    public LanguageOutput(ILanguage language, string interpretation, string output)
    {
      Language = language;
      Interpretation = interpretation;
      Output = output;
    }
  }
}
