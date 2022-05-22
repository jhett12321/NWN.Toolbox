using System.ComponentModel;
using Jorteck.Toolbox.Features.Languages;

namespace Jorteck.Toolbox.Core
{
  public sealed class LanguageConfig : IFeatureConfig
  {
    [Description("Enable/disable the language system.")]
    public bool Enabled { get; set; } = false;

    [Description("Should the built-in languages be enabled? (Abyssal, Animal, Celestial, Draconic, Drow, Dwarven, Elven, Gnome, Goblin, Halfling, Infernal, Mulhorandi, Orc, Rashemi, Sylvan, Thieves' Cant)")]
    public bool EnableBuiltIn { get; set; } = true;

    // ReSharper disable once UnusedParameter.Global
    internal bool IsLanguageEnabled(ILanguage language)
    {
      return EnableBuiltIn;
    }
  }
}
