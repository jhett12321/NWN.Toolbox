using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageDwarven : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "dwarven";

    public string Name => "Dwarven";

    public string[] Aliases => new[] { "dwarf" };

    public Color ChatColor => new Color(255, 255, 51);

    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    private readonly Dictionary<char, string> dictionary = new Dictionary<char, string>
    {
      ['a'] = "az",
      ['A'] = "Az",
      ['b'] = "po",
      ['B'] = "Po",
      ['c'] = "zi",
      ['C'] = "Zi",
      ['d'] = "t",
      ['D'] = "T",
      ['e'] = "a",
      ['E'] = "A",
      ['f'] = "wa",
      ['F'] = "Wa",
      ['g'] = "k",
      ['G'] = "K",
      ['h'] = "'",
      ['H'] = "'",
      ['i'] = "a",
      ['I'] = "A",
      ['j'] = "dr",
      ['J'] = "Dr",
      ['k'] = "g",
      ['K'] = "G",
      ['l'] = "n",
      ['L'] = "N",
      ['m'] = "l",
      ['M'] = "L",
      ['n'] = "r",
      ['N'] = "R",
      ['o'] = "ur",
      ['O'] = "Ur",
      ['p'] = "rh",
      ['P'] = "Rh",
      ['q'] = "k",
      ['Q'] = "K",
      ['r'] = "h",
      ['R'] = "H",
      ['s'] = "th",
      ['S'] = "Th",
      ['t'] = "k",
      ['T'] = "K",
      ['u'] = "'",
      ['U'] = "'",
      ['v'] = "g",
      ['V'] = "G",
      ['w'] = "zh",
      ['W'] = "Zh",
      ['x'] = "q",
      ['X'] = "Q",
      ['y'] = "o",
      ['Y'] = "O",
      ['z'] = "j",
      ['Z'] = "J",
    };

    public LanguageOutput Translate(string phrase, int proficiency)
    {
      return LanguageUtils.TranslateUsingDictionary(this, dictionary, phrase, proficiency);
    }
  }
}
