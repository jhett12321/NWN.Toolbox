using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.Toolbox.Features.Chat;

namespace Jorteck.Toolbox.Features
{
  [ServiceBinding(typeof(IChatCommand))]
  public sealed class SkillCheckCommand : IChatCommand
  {
    [Inject]
    private DiceRollService DiceRollService { get; init; }

    [Inject]
    private HelpCommand HelpCommand { get; init; }

    public string Command => "skill";
    public bool DMOnly => false;
    public Range ArgCount => 1..;

    public string Description => "Perform a skill check.";

    public CommandUsage[] Usages { get; }

    public SkillCheckCommand()
    {
      Usages = new CommandUsage[NwRuleset.Skills.Count];
      for (int i = 0; i < NwRuleset.Skills.Count; i++)
      {
        NwSkill skill = NwRuleset.Skills[i];
        string skillName = skill.Name.ToString();
        if (string.IsNullOrEmpty(skillName))
        {
          continue;
        }

        if (skillName[0] is 'a' or 'e' or 'i' or 'o' or 'u')
        {
          Usages[i] = new CommandUsage(skill.Name.ToString().ToLower(), $"Roll an {skill.Name} skill check.");
        }
        else
        {
          Usages[i] = new CommandUsage(skill.Name.ToString().ToLower(), $"Roll a {skill.Name} skill check.");
        }
      }
    }

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      string skillName = string.Join(' ', args);
      foreach (NwSkill skill in NwRuleset.Skills)
      {
        if (skill.Name.ToString().Equals(skillName, StringComparison.OrdinalIgnoreCase))
        {
          DiceRollService.SkillRoll(caller.ControlledCreature, skill, DiceRollService.GetBroadcastMode(caller));
          return;
        }
      }

      HelpCommand.ShowCommandHelpToPlayer(caller, this);
    }
  }
}
