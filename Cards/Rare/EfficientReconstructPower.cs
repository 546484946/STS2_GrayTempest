using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using GrayTempest.Cards.Keywords;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Rare;

[RegisterPower]
public class EfficientReconstructPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Amount", 2)];
    public override PowerAssetProfile AssetProfile => new(
        IconPath: GrayTempestConst.Paths.CardEfficientReconstruct,
        BigIconPath: GrayTempestConst.Paths.CardEfficientReconstruct
    );

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.CanonicalKeywords.Any(k => k.Equals(GrayTempestKeywords.Reconstruct)))
            await CreatureCmd.GainMaxHp(Owner, Amount);
    }
}