using System;
using System.Text.RegularExpressions;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using Jorteck.Toolbox.Core;
using Jorteck.Toolbox.Features.Chat;
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

    [Inject]
    private AreaShoutService AreaShoutService { get; init; }

    private readonly Regex translatePattern = new Regex(@"\[(?<message>.*?)\]", RegexOptions.Compiled);

    void IInitializable.Init()
    {
      if (ConfigService.Config.Languages.IsEnabled())
      {
        NwModule.Instance.OnPlayerChat += OnPlayerChat;
      }
    }

    private LanguageOutput GetTranslatedChatFromPattern(ILanguage language, int proficiency, string message)
    {
      string interpretation = translatePattern.Replace(message, match => language.Translate(match.Groups["message"].Value, proficiency).Interpretation);
      string output = translatePattern.Replace(message, match => language.Translate(match.Groups["message"].Value, proficiency).Output);
      return new LanguageOutput(language, interpretation, output);
    }

    public void SendTranslatedMessage(NwCreature sender, ChatVolume volume, bool isDm, bool matchChatPattern, string message, ILanguage language, int proficiency)
    {
      NwArea area = sender?.Area;
      if (area == null)
      {
        return;
      }

      LanguageOutput translatedMessage = matchChatPattern ? GetTranslatedChatFromPattern(language, proficiency, message) : language.Translate(message, proficiency);

      Log.Info($"[{area.Name}] {sender.Name}: [{translatedMessage.Language.Name}] {translatedMessage.Interpretation}");

      ChatChannel channel = GetChannel(volume, isDm);
      switch (volume)
      {
        case ChatVolume.Talk:
        case ChatVolume.Whisper:
          foreach (NwPlayer onlinePlayer in NwModule.Instance.Players)
          {
            if (!onlinePlayer.IsValid || !onlinePlayer.IsConnected)
            {
              continue;
            }

            NwCreature onlinePlayerCreature = onlinePlayer.ControlledCreature;
            if (onlinePlayerCreature == null || onlinePlayerCreature.Area != area)
            {
              continue;
            }

            float chatDistance = ChatService.GetPlayerChatHearingDistance(onlinePlayer, channel);
            if (onlinePlayerCreature.DistanceSquared(sender) < chatDistance * chatDistance)
            {
              ChatService.SendMessage(channel, GetMessageForPlayer(onlinePlayer, translatedMessage), sender, onlinePlayer);
            }
          }

          break;
        case ChatVolume.Area:
          foreach (NwPlayer onlinePlayer in NwModule.Instance.Players)
          {
            if (!onlinePlayer.IsValid || !onlinePlayer.IsConnected)
            {
              continue;
            }

            if (onlinePlayer.ControlledCreature.Area == area)
            {
              string rawMessage = GetMessageForPlayer(onlinePlayer, translatedMessage);
              ChatService.SendMessage(channel, AreaShoutService.GetFormattedAreaMessage(rawMessage), sender, onlinePlayer);
            }
          }

          break;
        case ChatVolume.Party when sender.IsPlayerControlled(out NwPlayer player):
          foreach (NwPlayer partyMember in player.PartyMembers)
          {
            ChatService.SendMessage(channel, GetMessageForPlayer(partyMember, translatedMessage), sender, partyMember);
          }

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

      SendTranslatedMessage(eventData.Sender.ControlledCreature, eventData.Volume.ToChatVolume(), eventData.Sender.IsDM, true, eventData.Message, language, proficiency.Value);
      eventData.Message = string.Empty;
    }

    private ChatChannel GetChannel(ChatVolume volume, bool isDM)
    {
      return volume switch
      {
        ChatVolume.Talk => isDM ? ChatChannel.DmTalk : ChatChannel.PlayerTalk,
        ChatVolume.Area => isDM ? ChatChannel.DmTalk : ChatChannel.PlayerTalk,
        ChatVolume.Whisper => isDM ? ChatChannel.DmWhisper : ChatChannel.PlayerWhisper,
        ChatVolume.Party => isDM ? ChatChannel.DmParty : ChatChannel.PlayerParty,
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
