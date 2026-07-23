using System.Collections.Generic;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using GrayTempest.Cards.Pool;
using GrayTempest.Relics;

namespace GrayTempest.Systems;

public static class AssociationSystem
{
    private static readonly Dictionary<int, HashSet<string>> _granted = new();

    public static bool IsGranted(Player player, string id)
    {
        return _granted.TryGetValue(player.GetHashCode(), out var set) && set.Contains(id);
    }

    public static void MarkGranted(Player player, string id)
    {
        var key = player.GetHashCode();
        if (!_granted.TryGetValue(key, out var set))
            _granted[key] = set = new HashSet<string>();
        set.Add(id);
    }

    public static async Task CheckCardAdded(PlayerChoiceContext ctx, CardModel card, Player owner)
    {
        if (card is ShieldI && !IsGranted(owner, "shield"))
        {
            await RelicCmd.Obtain<ShieldRelic>(owner);
            MarkGranted(owner, "shield");
        }

        if (card is RepairArmor && !IsGranted(owner, "armor"))
        {
            await RelicCmd.Obtain<ArmorRelic>(owner);
            MarkGranted(owner, "armor");
        }

        if (card is BackupPower && !IsGranted(owner, "reactor"))
        {
            await RelicCmd.Obtain<ReactorRelic>(owner);
            MarkGranted(owner, "reactor");
        }

        if (card is LaunchStrikeCraft && !IsGranted(owner, "strikecraft"))
        {
            await RelicCmd.Obtain<StrikeCraftRelic>(owner);
            MarkGranted(owner, "strikecraft");
        }

        if (card is TacticalAdvance && !IsGranted(owner, "thruster"))
        {
            await RelicCmd.Obtain<ThrusterRelic>(owner);
            MarkGranted(owner, "thruster");
        }
    }

    public static void ClearCombatState()
    {
        _granted.Clear();
    }

    public static async Task CheckCardUpgraded(PlayerChoiceContext ctx, CardModel card, Player owner)
    {
        int level = 1;
        if (card is IMultiUpgradable mu)
            level = mu.UpgradeCount + 1;
        else if (card.IsUpgraded)
            level = 2;

        if (card is ShieldI)
        {
            var relic = owner.GetRelic<ShieldRelic>();
            if (relic != null && level > relic.Counter)
                relic.Counter = level;
        }

        if (card is RepairArmor)
        {
            var relic = owner.GetRelic<ArmorRelic>();
            if (relic != null && level > relic.Counter)
                relic.Counter = level;
        }

        if (card is BackupPower)
        {
            var relic = owner.GetRelic<ReactorRelic>();
            if (relic != null && level > relic.Counter)
                relic.Counter = level;
        }

        if (card is LaunchStrikeCraft)
        {
            var relic = owner.GetRelic<StrikeCraftRelic>();
            if (relic != null && level > relic.Counter)
                relic.Counter = level;
        }

        if (card is TacticalAdvance)
        {
            var relic = owner.GetRelic<ThrusterRelic>();
            if (relic != null && level > relic.Counter)
                relic.Counter = level;
        }
    }
}
