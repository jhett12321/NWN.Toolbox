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

    public void Init()
    {
      if (ConfigService.Config.VersionCheck.IsEnabled())
      {
        minVersion = ConfigService.Config.VersionCheck.MinimumVersion.AsVersion();
        recommendedVersion = ConfigService.Config.VersionCheck.RecommendedVersion.AsVersion();

        NwModule.Instance.OnClientEnter += OnClientEnter;
      }
    }

    private void OnClientEnter(ModuleEvents.OnClientEnter eventData)
    {
      if (eventData.Player.ClientVersion < minVersion)
      {
        eventData.Player.BootPlayer($"This server requires updating your client to {minVersion.ToString(2)} or newer.");
      }
      else if (eventData.Player.ClientVersion < recommendedVersion)
      {
        eventData.Player.SendErrorMessage($"This server recommends updating your client to {recommendedVersion.ToString(2)} or newer.");
      }
    }
  }
}
