using System.Linq;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Uncommon;

[RegisterCard(typeof(GrayTempestCardPool))]
public class DoubleCast : ModCardTemplate
{
    public DoubleCast() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (IsUpgraded)
        {
            foreach (var card in PileType.Hand.GetPile(Owner).Cards)
            {
                if (card.CanonicalKeywords.Any(k => k.Equals(GrayTempestKeywords.OneShot)))
                    card.BaseReplayCount += 1;
            }
        }
        else
        {
            var prefs = new CardSelectorPrefs(new LocString("cards", "GRAY_TEMPEST_DOUBLE_CAST_CHOOSE"), 1);
            var selected = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, c =>
                c.CanonicalKeywords.Any(k => k.Equals(GrayTempestKeywords.OneShot)), this)).ToList();
            foreach (var card in selected)
                card.BaseReplayCount += 1;
        }
    }

    protected override void OnUpgrade() { }
}
