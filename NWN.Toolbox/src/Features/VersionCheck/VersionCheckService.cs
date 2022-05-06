using System;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.VersionCheck
{
  [ServiceBinding(typeof(VersionCheckService))]
  internal sealed class VersionCheckService : IInitializable
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    private Version minVersion;
    private Version recommendedVersion;

    private Version noVersion = new Version(0, 0);

    public void Init()
    {
      if (ConfigService.Config.VersionCheck.IsEnabled())
      {
        minVersion = ConfigService.Config.VersionCheck.MinimumVersion.AsVersion();
        recommendedVersion = ConfigService.Config.VersionCheck.RecommendedVersion.AsVersion();

        NwModule.Instance.OnClientEnter += OnClientEnter;
        NwModule.Instance.OnClientConnect += OnClientConnect;
      }
    }

    private void OnClientConnect(OnClientConnect eventData)
    {
      if (eventData.ClientVersion == noVersion)
      {
        eventData.KickMessage = "Could not determine your game version. Please update your client and try again.";
        eventData.BlockConnection = true;
      }
      else if (eventData.ClientVersion < minVersion)
      {
        eventData.KickMessage = $"This server requires updating your client to {minVersion.ToString(2)} or newer.";
        eventData.BlockConnection = true;
      }
    }

    private void OnClientEnter(ModuleEvents.OnClientEnter eventData)
    {
      if (eventData.Player.ClientVersion < recommendedVersion)
      {
        eventData.Player.SendErrorMessage($"This server recommends updating your client to {recommendedVersion.ToString(2)} or newer.");
      }
    }
  }
}
