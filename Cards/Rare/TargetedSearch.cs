using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Rare;

[RegisterCard(typeof(GrayTempestCardPool))]
public class TargetedSearch : ModCardTemplate
{
    public TargetedSearch() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CardPileCmd.Draw(choiceContext, 1m, Owner, false);

        var drawPrefs = new CardSelectorPrefs(new LocString("cards", "GRAY_TEMPEST_TARGETED_SEARCH_CHOOSE_DRAW"), 1);
        var drawCard = (await CardSelectCmd.FromCombatPile(
            context: choiceContext,
            pile: PileType.Draw.GetPile(Owner),
            player: Owner,
            prefs: drawPrefs)).FirstOrDefault();
        if (drawCard != null)
            await CardPileCmd.Add(drawCard, PileType.Hand);

        var discardPrefs = new CardSelectorPrefs(new LocString("cards", "GRAY_TEMPEST_TARGETED_SEARCH_CHOOSE_DISCARD"), 1);
        var discardCard = (await CardSelectCmd.FromCombatPile(
            context: choiceContext,
            pile: PileType.Discard.GetPile(Owner),
            player: Owner,
            prefs: discardPrefs)).FirstOrDefault();
        if (discardCard != null)
            await CardPileCmd.Add(discardCard, PileType.Hand);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
        AddKeyword(CardKeyword.Innate);
    }
}
