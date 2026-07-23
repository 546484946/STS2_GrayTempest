using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Common;

[RegisterCard(typeof(GrayTempestCardPool))]
public class ComputeAllocation : ModCardTemplate
{
    public ComputeAllocation() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("DrawCount", 2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner.PlayerCombatState.OrbQueue.Orbs.Count > 0)
        {
            await OrbCmd.EvokeLast(choiceContext, Owner, true);
            await Cmd.CustomScaledWait(0.1f, 0.25f);
        }
        int drawCount = IsUpgraded ? 3 : 2;
        await CardPileCmd.Draw(choiceContext, (decimal)drawCount, Owner, false);
    }

    protected override void OnUpgrade() { DynamicVars["DrawCount"].UpgradeValueBy(1); }
}
