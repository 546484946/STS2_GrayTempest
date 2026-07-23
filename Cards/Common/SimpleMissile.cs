using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Common;

[RegisterCard(typeof(GrayTempestCardPool))]
public class SimpleMissile : ModCardTemplate
{
    public SimpleMissile() : base(0, CardType.Attack, CardRarity.Common, TargetType.AllEnemies) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, GrayTempestKeywords.OneShot, GrayTempestKeywords.Burst, GrayTempestKeywords.ImproveCost];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(20m, ValueProp.Move), new IntVar("ImproveCost", 10)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardSimpleMissile, GrayTempestConst.Paths.CardSimpleMissile);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner.Creature.CombatState is not { } combatState) return;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).TargetingAllOpponents(combatState).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        try { if (DeckVersion is { } dv) await CardPileCmd.RemoveFromDeck(dv, false); } catch { }
    }
    protected override void OnUpgrade() { DynamicVars.Damage.UpgradeValueBy(25m); }
}
