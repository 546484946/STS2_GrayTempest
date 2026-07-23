using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards;

[RegisterCard(typeof(GrayTempestCardPool))]
public class SystemRedundancy : ModCardTemplate
{
    public SystemRedundancy() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [GrayTempestKeywords.OneShot, CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Amount", 1)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var count = DynamicVars["Amount"].IntValue;
        for (int i = 0; i < count; i++)
        {
            var card = CombatState.CreateCard<Redundancy>(Owner);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        }
        try { if (DeckVersion is { } dv) await CardPileCmd.RemoveFromDeck(dv, false); } catch { }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Amount"].UpgradeValueBy(2);
    }
}
