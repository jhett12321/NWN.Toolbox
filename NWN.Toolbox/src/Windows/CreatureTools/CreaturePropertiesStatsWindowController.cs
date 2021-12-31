using System.Collections.Generic;
using Anvil.API;
using Anvil.API.Events;

namespace Jorteck.Toolbox
{
  public sealed class CreaturePropertiesStatsWindowController : WindowController<CreaturePropertiesStatsWindowView>
  {
    private NuiBind<bool>[] widgetEnabledBinds;
    private NwCreature selectedCreature;

    public override void Init()
    {
      widgetEnabledBinds = new[]
      {
        View.StrengthScoreRawEnabled,
        View.DexterityScoreRawEnabled,
        View.ConstitutionScoreRawEnabled,
        View.IntelligenceScoreRawEnabled,
        View.WisdomScoreRawEnabled,
        View.CharismaScoreRawEnabled,
        View.FortitudeBonusEnabled,
        View.ReflexBonusEnabled,
        View.WillBonusEnabled,
        View.NaturalACEnabled,
        View.BaseHitPointsEnabled,
        View.MovementRateEnabled,
        View.SaveEnabled,
      };

      Update();
    }

    public override void ProcessEvent(ModuleEvents.OnNuiEvent eventData)
    {
      switch (eventData.EventType)
      {
        case NuiEventType.Click:
          HandleButtonClick(eventData);
          break;
        case NuiEventType.Open:
          Update();
          break;
      }
    }

    protected override void OnClose()
    {
      selectedCreature = null;
    }

    private void Update()
    {
      if (selectedCreature == null)
      {
        SetElementsEnabled(false);

        RefreshAbilityScoreContainer();
        RefreshSavesContainer();
        RefreshACContainer();
        RefreshHitPointsContainer();
        RefreshSpeedContainer();
        return;
      }

      ApplyPermissionBindings(widgetEnabledBinds);

      BindAbilityScores(Ability.Strength, View.StrengthScoreRaw, View.StrengthScoreRacial, View.StrengthScoreTotal, View.StrengthScoreMod);
      BindAbilityScores(Ability.Dexterity, View.DexterityScoreRaw, View.DexterityScoreRacial, View.DexterityScoreTotal, View.DexterityScoreMod);
      BindAbilityScores(Ability.Constitution, View.ConstitutionScoreRaw, View.ConstitutionScoreRacial, View.ConstitutionScoreTotal, View.ConstitutionScoreMod);
      BindAbilityScores(Ability.Intelligence, View.IntelligenceScoreRaw, View.IntelligenceScoreRacial, View.IntelligenceScoreTotal, View.IntelligenceScoreMod);
      BindAbilityScores(Ability.Wisdom, View.WisdomScoreRaw, View.WisdomScoreRacial, View.WisdomScoreTotal, View.WisdomScoreMod);
      BindAbilityScores(Ability.Charisma, View.CharismaScoreRaw, View.CharismaScoreRacial, View.CharismaScoreTotal, View.CharismaScoreMod);

      BindSavingThrow(SavingThrow.Fortitude, View.FortitudeBase, View.FortitudeBonus, View.FortitudeTotal);
      BindSavingThrow(SavingThrow.Reflex, View.ReflexBase, View.ReflexBonus, View.ReflexTotal);
      BindSavingThrow(SavingThrow.Will, View.WillBase, View.WillBonus, View.WillTotal);

      Token.SetBindValue(View.NaturalAC, selectedCreature.BaseAC.ToString());
      Token.SetBindValue(View.DexterityAC, selectedCreature.GetAbilityModifier(Ability.Dexterity).ToString());
      Token.SetBindValue(View.SizeModifierAC, selectedCreature.Size.ACModifier().ToString());
      Token.SetBindValue(View.TotalAC, selectedCreature.AC.ToString());
      Token.SetBindValue(View.MovementRate, (int)selectedCreature.MovementRate);

      int bonusHitPoints = selectedCreature.GetAbilityModifier(Ability.Constitution) * selectedCreature.Level;

      Token.SetBindValue(View.BaseHitPoints, (selectedCreature.MaxHP - bonusHitPoints).ToString());
      Token.SetBindValue(View.BonusHitPoints, bonusHitPoints.ToString());
      Token.SetBindValue(View.TotalHitPoints, selectedCreature.MaxHP.ToString());

      RefreshAbilityScoreContainer();
      RefreshSavesContainer();
      RefreshACContainer();
      RefreshHitPointsContainer();
      RefreshSpeedContainer();
    }

