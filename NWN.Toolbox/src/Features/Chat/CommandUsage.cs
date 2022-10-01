namespace Jorteck.Toolbox.Features.Chat
{
  public sealed class CommandUsage
  {
    public string SubCommand { get; }
    public string Description { get; }

    public CommandUsage(string subCommand, string description)
    {
      SubCommand = subCommand;
      Description = description;
    }

    public CommandUsage(string description)
    {
      Description = description;
    }
  }
}
