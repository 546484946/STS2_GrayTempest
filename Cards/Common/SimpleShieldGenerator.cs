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

namespace GrayTempest.Cards.Common;

[RegisterCard(typeof(GrayTempestCardPool))]
public class SimpleShieldGenerator : ModCardTemplate
{
    public SimpleShieldGenerator() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self) { }
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8, ValueProp.Move), new IntVar("ImproveCost", 10), new IntVar("ChannelCount", 1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Retain, GrayTempestKeywords.OneShot, GrayTempestKeywords.ImproveCost];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardSimpleShieldGenerator, GrayTempestConst.Paths.CardSimpleShieldGenerator);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        int count = IsUpgraded ? 3 : 1;
        for (int i = 0; i < count; i++)
            await OrbCmd.Channel<ShieldGeneratorNode>(choiceContext, Owner);
        try { if (DeckVersion is { } dv) await CardPileCmd.RemoveFromDeck(dv, false); } catch { }
    }
    protected override void OnUpgrade() { DynamicVars["Block"].UpgradeValueBy(12m); DynamicVars["ChannelCount"].UpgradeValueBy(2); }
}
