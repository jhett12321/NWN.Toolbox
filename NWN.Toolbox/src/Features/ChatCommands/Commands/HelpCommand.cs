using System;
using System.Collections.Generic;
using System.Text;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Config;

namespace Jorteck.Toolbox.Features.ChatCommands
{
  [ServiceBinding(typeof(HelpCommand))]
  [ServiceBinding(typeof(IChatCommand))]
  public sealed class HelpCommand : IChatCommand
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    [Inject]
    private Lazy<ChatCommandService> ChatCommandService { get; init; }

    public string Command => "help";
    public int? ArgCount => null;
    public bool DMOnly => false;
    public string Description => "Shows this command list, or help for a specific command.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("Show a list of all available commands."),
      new CommandUsage("<command>", "Show help for a specific command."),
    };

    private readonly StringBuilder stringBuilder = new StringBuilder();

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      if (args.Count == 0)
      {
        ShowAvailableCommandsToPlayer(caller);
      }
      else
      {
        string specifiedCommand = string.Join(" ", args);
        IChatCommand command = GetCommand(specifiedCommand);

        if (command != null)
        {
          ShowCommandHelpToPlayer(caller, command);
        }
        else
        {
          caller.SendServerMessage($"Unknown Command {specifiedCommand}.", ColorConstants.Red);
        }
      }
    }

    public void ShowAvailableCommandsToPlayer(NwPlayer player)
    {
      IEnumerable<IChatCommand> availableCommands = GetAvailableCommands(player);
      string message = GetCommandHelp(availableCommands);
      player.SendServerMessage(message, ColorConstants.White);
    }

    public void ShowCommandHelpToPlayer(NwPlayer player, IChatCommand command)
    {
      string message = GetCommandHelp(command);
      player.SendServerMessage(message, ColorConstants.White);
    }

    public string GetCommandHelp(IChatCommand command)
    {
      try
      {
        string formattedBaseCommand = $"{ConfigService.Config.ChatCommands.CommandPrefixes[0]}{command.Command}".ColorString(ColorConstants.Orange);
        stringBuilder.AppendLine("=================");
        stringBuilder.Append(formattedBaseCommand);
        stringBuilder.Append(": ");
        stringBuilder.AppendLine(command.Description);

        if (command.Usages != null && command.Usages.Length > 0)
        {
          stringBuilder.AppendLine();
          stringBuilder.AppendLine("==Usages==");
          foreach (CommandUsage usage in command.Usages)
          {
            stringBuilder.Append(formattedBaseCommand);
            if (!string.IsNullOrEmpty(usage.SubCommand))
            {
              stringBuilder.Append(' ');
              stringBuilder.Append(usage.SubCommand.ColorString(ColorConstants.Orange));
            }

            stringBuilder.Append(": ");
            stringBuilder.AppendLine(usage.Description);
          }
        }

        stringBuilder.Append("=================");
        return stringBuilder.ToString().ColorString(ColorConstants.White);
      }
      finally
      {
        stringBuilder.Clear();
      }
    }

    public string GetCommandHelp(IEnumerable<IChatCommand> commands)
    {
      try
      {
        stringBuilder.AppendLine("=================");
        stringBuilder.AppendLine($"Use \"{ConfigService.Config.ChatCommands.CommandPrefixes[0]}help <command>\" for help on a specific command.".ColorString(ColorConstants.Silver));

        foreach (IChatCommand command in commands)
        {
          stringBuilder.Append($"{ConfigService.Config.ChatCommands.CommandPrefixes[0]}{command.Command}".ColorString(ColorConstants.Orange));
          stringBuilder.Append(": ");
          stringBuilder.AppendLine(command.Description);
        }

        stringBuilder.Append("=================");
        return stringBuilder.ToString().ColorString(ColorConstants.White);
      }
      finally
      {
        stringBuilder.Clear();
      }
    }

    private IEnumerable<IChatCommand> GetAvailableCommands(NwPlayer player)
    {
      foreach (IChatCommand command in ChatCommandService.Value.Commands)
      {
        if (command.IsAvailable && ChatCommandService.Value.CanUseCommand(player, command))
        {
          yield return command;
        }
      }
    }

    private IChatCommand GetCommand(string commandName)
    {
      foreach (IChatCommand command in ChatCommandService.Value.Commands)
      {
        if (command.Command == commandName)
        {
          return command;
        }
      }

      return null;
    }
  }
}
