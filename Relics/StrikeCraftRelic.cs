using System.Collections.Generic;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using GrayTempest.Character;
using GrayTempest.Orbs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Relics;

[RegisterRelic(typeof(GrayTempestAssociationRelicPool))]
public class StrikeCraftRelic : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override bool ShowCounter => true;

    private int _counter = 1;

    [SavedProperty]
    public int Counter
    {
        get => _counter;
        set
        {
            AssertMutable();
            _counter = value;
            DynamicVars["Counter"].BaseValue = value;
            UpdateDisplay();
        }
    }

    public override int DisplayAmount => Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Counter", 1m)];

    public override RelicAssetProfile AssetProfile => new(
        GrayTempestConst.Paths.RelicGrayGooCore,
        GrayTempestConst.Paths.RelicGrayGooCore,
        GrayTempestConst.Paths.RelicGrayGooCore
    );

    private void UpdateDisplay()
    {
        InvokeDisplayAmountChanged();
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner) return;
        if (player.PlayerCombatState?.TurnNumber != 1) return;
        Flash();
        await OrbCmd.AddSlots(Owner, Counter);
        for (int i = 0; i < Counter; i++)
            await OrbCmd.Channel<BasicStrikeCraftOrb>(choiceContext, Owner);
    }
}
