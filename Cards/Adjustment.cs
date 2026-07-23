using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using GrayTempest.Character;
using STS2RitsuLib.Combat.HandSize;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards;

[RegisterCard(typeof(GrayTempestCardPool))]
public class Adjustment : ModCardTemplate
{
    public Adjustment() : base(1, CardType.Power, CardRarity.Common, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await OrbCmd.AddSlots(Owner, -1);
        await PowerCmd.Apply<AdjustmentPower>(choiceContext, Owner.Creature, DynamicVars["Cards"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Cards"].UpgradeValueBy(1);
    }
}

[RegisterPower]
public class AdjustmentPower : ModPowerTemplate, IMaxHandSizeModifier
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerAssetProfile AssetProfile => new(
        IconPath: GrayTempestConst.Paths.CardPlaceholder,
        BigIconPath: GrayTempestConst.Paths.CardPlaceholder
    );

    public int ModifyMaxHandSize(Player player, int currentMaxHandSize)
    {
        if (player.Creature != Owner) return currentMaxHandSize;
        return currentMaxHandSize + 1;
    }

    public int ModifyMaxHandSizeLate(Player player, int currentMaxHandSize)
    {
        return currentMaxHandSize;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner) return;
        await CardPileCmd.Draw(choiceContext, Amount, player);
    }
}
