using System.Collections.Generic;
using Anvil.API;
using Jorteck.Toolbox.Core;

namespace Jorteck.Toolbox.Features.ToolWindows
{
  public sealed class CreaturePropertiesStatsWindowView : WindowView<CreaturePropertiesStatsWindowView>
  {
    public override string Id => "creature.stats";
    public override string Title => "Creature Properties: Statistics";
    public override NuiWindow WindowTemplate { get; }

    public override IWindowController CreateDefaultController(NwPlayer player)
    {
      return CreateController<CreaturePropertiesStatsWindowController>(player);
    }

    // Sub-views
    public readonly NuiGroup AbilityScoreListContainer;
    public readonly NuiGroup SavesListContainer;
    public readonly NuiGroup ACListContainer;
    public readonly NuiGroup HitPointsListContainer;
    public readonly NuiGroup SpeedContainer;

    // Permission Binds

    #region AbilityScoreEnabled
    public readonly NuiBind<bool> StrengthScoreRawEnabled = new NuiBind<bool>("strength_score_raw");
    public readonly NuiBind<bool> DexterityScoreRawEnabled = new NuiBind<bool>("dexterity_score_raw");
    public readonly NuiBind<bool> ConstitutionScoreRawEnabled = new NuiBind<bool>("constitution_score_raw");
    public readonly NuiBind<bool> IntelligenceScoreRawEnabled = new NuiBind<bool>("intelligence_score_raw");
    public readonly NuiBind<bool> WisdomScoreRawEnabled = new NuiBind<bool>("wisdom_score_raw");
    public readonly NuiBind<bool> CharismaScoreRawEnabled = new NuiBind<bool>("charisma_score_raw");
    #endregion

    #region SavesEnabled
    public readonly NuiBind<bool> FortitudeBonusEnabled = new NuiBind<bool>("fortitude_bonus");
    public readonly NuiBind<bool> ReflexBonusEnabled = new NuiBind<bool>("reflex_bonus");
    public readonly NuiBind<bool> WillBonusEnabled = new NuiBind<bool>("will_bonus");
    #endregion

    #region ArmorClassEnabled
    public readonly NuiBind<bool> NaturalACEnabled = new NuiBind<bool>("natural_ac");
    #endregion

    #region HitPointEnabled
    public readonly NuiBind<bool> BaseHitPointsEnabled = new NuiBind<bool>("base_hit_points");
    #endregion

    #region SpeedEnabled
    public readonly NuiBind<bool> MovementRateEnabled = new NuiBind<bool>("movement_rate");
    #endregion

    public readonly NuiBind<bool> SaveEnabled = new NuiBind<bool>("save");

    // Value Binds

    #region AbilityScoreValue
    public readonly NuiBind<string> StrengthScoreRaw = new NuiBind<string>("strength_score_raw_val");
    public readonly NuiBind<string> StrengthScoreRacial = new NuiBind<string>("strength_score_racial_val");
    public readonly NuiBind<string> StrengthScoreTotal = new NuiBind<string>("strength_score_total_val");
    public readonly NuiBind<string> StrengthScoreMod = new NuiBind<string>("strength_score_mod_val");

    public readonly NuiBind<string> DexterityScoreRaw = new NuiBind<string>("dexterity_score_raw_val");
    public readonly NuiBind<string> DexterityScoreRacial = new NuiBind<string>("dexterity_score_racial_val");
    public readonly NuiBind<string> DexterityScoreTotal = new NuiBind<string>("dexterity_score_total_val");
    public readonly NuiBind<string> DexterityScoreMod = new NuiBind<string>("dexterity_score_mod_val");

    public readonly NuiBind<string> ConstitutionScoreRaw = new NuiBind<string>("constitution_score_raw_val");
    public readonly NuiBind<string> ConstitutionScoreRacial = new NuiBind<string>("constitution_score_racial_val");
    public readonly NuiBind<string> ConstitutionScoreTotal = new NuiBind<string>("constitution_score_total_val");
    public readonly NuiBind<string> ConstitutionScoreMod = new NuiBind<string>("constitution_score_mod_val");

    public readonly NuiBind<string> IntelligenceScoreRaw = new NuiBind<string>("intelligence_score_raw_val");
    public readonly NuiBind<string> IntelligenceScoreRacial = new NuiBind<string>("intelligence_score_racial_val");
    public readonly NuiBind<string> IntelligenceScoreTotal = new NuiBind<string>("intelligence_score_total_val");
    public readonly NuiBind<string> IntelligenceScoreMod = new NuiBind<string>("intelligence_score_mod_val");

    public readonly NuiBind<string> WisdomScoreRaw = new NuiBind<string>("wisdom_score_raw_val");
    public readonly NuiBind<string> WisdomScoreRacial = new NuiBind<string>("wisdom_score_racial_val");
    public readonly NuiBind<string> WisdomScoreTotal = new NuiBind<string>("wisdom_score_total_val");
    public readonly NuiBind<string> WisdomScoreMod = new NuiBind<string>("wisdom_score_mod_val");

