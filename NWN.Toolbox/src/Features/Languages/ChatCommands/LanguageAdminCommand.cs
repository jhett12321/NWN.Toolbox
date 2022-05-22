using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Features.ChatCommands;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(IChatCommand))]
  public sealed class LanguageAdminCommand : IChatCommand
  {
    [Inject]
    private LanguageService LanguageService { get; init; }

    [Inject]
    private HelpCommand HelpCommand { get; init; }

    public string Command => "languageadmin";

    public string[] Aliases => null;

    public bool DMOnly => true;

    public Range ArgCount => 3..3;

    public string Description => "Manage character languages.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("add <language> <proficiency>", "Grants the specified language and proficiency."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      switch (args[0])
      {
        case "add" when args.Count == 3:
          if (LanguageService.TryGetLanguage(args[1], out ILanguage language) && args[2].TryParseInt(out int proficiency))
          {
            caller.EnterPlayerTargetMode((eventData) => AddLanguage(eventData, language, proficiency));
          }
          else
          {
            goto default;
          }

          break;
        default:
          HelpCommand.ShowCommandHelpToPlayer(caller, this);
          break;
      }
    }

    private void AddLanguage(NwPlayerExtensions.PlayerTargetPlayerEvent eventData, ILanguage language, int proficiency)
    {
      throw new NotImplementedException();
    }
  }
}

