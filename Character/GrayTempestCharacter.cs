using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Scaffolding.Godot;

namespace GrayTempest.Character;

[RegisterCharacter]
public class GrayTempestCharacter : ModCharacterTemplate<GrayTempestCardPool, GrayTempestRelicPool, GrayTempestPotionPool>
{
    public static readonly Color CharacterColor = new("808080");
    public override Color NameColor => CharacterColor;
    public override Color MapDrawingColor => Colors.Gray;
    public override Color EnergyLabelOutlineColor => CharacterColor;
    public override int StartingHp => 50;
    public override int StartingGold => 99;
    public override int BaseOrbSlotCount => 5;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override float AttackAnimDelay => 0.15f;
    public override float CastAnimDelay => 0.25f;
    public override bool RequiresEpochAndTimeline => false;

    protected override Type? UnlocksAfterRunAsType => typeof(Ironclad);

    public override CharacterAssetProfile AssetProfile => CharacterAssetProfiles.Merge(
        CharacterAssetProfiles.Ironclad(),
        new(
            Scenes: new(
                VisualsPath: GrayTempestConst.Paths.SceneCharacter,
                EnergyCounterPath: GrayTempestConst.Paths.SceneEnergyCounter,
                MerchantAnimPath: GrayTempestConst.Paths.SceneMerchant,
                RestSiteAnimPath: GrayTempestConst.Paths.SceneRestSite
            ),
            Ui: new(
                IconTexturePath:GrayTempestConst.Paths.Icon,
                CharacterSelectBgPath: GrayTempestConst.Paths.SceneSelectBg,
                IconPath: GrayTempestConst.Paths.CharacterIcon,
                CharacterSelectIconPath: GrayTempestConst.Paths.CharacterSelectIcon,
                MapMarkerPath:GrayTempestConst.Paths.Icon
            )
        ));

    protected override NCreatureVisuals? TryCreateCreatureVisuals() =>
        RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.Scenes!.VisualsPath!);

    public override List<string> GetArchitectAttackVfx() =>
        ["vfx/vfx_attack_blunt", "vfx/vfx_heavy_blunt", "vfx/vfx_attack_slash"];
}