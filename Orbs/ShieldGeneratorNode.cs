using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Godot;

namespace GrayTempest.Orbs;

[RegisterOrb]
public class ShieldGeneratorNode : ModOrbTemplate
{
    public override Color DarkenedColor => new(0.5f, 0.7f, 0.9f);
    public override decimal PassiveVal => ModifyOrbValue(3m);
    public override decimal EvokeVal => ModifyOrbValue(7m);
    protected override string ChannelSfx => "";
    protected override string PassiveSfx => "";
    protected override string EvokeSfx => "";

    public override OrbAssetProfile AssetProfile => new(
        IconPath: GrayTempestConst.Paths.OrbShieldGenerator,
        VisualsScenePath: GrayTempestConst.Paths.SceneShieldGeneratorOrb
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
        await CreatureCmd.GainBlock(base.Owner.Creature, PassiveVal, ValueProp.Unpowered, null);
    }

    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext choiceContext)
    {
        ActivateEvoke(new Creature[1] { base.Owner.Creature });
        await CreatureCmd.GainBlock(base.Owner.Creature, EvokeVal, ValueProp.Unpowered, null);
        return [base.Owner.Creature];
    }
}
