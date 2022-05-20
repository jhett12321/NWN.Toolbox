using System.Collections.Generic;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageElven : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "";

    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    private readonly Dictionary<char, string> dictionary = new Dictionary<char, string>
    {
      ['a'] = "il",
      ['A'] = "Il",
      ['b'] = "f",
      ['B'] = "F",
      ['c'] = "ny",
      ['C'] = "Ny",
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
      ['j'] = "qu",
      ['J'] = "Qu",
      ['k'] = "n",
      ['K'] = "N",
      ['l'] = "c",
      ['L'] = "C",
      ['m'] = "s",
      ['M'] = "S",
      ['n'] = "l",
      ['N'] = "L",
      ['o'] = "e",
      ['O'] = "E",
      ['p'] = "ty",
      ['P'] = "Ty",
      ['q'] = "h",
      ['Q'] = "H",
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
      ['w'] = "am",
      ['W'] = "Am",
      ['x'] = "'",
      ['X'] = "'",
      ['y'] = "a",
      ['Y'] = "A",
      ['z'] = "j",
      ['Z'] = "J",
    };

    public LanguageOutput Translate(string phrase, int proficiency)
    {
      return LanguageUtils.TranslateUsingDictionary(dictionary, phrase, proficiency);
    }
  }
}
