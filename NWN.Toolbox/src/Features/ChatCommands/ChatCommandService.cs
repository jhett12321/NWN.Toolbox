using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using Jorteck.Toolbox.Core;
using Jorteck.Toolbox.Features.Permissions;

namespace Jorteck.Toolbox.Features.ChatCommands
{
  [ServiceBinding(typeof(ChatCommandService))]
  internal sealed class ChatCommandService : IInitializable
  {
    private string helpCommandText;

    [Inject]
    private ConfigService ConfigService { get; init; }

    [Inject]
    private PermissionsService PermissionsService { get; init; }

    [Inject]
    public IReadOnlyList<IChatCommand> Commands { get; set; }

    [Inject]
    private HelpCommand HelpCommand { get; init; }

    public void Init()
    {
      if (!ConfigService.Config.ChatCommands.IsEnabled())
      {
        return;
      }

      if (ConfigService.Config.ChatCommands.CommandPrefixes == null || ConfigService.Config.ChatCommands.CommandPrefixes.Count == 0)
      {
        throw new InvalidOperationException("No command prefixes defined!");
      }

      Commands = Commands.OrderBy(command => command.Command).ToList();
      helpCommandText = $"{ConfigService.Config.ChatCommands.CommandPrefixes[0]}{HelpCommand.Command}".ColorString(ColorConstants.Orange);

      NwModule.Instance.OnChatMessageSend += OnChatMessageSend;
    }

    public bool CanUseCommand(NwPlayer player, IChatCommand command)
    {
      return PermissionsService.HasPermission(player, command.PermissionKey, !command.DMOnly || player.IsDM);
    }

    private void OnChatMessageSend(OnChatMessageSend eventData)
    {
      if (!eventData.Sender.IsPlayerControlled(out NwPlayer player))
      {
        return;
      }

      foreach (string commandPrefix in ConfigService.Config.ChatCommands.CommandPrefixes)
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
      foreach (string ignoreCommand in ConfigService.Config.ChatCommands.IgnoreCommands)
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
      foreach (IChatCommand command in Commands)
      {
        if (rawCommand == command.Command || command.Aliases?.Any(commandAlias => rawCommand == commandAlias) == true)
        {
          TryExecuteCommand(sender, command, ImmutableArray<string>.Empty);
          return true;
        }
      }

      foreach (IChatCommand command in Commands)
      {
        if (rawCommand.StartsWith(command.Command + " "))
        {
          string[] args = GetArgs(rawCommand[command.Command.Length..]);
          TryExecuteCommand(sender, command, args);
          return true;
        }

        if (command.Aliases == null)
        {
          continue;
        }

        foreach (string commandAlias in command.Aliases)
        {
          if (rawCommand.StartsWith(commandAlias))
          {
            string[] args = GetArgs(rawCommand[commandAlias.Length..]);
            TryExecuteCommand(sender, command, args);
            return true;
          }
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
      else if (!CanUseCommand(sender, command))
      {
        ShowNoPermissionError(sender);
      }
      else if (!IsValidArgCount(command.ArgCount, args.Count))
      {
        TryExecuteCommand(sender, HelpCommand, new[] { command.Command });
      }
      else
      {
        command.ProcessCommand(sender, args);
      }
    }

    private bool IsValidArgCount(Range commandRange, int argCount)
    {
      if (commandRange.Start.IsFromEnd || commandRange.End.IsFromEnd && commandRange.End.Value != 0)
      {
        throw new InvalidOperationException("Invalid arg count range specified.");
      }

      int min = commandRange.Start.Value;
      int max = commandRange.End.IsFromEnd ? int.MaxValue : commandRange.End.Value;

      return argCount >= min && argCount <= max;
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
