using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using Godot;
using GrayTempest.Cards.Basic;
using GrayTempest.Cards.Common;
using GrayTempest.Cards.Uncommon;
using GrayTempest.Cards.Rare;
using GrayTempest.Cards.Pool;

namespace GrayTempest.Patches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.PortraitPath), MethodType.Getter)]
public static class CardPortraitPatch
{
    private static readonly Dictionary<string, string> CustomPortraits = new(StringComparer.OrdinalIgnoreCase)
    {
        [nameof(GrayTempestStrike)] = GrayTempestConst.Paths.Root + "/cards/portraits/strike.png",
        [nameof(GrayTempestDefend)] = GrayTempestConst.Paths.Root + "/cards/portraits/defend.png",
        [nameof(NaniteSynthesis)] = GrayTempestConst.Paths.Root + "/cards/portraits/nanite_synthesis.png",
        [nameof(StrikeCraftDirective)] = GrayTempestConst.Paths.Root + "/cards/portraits/strike_craft_directive.png",
        [nameof(SimpleMissile)] = GrayTempestConst.Paths.Root + "/cards/portraits/simple_missile.png",
        [nameof(SimpleShieldGenerator)] = GrayTempestConst.Paths.Root + "/cards/portraits/simple_shield_generator.png",
        [nameof(FieldImprove)] = GrayTempestConst.Paths.Root + "/cards/portraits/field_improve.png",
        [nameof(Recycling)] = GrayTempestConst.Paths.Root + "/cards/portraits/recycling.png",
        [nameof(Decomposition)] = GrayTempestConst.Paths.Root + "/cards/portraits/decomposition.png",
        [nameof(EfficientReconstruct)] = GrayTempestConst.Paths.Root + "/cards/portraits/efficient_reconstruct.png",
        [nameof(Laser)] = GrayTempestConst.Paths.Root + "/cards/portraits/laser.png",
        [nameof(KineticCannon)] = GrayTempestConst.Paths.Root + "/cards/portraits/kinetic_cannon.png",
        [nameof(Missile)] = GrayTempestConst.Paths.Root + "/cards/portraits/missile.png",
        [nameof(EfficientReconstructPower)] = GrayTempestConst.Paths.Root + "/cards/portraits/efficient_reconstruct.png",
        [nameof(GrayTempestLegion)] = GrayTempestConst.Paths.CardGrayTempestLegion,
        [nameof(ColossusProject)] = GrayTempestConst.Paths.CardColossusProject,
    };

    [HarmonyPostfix]
    static void Postfix(CardModel __instance, ref string __result)
    {
        var className = __instance?.GetType().Name;
        if (string.IsNullOrEmpty(className)) return;
        if (CustomPortraits.TryGetValue(className, out var path) && ResourceLoader.Exists(path))
        {
            __result = path;
            return;
        }
        if (__instance?.GetType().Namespace?.StartsWith("GrayTempest") == true)
        {
            var fallback = GrayTempestConst.Paths.CardPlaceholder;
            if (ResourceLoader.Exists(fallback))
                __result = fallback;
        }
    }
}