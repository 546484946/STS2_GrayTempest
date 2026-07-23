using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Godot;

namespace GrayTempest.Orbs;

[RegisterOrb]
public class BasicStrikeCraftOrb : ModOrbTemplate
{
    public override Color DarkenedColor => new(0.2f, 0.2f, 0.2f);
    public override decimal PassiveVal => ModifyOrbValue(4m);
    public override decimal EvokeVal => ModifyOrbValue(10m);
    protected override string ChannelSfx => "";
    protected override string PassiveSfx => "";
    protected override string EvokeSfx => "";

    public override OrbAssetProfile AssetProfile => new(
        IconPath: "res://GrayTempest/orbs/strike_craft.png",
        VisualsScenePath: "res://GrayTempest/scenes/strike_craft_orb.tscn"
    );

    protected override Node2D? TryCreateOrbSprite() =>
        RitsuGodotNodeFactories.CreateFromScenePath<Node2D>(AssetProfile.VisualsScenePath!);

    public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
    {
        await Passive(choiceContext, null);
    }

    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        ActivatePassive();
        if (Owner?.Creature is not { } creature) return;
        if (creature.CombatState is not { } cs) return;
        var enemies = cs.Enemies.Where(e => e.IsHittable).ToList();
        if (enemies.Count == 0) return;

        var pick = enemies[System.Random.Shared.Next(enemies.Count)];
        VfxCmd.PlayOnCreature(pick, "vfx/vfx_attack_blunt");
        await CreatureCmd.Damage(choiceContext, new List<Creature> { pick }, PassiveVal, ValueProp.Unpowered, creature);
    }

    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext choiceContext)
    {
        if (Owner?.Creature is not { } creature) return [];
        if (creature.CombatState is not { } combatState) return [];

        var enemies = combatState.Enemies.Where(e => e.IsHittable).ToList();
        if (enemies.Count == 0) return [];

        ActivateEvoke(enemies.ToArray());
        foreach (var enemy in enemies)
        {
            await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), enemy, 1m, creature, null);
            VfxCmd.PlayOnCreature(enemy, "vfx/vfx_attack_blunt");
        }
        await CreatureCmd.Damage(choiceContext, enemies, EvokeVal, ValueProp.Unpowered, creature);
        return enemies;
    }
}
