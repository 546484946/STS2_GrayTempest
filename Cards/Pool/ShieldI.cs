using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using GrayTempest.Orbs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Pool;

[RegisterCard(typeof(GrayTempestCraftingCardPool))]
public class ShieldI : ModCardTemplate, IMultiUpgradable
{
    private int _upgradeCount;

    public ShieldI() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }
    public override bool GainsBlock => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, GrayTempestKeywords.Recyclable, GrayTempestKeywords.ImproveCost, GrayTempestKeywords.MultiUpgrade, GrayTempestKeywords.Association];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(3m, ValueProp.Move), new IntVar("ReconstructValue", 5), new IntVar("ImproveCost", 8), new IntVar("MaxUpgrades", 4)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);
    public override int MaxUpgradeLevel => 0;

    public int UpgradeCount => _upgradeCount;
    public int MaxUpgrades => DynamicVars["MaxUpgrades"].IntValue;

    public void FakeUpgrade()
    {
        _upgradeCount++;
        DynamicVars["Block"].UpgradeValueBy(3m);
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
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        await OrbCmd.Channel<ShieldGeneratorNode>(choiceContext, Owner);
        await PowerCmd.Apply<FocusedStrikePower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade() { }
}
