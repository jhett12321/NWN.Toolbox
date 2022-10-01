using System.Collections.Generic;
using Anvil.API;
using Jorteck.Toolbox.Core;
using Jorteck.Toolbox.Features.Chat;

namespace Jorteck.Toolbox.Features.ToolWindows
{
  public sealed class ChatImpersonatorWindowView : WindowView<ChatImpersonatorWindowView>
  {
    public override string Id => "chat.impersonator";
    public override string Title => "Chat Impersonator";
    public override NuiWindow WindowTemplate { get; }

    public readonly NuiBind<string> ChatHistory = new NuiBind<string>("chat_history");
    public readonly NuiBind<string> Message = new NuiBind<string>("message");
    public readonly NuiBind<List<NuiComboEntry>> Languages = new NuiBind<List<NuiComboEntry>>("languages");
    public readonly NuiBind<string> WindowTitle = new NuiBind<string>("title");

    public readonly NuiBind<int> SelectedChatVolume = new NuiBind<int>("selected_chat_volume");
    public readonly NuiBind<int> SelectedLanguage = new NuiBind<int>("selected_language");

    public readonly NuiBind<bool> LanguagesEnabled = new NuiBind<bool>("language_enable");

    public readonly NuiButton SendButton;
    public readonly NuiButtonImage SelectObjectButton;

    public override IWindowController CreateDefaultController(NwPlayer player)
    {
      return CreateController<ChatImpersonatorWindowController>(player);
    }

    public ChatImpersonatorWindowView()
    {
      NuiColumn root = new NuiColumn
      {
        Width = -1,
        Height = -1,
        Children = new List<NuiElement>
        {
          new NuiText(ChatHistory)
          {
            Border = true,
            Scrollbars = NuiScrollbars.Y,
          },
          new NuiRow
          {
            Children = new List<NuiElement>
            {
              new NuiButtonImage("dm_findnext")
              {
                Id = "select_object",
                Width = 32f,
                Height = 32f,
                Tooltip = "Select Object",
              }.Assign(out SelectObjectButton),
              NuiUtils.CreateComboForEnum<ChatVolume>(SelectedChatVolume),
              new NuiCombo
              {
                Entries = Languages,
                Selected = SelectedLanguage,
                Enabled = LanguagesEnabled,
              },
            },
          },
          new NuiRow
          {
            Children = new List<NuiElement>
            {
              new NuiTextEdit("Enter a message to speak...", Message, 4096, false)
              {
                Height = 32f,
              },
              new NuiButton("Send")
              {
                Id = "send_button",
                Width = 50f,
                Height = 32f,
              }.Assign(out SendButton),
            },
          },
        },
      };

      WindowTemplate = new NuiWindow(root, WindowTitle)
      {
        Geometry = new NuiRect(-1, -1, 370, 600),
      };
    }
  }
}
