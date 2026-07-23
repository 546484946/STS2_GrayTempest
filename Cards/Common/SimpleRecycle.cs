using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using GrayTempest.Systems;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Common;

[RegisterCard(typeof(GrayTempestCardPool))]
public class SimpleRecycle : ModCardTemplate
{
    public SimpleRecycle() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, GrayTempestKeywords.OneShot, GrayTempestKeywords.Recycle];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("RecycleCount", 1)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int maxCount = IsUpgraded ? 3 : 1;
        var prefs = new CardSelectorPrefs(new LocString("cards", "GRAY_TEMPEST_RECYCLE_CHOOSE"), 1, maxCount);
        var selected = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).ToList();
        foreach (var card in selected)
            await RecycleSystem.RecycleCard(choiceContext, card, Owner);
        try { if (DeckVersion is { } dv) await CardPileCmd.RemoveFromDeck(dv, false); } catch { }
    }

    protected override void OnUpgrade() { RemoveKeyword(CardKeyword.Exhaust); AddKeyword(CardKeyword.Retain); DynamicVars["RecycleCount"].UpgradeValueBy(2); }
}
