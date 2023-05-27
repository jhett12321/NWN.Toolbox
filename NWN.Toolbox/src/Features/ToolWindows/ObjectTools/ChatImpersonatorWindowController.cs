using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using Jorteck.Toolbox.Core;
using Jorteck.Toolbox.Features.Chat;
using Jorteck.Toolbox.Features.Languages;

namespace Jorteck.Toolbox.Features.ToolWindows
{
  public sealed class ChatImpersonatorWindowController : WindowController<ChatImpersonatorWindowView>
  {
    [Inject]
    private LanguageService LanguageService { get; init; }

    [Inject]
    private LanguageChatService LanguageChatService { get; init; }

    [Inject]
    private AreaShoutService AreaShoutService { get; init; }

    [Inject]
    private ConfigService ConfigService { get; init; }

    private readonly StringBuilder chatHistory = new StringBuilder();
    private List<ILanguage> languages;

    private NwGameObject selectedObject;

    public override void Init()
    {
      languages = LanguageService.Languages.OrderBy(language => language.Name).ToList();
      languages.Insert(0, null);
      List<NuiComboEntry> languageEntries = new List<NuiComboEntry>(languages.Count);

      for (int i = 0; i < languages.Count; i++)
      {
        string languageName = languages[i]?.Name;
        languageEntries.Add(languageName != null ? new NuiComboEntry(languageName, i) : new NuiComboEntry("None", i));
      }

      Token.SetBindValue(View.WindowTitle, View.Title);
      Token.SetBindValue(View.Languages, languageEntries);
      Token.SetBindValue(View.SelectedLanguage, 0);
      Token.SetBindValue(View.SelectedChatVolume, 0);
      Token.SetBindValue(View.LanguagesEnabled, false);
    }

    public override void ProcessEvent(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.EventType != NuiEventType.Click)
      {
        return;
      }

      if (eventData.ElementId == View.SendButton.Id)
      {
        SendMessage();
      }
      else if (eventData.ElementId == View.SelectObjectButton.Id)
      {
        Token.Player.EnterTargetMode(SelectObject, new TargetModeSettings
        {
          ValidTargets = ObjectTypes.All & ~ObjectTypes.Tile,
        });
      }
    }

    private void SelectObject(ModuleEvents.OnPlayerTarget eventData)
    {
      if (eventData.IsCancelled || eventData.TargetObject is not NwGameObject gameObject)
      {
        return;
      }

      selectedObject = gameObject;
      chatHistory.Clear();
      Token.SetBindValue(View.WindowTitle, selectedObject.Name);
      Token.SetBindValue(View.ChatHistory, chatHistory.ToString());
      Token.SetBindValue(View.SelectedLanguage, 0);
      Token.SetBindValue(View.LanguagesEnabled, gameObject is NwCreature && ConfigService.Config.Languages.IsEnabled());
    }

    private void SendMessage()
    {
      if (selectedObject == null)
      {
        return;
      }

      string message = Token.GetBindValue(View.Message);
      if (string.IsNullOrWhiteSpace(message))
      {
        return;
      }

      int languageIndex = Token.GetBindValue(View.SelectedLanguage);
      ChatVolume volume = (ChatVolume)Token.GetBindValue(View.SelectedChatVolume);

      if (languageIndex < 0 || languageIndex >= languages.Count || !Enum.IsDefined(volume))
      {
        return;
      }

      ILanguage language = languages[languageIndex];

      chatHistory.AppendLine($"{selectedObject.Name}: {message}");
      Token.SetBindValue(View.ChatHistory, chatHistory.ToString());

      NwCreature creature = selectedObject as NwCreature;

      if (language != null && creature != null)
      {
        LanguageChatService.SendTranslatedMessage(creature, volume, true, false, message, language, LanguageProficiency.Fluent);
      }
      else
      {
        if (volume != ChatVolume.Area)
        {
          _ = selectedObject.SpeakString(message, volume.ToTalkVolume());
        }
        else if (creature != null)
        {
          AreaShoutService.SendMessage(creature, message);
        }
      }

      Token.SetBindValue(View.Message, string.Empty);
    }

    protected override void OnClose() {}
  }
}
