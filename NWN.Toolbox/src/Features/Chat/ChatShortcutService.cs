using System.Collections.Generic;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.Chat
{
  [ServiceBinding(typeof(ChatShortcutService))]
  [ServiceBindingOptions(BindingPriority = BindingPriority.High)]
  internal sealed class ChatShortcutService
  {
    private const string ShortcutVarKey = "shortcuts";

    private readonly ConfigService configService;

    public ChatShortcutService(ConfigService configService)
    {
      this.configService = configService;
      NwModule.Instance.OnPlayerChat += OnPlayerChat;
    }

    public void SetShortcut(NwPlayer player, string shortcut, string replacement)
    {
      if (!IsValidShortcutName(shortcut))
      {
        player.SendErrorMessage($"Cannot set shortcuts that start with '{string.Join(',', configService.Config.ChatCommands.CommandPrefixes)}'");
      }

      Dictionary<string, string> shortcuts = GetShortcuts(player);
      shortcuts ??= new Dictionary<string, string>();

      shortcuts[shortcut] = replacement;
      player.LoginCreature!.GetObjectVariable<PersistentVariableString>(ShortcutVarKey).Value = JsonUtility.ToJson(shortcuts);

      player.SendServerMessage($"Set shortcut '{shortcut}' to '{replacement}'");
    }

    private bool IsValidShortcutName(string shortcut)
    {
      if (!configService.Config.ChatCommands.IsEnabled())
      {
        return true;
      }

      foreach (string prefix in configService.Config.ChatCommands.CommandPrefixes)
      {
        if (shortcut.StartsWith(prefix))
        {
          return false;
        }
      }

      return true;
    }

    public void UnsetShortcut(NwPlayer player, string shortcut)
    {
      Dictionary<string, string> shortcuts = GetShortcuts(player);

      if (shortcuts != null && shortcuts.Remove(shortcut))
      {
        player.LoginCreature!.GetObjectVariable<PersistentVariableString>(ShortcutVarKey).Value = JsonUtility.ToJson(shortcuts);
        player.SendServerMessage($"Unset shortcut '{shortcut}'");
      }
      else
      {
        player.SendErrorMessage($"Unknown shortcut '{shortcut}'");
      }
    }

    public void ClearShortcuts(NwPlayer player)
    {
      PersistentVariableString shortcutsVar = player.LoginCreature!.GetObjectVariable<PersistentVariableString>(ShortcutVarKey);
      shortcutsVar.Delete();
    }

    private void OnPlayerChat(ModuleEvents.OnPlayerChat eventData)
    {
      Dictionary<string, string> shortcuts = GetShortcuts(eventData.Sender);
      if (shortcuts == null)
      {
        return;
      }

      foreach ((string shortcut, string replacement) in shortcuts)
      {
        if (eventData.Message.StartsWith(shortcut))
        {
          eventData.Message = replacement + eventData.Message[shortcut.Length..];
        }
      }
    }

    private Dictionary<string, string> GetShortcuts(NwPlayer player)
    {
      PersistentVariableString shortcutsVar = player.LoginCreature!.GetObjectVariable<PersistentVariableString>(ShortcutVarKey);
      if (shortcutsVar.HasValue)
      {
        string rawShortcuts = shortcutsVar.Value;
        if (!string.IsNullOrEmpty(rawShortcuts))
        {
          return JsonUtility.FromJson<Dictionary<string, string>>(shortcutsVar.Value!);
        }
      }

      return null;
    }
  }
}
