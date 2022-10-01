using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anvil.API;
using Anvil.Services;
using NLog;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(LanguageService))]
  public sealed class LanguageService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly Dictionary<string, ILanguage> languages = new Dictionary<string, ILanguage>();

    public IReadOnlyCollection<ILanguage> Languages => languages.Values;

    public LanguageService(IReadOnlyList<ILanguage> languages)
    {
      foreach (ILanguage language in languages)
      {
        RegisterLanguage(language);
      }
    }

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
      foreach (ILanguage availableLanguage in languages.Values)
      {
        if (key == availableLanguage.Id || availableLanguage.Aliases?.Contains(key) == true)
        {
          language = availableLanguage;
          return true;
        }
      }

      language = null;
      return false;
    }

    public void RegisterLanguage(ILanguage language)
    {
      if (languages.TryAdd(language.Id, language))
      {
        Log.Debug($"Registered language {language.Name} ({language.Id})");
      }
      else
      {
        Log.Error($"Cannot register language as the ID {language.Id} is already in use.");
      }
    }

    public void ListPlayerLanguages(NwPlayer showTo, NwPlayer target)
    {
      LanguageState languageState = GetStateForPlayer(target);
      if (languageState == null)
      {
        return;
      }

      StringBuilder message = new StringBuilder();
      message.AppendLine($"===={target.ControlledCreature?.Name} - Known languages====");

      foreach (KeyValuePair<string, int> languageData in languageState.LanguageProficiencies.OrderBy(pair => pair.Key))
      {
        if (TryGetLanguage(languageData.Key, out ILanguage language))
        {
          message.AppendLine($"{language.Name.ColorString(language.ChatColor)} ({language.Id}): {GetFluencyNameFromProficiency(languageData.Value)} ({languageData.Value})");
        }
      }

      message.AppendLine("=========");
      showTo.SendServerMessage(message.ToString());
    }

    private string GetFluencyNameFromProficiency(int proficiency)
    {
      return proficiency switch
      {
        >= LanguageProficiency.Fluent => "Fluent",
        >= LanguageProficiency.Advanced => "Advanced",
        >= LanguageProficiency.Intermediate => "Intermediate",
        >= LanguageProficiency.Beginner => "Beginner",
        _ => "Untrained",
      };
    }
  }
}
