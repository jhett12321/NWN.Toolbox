using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageMulhorandi : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "mulhorandi";

    public string Name => "Mulhorandi";

    public Color ChatColor => new Color(255, 255, 51);

    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    private readonly Dictionary<char, string> dictionary = new Dictionary<char, string>
    {
      ['a'] = "ri",
      ['A'] = "Ri",
      ['b'] = "dj",
      ['B'] = "Dj",
      ['c'] = "p",
      ['C'] = "P",
      ['d'] = "al",
      ['D'] = "Al",
      ['e'] = "a",
      ['E'] = "A",
      ['f'] = "j",
      ['F'] = "J",
      ['g'] = "y",
      ['G'] = "Y",
      ['h'] = "u",
      ['H'] = "U",
      ['i'] = "o",
      ['I'] = "O",
      ['j'] = "f",
      ['J'] = "F",
      ['k'] = "ch",
      ['K'] = "Ch",
      ['l'] = "d",
      ['L'] = "D",
      ['m'] = "t",
      ['M'] = "T",
      ['n'] = "m",
      ['N'] = "M",
      ['o'] = "eh",
      ['O'] = "Eh",
      ['p'] = "k",
      ['P'] = "K",
      ['q'] = "ng",
      ['Q'] = "Ng",
      ['r'] = "sh",
      ['R'] = "Sh",
      ['s'] = "th",
      ['S'] = "Th",
      ['t'] = "s",
      ['T'] = "S",
      ['u'] = "e",
      ['U'] = "E",
      ['v'] = "z",
      ['V'] = "Z",
      ['w'] = "p",
      ['W'] = "P",
      ['x'] = "qu",
      ['X'] = "Qu",
      ['y'] = "o",
      ['Y'] = "O",
      ['z'] = "z",
      ['Z'] = "Z",
    };

    public LanguageOutput Translate(string phrase, int proficiency)
    {
      return LanguageUtils.TranslateUsingDictionary(this, dictionary, phrase, proficiency);
    }
  }
}
