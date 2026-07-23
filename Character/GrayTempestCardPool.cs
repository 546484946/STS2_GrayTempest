using Godot;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Character;

[RegisterSharedCardPool]
public class GrayTempestCardPool : TypeListCardPoolModel
{
    public override string Title => GrayTempestConst.EnergyColorName;
    public override string EnergyColorName => GrayTempestConst.EnergyColorName;
    public override string? BigEnergyIconPath => GrayTempestConst.Paths.EnergyIconBig;
    public override string? TextEnergyIconPath => GrayTempestConst.Paths.EnergyIcon;
    public override string CardFrameMaterialPath => "card_frame_blue";
    public override Color DeckEntryCardColor => new("808080");
    public override Color EnergyOutlineColor => new("505050");
    public override bool IsColorless => false;
}

