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
public class GrayTempestStorm : ModCardTemplate
{
    public GrayTempestStorm() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [GrayTempestKeywords.Kinetic];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        var hpDmg = Owner.Creature.CurrentHp;
        var dmg = play.Target.Block > 0 ? Math.Ceiling(hpDmg * 1.25m) : hpDmg;
        await DamageCmd.Attack(dmg).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
