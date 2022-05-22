using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Core;
using Jorteck.Toolbox.Features.ChatCommands;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(IChatCommand))]
  public sealed class LanguageCommand : IChatCommand
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    [Inject]
    private HelpCommand HelpCommand { get; init; }

    [Inject]
    private LanguageService LanguageService { get; init; }

    [Inject]
    private LanguageChatService LanguageChatService { get; init; }

    public string Command => "language";
    public string[] Aliases => new[] { "lang" };
    public Range ArgCount => 1..;
    public bool DMOnly => false;
    public bool IsAvailable => ConfigService.Config.Languages.IsEnabled();

    public string Description => "Commands for speaking different languages.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("list", "Lists languages your character can speak."),
      new CommandUsage("<language>", "Switches your active language to the language specified."),
      new CommandUsage("say <language> <message>", "Speaks the given message in the language specified."),
      new CommandUsage("whisper <language> <message>", "Whispers the given message in the language specified."),
      new CommandUsage("party <language> <message>", "Party shouts the given message in the language specified."),
      new CommandUsage("mode <native/translated/both>", "Changes how chat messages that are understood by your character display in the chat box."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      LanguageState languageState = LanguageService.GetStateForPlayer(caller);

      switch (args[0])
      {
        case "list":
          List<string> languages = languageState?.LanguageProficiencies?.Keys.ToList();
          if (languages != null && languages.Count > 0)
          {
            caller.SendServerMessage($"Known languages: {string.Join(',', languages)}");
          }
          else
          {
            caller.SendServerMessage("You have no known languages.");
          }

          break;
        case "say" when args.Count > 2:
          SpeakMessage(caller, languageState, args[1], string.Join(' ', args.Skip(2)), TalkVolume.Talk);
          break;
        case "whisper" when args.Count > 2:
          SpeakMessage(caller, languageState, args[1], string.Join(' ', args.Skip(2)), TalkVolume.Whisper);
          break;
        case "party" when args.Count > 2:
          SpeakMessage(caller, languageState, args[1], string.Join(' ', args.Skip(2)), TalkVolume.Party);
          break;
        case "mode" when args.Count == 2:
          if (Enum.TryParse(args[1], true, out LanguageDisplayType newType))
          {
            LanguageService.SetDisplayType(caller, newType);
            caller.SendServerMessage($"Changed chat display type to {newType}.");
          }
          else
          {
            caller.SendErrorMessage($"Unknown display type \"{args[1]}\".");
          }

          break;
        default:
          if (args.Count == 1 && LanguageService.TryGetLanguage(args[0], out ILanguage language))
          {
            if (LanguageService.PlayerKnowsLanguage(caller, language, languageState))
            {
              LanguageService.UpdateLanguageState(caller, state =>
              {
                state.CurrentLanguageId = language.Id;
              });

              caller.SendServerMessage($"Changed active language to {language.Name}.");
            }
            else
            {
              caller.SendErrorMessage($"You have not learned {language.Name} and cannot speak it.");
            }
          }
          else
          {
            HelpCommand.ShowCommandHelpToPlayer(caller, this);
          }

          break;
      }
    }

    private void SpeakMessage(NwPlayer sender, LanguageState languageState, string languageKey, string message, TalkVolume volume)
    {
      if (LanguageService.TryGetLanguage(languageKey, out ILanguage language))
      {
        int? languageProficiency = LanguageService.GetLanguageProficiency(sender, language, languageState);
        if (languageProficiency != null)
        {
          LanguageOutput output = language.Translate(message, languageProficiency.Value);
          LanguageChatService.SendTranslatedMessage(sender, output, volume);
        }
        else
        {
          sender.SendErrorMessage($"You have not learned {language.Name} and cannot speak it.");
        }
      }
      else
      {
        sender.SendErrorMessage($"Unknown language \"{languageKey}\"");
      }
    }
  }
}

