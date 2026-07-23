using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Rare;

[RegisterCard(typeof(GrayTempestCardPool))]
public class Assimilate : ModCardTemplate
{
    public Assimilate() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, GrayTempestKeywords.Kinetic];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4m, ValueProp.Move)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        var dmg = play.Target.Block > 0 ? Math.Ceiling(DynamicVars.Damage.BaseValue * 1.25m) : DynamicVars.Damage.BaseValue;
        var hpBefore = play.Target.CurrentHp;
        await DamageCmd.Attack(dmg).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        if (play.Target.IsDead)
        {
            var hpAfter = play.Target.CurrentHp;
            var actualDamage = hpBefore - Math.Max(0m, hpAfter);
            await CreatureCmd.GainMaxHp(Owner.Creature, actualDamage);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}
