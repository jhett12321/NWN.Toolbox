using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using Jorteck.Toolbox.Config;

namespace Jorteck.Toolbox.Features.ChatCommands
{
  [ServiceBinding(typeof(ChatCommandService))]
  internal sealed class ChatCommandService
  {
    private readonly ConfigService configService;

    private readonly CommandListProvider commandListProvider;
    private readonly HelpCommand helpCommand;
    private readonly string helpCommandText;

    public ChatCommandService(CommandListProvider commandListProvider, ConfigService configService, HelpCommand helpCommand)
    {
      if (!configService.Config.ChatCommands.IsEnabled())
      {
        return;
      }

      if (configService.Config.ChatCommands.CommandPrefixes == null || configService.Config.ChatCommands.CommandPrefixes.Count == 0)
      {
        throw new InvalidOperationException("No command prefixes defined!");
      }

      this.commandListProvider = commandListProvider;
      this.configService = configService;
      this.helpCommand = helpCommand;
      helpCommandText = $"{configService.Config.ChatCommands.CommandPrefixes[0]}{helpCommand.Command}".ColorString(ColorConstants.Orange);

      NwModule.Instance.OnChatMessageSend += OnChatMessageSend;
    }

    private void OnChatMessageSend(OnChatMessageSend eventData)
    {
      if (!eventData.Sender.IsPlayerControlled(out NwPlayer player))
      {
        return;
      }

      foreach (string commandPrefix in configService.Config.ChatCommands.CommandPrefixes)
      {
        if (eventData.Message.StartsWith(commandPrefix))
        {
          string commandText = eventData.Message[commandPrefix.Length..];
          if (ProcessCommandText(player, commandText))
          {
            eventData.Skip = true;
          }

          return;
        }
      }
    }

    private bool ProcessCommandText(NwPlayer player, string commandText)
    {
      if (ShouldIgnoreCommand(commandText))
      {
        return false;
      }

      bool validCommand = TryProcessCommand(player, commandText);
      if (!validCommand)
      {
        player.SendErrorMessage($"Unknown command. Type {helpCommandText} for help.");
      }

      return true;
    }

    private bool ShouldIgnoreCommand(string commandText)
    {
      foreach (string ignoreCommand in configService.Config.ChatCommands.IgnoreCommands)
      {
        if (commandText.StartsWith(ignoreCommand))
        {
          return true;
        }
      }

      return false;
    }

    private bool TryProcessCommand(NwPlayer sender, string rawCommand)
    {
      foreach (IChatCommand command in commandListProvider.Commands)
      {
        if (rawCommand == command.Command)
        {
          TryExecuteCommand(sender, command, ImmutableArray<string>.Empty);
          return true;
        }
      }

      foreach (IChatCommand command in commandListProvider.Commands)
      {
        if (rawCommand.StartsWith(command.Command))
        {
          string[] args = GetArgs(rawCommand[command.Command.Length..]);
          TryExecuteCommand(sender, command, args);
          return true;
        }
      }

      return false;
    }

    private string[] GetArgs(string rawArgs)
    {
      return Regex.Matches(rawArgs, @"\""(\""\""|[^\""])+\""|[^ ]+", RegexOptions.ExplicitCapture)
        .Select(m => m.Value.StartsWith("\"") ? m.Value.Substring(1, m.Length - 2).Replace("\"\"", "\"") : m.Value)
        .ToArray();
    }

    private void TryExecuteCommand(NwPlayer sender, IChatCommand command, IReadOnlyList<string> args)
    {
      if (!command.IsAvailable)
      {
        ShowCommandUnavailableError(command, sender);
      }
      else if (!commandListProvider.CanUseCommand(sender, command))
      {
        ShowNoPermissionError(sender);
      }
      else if (command.ArgCount.HasValue && command.ArgCount != args.Count)
      {
        TryExecuteCommand(sender, helpCommand, new[] { command.Command });
      }
      else
      {
        command.ProcessCommand(sender, args);
      }
    }

    private void ShowCommandUnavailableError(IChatCommand command, NwPlayer player)
    {
      player.SendErrorMessage($"Command \"{command.Command}\" is unavailable at this time.");
    }

    private void ShowNoPermissionError(NwPlayer player)
    {
      player.SendErrorMessage("You do not have permission to do that.");
    }
  }
}
