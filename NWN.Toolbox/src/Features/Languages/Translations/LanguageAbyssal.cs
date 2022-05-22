using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageAbyssal : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "abyssal";

    public string Name => "Abyssal";

    public Color ChatColor => new Color(160, 160, 160);

    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    private readonly Dictionary<char, string> dictionary = new Dictionary<char, string>
    {
      ['a'] = "oo",
      ['A'] = "OO",
      ['b'] = "n",
      ['B'] = "N",
      ['c'] = "m",
      ['C'] = "M",
      ['d'] = "g",
      ['D'] = "G",
      ['e'] = "a",
      ['E'] = "A",
      ['f'] = "k",
      ['F'] = "K",
      ['g'] = "s",
      ['G'] = "S",
      ['h'] = "d",
      ['H'] = "D",
      ['i'] = "oo",
      ['I'] = "OO",
      ['j'] = "h",
      ['J'] = "H",
      ['k'] = "b",
      ['K'] = "B",
      ['l'] = "l",
      ['L'] = "L",
      ['m'] = "p",
      ['M'] = "P",
      ['n'] = "t",
      ['N'] = "T",
      ['o'] = "e",
      ['O'] = "E",
      ['p'] = "b",
      ['P'] = "B",
      ['q'] = "ch",
      ['Q'] = "Ch",
      ['r'] = "n",
      ['R'] = "N",
      ['s'] = "m",
      ['S'] = "M",
      ['t'] = "g",
      ['T'] = "G",
      ['u'] = "ae",
      ['U'] = "Ae",
      ['v'] = "ts",
      ['V'] = "Ts",
      ['w'] = "b",
      ['W'] = "B",
      ['x'] = "bb",
      ['X'] = "Bb",
      ['y'] = "ee",
      ['Y'] = "Ee",
      ['z'] = "t",
      ['Z'] = "T",
    };

    public LanguageOutput Translate(string phrase, int proficiency)
    {
      return LanguageUtils.TranslateUsingDictionary(this, dictionary, phrase, proficiency);
    }
  }
}
