using Anvil.API;
using Anvil.Services;

namespace Jorteck.Toolbox.Features.Chat
{
  [ServiceBinding(typeof(AreaShoutService))]
  public sealed class AreaShoutService
  {
    [Inject]
    private ChatService ChatService { get; init; } = null!;

    [Inject]
    private ChatWatchService ChatWatchService { get; init; } = null!;

    public void SendMessage(NwCreature? sender, string message)
    {
      if (sender == null)
      {
        return;
      }

      NwArea? targetArea = sender.Area;
      if (targetArea == null)
      {
        return;
      }

      string formattedMessage = GetFormattedAreaMessage(message);
      foreach (NwPlayer player in NwModule.Instance.Players)
      {
        if (player.IsValid && player.ControlledCreature?.Area == targetArea)
        {
          ChatService.SendMessage(ChatChannel.PlayerShout, formattedMessage, sender, player);
        }
      }

      if (sender.IsPlayerControlled(out NwPlayer? senderPlayer))
      {
        ChatWatchService.LogMessage(senderPlayer, "Area", message);
      }
    }

    public string GetFormattedAreaMessage(string message)
    {
      return "[Area] ".ColorString(ColorConstants.Orange) + message;
    }
  }
}
