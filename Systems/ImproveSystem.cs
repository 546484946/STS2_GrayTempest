using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using GrayTempest.Cards.Pool;

namespace GrayTempest.Systems;

public static class ImproveSystem
{
    public static async Task ImproveCard(
        PlayerChoiceContext choiceContext,
        CardModel card,
        Player owner)
    {
        var cost = card.DynamicVars.ContainsKey("ImproveCost")
            ? card.DynamicVars["ImproveCost"].IntValue
            : 0;
        if (cost <= 0 || owner.Gold < cost)
            return;

        if (card is IMultiUpgradable mu && mu.UpgradeCount >= mu.MaxUpgrades)
            return;

        await PlayerCmd.LoseGold(cost, owner, GoldLossType.Spent);

        if (card is IMultiUpgradable multiUpgradable)
        {
            multiUpgradable.FakeUpgrade();
            if (card.DeckVersion is IMultiUpgradable dv)
                dv.FakeUpgrade();
        }
        else
        {
            CardCmd.Upgrade(card, CardPreviewStyle.None);
            if (card.DeckVersion is { } dv)
                CardCmd.Upgrade(dv, CardPreviewStyle.None);
        }
        await AssociationSystem.CheckCardUpgraded(choiceContext, card, owner);
    }
}