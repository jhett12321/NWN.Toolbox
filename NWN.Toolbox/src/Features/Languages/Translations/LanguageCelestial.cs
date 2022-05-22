using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageCelestial : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "celestial";

    public string Name => "Celestial";

    public Color ChatColor => new Color(51, 255, 255);

    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    private readonly Dictionary<char, string> dictionary = new Dictionary<char, string>
    {
      ['a'] = "a",
      ['A'] = "A",
      ['b'] = "p",
      ['B'] = "P",
      ['c'] = "v",
      ['C'] = "V",
      ['d'] = "t",
      ['D'] = "T",
      ['e'] = "el",
      ['E'] = "El",
      ['f'] = "b",
      ['F'] = "B",
      ['g'] = "w",
      ['G'] = "W",
      ['h'] = "r",
      ['H'] = "R",
      ['i'] = "i",
      ['I'] = "I",
      ['j'] = "m",
      ['J'] = "M",
      ['k'] = "x",
      ['K'] = "X",
      ['l'] = "h",
      ['L'] = "H",
      ['m'] = "s",
      ['M'] = "S",
      ['n'] = "c",
      ['N'] = "C",
      ['o'] = "u",
      ['O'] = "U",
      ['p'] = "q",
      ['P'] = "Q",
      ['q'] = "d",
      ['Q'] = "D",
      ['r'] = "n",
      ['R'] = "N",
      ['s'] = "l",
      ['S'] = "L",
      ['t'] = "y",
      ['T'] = "Y",
      ['u'] = "o",
      ['U'] = "O",
      ['v'] = "j",
      ['V'] = "J",
      ['w'] = "f",
      ['W'] = "F",
      ['x'] = "g",
      ['X'] = "G",
      ['y'] = "z",
      ['Y'] = "Z",
      ['z'] = "k",
      ['Z'] = "K",
    };

    public LanguageOutput Translate(string phrase, int proficiency)
    {
      return LanguageUtils.TranslateUsingDictionary(this, dictionary, phrase, proficiency);
    }
  }
}
