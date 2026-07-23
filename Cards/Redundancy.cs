using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards;

[RegisterCard(typeof(StatusCardPool))]
public class Redundancy : ModCardTemplate
{
    public Redundancy() : base(0, CardType.Status, CardRarity.Common, TargetType.Self) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CardPileCmd.Draw(choiceContext, 1m, Owner);
        await PlayerCmd.GainEnergy(1m, Owner);
    }

    protected override void OnUpgrade() { }
}
