using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using GrayTempest.Cards.Keywords;
using GrayTempest.Cards.Pool;
using GrayTempest.Character;
using GrayTempest.Systems;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Uncommon;

[RegisterCard(typeof(GrayTempestCardPool))]
public class FieldUpgrade : ModCardTemplate
{
    public FieldUpgrade() : base(3, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, GrayTempestKeywords.OneShot];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var prefs = new CardSelectorPrefs(new LocString("cards", "GRAY_TEMPEST_FIELD_UPGRADE_CHOOSE"), 1);
        var selected = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).ToList();
        foreach (var card in selected)
        {
            if (card is IMultiUpgradable mu)
            {
                mu.FakeUpgrade();
                if (card.DeckVersion is IMultiUpgradable dv)
                    dv.FakeUpgrade();
            }
            else
            {
                CardCmd.Upgrade(card, CardPreviewStyle.None);
                if (card.DeckVersion is { } dv)
                    CardCmd.Upgrade(dv, CardPreviewStyle.None);
            }
            await AssociationSystem.CheckCardUpgraded(choiceContext, card, Owner);
        }
        try { if (DeckVersion is { } dv) await CardPileCmd.RemoveFromDeck(dv, false); } catch { }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-3);
        AddKeyword(CardKeyword.Retain);
    }
}
