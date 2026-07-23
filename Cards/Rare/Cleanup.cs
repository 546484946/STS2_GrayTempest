using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Rare;

[RegisterCard(typeof(GrayTempestCardPool))]
public class Cleanup : ModCardTemplate
{
    public Cleanup() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("RedundancyCount", 2)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var handCount = PileType.Hand.GetPile(Owner).Cards.Count();
        var prefs = new CardSelectorPrefs(new LocString("cards", "GRAY_TEMPEST_CLEANUP_CHOOSE"), 0, handCount);
        var discarded = (await CardSelectCmd.FromHandForDiscard(
            prefs: prefs, context: choiceContext, player: Owner, filter: null, source: this)).ToList();

        if (discarded.Count > 0)
            await CardCmd.Discard(choiceContext, discarded);

        if (discarded.Count >= 5)
        {
            int count = IsUpgraded ? DynamicVars["RedundancyCount"].IntValue + 1 : DynamicVars["RedundancyCount"].IntValue;
            for (int i = 0; i < count; i++)
            {
                if (Owner is not null && CombatState is not null)
                {
                    var redundancy = CombatState.CreateCard<Redundancy>(Owner);
                    await CardPileCmd.AddGeneratedCardToCombat(redundancy, PileType.Hand, Owner);
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["RedundancyCount"].UpgradeValueBy(1);
    }
}
