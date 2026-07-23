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
public class Laser : ModCardTemplate, IMultiUpgradable
{
    private int _upgradeCount;

    public Laser() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [GrayTempestKeywords.Recyclable, GrayTempestKeywords.ImproveCost, GrayTempestKeywords.MultiUpgrade, GrayTempestKeywords.Energy];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9m, ValueProp.Move), new IntVar("ReconstructValue", 5), new IntVar("ImproveCost", 8), new IntVar("MaxUpgrades", 4)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardLaser, GrayTempestConst.Paths.CardLaser);
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
        DynamicVars.Damage.UpgradeValueBy(9m);
        DynamicVars["ImproveCost"].UpgradeValueBy(8);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        var baseDmg = DynamicVars.Damage.BaseValue;
        var dmg = play.Target.Block > 0 ? baseDmg : Math.Ceiling(baseDmg * 1.25m);
        await DamageCmd.Attack(dmg).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade() { }
}