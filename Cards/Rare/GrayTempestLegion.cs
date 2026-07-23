using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using GrayTempest.Orbs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Rare;

[RegisterCard(typeof(GrayTempestCardPool))]
public class GrayTempestLegion : ModCardTemplate
{
    public GrayTempestLegion() : base(1, CardType.Skill, CardRarity.Ancient, TargetType.Self) { }
    public override bool GainsBlock => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(12m, ValueProp.Move), new IntVar("ChannelCount", 1)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardGrayTempestLegion, GrayTempestConst.Paths.CardGrayTempestLegion);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var count = DynamicVars["ChannelCount"].IntValue;

        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);

        for (int i = 0; i < count; i++)
        {
            await OrbCmd.Channel<BasicStrikeCraftOrb>(choiceContext, Owner);
            await OrbCmd.Channel<ShieldGeneratorNode>(choiceContext, Owner);
            await OrbCmd.Channel<OrbitalBombardmentUnit>(choiceContext, Owner);
        }

        var orbQueue = Owner.PlayerCombatState.OrbQueue;
        foreach (var orb in orbQueue.Orbs)
            await OrbCmd.Passive(choiceContext, orb, null, false);
    }

    protected override CardLocation GetResultLocationForCardPlay()
    {
        var result = base.GetResultLocationForCardPlay();
        if (result.pileType == PileType.Discard)
            result.pileType = PileType.Hand;
        return result;
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Block"].UpgradeValueBy(4m);
        DynamicVars["ChannelCount"].UpgradeValueBy(1);
    }
}
