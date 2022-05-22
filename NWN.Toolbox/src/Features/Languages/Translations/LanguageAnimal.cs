using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageAnimal : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "animal";

    public string Name => "Animal";

    public Color ChatColor => new Color(51, 255, 153);

    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    public LanguageOutput Translate(string phrase, int proficiency)
    {
      char[] output = new char[phrase.Length];
      for (int i = 0; i < phrase.Length; i++)
      {
        char c = phrase[i];
        output[i] = char.IsLetter(c) ? '\'' : c;
      }

      return new LanguageOutput(this, phrase, new string(output));
    }
  }
}
