using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.Services;

namespace Jorteck.Toolbox.Features.Chat
{
  [ServiceBinding(typeof(IChatCommand))]
  public class ShortcutCommand : IChatCommand
  {
    [Inject]
    private HelpCommand HelpCommand { get; init; }

    [Inject]
    private ChatShortcutService ChatShortcutService { get; init; }

    public string Command => "shortcut";
    public Range ArgCount => 1..;
    public bool DMOnly => false;
    public string Description => "Create shortcuts for messages and other commands.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("set <shortcut> <message>", "Set a shortcut that expands to the specified message/command."),
      new CommandUsage("unset <shortcut>", "Unset the specified shortcut."),
      new CommandUsage("clear", "Clear all shortcuts."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      switch (args[0])
      {
        case "set" when args.Count > 2:
          ChatShortcutService.SetShortcut(caller, args[1], string.Join(' ', args.Skip(2)));
          break;
        case "unset" when args.Count == 2:
          ChatShortcutService.UnsetShortcut(caller, args[1]);
          break;
        case "clear" when args.Count == 1:
          ChatShortcutService.ClearShortcuts(caller);
          break;
        default:
          HelpCommand.ShowCommandHelpToPlayer(caller, this);
          break;
      }
    }
  }
}
