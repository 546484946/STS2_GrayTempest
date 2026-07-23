using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using GrayTempest.Systems;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Uncommon;

[RegisterCard(typeof(GrayTempestCardPool))]
public class Decomposition : ModCardTemplate
{
    public Decomposition() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, GrayTempestKeywords.Recycle];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardDecomposition, GrayTempestConst.Paths.CardDecomposition);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var handCards = PileType.Hand.GetPile(Owner).Cards
            .Where(c => c.Type == CardType.Status || c.Type == CardType.Curse)
            .ToList();

        foreach (var card in handCards)
            await RecycleSystem.RecycleCard(choiceContext, card, Owner);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
        AddKeyword(CardKeyword.Retain);
    }
}
