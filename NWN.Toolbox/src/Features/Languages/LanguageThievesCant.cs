using System.Collections.Generic;
using System.Linq;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(ILanguage))]
  public sealed class LanguageThievesCant : ILanguage
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Id => "";
    public bool Enabled => ConfigService.Config.Languages.IsEnabled() && ConfigService.Config.Languages.IsLanguageEnabled(this);

    private readonly Dictionary<char, string> dictionary = new Dictionary<char, string>
    {
      ['a'] = "*shields eyes*",
      ['A'] = "*shields eyes*",
      ['b'] = "*blusters*",
      ['B'] = "*blusters*",
      ['c'] = "*coughs*",
      ['C'] = "*coughs*",
      ['d'] = "*furrows brow*",
      ['D'] = "*furrows brow*",
      ['e'] = "*examines ground*",
      ['E'] = "*examines ground*",
      ['f'] = "*frowns*",
      ['F'] = "*frowns*",
      ['g'] = "*glances up*",
      ['G'] = "*glances up*",
      ['h'] = "*looks thoughtful*",
      ['H'] = "*looks thoughtful*",
      ['i'] = "*looks bored*",
      ['I'] = "*looks bored*",
      ['j'] = "*rubs chin*",
      ['J'] = "*rubs chin*",
      ['k'] = "*scratches ear*",
      ['K'] = "*scratches ear*",
      ['l'] = "*looks around*",
      ['L'] = "*looks around*",
      ['m'] = "*mmm hmm*",
      ['M'] = "*mmm hmm*",
      ['n'] = "*nods*",
      ['N'] = "*nods*",
      ['o'] = "*grins*",
      ['O'] = "*grins*",
      ['p'] = "*smiles*",
      ['P'] = "*smiles*",
      ['q'] = "*shivers*",
      ['Q'] = "*shivers*",
      ['r'] = "*rolls eyes*",
      ['R'] = "*rolls eyes*",
      ['s'] = "*scratches nose*",
      ['S'] = "*scratches nose*",
      ['t'] = "*turns a bit*",
      ['T'] = "*turns a bit*",
      ['u'] = "*glances idly*",
      ['U'] = "*glances idly*",
      ['v'] = "*runs hand through hair*",
      ['V'] = "*runs hand through hair*",
      ['w'] = "*waves*",
      ['W'] = "*waves*",
      ['x'] = "*stretches*",
      ['X'] = "*stretches*",
      ['y'] = "*yawns*",
      ['Y'] = "*yawns*",
      ['z'] = "*shrugs*",
      ['Z'] = "*shrugs*",
    };

    public LanguageOutput Translate(string phrase, int proficiency)
    {
      if (!dictionary.TryGetValue(phrase[0], out string output))
      {
        output = "*nods*";
      }

      return new LanguageOutput
      {
        Interpretation = phrase,
        SpokenText = output,
      };
    }
  }
}
