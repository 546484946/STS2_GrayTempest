using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using GrayTempest.Systems;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Uncommon;

[RegisterCard(typeof(GrayTempestCardPool))]
public class Recycling : ModCardTemplate
{
    public Recycling() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, GrayTempestKeywords.Recycle];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardRecycling, GrayTempestConst.Paths.CardRecycling);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var prefs = new CardSelectorPrefs(new LocString("cards", "GRAY_TEMPEST_RECYCLE_CHOOSE"), 1);
        var selected = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).ToList();
        if (selected.Count == 0) return;
        await RecycleSystem.RecycleCard(choiceContext, selected[0], Owner);
    }
    protected override void OnUpgrade() { EnergyCost.UpgradeBy(-1); AddKeyword(CardKeyword.Retain); }
}
