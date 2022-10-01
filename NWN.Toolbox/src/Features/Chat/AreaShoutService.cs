using Anvil.API;
using Anvil.Services;

namespace Jorteck.Toolbox.Features.Chat
{
  [ServiceBinding(typeof(AreaShoutService))]
  public sealed class AreaShoutService
  {
    [Inject]
    private ChatService ChatService { get; init; }

    public void SendMessage(NwCreature sender, string message)
    {
      NwArea targetArea = sender?.Area;
      if (targetArea == null)
      {
        return;
      }

      string formattedMessage = GetFormattedAreaMessage(message);
      foreach (NwPlayer player in NwModule.Instance.Players)
      {
        if (player.ControlledCreature?.Area == targetArea)
        {
          ChatService.SendMessage(ChatChannel.PlayerTalk, formattedMessage, sender, player);
        }
      }
    }

    public string GetFormattedAreaMessage(string message)
    {
      return "[Area] ".ColorString(ColorConstants.Yellow) + message;
    }
  }
}
