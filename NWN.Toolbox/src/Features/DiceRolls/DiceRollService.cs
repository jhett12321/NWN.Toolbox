using System;
using Anvil.API;
using Anvil.Services;

namespace Jorteck.Toolbox.Features
{
  [ServiceBinding(typeof(DiceRollService))]
  public sealed class DiceRollService
  {
    private const string DiceBroadcastVariableName = "roll_broadcast";

    [Inject]
    private Lazy<ChatService> ChatService { get; set; } = null!;

    public static readonly Color NameColor = new Color(153, 255, 255);
    public static readonly Color CheckMessageColor = new Color(1, 102, 255);
    public static readonly Color SaveMessageColor = new Color(138, 240, 240);

    /// <summary>
    /// Get a player's dice roll broadcast setting.
    /// </summary>
    /// <param name="player">The player to query.</param>
    /// <returns>The player's current broadcast setting.</returns>
    public RollBroadcastTargets GetBroadcastMode(NwPlayer player)
    {
      PersistentVariableEnum<RollBroadcastTargets> mode = player.LoginCreature!.GetObjectVariable<PersistentVariableEnum<RollBroadcastTargets>>(DiceBroadcastVariableName);
      if (mode.HasValue)
      {
        return mode.Value;
      }

      return RollBroadcastTargets.LocalTalk | RollBroadcastTargets.DM;
    }

    /// <summary>
    /// Set a player's dice roll broadcast setting.
    /// </summary>
    /// <param name="player">The player to change setting.</param>
    /// <param name="newValue">The new broadcast targets.</param>
    public void SetBroadcastMode(NwPlayer player, RollBroadcastTargets newValue)
    {
      player.LoginCreature!.GetObjectVariable<PersistentVariableEnum<RollBroadcastTargets>>(DiceBroadcastVariableName).Value = newValue;
    }

    /// <summary>
    /// Performs a basic dice roll with no modifiers.
    /// </summary>
    /// <param name="creature">The creature to perform the roll.</param>
    /// <param name="numSides">The number of dice sides.</param>
    /// <param name="numDice">The number of dice to roll.</param>
    /// <param name="broadcastTargets">The targets that should see the results of the dice roll.</param>
    /// <returns></returns>
    public int RollDice(NwCreature creature, int numSides, int numDice, RollBroadcastTargets broadcastTargets = RollBroadcastTargets.LocalTalk | RollBroadcastTargets.DM)
    {
      int roll = Random.Shared.Roll(numSides, numDice);
      string message = creature.Name.ColorString(NameColor) + $" : {numDice}d{numSides} = {roll}".ColorString(CheckMessageColor);

      BroadcastRoll(creature, message, broadcastTargets);

      return roll;
    }

    /// <summary>
    /// Performs an ability check vs the specified dc.
    /// </summary>
    /// <param name="creature">The creature to perform the roll.</param>
    /// <param name="ability">The ability to roll.</param>
    /// <param name="dc">The DC of the ability check to exceed or meet to be considered a success.</param>
    /// <param name="abilityCheckDifference">The difference between the creatures roll, and the DC.</param>
    /// <param name="settings">Additional settings for the roll.</param>
    /// <returns>True if the ability check passed the DC.</returns>
    public bool AbilityCheckVsDc(NwCreature creature, Ability ability, int dc, out int abilityCheckDifference, CreatureRollSettings settings = null)
    {
      settings ??= new CreatureRollSettings();

      int abilityMod = creature.GetAbilityModifier(ability);
      bool result = DoRollVsDc(dc, abilityMod, settings, out int roll, out int total);

      string resultText = result ? "success" : "failure";
      string message = creature.Name.ColorString(NameColor) + $" : {ability} : *{resultText}* : ({roll} + {abilityMod} = {total} vs. DC: {dc})".ColorString(CheckMessageColor);

      BroadcastRoll(creature, message, settings.RollBroadcastTargets);

      abilityCheckDifference = dc - total;
      return result;
    }

    /// <summary>
    /// Performs an ability roll on the specified creature.
    /// </summary>
    /// <param name="creature">The creature to perform the roll.</param>
    /// <param name="ability">The ability to roll.</param>
    /// <param name="broadcastTargets">The targets that should see the results of the ability roll.</param>
    /// <returns>The ability roll total.</returns>
    public int AbilityRoll(NwCreature creature, Ability ability, RollBroadcastTargets broadcastTargets = RollBroadcastTargets.LocalTalk | RollBroadcastTargets.DM)
    {
      int roll = Random.Shared.Roll(20);
      int abilityMod = creature.GetAbilityModifier(ability);
      int total = roll + abilityMod;

      string message = creature.Name.ColorString(NameColor) + $" : {ability} Check : ({roll} + {abilityMod} = {total})".ColorString(CheckMessageColor);
      BroadcastRoll(creature, message, broadcastTargets);

      return total;
    }

    /// <summary>
    /// Performs a skill roll on the specified creature.
    /// </summary>
    /// <param name="creature">The creature to perform the roll.</param>
    /// <param name="skill">The skill to roll.</param>
    /// <param name="broadcastTargets">The targets that should see the results of the skill roll.</param>
    /// <returns>The skill roll total.</returns>
    public int SkillRoll(NwCreature creature, NwSkill skill, RollBroadcastTargets broadcastTargets = RollBroadcastTargets.LocalTalk | RollBroadcastTargets.DM)
    {
      int roll = Random.Shared.Roll(20);
      int skillRanks = creature.GetSkillRank(skill);
      int total = roll + skillRanks;

      string message;

      if (skill.IsUntrained || skillRanks > 0)
      {
        message = creature.Name.ColorString(NameColor) + $" : {skill.Name} Check : ({roll} + {skillRanks} = {total})".ColorString(CheckMessageColor);
      }
      else
      {
        message = creature.Name.ColorString(NameColor) + $" : {skill.Name} Check : Untrained. Cannot use skill.".ColorString(CheckMessageColor);
      }

      BroadcastRoll(creature, message, broadcastTargets);

      return total;
    }

