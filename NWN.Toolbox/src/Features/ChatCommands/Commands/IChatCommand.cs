using System;
using System.Collections.Generic;
using Anvil.API;

namespace Jorteck.Toolbox.Features.ChatCommands
{
  /// <summary>
  /// A game chat command. Implement to add your own chat command.
  /// </summary>
  public interface IChatCommand
  {
    /// <summary>
    /// The default name/alias for this command. Shown in the help list.
    /// </summary>
    string Command { get; }

    /// <summary>
    /// Additional aliases for this command. Not shown in the help list.
    /// </summary>
    string[] Aliases => null;

    /// <summary>
    /// If this command can only be used by DMs.<br/>
    /// Ignored if the Permissions Module is enabled.
    /// </summary>
    bool DMOnly { get; }

    /// <summary>
    /// The permission key required to use this command. Defaults to "command.{Command}", with spaces replaced with "." to indicate sub commands.<br/>
    /// Ignored if the Permissions Module is disabled.
    /// </summary>
    string PermissionKey => $"command.{Command.Replace(" ", ".")}";

    /// <summary>
    /// The number of arguments expected by this command. Supports the range syntax:<br/>
    /// 2 arguments: "2..2"<br/>
    /// 2 or more arguments: "2.."<br/>
    /// No more than 2 arguments: "..2"<br/>
    /// 1-3 arguments: "1..3"
    /// </summary>
    Range ArgCount { get; }

    /// <summary>
    /// The description for this command. Shown in the help list.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// A list of valid usages for this command. Shown in the help list.
    /// </summary>
    CommandUsage[] Usages { get; }

    /// <summary>
    /// Determines if this command should show in help lists, and can be executed.<br/>
    /// This is not for permissions, but raw server requirements (e.g. dependencies)
    /// </summary>
    bool IsAvailable => true;

    /// <summary>
    /// Process this command with the specified arguments.
    /// </summary>
    /// <param name="caller">The calling player of this command.</param>
    /// <param name="args">Any additional arguments specified.</param>
    void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args);
  }
}
