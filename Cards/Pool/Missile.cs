using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Pool;

[RegisterCard(typeof(GrayTempestCraftingCardPool))]
public class Missile : ModCardTemplate, IMultiUpgradable
{
    private int _upgradeCount;

    public Missile() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [GrayTempestKeywords.Recyclable, GrayTempestKeywords.ImproveCost, GrayTempestKeywords.MultiUpgrade, GrayTempestKeywords.Burst];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move), new IntVar("ReconstructValue", 5), new IntVar("ImproveCost", 8), new IntVar("MaxUpgrades", 4)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardMissile, GrayTempestConst.Paths.CardMissile);
    public override int MaxUpgradeLevel => 0;

    public override string Title
    {
        get
        {
            var title = base.Title;
            return $"{title} {RomanNumeral.ToRoman(_upgradeCount + 1)}";
        }
    }

    public int UpgradeCount => _upgradeCount;
    public int MaxUpgrades => DynamicVars["MaxUpgrades"].IntValue;

    public void FakeUpgrade()
    {
        _upgradeCount++;
        DynamicVars.Damage.UpgradeValueBy(8m);
        DynamicVars["ImproveCost"].UpgradeValueBy(8);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner.Creature.CombatState is not { } combatState) return;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).TargetingAllOpponents(combatState).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade() { }
}