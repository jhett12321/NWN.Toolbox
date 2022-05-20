using System.Collections.Generic;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageHalfling : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "";
    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    private readonly Dictionary<char, string> dictionary = new Dictionary<char, string>
    {
      ['a'] = "e",
      ['A'] = "E",
      ['b'] = "p",
      ['B'] = "P",
      ['c'] = "s",
      ['C'] = "S",
      ['d'] = "t",
      ['D'] = "T",
      ['e'] = "i",
      ['E'] = "I",
      ['f'] = "w",
      ['F'] = "W",
      ['g'] = "k",
      ['G'] = "K",
      ['h'] = "n",
      ['H'] = "N",
      ['i'] = "u",
      ['I'] = "U",
      ['j'] = "v",
      ['J'] = "V",
      ['k'] = "g",
      ['K'] = "G",
      ['l'] = "c",
      ['L'] = "C",
      ['m'] = "l",
      ['M'] = "L",
      ['n'] = "r",
      ['N'] = "R",
      ['o'] = "y",
      ['O'] = "Y",
      ['p'] = "b",
      ['P'] = "B",
      ['q'] = "x",
      ['Q'] = "X",
      ['r'] = "h",
      ['R'] = "H",
      ['s'] = "m",
      ['S'] = "M",
      ['t'] = "d",
      ['T'] = "D",
      ['u'] = "o",
      ['U'] = "O",
      ['v'] = "f",
      ['V'] = "F",
      ['w'] = "z",
      ['W'] = "Z",
      ['x'] = "q",
      ['X'] = "Q",
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
