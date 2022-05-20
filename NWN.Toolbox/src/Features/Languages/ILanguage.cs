namespace Jorteck.Toolbox.Features.Languages
{
  public interface ILanguage
  {
    string Id { get; }

    bool Enabled { get; }

    LanguageOutput Translate(string phrase, int proficiency);
  }
}
