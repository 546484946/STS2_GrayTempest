using System.Linq;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Rare;

[RegisterCard(typeof(GrayTempestCardPool))]
public class ColossusProject : ModCardTemplate
{
    public ColossusProject() : base(5, CardType.Power, CardRarity.Ancient, TargetType.Self) { }
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardColossusProject, GrayTempestConst.Paths.CardColossusProject);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<ColossusProjectPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-2);
    }
}

[RegisterPower]
public class ColossusProjectPower : ModPowerTemplate
{
    private bool _readyToWin;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerAssetProfile AssetProfile => new(
        IconPath: GrayTempestConst.Paths.CardPlaceholder,
        BigIconPath: GrayTempestConst.Paths.CardPlaceholder
    );

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner) return;

        if (_readyToWin)
        {
            var enemies = Owner.CombatState.Enemies.Where(e => e.IsAlive).ToList();
            foreach (var enemy in enemies)
                await CreatureCmd.Escape(enemy);
        }

        _readyToWin = true;
    }
}
