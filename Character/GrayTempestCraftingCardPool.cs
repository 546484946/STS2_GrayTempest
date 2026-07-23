using Godot;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Character;

[RegisterSharedCardPool]
public class GrayTempestCraftingCardPool : TypeListCardPoolModel
{
    public override string Title => GrayTempestConst.EnergyColorName + " crafting";
    public override string EnergyColorName => GrayTempestConst.EnergyColorName;
    public override string CardFrameMaterialPath => "card_frame_blue";
    public override Color DeckEntryCardColor => Colors.Gray;
    public override bool IsColorless => false;
    public override bool ShouldAddToDeck(CardModel card) => true;
}

