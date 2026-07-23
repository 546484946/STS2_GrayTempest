using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using GrayTempest.Character;
using GrayTempest.Orbs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Relics;

[RegisterRelic(typeof(GrayTempestRelicPool))]
[RegisterRelic(typeof(GrayTempestSharedRelicPool))]
public class GrayGooCorePlus : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override RelicAssetProfile AssetProfile => new(GrayTempestConst.Paths.RelicGrayGooCore, GrayTempestConst.Paths.RelicGrayGooCore, GrayTempestConst.Paths.RelicGrayGooCore);

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner) return;
        if (player.PlayerCombatState?.TurnNumber != 1) return;
        Flash();
        await CreatureCmd.GainMaxHp(Owner.Creature, 3);
        await PowerCmd.Apply<ArtifactPower>(choiceContext, Owner.Creature, 3m, Owner.Creature, null);
        await OrbCmd.Channel<BasicStrikeCraftOrb>(choiceContext, Owner);
        await OrbCmd.Channel<ShieldGeneratorNode>(choiceContext, Owner);
        await OrbCmd.Channel<OrbitalBombardmentUnit>(choiceContext, Owner);
    }
}
