using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageGnome : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "gnome";

    public string Name => "Gnome";

    public string[] Aliases => new[] { "gnomish" };

    public Color ChatColor => new Color(51, 51, 255);

    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    private readonly Dictionary<char, string> dictionary = new Dictionary<char, string>
    {
      ['a'] = "y",
      ['A'] = "Y",
      ['b'] = "p",
      ['B'] = "P",
      ['c'] = "l",
      ['C'] = "L",
      ['d'] = "t",
      ['D'] = "T",
      ['e'] = "a",
      ['E'] = "A",
      ['f'] = "v",
      ['F'] = "V",
      ['g'] = "k",
      ['G'] = "K",
      ['h'] = "r",
      ['H'] = "R",
      ['i'] = "e",
      ['I'] = "E",
      ['j'] = "z",
      ['J'] = "Z",
      ['k'] = "g",
      ['K'] = "G",
      ['l'] = "m",
      ['L'] = "M",
      ['m'] = "s",
      ['M'] = "S",
      ['n'] = "h",
      ['N'] = "H",
      ['o'] = "u",
      ['O'] = "U",
      ['p'] = "b",
      ['P'] = "B",
      ['q'] = "x",
      ['Q'] = "X",
      ['r'] = "n",
      ['R'] = "N",
      ['s'] = "c",
      ['S'] = "C",
      ['t'] = "d",
      ['T'] = "D",
      ['u'] = "i",
      ['U'] = "I",
      ['v'] = "j",
      ['V'] = "J",
      ['w'] = "f",
      ['W'] = "F",
      ['x'] = "q",
      ['X'] = "Q",
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
