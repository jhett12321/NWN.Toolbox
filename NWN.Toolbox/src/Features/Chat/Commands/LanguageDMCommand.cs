using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;
using Jorteck.Toolbox.Features.Languages;

namespace Jorteck.Toolbox.Features.Chat
{
  [ServiceBinding(typeof(IChatCommand))]
  public sealed class LanguageDMCommand : IChatCommand
  {
    [Inject]
    private LanguageService LanguageService { get; init; }

    [Inject]
    private ConfigService ConfigService { get; init; }

    [Inject]
    private HelpCommand HelpCommand { get; init; }

    public string Command => "languagedm";
    public string[] Aliases => new[] { "langdm" };
    public Range ArgCount => 1..3;
    public bool DMOnly => true;
    public bool IsAvailable => ConfigService.Config.Languages.IsEnabled();

    public string Description => "Manage character languages.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("grant <language> <proficiency>", "Grants the specified language and proficiency."),
      new CommandUsage("remove <language>", "Removes the specified language from the character's known languages."),
      new CommandUsage("list", "Lists a character's known languages & proficiency."),
      new CommandUsage("list all", "Lists available languages that may be granted to players."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      switch (args[0])
      {
        case "grant" when args.Count == 3:
          if (LanguageService.TryGetLanguage(args[1], out ILanguage language) && args[2].TryParseInt(out int proficiency))
          {
            caller.EnterPlayerTargetMode(eventData => AddLanguage(eventData, language, proficiency));
          }
          else
          {
            caller.SendErrorMessage($"Unknown language {args[1]}.");
            HelpCommand.ShowCommandHelpToPlayer(caller, this);
          }

          break;
        case "remove" when args.Count == 2:
          caller.EnterPlayerTargetMode(eventData => RemoveLanguage(eventData, args[1]));
          break;
        case "list" when args.Count == 1:
          caller.EnterPlayerTargetMode(ListPlayerLanguages);
          break;
        case "list" when args.Count == 2 && args[1] == "all":
          caller.EnterPlayerTargetMode(ListAvailableLanguages);
          break;
        default:
          HelpCommand.ShowCommandHelpToPlayer(caller, this);
          break;
      }
    }

    private void AddLanguage(NwPlayerExtensions.PlayerTargetPlayerEvent eventData, ILanguage language, int proficiency)
    {
      LanguageService.UpdateLanguageState(eventData.Target, state =>
      {
        state.LanguageProficiencies ??= new Dictionary<string, int>();
        state.LanguageProficiencies[language.Id] = proficiency;
      });
    }

    private void RemoveLanguage(NwPlayerExtensions.PlayerTargetPlayerEvent eventData, string languageId)
    {
      LanguageService.UpdateLanguageState(eventData.Target, state =>
      {
        if (state.LanguageProficiencies?.Remove(languageId) == true)
        {
          eventData.Caller.SendServerMessage($"Successfully removed known language \"{languageId}\" {eventData.Target.ControlledCreature?.Name}");
        }
        else
        {
          eventData.Caller.SendErrorMessage($"Could not find known language \"{languageId}\"");
        }
      });
    }

    private void ListPlayerLanguages(NwPlayerExtensions.PlayerTargetPlayerEvent eventData)
    {
      LanguageState languageState = LanguageService.GetStateForPlayer(eventData.Target);
      if (languageState?.LanguageProficiencies == null || languageState.LanguageProficiencies.Count == 0)
      {
        eventData.Caller.SendServerMessage($"{eventData.Target.ControlledCreature?.Name} has no known languages.");
        return;
      }

      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine($"{eventData.Target.ControlledCreature?.Name} - Known languages");

      foreach (ILanguage language in LanguageService.Languages.OrderBy(language => language.Name))
      {
        if (language.Enabled && languageState.LanguageProficiencies.ContainsKey(language.Id))
        {
          stringBuilder.AppendLine($"{language.Name.ColorString(language.ChatColor)} ({language.Id})");
        }
      }

      eventData.Caller.SendServerMessage(stringBuilder.ToString());
    }

    private void ListAvailableLanguages(NwPlayerExtensions.PlayerTargetPlayerEvent eventData)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Available languages:");

      foreach (ILanguage language in LanguageService.Languages.OrderBy(language => language.Name))
      {
        if (language.Enabled)
        {
          stringBuilder.AppendLine($"{language.Name.ColorString(language.ChatColor)} ({language.Id})");
        }
      }

      eventData.Caller.SendServerMessage(stringBuilder.ToString());
    }
  }
}
