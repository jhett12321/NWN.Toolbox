using System.Collections.Generic;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageRashemi : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "";
    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    private readonly Dictionary<char, string> dictionary = new Dictionary<char, string>
    {
      ['a'] = "a",
      ['A'] = "A",
      ['b'] = "s",
      ['B'] = "S",
      ['c'] = "n",
      ['C'] = "N",
      ['d'] = "y",
      ['D'] = "Y",
      ['e'] = "ov",
      ['E'] = "Ov",
      ['f'] = "d",
      ['F'] = "D",
      ['g'] = "sk",
      ['G'] = "Sk",
      ['h'] = "fr",
      ['H'] = "Fr",
      ['i'] = "u",
      ['I'] = "U",
      ['j'] = "o",
      ['J'] = "O",
      ['k'] = "f",
      ['K'] = "F",
      ['l'] = "r",
      ['L'] = "R",
      ['m'] = "z",
      ['M'] = "Z",
      ['n'] = "s",
      ['N'] = "S",
      ['o'] = "o",
      ['O'] = "O",
      ['p'] = "j",
      ['P'] = "J",
      ['q'] = "sk",
      ['Q'] = "Sk",
      ['r'] = " ",
      ['R'] = "M",
      ['s'] = "or",
      ['S'] = "Or",
      ['t'] = "ka",
      ['T'] = "Ka",
      ['u'] = "o",
      ['U'] = "O",
      ['v'] = "ka",
      ['V'] = "Ka",
      ['w'] = "ma",
      ['W'] = "Ma",
      ['x'] = "o",
      ['X'] = "O",
      ['y'] = "oj",
      ['Y'] = "Oj",
      ['z'] = "y",
      ['Z'] = "Y",
    };

    public LanguageOutput Translate(string phrase, int proficiency)
    {
      return LanguageUtils.TranslateUsingDictionary(dictionary, phrase, proficiency);
    }
  }
}
