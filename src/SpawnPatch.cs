using HarmonyLib;
using Il2CppVehiclePhysics;

namespace OTH.ExtraGears;

// When a car spawns, Photon Fusion's CopyBackingFieldsToState saves its 2-gear
// gearbox settings into network state, and the game restores them from there.
// Extend right after it (both the vehicle's settings and the live gearbox's
// copy), and remember the car for the periodic check.
[HarmonyPatch(typeof(VPVehicleController), nameof(VPVehicleController.CopyBackingFieldsToState))]
internal static class SpawnPatch
{
    private static void Postfix(VPVehicleController __instance)
    {
        if (!Mod.Enabled || __instance == null)
        {
            return;
        }
        string name = __instance.name;
        bool changed = Mod.Extend($"'{name}' (spawn)", __instance.gearbox);
        Gearbox live = __instance.m_gearbox;
        if (live != null)
        {
            changed |= Mod.Extend($"'{name}' live gearbox (spawn)", live.settings);
        }
        // One line per car, whether it was extended here or already had 4 gears
        // from its setup asset.
        if (Mod.Track(__instance) || changed)
        {
            int gears = __instance.gearbox?.forwardGearRatios?.Length ?? 0;
            Mod.Log.Msg($"'{name}': {gears} forward gears");
        }
    }
}
