using System;
using System.Collections.Generic;

namespace Jorteck.Toolbox.Features.Languages
{
  [Serializable]
  public sealed class LanguageState
  {
    public Dictionary<string, int> LanguageProficiencies { get; set; } = new Dictionary<string, int>();

    public string CurrentLanguageId { get; set; }
  }
}
