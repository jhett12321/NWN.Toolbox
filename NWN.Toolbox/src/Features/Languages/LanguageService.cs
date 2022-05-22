using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.Services;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(LanguageService))]
  public sealed class LanguageService
  {
    [Inject]
    private IReadOnlyList<ILanguage> Languages { get; init; }

    public bool PlayerKnowsLanguage(NwPlayer player, ILanguage language, LanguageState languageState)
    {
      return GetLanguageProficiency(player, language, languageState) != null;
    }

    public int? GetLanguageProficiency(NwPlayer player, ILanguage language, LanguageState languageState)
    {
      if (player.IsDM)
      {
        return LanguageProficiency.Fluent;
      }

      if (languageState?.LanguageProficiencies == null)
      {
        return null;
      }

      return languageState.LanguageProficiencies.TryGetValue(language.Id, out int proficiency) ? proficiency : null;
    }

    public LanguageState GetStateForPlayer(NwPlayer player)
    {
      PersistentVariableStruct<LanguageState> state = player.ControlledCreature?.GetObjectVariable<PersistentVariableStruct<LanguageState>>("toolbox_languages");
      return state?.HasValue == true ? state.Value : new LanguageState();
    }

    public void UpdateLanguageState(NwPlayer player, Action<LanguageState> transaction)
    {
      LanguageState state = GetStateForPlayer(player) ?? new LanguageState();
      transaction(state);
      player.ControlledCreature!.GetObjectVariable<PersistentVariableStruct<LanguageState>>("toolbox_languages").Value = state;
    }

    public LanguageDisplayType GetDisplayType(NwPlayer player)
    {
      return player.ControlledCreature!.GetObjectVariable<PersistentVariableEnum<LanguageDisplayType>>("toolbox_languages_display");
    }

    public void SetDisplayType(NwPlayer player, LanguageDisplayType displayType)
    {
      player.ControlledCreature!.GetObjectVariable<PersistentVariableEnum<LanguageDisplayType>>("toolbox_languages_display").Value = displayType;
    }

    public bool TryGetLanguage(string key, out ILanguage language)
    {
      foreach (ILanguage availableLanguage in Languages)
      {
        if (key == availableLanguage.Id || availableLanguage.Aliases != null && availableLanguage.Aliases.Contains(key))
        {
          language = availableLanguage;
          return true;
        }
      }

      language = null;
      return false;
    }
  }
}
