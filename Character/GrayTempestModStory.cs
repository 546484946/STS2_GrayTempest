using GrayTempest.Epoch;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Timeline.Scaffolding;

namespace GrayTempest.Character;

[RegisterStory]
public class GrayTempestModStory : ModStoryTemplate
{
    protected override string StoryKey => GrayTempestTimelineKeys.TimelineStoryId;
}
