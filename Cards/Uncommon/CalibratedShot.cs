using System;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Uncommon;

[RegisterCard(typeof(GrayTempestCardPool))]
public class CalibratedShot : ModCardTemplate
{
    public CalibratedShot() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [GrayTempestKeywords.Kinetic];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3m, ValueProp.Move), new IntVar("Hits", 3), new IntVar("DrawCount", 1)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        var baseDmg = DynamicVars.Damage.BaseValue;
        var dmg = play.Target.Block > 0 ? Math.Ceiling(baseDmg * 1.25m) : baseDmg;
        var hits = DynamicVars["Hits"].IntValue;

        await DamageCmd.Attack(dmg)
            .FromCard(this, play)
            .Targeting(play.Target)
            .WithHitCount(hits)
            .Execute(choiceContext);

        await CardPileCmd.Draw(choiceContext, DynamicVars["DrawCount"].IntValue, Owner);
        await PowerCmd.Apply<FocusedStrikePower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Hits"].UpgradeValueBy(1);
        DynamicVars["DrawCount"].UpgradeValueBy(1);
    }
}
