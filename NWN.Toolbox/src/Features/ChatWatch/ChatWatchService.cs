using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Toolbox.Features
{
  [ServiceBinding(typeof(ChatWatchService))]
  [ServiceBindingOptions(BindingPriority = BindingPriority.VeryLow)]
  internal sealed class ChatWatchService
  {
    private readonly Dictionary<NwPlayer, List<NwPlayer>> playerSubscriptions = new Dictionary<NwPlayer, List<NwPlayer>>();
    private readonly Dictionary<NwArea, List<NwPlayer>> areaSubscriptions = new Dictionary<NwArea, List<NwPlayer>>();
    private readonly List<NwPlayer> globalSubscriptions = new List<NwPlayer>();

    public ChatWatchService()
    {
      NwModule.Instance.OnPlayerChat += OnPlayerChat;
      NwModule.Instance.OnClientLeave += OnClientLeave;
    }

    private void OnClientLeave(ModuleEvents.OnClientLeave eventData)
    {
      globalSubscriptions.Remove(eventData.Player);
      foreach (List<NwPlayer> subscribers in playerSubscriptions.Values)
      {
        subscribers.Remove(eventData.Player);
      }

      foreach (List<NwPlayer> subscribers in areaSubscriptions.Values)
      {
        subscribers.Remove(eventData.Player);
      }
    }

    public void TogglerPartySubscribe(NwPlayer subscriber, NwPlayer partyPlayer)
    {
      List<NwPlayer> partyPlayers = partyPlayer.PartyMembers.ToList();
      if (partyPlayers.Count == 1)
      {
        ToggleSubscribe(subscriber, partyPlayer);
        return;
      }

      // Some party members are not subscribed, or none of them are subscribed.
      List<NwPlayer> playersToToggle = new List<NwPlayer>();
      if (playerSubscriptions.TryGetValue(partyPlayer, out List<NwPlayer> subscribers))
      {
        playersToToggle.AddRange(partyPlayers.Except(subscribers));
      }
      else
      {
        playersToToggle.AddRange(partyPlayers);
      }

      // Whole party is subscribed, so we toggle everyone to unsubscribe.
      if (playersToToggle.Count == 0)
      {
        playersToToggle = partyPlayers;
      }

      foreach (NwPlayer player in playersToToggle)
      {
        ToggleSubscribe(subscriber, player);
      }
    }

    public void ToggleSubscribe(NwPlayer subscriber, NwPlayer player)
    {
      if (playerSubscriptions.RemoveElement(player, subscriber))
      {
        subscriber.SendServerMessage($"Unsubscribed from chat events from player '{player.ControlledCreature!.Name}'");
      }
      else
      {
        playerSubscriptions.AddElement(player, subscriber);
        subscriber.SendServerMessage($"Subscribed to chat events from player '{player.ControlledCreature!.Name}'");
      }
    }

    public void ToggleSubscribe(NwPlayer subscriber, NwArea area)
    {
      if (areaSubscriptions.RemoveElement(area, subscriber))
      {
        subscriber.SendServerMessage($"Unsubscribed from chat events in area '{area.Name}'");
      }
      else
      {
        areaSubscriptions.AddElement(area, subscriber);
        subscriber.SendServerMessage($"Subscribed to chat events in area '{area.Name}'");
      }
    }

    public void ToggleSubscribe(NwPlayer subscriber)
    {
      if (globalSubscriptions.Remove(subscriber))
      {
        subscriber.SendServerMessage("Unsubscribed from all module chat events");
      }
      else
      {
        globalSubscriptions.Add(subscriber);
        subscriber.SendServerMessage("Subscribed to all module chat events");
      }
    }

    public void LogMessage(NwPlayer sender, string channelName, string message)
    {
      NwCreature creature = sender.ControlledCreature!;
      NwArea area = creature.Area;

      string areaName = area?.Name ?? "Unknown";
      string playerName = creature.Name;

      string logMessage = $"[{channelName}][{areaName}]".ColorString(ColorConstants.Cyan) + $" {playerName}: " + $"{message}".ColorString(ColorConstants.White);
      if (playerSubscriptions.TryGetValue(sender, out List<NwPlayer> subscribers))
      {
        NotifySubscribers(subscribers, logMessage);
      }

      if (area != null && areaSubscriptions.TryGetValue(area, out subscribers))
      {
        NotifySubscribers(subscribers, logMessage);
      }

      NotifySubscribers(globalSubscriptions, logMessage);
    }

    private void NotifySubscribers(List<NwPlayer> players, string logMessage)
    {
      for (int i = players.Count - 1; i >= 0; i--)
      {
        NwPlayer player = players[i];
        if (!player.IsValid)
        {
          players.RemoveAt(i);
        }

        player.SendServerMessage(logMessage);
      }
    }

    private void OnPlayerChat(ModuleEvents.OnPlayerChat eventData)
    {
      if (string.IsNullOrEmpty(eventData.Message))
      {
        return;
      }

      LogMessage(eventData.Sender, eventData.Volume.ToString(), eventData.Message);
    }
  }
}
