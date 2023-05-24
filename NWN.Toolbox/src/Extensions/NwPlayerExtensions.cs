using System;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Toolbox
{
  internal static class NwPlayerExtensions
  {
    public static void SendErrorMessage(this NwPlayer player, string message)
    {
      player.SendServerMessage(message, ColorConstants.Red);
    }

    private static string GetObjectNameWithAccountNameAndCDKey(this NwObject gameObject)
    {
      if (gameObject.IsPlayerControlled(out NwPlayer player))
      {
        return $"{gameObject.Name.StripColors()} ({player.PlayerName}, {player.CDKey})";
      }

      return $"{gameObject.Name.StripColors()}";
    }

    internal static void EnterPlayerTargetMode(this NwPlayer player, Action<PlayerTargetPlayerEvent> callback, MouseCursor cursorType = MouseCursor.Magic, MouseCursor badTargetCursor = MouseCursor.NoMagic)
    {
      void TargetHandler(ModuleEvents.OnPlayerTarget eventData)
      {
        if (eventData.IsCancelled)
        {
          return;
        }

        if (eventData.TargetObject is NwCreature targetCreature && targetCreature.IsLoginPlayerCharacter(out NwPlayer target))
        {
          NwPlayer caller = eventData.Player;
          caller.SendServerMessage($"Target: {targetCreature.GetObjectNameWithAccountNameAndCDKey()}", ColorConstants.Pink);
          callback?.Invoke(new PlayerTargetPlayerEvent(caller, target));
        }
        else
        {
          eventData.Player.SendErrorMessage("Target must be a player character.");
        }
      }

      player.EnterTargetMode(TargetHandler, new TargetModeSettings
      {
        ValidTargets = ObjectTypes.Creature,
        CursorType = cursorType,
        BadCursorType = badTargetCursor,
      });
    }

    internal sealed class PlayerTargetPlayerEvent
    {
      public NwPlayer Caller { get; }
      public NwPlayer Target { get; }

      public PlayerTargetPlayerEvent(NwPlayer caller, NwPlayer target)
      {
        Caller = caller;
        Target = target;
      }
    }
  }
}
