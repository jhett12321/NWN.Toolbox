using System;
using System.Text.RegularExpressions;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using Jorteck.Toolbox.Core;
using NLog;

namespace Jorteck.Toolbox.Features.Languages
{
  [ServiceBinding(typeof(LanguageChatService))]
  public sealed class LanguageChatService : IInitializable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    [Inject]
    private ChatService ChatService { get; init; }

    [Inject]
    private LanguageService LanguageService { get; init; }

    [Inject]
    private ConfigService ConfigService { get; init; }

    private readonly Regex translatePattern = new Regex(@"\[(?<message>.*?)\]", RegexOptions.Compiled);

    void IInitializable.Init()
    {
      if (ConfigService.Config.Languages.IsEnabled())
      {
        NwModule.Instance.OnPlayerChat += OnPlayerChat;
      }
    }

    public void SendTranslatedMessage(NwPlayer player, LanguageOutput message, TalkVolume volume)
    {
      NwCreature playerCreature = player.ControlledCreature!;
      NwArea area = playerCreature.Area;

      if (area == null)
      {
        return;
      }

      Log.Info($"[{area.Name}] {playerCreature.Name}: [{message.Language.Name}] {message.Interpretation}");

      switch (volume)
      {
        case TalkVolume.Talk:
        case TalkVolume.Whisper:
          ChatChannel channel = GetChannel(player, volume);
          foreach (NwPlayer onlinePlayer in NwModule.Instance.Players)
          {
            if (!onlinePlayer.IsConnected)
            {
              continue;
            }

            NwCreature onlinePlayerCreature = onlinePlayer.ControlledCreature;
            if (onlinePlayerCreature == null || onlinePlayerCreature.Area != area)
            {
              continue;
            }

            float chatDistance = ChatService.GetPlayerChatHearingDistance(onlinePlayer, channel);
            if (onlinePlayerCreature.DistanceSquared(playerCreature) < chatDistance * chatDistance)
            {
              ChatService.SendMessage(channel, GetMessageForPlayer(onlinePlayer, message), playerCreature, onlinePlayer);
            }
          }

          break;
        case TalkVolume.Party:
          foreach (NwPlayer partyMember in player.PartyMembers)
          {
            ChatService.SendMessage(ChatChannel.PlayerParty, GetMessageForPlayer(partyMember, message), playerCreature, partyMember);
          }

          break;
        case TalkVolume.Tell:
          break;
      }
    }

    private void OnPlayerChat(ModuleEvents.OnPlayerChat eventData)
    {
      if (!translatePattern.IsMatch(eventData.Message))
      {
        return;
      }

      LanguageState languageState = LanguageService.GetStateForPlayer(eventData.Sender);
      if (languageState?.CurrentLanguageId == null)
      {
        eventData.Sender.SendErrorMessage("You have not selected a language. Select a language first with the lang command.");
        return;
      }

      if (!LanguageService.TryGetLanguage(languageState.CurrentLanguageId, out ILanguage language))
      {
        return;
      }

      int? proficiency = LanguageService.GetLanguageProficiency(eventData.Sender, language, languageState);
      if (proficiency == null)
      {
        eventData.Sender.SendErrorMessage("You have not selected a language. Select a language first with the lang command.");
        return;
      }

      LanguageOutput message = ParseMessage(eventData.Message, language, proficiency.Value);
      eventData.Message = string.Empty;

      SendTranslatedMessage(eventData.Sender, message, eventData.Volume);
    }

    private LanguageOutput ParseMessage(string message, ILanguage language, int proficiency)
    {
      string interpretation = translatePattern.Replace(message, match => language.Translate(match.Groups["message"].Value, proficiency).Interpretation);
      string output = translatePattern.Replace(message, match => language.Translate(match.Groups["message"].Value, proficiency).Output);

      return new LanguageOutput(language, interpretation, output);
    }

    private ChatChannel GetChannel(NwPlayer player, TalkVolume volume)
    {
      return volume switch
      {
        TalkVolume.Talk => player.IsDM ? ChatChannel.DmTalk : ChatChannel.PlayerTalk,
        TalkVolume.Whisper => player.IsDM ? ChatChannel.DmWhisper : ChatChannel.PlayerWhisper,
        TalkVolume.Shout => player.IsDM ? ChatChannel.DmShout : ChatChannel.PlayerShout,
        TalkVolume.SilentTalk => player.IsDM ? ChatChannel.DmTalk : ChatChannel.PlayerTalk,
        TalkVolume.SilentShout => player.IsDM ? ChatChannel.DmShout : ChatChannel.PlayerShout,
        TalkVolume.Party => player.IsDM ? ChatChannel.DmParty : ChatChannel.PlayerParty,
        TalkVolume.Tell => player.IsDM ? ChatChannel.DmTell : ChatChannel.PlayerTell,
        _ => throw new ArgumentOutOfRangeException(nameof(volume), volume, null),
      };
    }

    private string GetMessageForPlayer(NwPlayer player, LanguageOutput message)
    {
      LanguageState languageState = LanguageService.GetStateForPlayer(player);
      bool knowsLanguage = LanguageService.PlayerKnowsLanguage(player, message.Language, languageState);

      string prefix = knowsLanguage ? $"[{message.Language.Name}] ".ColorString(message.Language.ChatColor) : "[Unknown Language] ".ColorString(ColorConstants.Gray);
      LanguageDisplayType displayType = knowsLanguage ? LanguageService.GetDisplayType(player) : LanguageDisplayType.Native;

      return displayType switch
      {
        LanguageDisplayType.Translated => prefix + message.Interpretation,
        LanguageDisplayType.Native => prefix + message.Output,
        LanguageDisplayType.Both => prefix + message.Output + $"\n({message.Interpretation})",
        _ => throw new ArgumentOutOfRangeException(),
      };
    }
  }
}
