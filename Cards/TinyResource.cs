using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards;

[RegisterCard(typeof(StatusCardPool))]
public class TinyResource : ModCardTemplate
{
    public TinyResource() : base(1, CardType.Status, CardRarity.Common, TargetType.Self) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, GrayTempestKeywords.Recyclable];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("ReconstructValue", 2), new IntVar("GoldAmount", 5)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PlayerCmd.GainGold((decimal)DynamicVars["GoldAmount"].IntValue, Owner);
    }

    protected override void OnUpgrade() { }
}
