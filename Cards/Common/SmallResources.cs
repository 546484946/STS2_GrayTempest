using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Common;

[RegisterCard(typeof(GrayTempestCardPool))]
public class SmallResources : ModCardTemplate
{
    public SmallResources() : base(0, CardType.Power, CardRarity.Common, TargetType.Self) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [GrayTempestKeywords.OneShot, GrayTempestKeywords.Recyclable];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("ReconstructValue", 2), new IntVar("GoldAmount", 20)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PlayerCmd.GainGold((decimal)DynamicVars["GoldAmount"].IntValue, Owner);
        try { if (DeckVersion is { } dv) await CardPileCmd.RemoveFromDeck(dv, false); } catch { }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ReconstructValue"].UpgradeValueBy(1);
        DynamicVars["GoldAmount"].UpgradeValueBy(10);
    }
}
