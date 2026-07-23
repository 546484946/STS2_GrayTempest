using Godot;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Character;

[RegisterSharedRelicPool]
public class GrayTempestAssociationRelicPool : TypeListRelicPoolModel
{
    public override string EnergyColorName => GrayTempestConst.EnergyColorName;
    public override string? BigEnergyIconPath => GrayTempestConst.Paths.EnergyIconBig;
    public override string? TextEnergyIconPath => GrayTempestConst.Paths.EnergyIcon;
    public override Color LabOutlineColor => GrayTempestCharacter.CharacterColor;
}
