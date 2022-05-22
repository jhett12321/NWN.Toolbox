using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageGoblin : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "goblin";

    public string Name => "Goblin";

    public Color ChatColor => new Color(51, 255, 51);

    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    private readonly Dictionary<char, string> dictionary = new Dictionary<char, string>
    {
      ['a'] = "u",
      ['A'] = "U",
      ['b'] = "p",
      ['B'] = "P",
      ['c'] = "",
      ['C'] = "",
      ['d'] = "t",
      ['D'] = "T",
      ['e'] = "'",
      ['E'] = "'",
      ['f'] = "v",
      ['F'] = "V",
      ['g'] = "k",
      ['G'] = "K",
      ['h'] = "r",
      ['H'] = "R",
      ['i'] = "o",
      ['I'] = "O",
      ['j'] = "z",
      ['J'] = "Z",
      ['k'] = "g",
      ['K'] = "G",
      ['l'] = "m",
      ['L'] = "M",
      ['m'] = "s",
      ['M'] = "S",
      ['n'] = "",
      ['N'] = "",
      ['o'] = "u",
      ['O'] = "U",
      ['p'] = "b",
      ['P'] = "B",
      ['q'] = "",
      ['Q'] = "",
      ['r'] = "n",
      ['R'] = "N",
      ['s'] = "k",
      ['S'] = "K",
      ['t'] = "d",
      ['T'] = "D",
      ['u'] = "u",
      ['U'] = "U",
      ['v'] = "",
      ['V'] = "",
      ['w'] = "'",
      ['W'] = "'",
      ['x'] = "",
      ['X'] = "",
      ['y'] = "o",
      ['Y'] = "O",
      ['z'] = "w",
      ['Z'] = "W",
    };

    public LanguageOutput Translate(string phrase, int proficiency)
    {
      return LanguageUtils.TranslateUsingDictionary(this, dictionary, phrase, proficiency);
    }
  }
}
