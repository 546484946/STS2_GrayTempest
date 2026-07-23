using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using GrayTempest.Orbs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Pool;

[RegisterCard(typeof(GrayTempestCraftingCardPool))]
public class LaunchStrikeCraft : ModCardTemplate, IMultiUpgradable
{
    private int _upgradeCount;

    public LaunchStrikeCraft() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [GrayTempestKeywords.Recyclable, GrayTempestKeywords.ImproveCost, GrayTempestKeywords.MultiUpgrade, GrayTempestKeywords.Association];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("ChannelCount", 1), new IntVar("ReconstructValue", 5), new IntVar("ImproveCost", 8), new IntVar("MaxUpgrades", 4)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);
    public override int MaxUpgradeLevel => 0;

    public int UpgradeCount => _upgradeCount;
    public int MaxUpgrades => DynamicVars["MaxUpgrades"].IntValue;

    public void FakeUpgrade()
    {
        _upgradeCount++;
        DynamicVars["ChannelCount"].UpgradeValueBy(1);
        DynamicVars["ImproveCost"].UpgradeValueBy(8);
    }

    public override string Title
    {
        get
        {
            var title = base.Title;
            return $"{title} {RomanNumeral.ToRoman(_upgradeCount + 1)}";
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<FocusedStrikePower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
        for (int i = 0; i < DynamicVars["ChannelCount"].IntValue; i++)
            await OrbCmd.Channel<BasicStrikeCraftOrb>(choiceContext, Owner);
    }

    protected override void OnUpgrade() { }
}