    private void BindAbilityScores(Ability ability, NuiBind<string> scoreRaw, NuiBind<string> scoreRacial, NuiBind<string> scoreTotal, NuiBind<string> scoreMod)
    {
      Token.SetBindValue(scoreRaw, selectedCreature.GetRawAbilityScore(ability).ToString());
      Token.SetBindValue(scoreRacial, selectedCreature.Race.GetAbilityAdjustment(ability).ToString());
      Token.SetBindValue(scoreTotal, selectedCreature.GetAbilityScore(ability).ToString());
      Token.SetBindValue(scoreMod, selectedCreature.GetAbilityModifier(ability).ToString());
    }

    private void BindSavingThrow(SavingThrow savingThrow, NuiBind<string> throwBase, NuiBind<string> throwBonus, NuiBind<string> throwTotal)
    {
      Token.SetBindValue(throwBase, selectedCreature.GetBaseSavingThrow(savingThrow).ToString());
      Token.SetBindValue(throwBonus, string.Empty); // TODO
      Token.SetBindValue(throwTotal, selectedCreature.GetSavingThrow(savingThrow).ToString());
    }

    private void HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.SelectCreatureButton.Id)
      {
        Token.Player.TryEnterTargetMode(OnCreatureSelected, ObjectTypes.Creature);
      }
      else if (eventData.ElementId == View.SaveChangesButton.Id)
      {
        SaveChanges();
      }
      else if (eventData.ElementId == View.DiscardChangesButton.Id)
      {
        Update();
      }
    }

    private void SaveChanges()
    {
      if (selectedCreature == null || !selectedCreature.IsValid)
      {
        return;
      }

      selectedCreature.SetsRawAbilityScore(Ability.Strength, byte.Parse(Token.GetBindValue(View.StrengthScoreRaw)));
      selectedCreature.SetsRawAbilityScore(Ability.Dexterity, byte.Parse(Token.GetBindValue(View.DexterityScoreRaw)));
      selectedCreature.SetsRawAbilityScore(Ability.Constitution, byte.Parse(Token.GetBindValue(View.ConstitutionScoreRaw)));
      selectedCreature.SetsRawAbilityScore(Ability.Intelligence, byte.Parse(Token.GetBindValue(View.IntelligenceScoreRaw)));
      selectedCreature.SetsRawAbilityScore(Ability.Wisdom, byte.Parse(Token.GetBindValue(View.WisdomScoreRaw)));
      selectedCreature.SetsRawAbilityScore(Ability.Charisma, byte.Parse(Token.GetBindValue(View.CharismaScoreRaw)));

      selectedCreature.BaseAC = sbyte.Parse(Token.GetBindValue(View.NaturalAC));

      // TODO: missing the saves
      // TODO: missing the base HP

      selectedCreature.MovementRate = (MovementRate)Token.GetBindValue(View.MovementRate);

      Update();
    }

    private void OnCreatureSelected(ModuleEvents.OnPlayerTarget eventData)
    {
      if (eventData.TargetObject == null || eventData.TargetObject is not NwCreature creature)
      {
        return;
      }

      if (creature.IsLoginPlayerCharacter)
      {
        eventData.Player.SendServerMessage("You may not select players.");
        return;
      }

      selectedCreature = creature;
      Update();
    }

    private void SetElementsEnabled(bool enabled)
    {
      foreach (NuiBind<bool> bind in widgetEnabledBinds)
      {
        Token.SetBindValue(bind, enabled);
      }
    }

    private void RefreshAbilityScoreContainer()
    {
      NuiColumn col = new NuiColumn() { Height = 500f };

      col.Children.Add(new NuiRow
      {
        Margin = 5f,
        Height = 15f,
        Children = new List<NuiElement>
        {
          new NuiLabel("") { Width = 120f },
          new NuiLabel("Score") { Width = 50f },
          new NuiLabel("") { Width = 0f },
          new NuiLabel("Racial Modifier") { Width = 150f, HorizontalAlign = NuiHAlign.Center },
          new NuiLabel("") { Width = 0f },
          new NuiLabel("Total") { Width = 50f },
          new NuiLabel("") { Width = 50f },
          new NuiLabel("Bonus") { Width = 50f },
        },
      });
      col.Children.Add(new NuiRow
      {
        Height = 40f,
        Children = new List<NuiElement>
        {
          new NuiLabel("Strength") { Width = 120f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.StrengthScoreRaw, 3, false) { Width = 50f, Enabled = View.StrengthScoreRawEnabled },
          new NuiLabel("+") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.StrengthScoreRacial, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("=") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.StrengthScoreTotal, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("") { Width = 50f },
          new NuiTextEdit(string.Empty, View.StrengthScoreMod, 3, false) { Enabled = false, Width = 50f },
        },
      });

      col.Children.Add(new NuiRow
      {
        Height = 40f,
        Children = new List<NuiElement>
        {
          new NuiLabel("Dexterity") { Width = 120f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.DexterityScoreRaw, 3, false) { Width = 50f, Enabled = View.DexterityScoreRawEnabled },
          new NuiLabel("+") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.DexterityScoreRacial, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("=") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.DexterityScoreTotal, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("") { Width = 50f },
          new NuiTextEdit(string.Empty, View.DexterityScoreMod, 3, false) { Enabled = false, Width = 50f },
        },
      });
      col.Children.Add(new NuiRow
      {
        Height = 40f,
        Children = new List<NuiElement>
        {
          new NuiLabel("Constitution") { Width = 120f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.ConstitutionScoreRaw, 3, false) { Width = 50f, Enabled = View.ConstitutionScoreRawEnabled },
          new NuiLabel("+") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.ConstitutionScoreRacial, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("=") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.ConstitutionScoreTotal, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("") { Width = 50f },
          new NuiTextEdit(string.Empty, View.ConstitutionScoreMod, 3, false) { Enabled = false, Width = 50f },
        },
      });
      col.Children.Add(new NuiRow
      {
        Height = 40f,
        Children = new List<NuiElement>
        {
          new NuiLabel("Intelligence") { Width = 120f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.IntelligenceScoreRaw, 3, false) { Width = 50f, Enabled = View.IntelligenceScoreRawEnabled },
          new NuiLabel("+") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.IntelligenceScoreRacial, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("=") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.IntelligenceScoreTotal, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("") { Width = 50f },
          new NuiTextEdit(string.Empty, View.IntelligenceScoreMod, 3, false) { Enabled = false, Width = 50f },
        },
      });
      col.Children.Add(new NuiRow
      {
        Height = 40f,
        Children = new List<NuiElement>
        {
          new NuiLabel("Wisdom") { Width = 120f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.WisdomScoreRaw, 3, false) { Width = 50f, Enabled = View.WisdomScoreRawEnabled },
          new NuiLabel("+") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.WisdomScoreRacial, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("=") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.WisdomScoreTotal, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("") { Width = 50f },
          new NuiTextEdit(string.Empty, View.WisdomScoreMod, 3, false) { Enabled = false, Width = 50f },
        },
      });
      col.Children.Add(new NuiRow
      {
        Height = 40f,
        Children = new List<NuiElement>
        {
          new NuiLabel("Charisma") { Width = 120f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.CharismaScoreRaw, 3, false) { Width = 50f, Enabled = View.CharismaScoreRawEnabled },
          new NuiLabel("+") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.CharismaScoreRacial, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("=") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.CharismaScoreTotal, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("") { Width = 50f },
          new NuiTextEdit(string.Empty, View.CharismaScoreMod, 3, false) { Enabled = false, Width = 50f },
        },
      });

      Token.SetGroupLayout(View.AbilityScoreListContainer, col);
    }

    private void RefreshSavesContainer()
    {
      NuiColumn col = new NuiColumn();

      col.Children.Add(new NuiRow
      {
        Margin = 5f,
        Height = 15f,
        Children = new List<NuiElement>
        {
          new NuiLabel("") { Width = 90f },
          new NuiLabel("Base") { Width = 50f, HorizontalAlign = NuiHAlign.Center },
          new NuiLabel("") { Width = 0f },
          new NuiLabel("Ability Modifier") { Width = 150f, HorizontalAlign = NuiHAlign.Center },
          new NuiLabel("") { Width = 0f },
          new NuiLabel("Bonus") { Width = 50f },
          new NuiLabel("") { Width = 50f },
          new NuiLabel("Total") { Width = 50f },
        },
      });
      col.Children.Add(new NuiRow
      {
        Height = 40f,
        Children = new List<NuiElement>
        {
          new NuiLabel("Fortitude") { Width = 90f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.FortitudeBase, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("+") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.ConstitutionScoreMod, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("=") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.FortitudeBonus, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("") { Width = 50f },
          new NuiTextEdit(string.Empty, View.FortitudeTotal, 3, false) { Enabled = false, Width = 50f },
        },
      });
      col.Children.Add(new NuiRow
      {
        Height = 40f,
        Children = new List<NuiElement>
        {
          new NuiLabel("Reflex") { Width = 90f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.ReflexBase, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("+") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.DexterityScoreMod, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("=") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.ReflexBonus, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("") { Width = 50f },
          new NuiTextEdit(string.Empty, View.ReflexTotal, 3, false) { Enabled = false, Width = 50f },
        },
      });
      col.Children.Add(new NuiRow
      {
        Height = 40f,
        Children = new List<NuiElement>
        {
          new NuiLabel("Will") { Width = 90f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.WillBase, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("+") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.WisdomScoreMod, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("=") { Width = 50f, HorizontalAlign = NuiHAlign.Center, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.WillBonus, 3, false) { Enabled = false, Width = 50f },
          new NuiLabel("") { Width = 50f },
          new NuiTextEdit(string.Empty, View.WillTotal, 3, false) { Enabled = false, Width = 50f },
        },
      });

      Token.SetGroupLayout(View.SavesListContainer, col);
    }

    private void RefreshACContainer()
    {
      NuiColumn col = new NuiColumn();

      col.Children.Add(new NuiRow
      {
        Height = 40f,
        Children = new List<NuiElement>
        {
          new NuiLabel("Natural AC") { Width = 150f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.NaturalAC, 3, false) { Enabled = View.NaturalACEnabled, Width = 50f },
        },
      });
      col.Children.Add(new NuiRow
      {
        Height = 40f,
        Children = new List<NuiElement>
        {
          new NuiLabel("Base") { Width = 150f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, "10", 3, false) { Enabled = false, Width = 50f },
        },
      });
      col.Children.Add(new NuiRow
      {
        Height = 40f,
        Children = new List<NuiElement>
        {
          new NuiLabel("Dexterity Bonus") { Width = 150f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.DexterityAC, 3, false) { Enabled = false, Width = 50f },
        },
      });
      col.Children.Add(new NuiRow
      {
        Height = 40f,
        Children = new List<NuiElement>
        {
          new NuiLabel("Size Modifier") { Width = 150f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.SizeModifierAC, 3, false) { Enabled = false, Width = 50f },
        },
      });
      col.Children.Add(new NuiRow
      {
        Height = 40f,
        Children = new List<NuiElement>
        {
          new NuiLabel("Total AC") { Width = 150f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.TotalAC, 3, false) { Enabled = false, Width = 50f },
        },
      });

      Token.SetGroupLayout(View.ACListContainer, col);
    }

    private void RefreshHitPointsContainer()
    {
      NuiColumn col = new NuiColumn();

      col.Children.Add(new NuiRow
      {
        Children = new List<NuiElement>
        {
          new NuiLabel("Base Hit Points") { Height = 20f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.BaseHitPoints, 4, false) { Width = 50f, Enabled = View.BaseHitPointsEnabled },
        },
      });

      col.Children.Add(new NuiRow
      {
        Children = new List<NuiElement>
        {
          new NuiLabel("Hit Point Bonuses") { Height = 20f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.BonusHitPoints, 4, false) { Width = 50f, Enabled = false },
        },
      });

      col.Children.Add(new NuiRow
      {
        Children = new List<NuiElement>
        {
          new NuiLabel("Total Hit Points") { Height = 20f, VerticalAlign = NuiVAlign.Middle },
          new NuiTextEdit(string.Empty, View.TotalHitPoints, 4, false) { Width = 50f, Enabled = false },
        },
      });

      Token.SetGroupLayout(View.HitPointsListContainer, col);
    }

    private void RefreshSpeedContainer()
    {
      NuiColumn col = new NuiColumn();

      col.Children.Add(new NuiRow
      {
        Children = new List<NuiElement>
        {
          new NuiLabel("Movement Rate") { Height = 20f, Width = 110f, VerticalAlign = NuiVAlign.Middle },
          NuiUtils.CreateComboForEnum<MovementRate>(View.MovementRate),
        },
      });

      Token.SetGroupLayout(View.SpeedContainer, col);
    }
  }
}
