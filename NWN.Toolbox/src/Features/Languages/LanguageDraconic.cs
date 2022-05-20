using System.Collections.Generic;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageDraconic : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "";

    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    private readonly Dictionary<char, string> dictionary = new Dictionary<char, string>
    {
      ['a'] = "e",
      ['A'] = "E",
      ['b'] = "po",
      ['B'] = "Po",
      ['c'] = "st",
      ['C'] = "St",
      ['d'] = "ty",
      ['D'] = "Ty",
      ['e'] = "i",
      ['E'] = "I",
      ['f'] = "w",
      ['F'] = "W",
      ['g'] = "k",
      ['G'] = "K",
      ['h'] = "ni",
      ['H'] = "Ni",
      ['i'] = "un",
      ['I'] = "Un",
      ['j'] = "vi",
      ['J'] = "Vi",
      ['k'] = "go",
      ['K'] = "Go",
      ['l'] = "ch",
      ['L'] = "Ch",
      ['m'] = "li",
      ['M'] = "Li",
      ['n'] = "ra",
      ['N'] = "Ra",
      ['o'] = "y",
      ['O'] = "Y",
      ['p'] = "ba",
      ['P'] = "Ba",
      ['q'] = "x",
      ['Q'] = "X",
      ['r'] = "hu",
      ['R'] = "Hu",
      ['s'] = "my",
      ['S'] = "My",
      ['t'] = "dr",
      ['T'] = "Dr",
      ['u'] = "on",
      ['U'] = "On",
      ['v'] = "fi",
      ['V'] = "Fi",
      ['w'] = "zi",
      ['W'] = "Zi",
      ['x'] = "qu",
      ['X'] = "Qu",
      ['y'] = "an",
      ['Y'] = "An",
      ['z'] = "ji",
      ['Z'] = "Ji",
    };

    public LanguageOutput Translate(string phrase, int proficiency)
    {
      return LanguageUtils.TranslateUsingDictionary(dictionary, phrase, proficiency);
    }
  }
}
