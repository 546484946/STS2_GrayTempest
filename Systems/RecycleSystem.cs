using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using GrayTempest.Cards.Keywords;

namespace GrayTempest.Systems;

public static class RecycleSystem
{
    public static async Task RecycleCard(
        PlayerChoiceContext choiceContext,
        CardModel card,
        Player owner)
    {
        var hasRecyclable = card.CanonicalKeywords.Any(k => k.Equals(GrayTempestKeywords.Recyclable));
        var hpGain = hasRecyclable && card.DynamicVars.ContainsKey("ReconstructValue")
            ? card.DynamicVars["ReconstructValue"].IntValue : 1;
        await CreatureCmd.GainMaxHp(owner.Creature, hpGain);
        await CardCmd.Exhaust(choiceContext, card);
        try { if (card.DeckVersion is { } dv) await CardPileCmd.RemoveFromDeck(dv, false); } catch { }
    }
}
