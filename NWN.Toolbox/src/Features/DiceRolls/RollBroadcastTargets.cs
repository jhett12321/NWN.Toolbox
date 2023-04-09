using System;
using Jorteck.Toolbox.Features.Chat;

namespace Jorteck.Toolbox.Features
{
  [Flags]
  public enum RollBroadcastTargets
  {
    /// <summary>
    /// Don't broadcast the roll
    /// </summary>
    None = 0,

    /// <summary>
    /// The player's combat log
    /// </summary>
    PrivateLog = 1 << 0,

    /// <summary>
    /// The player's chat log
    /// </summary>
    PrivateChat = 1 << 1,

    /// <summary>
    /// Broadcast in <see cref="ChatVolume.Talk"/>, shown to nearby players.
    /// </summary>
    LocalTalk = 1 << 2,

    /// <summary>
    /// The DM chat channel
    /// </summary>
    DM = 1 << 3,
  }
}
