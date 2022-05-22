using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageOrc : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "orc";

    public string Name => "Orc";

    public Color ChatColor => new Color(51, 255, 153);

    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    private readonly Dictionary<char, string> dictionary = new Dictionary<char, string>
    {
      ['a'] = "ha",
      ['A'] = "Ha",
      ['b'] = "p",
      ['B'] = "P",
      ['c'] = "z",
      ['C'] = "Z",
      ['d'] = "t",
      ['D'] = "T",
      ['e'] = "o",
      ['E'] = "O",
      ['f'] = "",
      ['F'] = "",
      ['g'] = "k",
      ['G'] = "K",
      ['h'] = "r",
      ['H'] = "R",
      ['i'] = "a",
      ['I'] = "A",
      ['j'] = "m",
      ['J'] = "M",
      ['k'] = "g",
      ['K'] = "G",
      ['l'] = "h",
      ['L'] = "H",
      ['m'] = "r",
      ['M'] = "R",
      ['n'] = "k",
      ['N'] = "K",
      ['o'] = "u",
      ['O'] = "U",
      ['p'] = "b",
      ['P'] = "B",
      ['q'] = "k",
      ['Q'] = "K",
      ['r'] = "h",
      ['R'] = "H",
      ['s'] = "g",
      ['S'] = "G",
      ['t'] = "n",
      ['T'] = "N",
      ['u'] = "",
      ['U'] = "",
      ['v'] = "g",
      ['V'] = "G",
      ['w'] = "r",
      ['W'] = "R",
      ['x'] = "r",
      ['X'] = "R",
      ['y'] = "'",
      ['Y'] = "'",
      ['z'] = "m",
      ['Z'] = "M",
    };

    public LanguageOutput Translate(string phrase, int proficiency)
    {
      return LanguageUtils.TranslateUsingDictionary(this, dictionary, phrase, proficiency);
    }
  }
}
