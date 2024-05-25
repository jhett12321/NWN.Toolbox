using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core.Persistence;
using NLog;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(LanguageService))]
  public sealed class LanguageService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly PersistenceStorageService persistenceStorageService;
    private readonly Dictionary<string, ILanguage> languages = new Dictionary<string, ILanguage>();

    public IReadOnlyCollection<ILanguage> Languages => languages.Values;

    public LanguageService(IReadOnlyList<ILanguage> languages, PersistenceStorageService persistenceStorageService)
    {
      this.persistenceStorageService = persistenceStorageService;
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
      LanguageState state = persistenceStorageService.GetState<LanguageState>(player, "toolbox_languages") ?? new LanguageState();
      return state;
    }

    public void UpdateLanguageState(NwPlayer player, Action<LanguageState> transaction)
    {
      LanguageState state = GetStateForPlayer(player) ?? new LanguageState();
      transaction(state);
      persistenceStorageService.UpdateState(player, "toolbox_languages", state);
    }

    public LanguageDisplayType GetDisplayType(NwPlayer player)
    {
      return GetStateForPlayer(player).DisplayType;
    }

    public void SetDisplayType(NwPlayer player, LanguageDisplayType displayType)
    {
      UpdateLanguageState(player, state =>
      {
        state.DisplayType = displayType;
      });
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
      if (!language.Enabled)
      {
        return;
      }

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
