using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using GrayTempest.Cards.Rare;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Godot;

namespace GrayTempest.Orbs;

[RegisterOrb]
public class OrbitalBombardmentUnit : ModOrbTemplate
{
    private decimal _evokeVal = 3m;

    public override Color DarkenedColor => new(0.8f, 0.3f, 0.1f);
    public override decimal PassiveVal => ModifyOrbValue(3m);
    public override decimal EvokeVal => _evokeVal;
    protected override string ChannelSfx => "";
    protected override string PassiveSfx => "";
    protected override string EvokeSfx => "";

    public override OrbAssetProfile AssetProfile => new(
        IconPath: GrayTempestConst.Paths.OrbOrbitalBombardment,
        VisualsScenePath: GrayTempestConst.Paths.SceneOrbitalBombardmentOrb
    );

    protected override Node2D? TryCreateOrbSprite() =>
        RitsuGodotNodeFactories.CreateFromScenePath<Node2D>(AssetProfile.VisualsScenePath!);

    public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
    {
        await TriggerPassive(choiceContext, null);
    }

    public override Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        ActivatePassive();
        _evokeVal += PassiveVal;
        NCombatRoom.Instance?.GetCreatureNode(base.Owner.Creature)?.OrbManager?.UpdateVisuals(default);
        return Task.CompletedTask;
    }

    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext choiceContext)
    {
        var enemies = base.CombatState.HittableEnemies;
        if (enemies.Count == 0) return [];

        var dmg = EvokeVal;
        var power = base.Owner.Creature.GetPower<ArmageddonPower>();
        if (power != null)
            dmg = Math.Ceiling(dmg * (1m + power.Amount / 100m));

        ActivateEvoke(enemies.ToArray());
        await CreatureCmd.Damage(choiceContext, enemies, dmg, ValueProp.Unpowered, base.Owner.Creature);

        var otherOrb = base.Owner.PlayerCombatState.OrbQueue.Orbs
            .OfType<OrbitalBombardmentUnit>()
            .FirstOrDefault(o => o != this);
        if (otherOrb != null)
            otherOrb._evokeVal += dmg;

        return enemies;
    }
}
