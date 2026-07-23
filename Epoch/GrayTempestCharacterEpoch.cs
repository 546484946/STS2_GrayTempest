using MegaCrit.Sts2.Core.Timeline;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Timeline.Scaffolding;

namespace GrayTempest.Epoch;

[RegisterEpoch]
[RegisterStoryEpoch(typeof(GrayTempestModStory))]
[AutoTimelineSlotBeforeColumn(EpochEra.Seeds0)]
[RequireAllCardsInPool(typeof(GrayTempestCardPool))]
public class GrayTempestCharacterEpoch : CharacterUnlockEpochTemplate<GrayTempestCharacter>
{
    public override string Id => GrayTempestTimelineKeys.CharacterEpochId;
    public override string StoryId => GrayTempestTimelineKeys.TimelineStoryId;
    public override EpochAssetProfile AssetProfile => new(
        PackedPortraitPath: GrayTempestConst.Paths.EpochPortrait,
        BigPortraitPath: GrayTempestConst.Paths.EpochPortrait);
    protected override IEnumerable<Type> ExpansionEpochTypes => [];
}
