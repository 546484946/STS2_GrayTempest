using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Basic;

[RegisterCard(typeof(GrayTempestCardPool))]
[RegisterCharacterStarterCard(typeof(GrayTempestCharacter))]
public class StrikeCraftDirective : ModCardTemplate
{
    public StrikeCraftDirective() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self) { }
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6, ValueProp.Move), new IntVar("DrawCount", 0)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardStrikeCraftDirective, GrayTempestConst.Paths.CardStrikeCraftDirective);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        var orbQueue = Owner.PlayerCombatState.OrbQueue;
        foreach (var orb in orbQueue.Orbs)
        {
            await OrbCmd.Passive(choiceContext, orb, null, false);
        }
        if (IsUpgraded)
            await CardPileCmd.Draw(choiceContext, 1m, Owner, false);
    }
    protected override void OnUpgrade() { DynamicVars["Block"].UpgradeValueBy(4m); DynamicVars["DrawCount"].UpgradeValueBy(1); }
}