    /// <summary>
    /// An extended version of <see cref="NwCreature.DoSkillCheck"/> exposing additional data and settings.
    /// </summary>
    /// <param name="creature">The creature to perform the roll.</param>
    /// <param name="skill">The skill to roll.</param>
    /// <param name="dc">The DC of the skill check to exceed or meet to be considered a success.</param>
    /// <param name="skillCheckDifference">The difference between the creatures roll, and the DC.</param>
    /// <param name="settings">Additional settings for the roll.</param>
    /// <returns>True if the skill check passed the DC.</returns>
    public bool SkillCheckVsDc(NwCreature creature, NwSkill skill, int dc, out int skillCheckDifference, CreatureRollSettings settings = null)
    {
      settings ??= new CreatureRollSettings();

      int ranks = creature.GetSkillRank(skill);
      bool result = DoRollVsDc(dc, ranks, settings, out int roll, out int total);

      string resultText = result ? "success" : "failure";
      string message = creature.Name.ColorString(NameColor) + $" : {skill.Name} : *{resultText}* : ({roll} + {ranks} = {total} vs. DC: {dc})".ColorString(CheckMessageColor);

      BroadcastRoll(creature, message, settings.RollBroadcastTargets);

      skillCheckDifference = dc - total;
      return result;
    }

    /// <summary>
    /// Performs a saving throw on the specified creature.
    /// </summary>
    /// <param name="creature">The creature to perform the roll.</param>
    /// <param name="savingThrow">The saving throw to roll.</param>
    /// <param name="broadcastTargets">The targets that should see the results of the saving throw roll.</param>
    /// <returns>The saving throw roll total.</returns>
    public int SavingThrowRoll(NwCreature creature, SavingThrow savingThrow, RollBroadcastTargets broadcastTargets = RollBroadcastTargets.LocalTalk | RollBroadcastTargets.DM)
    {
      int roll = Random.Shared.Roll(20);
      int save = creature.GetSavingThrow(savingThrow);
      int total = roll + save;

      string message = creature.Name.ColorString(NameColor) + $" : {savingThrow} Save : ({roll} + {save} = {total})".ColorString(SaveMessageColor);
      BroadcastRoll(creature, message, broadcastTargets);

      return total;
    }

    /// <summary>
    /// Performs a saving throw vs the specified dc.
    /// </summary>
    /// <param name="creature">The creature to perform the roll.</param>
    /// <param name="savingThrow">The saving throw to roll.</param>
    /// <param name="dc">The DC of the saving throw to exceed or meet to be considered a success.</param>
    /// <param name="savingThrowDifference">The difference between the creatures roll, and the DC.</param>
    /// <param name="settings">Additional settings for the roll.</param>
    /// <returns>True if the saving throw passed the DC.</returns>
    public bool SavingThrowRollVsDc(NwCreature creature, SavingThrow savingThrow, int dc, out int savingThrowDifference, CreatureRollSettings settings = null)
    {
      if (savingThrow == SavingThrow.All)
      {
        throw new ArgumentOutOfRangeException(nameof(savingThrow));
      }

      settings ??= new CreatureRollSettings();

      int save = creature.GetSavingThrow(savingThrow);
      bool result = DoRollVsDc(dc, save, settings, out int roll, out int total);

      string resultText = result ? "success" : "failure";
      string message = creature.Name.ColorString(NameColor) + $" : {savingThrow} Save : *{resultText}* : ({roll} + {save} = {total} vs. DC: {dc})".ColorString(SaveMessageColor);

      BroadcastRoll(creature, message, settings.RollBroadcastTargets);

      savingThrowDifference = dc - total;
      return result;
    }

    private bool DoRollVsDc(int dc, int bonus, CreatureRollSettings settings, out int roll, out int total)
    {
      roll = Random.Shared.Roll(20);
      total = roll + bonus;

      bool result = total >= dc;
      if (roll == 1 && settings.AutoFail)
      {
        result = false;
      }
      else if (roll == 20 && settings.AutoSuccess)
      {
        result = true;
      }

      return result;
    }

    private void BroadcastRoll(NwCreature creature, string message, RollBroadcastTargets broadcastTargets)
    {
      if (creature.IsPlayerControlled(out NwPlayer player))
      {
        if (broadcastTargets.HasFlag(RollBroadcastTargets.PrivateLog))
        {
          player.SendServerMessage(message);
        }

        if (broadcastTargets.HasFlag(RollBroadcastTargets.PrivateChat))
        {
          ChatService.Value.SendMessage(ChatChannel.PlayerTalk, message, creature, player);
        }
      }

      if (broadcastTargets.HasFlag(RollBroadcastTargets.LocalTalk))
      {
        _ = creature.SpeakString(message);
      }

      if (broadcastTargets.HasFlag(RollBroadcastTargets.DM))
      {
        NwModule.Instance.SendMessageToAllDMs(message);
      }
    }
  }
}
