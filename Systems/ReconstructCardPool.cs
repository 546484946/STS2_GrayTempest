using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using GrayTempest.Cards.Pool;

namespace GrayTempest.Systems;

public static class ReconstructCardPool
{
    public delegate CardModel? CardFactory(ICombatState state, Player owner);

    public static IReadOnlyList<CardFactory> GetPoolFactories(int reconstructValue)
    {
        return reconstructValue switch
        {
            5 => new CardFactory[]
            {
                (s, o) => s.CreateCard<Laser>(o),
                (s, o) => s.CreateCard<KineticCannon>(o),
                (s, o) => s.CreateCard<Missile>(o),
                (s, o) => s.CreateCard<ShieldI>(o),
                (s, o) => s.CreateCard<RepairArmor>(o),
                (s, o) => s.CreateCard<BackupPower>(o),
                (s, o) => s.CreateCard<LaunchStrikeCraft>(o),
                (s, o) => s.CreateCard<TacticalAdvance>(o),
            },
            _ => Array.Empty<CardFactory>()
        };
    }
}
