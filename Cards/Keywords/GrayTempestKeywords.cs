using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace GrayTempest.Cards.Keywords;

[RegisterOwnedCardKeyword(nameof(Reconstruct))]
[RegisterOwnedCardKeyword(nameof(Recyclable))]
[RegisterOwnedCardKeyword(nameof(Recycle))]
    [RegisterOwnedCardKeyword(nameof(OneShot))]
    [RegisterOwnedCardKeyword(nameof(ImproveAction))]
    [RegisterOwnedCardKeyword(nameof(ImproveCost))]
    [RegisterOwnedCardKeyword(nameof(MultiUpgrade))]
    [RegisterOwnedCardKeyword(nameof(Nanite))]
    [RegisterOwnedCardKeyword(nameof(Energy))]
    [RegisterOwnedCardKeyword(nameof(Kinetic))]
    [RegisterOwnedCardKeyword(nameof(Burst))]
    [RegisterOwnedCardKeyword(nameof(Association))]
    public class GrayTempestKeywords
    {
        public static readonly CardKeyword Reconstruct = ModContentRegistry.GetQualifiedKeywordId(GrayTempestConst.ModId, nameof(Reconstruct)).GetModCardKeyword();
        public static readonly CardKeyword Recyclable = ModContentRegistry.GetQualifiedKeywordId(GrayTempestConst.ModId, nameof(Recyclable)).GetModCardKeyword();
        public static readonly CardKeyword Recycle = ModContentRegistry.GetQualifiedKeywordId(GrayTempestConst.ModId, nameof(Recycle)).GetModCardKeyword();
        public static readonly CardKeyword OneShot = ModContentRegistry.GetQualifiedKeywordId(GrayTempestConst.ModId, nameof(OneShot)).GetModCardKeyword();
        public static readonly CardKeyword ImproveAction = ModContentRegistry.GetQualifiedKeywordId(GrayTempestConst.ModId, nameof(ImproveAction)).GetModCardKeyword();
        public static readonly CardKeyword ImproveCost = ModContentRegistry.GetQualifiedKeywordId(GrayTempestConst.ModId, nameof(ImproveCost)).GetModCardKeyword();
        public static readonly CardKeyword MultiUpgrade = ModContentRegistry.GetQualifiedKeywordId(GrayTempestConst.ModId, nameof(MultiUpgrade)).GetModCardKeyword();
        public static readonly CardKeyword Nanite = ModContentRegistry.GetQualifiedKeywordId(GrayTempestConst.ModId, nameof(Nanite)).GetModCardKeyword();
        public static readonly CardKeyword Energy = ModContentRegistry.GetQualifiedKeywordId(GrayTempestConst.ModId, nameof(Energy)).GetModCardKeyword();
        public static readonly CardKeyword Kinetic = ModContentRegistry.GetQualifiedKeywordId(GrayTempestConst.ModId, nameof(Kinetic)).GetModCardKeyword();
        public static readonly CardKeyword Burst = ModContentRegistry.GetQualifiedKeywordId(GrayTempestConst.ModId, nameof(Burst)).GetModCardKeyword();
        public static readonly CardKeyword Association = ModContentRegistry.GetQualifiedKeywordId(GrayTempestConst.ModId, nameof(Association)).GetModCardKeyword();
    }
