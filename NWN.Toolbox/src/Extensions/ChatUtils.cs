using System;
using Anvil.API;
using Jorteck.Toolbox.Features.Chat;

namespace Jorteck.Toolbox
{
  public static class ChatExtensions
  {
    public static ChatVolume ToChatVolume(this TalkVolume talkVolume)
    {
      return talkVolume switch
      {
        TalkVolume.Talk => ChatVolume.Talk,
        TalkVolume.Whisper => ChatVolume.Whisper,
        TalkVolume.Shout => ChatVolume.Talk,
        TalkVolume.SilentTalk => ChatVolume.Talk,
        TalkVolume.SilentShout => ChatVolume.Talk,
        TalkVolume.Party => ChatVolume.Party,
        TalkVolume.Tell => ChatVolume.Whisper,
        _ => throw new ArgumentOutOfRangeException(nameof(talkVolume), talkVolume, null),
      };
    }

    public static TalkVolume ToTalkVolume(this ChatVolume talkVolume)
    {
      return talkVolume switch
      {
        ChatVolume.Talk => TalkVolume.Talk,
        ChatVolume.Whisper => TalkVolume.Whisper,
        ChatVolume.Party => TalkVolume.Party,
        ChatVolume.Area => TalkVolume.Talk,
        _ => throw new ArgumentOutOfRangeException(nameof(talkVolume), talkVolume, null),
      };
    }

    public static string GetAreaShoutMessage(string message)
    {
      return "[Area] ".ColorString(ColorConstants.Yellow) + message;
    }
  }
}
