using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using GrayTempest.Cards;
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Relics;

[RegisterRelic(typeof(GrayTempestAssociationRelicPool))]
public class ReactorRelic : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override bool ShowCounter => true;

    private int _counter = 1;
    private int _grantedThisCombat;

    [SavedProperty]
    public int Counter
    {
        get => _counter;
        set
        {
            AssertMutable();
            _counter = value;
            DynamicVars["Counter"].BaseValue = value;
            UpdateDisplay();
        }
    }

    public override int DisplayAmount => Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Counter", 1m)];

    public override RelicAssetProfile AssetProfile => new(
        GrayTempestConst.Paths.RelicGrayGooCore,
        GrayTempestConst.Paths.RelicGrayGooCore,
        GrayTempestConst.Paths.RelicGrayGooCore
    );

    private void UpdateDisplay()
    {
        InvokeDisplayAmountChanged();
    }

    public override Task BeforeCombatStart()
    {
        _grantedThisCombat = 0;
        return Task.CompletedTask;
    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature)) return;
        if (_grantedThisCombat >= Counter) return;

        _grantedThisCombat++;
        var card = combatState.CreateCard<Redundancy>(Owner);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }
}
