using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageUndercommon : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "undercommon";

    public string[] Aliases => new[] { "drow" };

    public string Name => "Undercommon";

    public Color ChatColor => new Color(153, 51, 255);

    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    private readonly Dictionary<char, string> dictionary = new Dictionary<char, string>
    {
      ['a'] = "il",
      ['A'] = "Il",
      ['b'] = "f",
      ['B'] = "F",
      ['c'] = "st",
      ['C'] = "St",
      ['d'] = "w",
      ['D'] = "W",
      ['e'] = "a",
      ['E'] = "A",
      ['f'] = "o",
      ['F'] = "O",
      ['g'] = "v",
      ['G'] = "V",
      ['h'] = "ir",
      ['H'] = "Ir",
      ['i'] = "e",
      ['I'] = "E",
      ['j'] = "vi",
      ['J'] = "Vi",
      ['k'] = "go",
      ['K'] = "Go",
      ['l'] = "c",
      ['L'] = "C",
      ['m'] = "li",
      ['M'] = "Li",
      ['n'] = "l",
      ['N'] = "L",
      ['o'] = "e",
      ['O'] = "E",
      ['p'] = "ty",
      ['P'] = "Ty",
      ['q'] = "r",
      ['Q'] = "R",
      ['r'] = "m",
      ['R'] = "M",
      ['s'] = "la",
      ['S'] = "La",
      ['t'] = "an",
      ['T'] = "An",
      ['u'] = "y",
      ['U'] = "Y",
      ['v'] = "el",
      ['V'] = "El",
      ['w'] = "ky",
      ['W'] = "Ky",
      ['x'] = "'",
      ['X'] = "'",
      ['y'] = "a",
      ['Y'] = "A",
      ['z'] = "p'",
      ['Z'] = "P'",
    };

    public LanguageOutput Translate(string phrase, int proficiency)
    {
      return LanguageUtils.TranslateUsingDictionary(this, dictionary, phrase, proficiency);
    }
  }
}
