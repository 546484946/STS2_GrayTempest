using System.Reflection;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using STS2RitsuLib;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop;
using STS2RitsuLib.Utils;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace GrayTempest.Scripts;

[ModInitializer(nameof(Init))]
public class Entry
{
    public static readonly Logger ModLogger = RitsuLibFramework.CreateLogger(GrayTempestConst.ModId);
    public static bool IsModActive { get; private set; }

    public static void Init()
    {
        if (IsModActive) return;

        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            ModLogger.Info("Step 1: EnsureGodotScriptsRegistered");
            RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, ModLogger);

            ModLogger.Info("Step 2: Register content");
            var publicEntry = ModContentRegistry.GetFixedPublicEntry(
                GrayTempestConst.ModId, typeof(Character.GrayTempestCharacter));
            ModTypeDiscoveryHub.RegisterModAssembly(GrayTempestConst.ModId, assembly);

            ModLogger.Info("Step 3: Register special content mappings");
            RitsuLibFramework.RegisterArchaicToothTranscendenceMapping<Cards.Basic.StrikeCraftDirective, Cards.Rare.GrayTempestLegion>();
            RitsuLibFramework.RegisterTouchOfOrobasRefinementMapping<Relics.GrayGooCore, Relics.GrayGooCorePlus>();

            var harmony = new Harmony("graytempest.mod");
            harmony.PatchAll();

            ScriptManagerBridge.LookupScriptsInAssembly(assembly);

            IsModActive = true;
            ModLogger.Info("Mod initialized successfully!");
        }
        catch (Exception ex)
        {
            ModLogger.Error($"Failed to initialize: {ex}");
            IsModActive = false;
        }
    }
}
