using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using GrayTempest.Cards.Keywords;
using GrayTempest.Cards;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Common;

[RegisterCard(typeof(GrayTempestCardPool))]
public class Mining : ModCardTemplate
{
    public Mining() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, GrayTempestKeywords.Energy];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move), new IntVar("ResourceCount", 1)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        var baseDmg = DynamicVars.Damage.BaseValue;
        var dmg = play.Target.Block > 0 ? baseDmg : Math.Ceiling(baseDmg * 1.25m);
        await DamageCmd.Attack(dmg).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);

        int count = IsUpgraded ? 2 : 1;
        for (int i = 0; i < count; i++)
        {
            var tiny = CombatState.CreateCard<TinyResource>(Owner);
            await CardPileCmd.AddGeneratedCardToCombat(tiny, PileType.Hand, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
        DynamicVars["ResourceCount"].UpgradeValueBy(1);
    }
}
