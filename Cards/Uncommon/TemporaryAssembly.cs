using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using GrayTempest.Cards.Common;
using GrayTempest.Cards.Rare;
// SimpleImprove and SimpleRecycle are in GrayTempest.Cards.Common (already imported)
using GrayTempest.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Uncommon;

public static class OneShotCardPool
{
    private static readonly List<System.Type> _cardTypes = [];

    public static IReadOnlyList<System.Type> CardTypes => _cardTypes.AsReadOnly();

    public static void Register<T>() where T : CardModel
    {
        if (!_cardTypes.Contains(typeof(T)))
            _cardTypes.Add(typeof(T));
    }

    [ModuleInitializer]
    internal static void Init()
    {
        Register<SimpleMissile>();
        Register<SimpleShieldGenerator>();
        Register<FieldUpgrade>();
        Register<SmallResources>();
        Register<SomeResources>();
        Register<LargeResources>();
        Register<SimpleImprove>();
        Register<SimpleRecycle>();
    }
}

[RegisterCard(typeof(GrayTempestCardPool))]
public class TemporaryAssembly : ModCardTemplate
{
    public TemporaryAssembly() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<TemporaryAssemblyPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}

[RegisterPower]
public class TemporaryAssemblyPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerAssetProfile AssetProfile => new(
        IconPath: GrayTempestConst.Paths.CardPlaceholder,
        BigIconPath: GrayTempestConst.Paths.CardPlaceholder
    );

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        if (OneShotCardPool.CardTypes.Count == 0) return;
        var player = room.CombatState.Players.FirstOrDefault(p => p.Creature == Owner);
        if (player == null) return;

        var idx = System.Random.Shared.Next(OneShotCardPool.CardTypes.Count);
        var cardType = OneShotCardPool.CardTypes[idx];
        var canonical = ModelDb.GetById<CardModel>(ModelDb.GetId(cardType));
        var card = ((RunState)player.RunState).CreateCard(canonical, player);
        await CardPileCmd.Add([card], player.Deck, CardPilePosition.Bottom, this, false, false);
    }
}