    public readonly NuiBind<string> CharismaScoreRaw = new NuiBind<string>("charisma_score_raw_val");
    public readonly NuiBind<string> CharismaScoreRacial = new NuiBind<string>("charisma_score_racial_val");
    public readonly NuiBind<string> CharismaScoreTotal = new NuiBind<string>("charisma_score_total_val");
    public readonly NuiBind<string> CharismaScoreMod = new NuiBind<string>("charisma_score_mod_val");
    #endregion

    #region SavesValue
    public readonly NuiBind<string> FortitudeBase = new NuiBind<string>("fortitude_base_val");
    public readonly NuiBind<string> FortitudeBonus = new NuiBind<string>("fortitude_bonus_val");
    public readonly NuiBind<string> FortitudeTotal = new NuiBind<string>("fortitude_total_val");

    public readonly NuiBind<string> ReflexBase = new NuiBind<string>("reflex_base_val");
    public readonly NuiBind<string> ReflexBonus = new NuiBind<string>("reflex_bonus_val");
    public readonly NuiBind<string> ReflexTotal = new NuiBind<string>("reflex_total_val");

    public readonly NuiBind<string> WillBase = new NuiBind<string>("will_base_val");
    public readonly NuiBind<string> WillBonus = new NuiBind<string>("will_bonus_val");
    public readonly NuiBind<string> WillTotal = new NuiBind<string>("will_total_val");
    #endregion

    #region ArmorClassValue
    public readonly NuiBind<string> NaturalAC = new NuiBind<string>("natural_ac_val");
    public readonly NuiBind<string> DexterityAC = new NuiBind<string>("dexterity_ac_val");
    public readonly NuiBind<string> SizeModifierAC = new NuiBind<string>("size_modifier_ac");
    public readonly NuiBind<string> TotalAC = new NuiBind<string>("total_ac_val");
    #endregion

    #region HitPointsValue
    public readonly NuiBind<string> BaseHitPoints = new NuiBind<string>("base_hit_points_val");
    public readonly NuiBind<string> BonusHitPoints = new NuiBind<string>("bonus_hit_points_val");
    public readonly NuiBind<string> TotalHitPoints = new NuiBind<string>("total_hit_points_val");
    #endregion

    #region SpeedValue
    public readonly NuiBind<int> MovementRate = new NuiBind<int>("movement_rate_val");
    #endregion

    // Buttons
    public readonly NuiButton SelectCreatureButton;
    public readonly NuiButton SaveChangesButton;
    public readonly NuiButton DiscardChangesButton;

    public CreaturePropertiesStatsWindowView()
    {
      NuiColumn root = new NuiColumn
      {
        Children = new List<NuiElement>
        {
          new NuiLabel("Ability Scores") { Height = 20f },
          new NuiGroup
          {
            Id = "ability_score_container",
            Scrollbars = NuiScrollbars.None,
            Height = 300f,
            Width = 560f,
          }.Assign(out AbilityScoreListContainer),
          new NuiSpacer(),
          new NuiLabel("Saves") { Height = 20f },
          new NuiGroup
          {
            Id = "saves_container",
            Scrollbars = NuiScrollbars.None,
            Height = 175f,
            Width = 560f,
          }.Assign(out SavesListContainer),
          new NuiSpacer(),
          new NuiRow
          {
            Children = new List<NuiElement>
            {
              new NuiColumn
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Armor Class") { Height = 20f },
                  new NuiGroup
                  {
                    Id = "ac_container",
                    Scrollbars = NuiScrollbars.None,
                    Height = 225f,
                    Width = 270f,
                  }.Assign(out ACListContainer),
                },
              },
              new NuiColumn
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Hit Points") { Height = 20f },
                  new NuiGroup
                  {
                    Id = "hp_container",
                    Scrollbars = NuiScrollbars.None,
                    Height = 145f,
                    Width = 280f,
                  }.Assign(out HitPointsListContainer),
                  new NuiLabel("Speed") { Height = 20f },
                  new NuiGroup
                  {
                    Id = "speed_container",
                    Scrollbars = NuiScrollbars.None,
                    Height = 50f,
                    Width = 280f,
                  }.Assign(out SpeedContainer),
                },
              },
            },
          },
          new NuiSpacer(),
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              new NuiButton("Select Creature")
              {
                Id = "btn_crt_sel",
              }.Assign(out SelectCreatureButton),
              new NuiSpacer(),
              new NuiButton("Save")
              {
                Id = "btn_save",
                Enabled = SaveEnabled,
              }.Assign(out SaveChangesButton),
              new NuiButton("Discard")
              {
                Id = "btn_discard",
                Enabled = SaveEnabled,
              }.Assign(out DiscardChangesButton),
            },
          },
        },
      };

      WindowTemplate = new NuiWindow(root, Title)
      {
        Geometry = new NuiRect(500f, 100f, 600f, 720f),
      };
    }
  }
}
