using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageInfernal : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "infernal";

    public string Name => "Infernal";

    public Color ChatColor => new Color(255, 51, 51);

    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    private readonly Dictionary<char, string> dictionary = new Dictionary<char, string>
    {
      ['a'] = "o",
      ['A'] = "O",
      ['b'] = "c",
      ['B'] = "C",
      ['c'] = "r",
      ['C'] = "R",
      ['d'] = "j",
      ['D'] = "J",
      ['e'] = "a",
      ['E'] = "A",
      ['f'] = "v",
      ['F'] = "V",
      ['g'] = "k",
      ['G'] = "K",
      ['h'] = "r",
      ['H'] = "R",
      ['i'] = "y",
      ['I'] = "Y",
      ['j'] = "z",
      ['J'] = "Z",
      ['k'] = "g",
      ['K'] = "G",
      ['l'] = "m",
      ['L'] = "M",
      ['m'] = "z",
      ['M'] = "Z",
      ['n'] = "r",
      ['N'] = "R",
      ['o'] = "y",
      ['O'] = "Y",
      ['p'] = "k",
      ['P'] = "K",
      ['q'] = "r",
      ['Q'] = "R",
      ['r'] = "n",
      ['R'] = "N",
      ['s'] = "k",
      ['S'] = "K",
      ['t'] = "d",
      ['T'] = "D",
      ['u'] = "'",
      ['U'] = "'",
      ['v'] = "r",
      ['V'] = "R",
      ['w'] = "'",
      ['W'] = "'",
      ['x'] = "k",
      ['X'] = "K",
      ['y'] = "i",
      ['Y'] = "I",
      ['z'] = "g",
      ['Z'] = "G",
    };

    public LanguageOutput Translate(string phrase, int proficiency)
    {
      return LanguageUtils.TranslateUsingDictionary(this, dictionary, phrase, proficiency);
    }
  }
}
