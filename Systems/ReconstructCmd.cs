using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace GrayTempest.Systems;

public static class ReconstructCmd
{
    private const int ReconstructValue = 5;

    public static async Task<CardModel?> Execute(
        PlayerChoiceContext choiceContext,
        CardModel sourceCard)
    {
        var owner = sourceCard.Owner
            ?? throw new InvalidOperationException("Reconstruct card has no owner.");
        var creature = owner.Creature;
        var combatState = creature.CombatState
            ?? throw new InvalidOperationException("Reconstruct used outside combat.");

        var factories = ReconstructCardPool.GetPoolFactories(ReconstructValue);
        if (factories.Count == 0) return null;

        await CreatureCmd.LoseMaxHp(choiceContext, creature, ReconstructValue, true);

        var options = new List<CardModel>();
        foreach (var factory in factories)
        {
            try
            {
                if (factory(combatState, owner) is { } card)
                    options.Add(card);
            }
            catch { }
        }

        if (options.Count == 0) return null;

        var prefs = new CardSelectorPrefs(
            new LocString("cards", "GRAY_TEMPEST_RECONSTRUCT_CHOOSE"), 1);

        var selected = (await CardSelectCmd.FromSimpleGrid(
            choiceContext, options, owner, prefs)).ToList();

        foreach (var option in options)
        {
            if (!selected.Contains(option))
                option.RemoveFromState();
        }

        if (selected.Count == 0) return null;

        var chosen = selected[0];
        await CardPileCmd.AddGeneratedCardToCombat(chosen, PileType.Hand, owner);

        var canonical = ModelDb.GetById<CardModel>(ModelDb.GetId(chosen.GetType()));
        var deckCard = ((RunState)owner.RunState).CreateCard(canonical, owner);
        await CardPileCmd.Add(deckCard, PileType.Deck);

        await AssociationSystem.CheckCardAdded(choiceContext, chosen, owner);

        return chosen;
    }
}
