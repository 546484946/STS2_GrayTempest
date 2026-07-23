using System.Linq;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards;

[RegisterCard(typeof(GrayTempestCardPool))]
public class MemoryOptimization : ModCardTemplate
{
    public MemoryOptimization() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Amount", 2)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var selected = (await CardSelectCmd.FromHand(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1), null, this)).FirstOrDefault();
        if (selected != null)
            await CardCmd.Exhaust(choiceContext, selected);

        await CardCmd.Exhaust(choiceContext, this);

        var count = DynamicVars["Amount"].IntValue;
        for (int i = 0; i < count; i++)
        {
            var card = CombatState.CreateCard<Redundancy>(Owner);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Draw, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Amount"].UpgradeValueBy(1);
        EnergyCost.UpgradeBy(-1);
    }
}
