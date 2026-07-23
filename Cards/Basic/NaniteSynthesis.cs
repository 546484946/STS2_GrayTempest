using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using GrayTempest.Systems;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using MegaCrit.Sts2.Core.Logging;

namespace GrayTempest.Cards.Basic;

[RegisterCard(typeof(GrayTempestCardPool))]
[RegisterCharacterStarterCard(typeof(GrayTempestCharacter))]
public class NaniteSynthesis : ModCardTemplate
{
    public NaniteSynthesis() : base(2, CardType.Skill, CardRarity.Basic, TargetType.Self) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, GrayTempestKeywords.Reconstruct];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardBasicReconstruct, GrayTempestConst.Paths.CardBasicReconstruct);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        try
        {
            Log.Info("[GrayTempest] NaniteSynthesis played, starting reconstruct...");
            var result = await ReconstructCmd.Execute(choiceContext, this);
            Log.Info($"[GrayTempest] Reconstruct result: {(result != null ? result.GetType().Name : "null")}");
        }
        catch (Exception ex)
        {
            Log.Error($"[GrayTempest] NaniteSynthesis failed: {ex}");
        }
    }
    protected override void OnUpgrade() { RemoveKeyword(CardKeyword.Exhaust); }
}
