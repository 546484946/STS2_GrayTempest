using System.Linq;
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
public class SimpleImprove : ModCardTemplate
{
    public SimpleImprove() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, GrayTempestKeywords.OneShot];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Replay", 0)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var prefs = new CardSelectorPrefs(new LocString("cards", "GRAY_TEMPEST_IMPROVE_CHOOSE"), 1);
        var selected = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs,
            c => c.CanonicalKeywords.Any(k => k.Equals(GrayTempestKeywords.ImproveCost))
                && c.DynamicVars.ContainsKey("ImproveCost")
                && Owner.Gold >= c.DynamicVars["ImproveCost"].IntValue, this)).ToList();
        foreach (var card in selected)
            await ImproveSystem.ImproveCard(choiceContext, card, Owner);
        try { if (DeckVersion is { } dv) await CardPileCmd.RemoveFromDeck(dv, false); } catch { }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Replay"].UpgradeValueBy(2);
        BaseReplayCount += 2;
    }
}
