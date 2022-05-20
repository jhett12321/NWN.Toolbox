using System.Collections.Generic;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageSylvan : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "";
    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    private readonly Dictionary<char, string> dictionary = new Dictionary<char, string>
    {
      ['a'] = "i",
      ['A'] = "I",
      ['b'] = "ri",
      ['B'] = "Ri",
      ['c'] = "ba",
      ['C'] = "Ba",
      ['d'] = "ma",
      ['D'] = "Ma",
      ['e'] = "i",
      ['E'] = "I",
      ['f'] = "mo",
      ['F'] = "Mo",
      ['g'] = "yo",
      ['G'] = "Yo",
      ['h'] = "f",
      ['H'] = "F",
      ['i'] = "ya",
      ['I'] = "Ya",
      ['j'] = "ta",
      ['J'] = "Ta",
      ['k'] = "m",
      ['K'] = "M",
      ['l'] = "t",
      ['L'] = "T",
      ['m'] = "r",
      ['M'] = "R",
      ['n'] = "j",
      ['N'] = "J",
      ['o'] = "nu",
      ['O'] = "Nu",
      ['p'] = "wi",
      ['P'] = "Wi",
      ['q'] = "bo",
      ['Q'] = "Bo",
      ['r'] = "w",
      ['R'] = "W",
      ['s'] = "ne",
      ['S'] = "Ne",
      ['t'] = "na",
      ['T'] = "Na",
      ['u'] = "li",
      ['U'] = "Li",
      ['v'] = "v",
      ['V'] = "V",
      ['w'] = "ni",
      ['W'] = "Ni",
      ['x'] = "ya",
      ['X'] = "Ya",
      ['y'] = "mi",
      ['Y'] = "Mi",
      ['z'] = "og",
      ['Z'] = "Og",
    };

    public LanguageOutput Translate(string phrase, int proficiency)
    {
      return LanguageUtils.TranslateUsingDictionary(dictionary, phrase, proficiency);
    }
  }
}
