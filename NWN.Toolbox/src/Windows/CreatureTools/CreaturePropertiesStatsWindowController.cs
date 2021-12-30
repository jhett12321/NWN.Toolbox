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
      string value = selectedCreature.GetRawAbilityScore(Ability.Strength).ToString();
      Token.SetBindValue(View.StrengthScoreRaw, value);
      string value1 = selectedCreature.Race.GetAbilityAdjustment(Ability.Strength).ToString();
      Token.SetBindValue(View.StrengthScoreRacial, value1);
      string value2 = selectedCreature.GetAbilityScore(Ability.Strength).ToString();
      Token.SetBindValue(View.StrengthScoreTotal, value2);
      string value3 = selectedCreature.GetAbilityModifier(Ability.Strength).ToString();
      Token.SetBindValue(View.StrengthScoreMod, value3);

      string value4 = selectedCreature.GetRawAbilityScore(Ability.Dexterity).ToString();
      Token.SetBindValue(View.DexterityScoreRaw, value4);
      string value5 = selectedCreature.Race.GetAbilityAdjustment(Ability.Dexterity).ToString();
      Token.SetBindValue(View.DexterityScoreRacial, value5);
      string value6 = selectedCreature.GetAbilityScore(Ability.Dexterity).ToString();
      Token.SetBindValue(View.DexterityScoreTotal, value6);
      string value7 = selectedCreature.GetAbilityModifier(Ability.Dexterity).ToString();
      Token.SetBindValue(View.DexterityScoreMod, value7);

      string value8 = selectedCreature.GetRawAbilityScore(Ability.Constitution).ToString();
      Token.SetBindValue(View.ConstitutionScoreRaw, value8);
      string value9 = selectedCreature.Race.GetAbilityAdjustment(Ability.Constitution).ToString();
      Token.SetBindValue(View.ConstitutionScoreRacial, value9);
      string value10 = selectedCreature.GetAbilityScore(Ability.Constitution).ToString();
      Token.SetBindValue(View.ConstitutionScoreTotal, value10);
      string value11 = selectedCreature.GetAbilityModifier(Ability.Constitution).ToString();
      Token.SetBindValue(View.ConstitutionScoreMod, value11);

      string value12 = selectedCreature.GetRawAbilityScore(Ability.Intelligence).ToString();
      Token.SetBindValue(View.IntelligenceScoreRaw, value12);
      string value13 = selectedCreature.Race.GetAbilityAdjustment(Ability.Intelligence).ToString();
      Token.SetBindValue(View.IntelligenceScoreRacial, value13);
      string value14 = selectedCreature.GetAbilityScore(Ability.Intelligence).ToString();
      Token.SetBindValue(View.IntelligenceScoreTotal, value14);
      string value15 = selectedCreature.GetAbilityModifier(Ability.Intelligence).ToString();
      Token.SetBindValue(View.IntelligenceScoreMod, value15);

      string value16 = selectedCreature.GetRawAbilityScore(Ability.Wisdom).ToString();
      Token.SetBindValue(View.WisdomScoreRaw, value16);
      string value17 = selectedCreature.Race.GetAbilityAdjustment(Ability.Wisdom).ToString();
      Token.SetBindValue(View.WisdomScoreRacial, value17);
      string value18 = selectedCreature.GetAbilityScore(Ability.Wisdom).ToString();
      Token.SetBindValue(View.WisdomScoreTotal, value18);
      string value19 = selectedCreature.GetAbilityModifier(Ability.Wisdom).ToString();
      Token.SetBindValue(View.WisdomScoreMod, value19);

      string value20 = selectedCreature.GetRawAbilityScore(Ability.Charisma).ToString();
      Token.SetBindValue(View.CharismaScoreRaw, value20);
      string value21 = selectedCreature.Race.GetAbilityAdjustment(Ability.Charisma).ToString();
      Token.SetBindValue(View.CharismaScoreRacial, value21);
      string value22 = selectedCreature.GetAbilityScore(Ability.Charisma).ToString();
      Token.SetBindValue(View.CharismaScoreTotal, value22);
      string value23 = selectedCreature.GetAbilityModifier(Ability.Charisma).ToString();
      Token.SetBindValue(View.CharismaScoreMod, value23);

      string value24 = selectedCreature.GetBaseSavingThrow(SavingThrow.Fortitude).ToString();
      Token.SetBindValue(View.FortitudeBase, value24);
      //SetBindValue(View.FortitudeBonus, (selectedCreature.GetSavingThrow(SavingThrow.Fortitude) - selectedCreature.GetBaseSavingThrow(SavingThrow.Fortitude)).ToString());
      string value25 = selectedCreature.GetSavingThrow(SavingThrow.Fortitude).ToString();
      Token.SetBindValue(View.FortitudeTotal, value25);

      string value26 = selectedCreature.GetBaseSavingThrow(SavingThrow.Reflex).ToString();
      Token.SetBindValue(View.ReflexBase, value26);
      //SetBindValue(View.ReflexBonus, (selectedCreature.GetSavingThrow(SavingThrow.Reflex) - selectedCreature.GetBaseSavingThrow(SavingThrow.Reflex)).ToString());
      string value27 = selectedCreature.GetSavingThrow(SavingThrow.Reflex).ToString();
      Token.SetBindValue(View.ReflexTotal, value27);

      string value28 = selectedCreature.GetBaseSavingThrow(SavingThrow.Will).ToString();
      Token.SetBindValue(View.WillBase, value28);
      //SetBindValue(View.WillBonus, (selectedCreature.GetSavingThrow(SavingThrow.Will) - selectedCreature.GetBaseSavingThrow(SavingThrow.Will)).ToString());
      string value29 = selectedCreature.GetSavingThrow(SavingThrow.Will).ToString();
      Token.SetBindValue(View.WillTotal, value29);

      Token.SetBindValue(View.NaturalAC, selectedCreature.BaseAC.ToString());
      string value30 = selectedCreature.GetAbilityModifier(Ability.Dexterity).ToString();
      Token.SetBindValue(View.DexterityAC, value30);
      string value31 = selectedCreature.Size.ACModifier().ToString();
      Token.SetBindValue(View.SizeModifierAC, value31);
      Token.SetBindValue(View.TotalAC, selectedCreature.AC.ToString());

      int value32 = (int)selectedCreature.MovementRate;
      Token.SetBindValue(View.MovementRate, value32);

      int bonusHitPoints = selectedCreature.GetAbilityModifier(Ability.Constitution) * selectedCreature.Level;

      string value33 = (selectedCreature.MaxHP - bonusHitPoints).ToString();
      Token.SetBindValue(View.BaseHitPoints, value33);
      Token.SetBindValue(View.BonusHitPoints, bonusHitPoints.ToString());
      Token.SetBindValue(View.TotalHitPoints, selectedCreature.MaxHP.ToString());

      RefreshAbilityScoreContainer();
      RefreshSavesContainer();
      RefreshACContainer();
      RefreshHitPointsContainer();
      RefreshSpeedContainer();
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
