using Anvil.API;

namespace Jorteck.Toolbox.Features.Languages
{
  public interface ILanguage
  {
    string Id { get; }

    string[] Aliases => null;

    string Name { get; }

    Color ChatColor { get; }

    bool Enabled { get; }

    LanguageOutput Translate(string phrase, int proficiency);
  }
}
