namespace Jorteck.Toolbox.Features
{
  public sealed class CreatureRollSettings
  {
    public bool AutoSuccess { get; set; } = false;
    public bool AutoFail { get; set; } = false;
    public RollBroadcastTargets RollBroadcastTargets { get; set; } = RollBroadcastTargets.PrivateLog;
  }
}
