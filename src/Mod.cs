using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppVehiclePhysics;
using MelonLoader;
using UnityEngine;

namespace OTH.ExtraGears;

// Adds 3rd and 4th forward gears to every car in Over the Hill.
//
// Cars ship with two forward gears (Vehicle Physics Pro Gearbox.Settings.
// forwardGearRatios). The new gears continue each car's own 1->2 step as a
// geometric progression: step = g2 / g1, g3 = g2 * step, g4 = g3 * step. Every
// upshift then drops the revs by the same proportion the 1->2 shift does, so
// each car keeps its character.
//
// When a car spawns, Photon Fusion's CopyBackingFieldsToState stores its 2-gear
// settings in network state, and the game keeps restoring them from there.
// SpawnPatch re-extends right after that call. A cheap periodic check of known
// cars backs it up.
public class Mod : MelonMod
{
    internal static MelonLogger.Instance Log;

    private static MelonPreferences_Entry<bool> prefEnabled;

    private static MelonPreferences_Entry<bool> prefDebug;

    internal static bool Enabled => prefEnabled.Value;

    internal static bool Debug => prefDebug.Value;

    private const int StockGears = 2;

    private const int ModdedGears = 4;

    private const float CheckInterval = 2f;

    // Cars seen by SpawnPatch. Checked periodically, so no scene-wide searches are needed.
    private static readonly List<VPVehicleController> cars = new();

    private static readonly HashSet<System.IntPtr> described = new();

    private readonly Dictionary<System.IntPtr, int> lastGear = new();

    private float nextCheck;

    public override void OnInitializeMelon()
    {
        Log = LoggerInstance;
        MelonPreferences_Category cat = MelonPreferences.CreateCategory("ExtraGears");
        prefEnabled = cat.CreateEntry("Enabled", true, description: "Add 3rd and 4th gears (applies to cars spawned after the change)");
        prefDebug = cat.CreateEntry("DebugLog", false, description: "Log the ratios of each car and every gear change");
    }

    // Car setup assets and prefabs, once per scene. Cars spawned from them then
    // start with 4 gears; SpawnPatch covers the copy made at spawn.
    public override void OnSceneWasInitialized(int buildIndex, string sceneName)
    {
        if (!Enabled)
        {
            return;
        }
        try
        {
            foreach (VehicleControllerSetup setup in Resources.FindObjectsOfTypeAll<VehicleControllerSetup>())
            {
                Extend($"setup '{setup.name}'", setup.gearbox);
            }
            foreach (VPVehicleController vc in Resources.FindObjectsOfTypeAll<VPVehicleController>())
            {
                Extend($"vehicle '{vc.name}'", vc.gearbox);
            }
        }
        catch (System.Exception ex)
        {
            Log.Warning("Scene scan failed: " + ex);
        }
    }

    // Returns true the first time a car is seen.
    internal static bool Track(VPVehicleController vc)
    {
        if (cars.Any(c => c != null && c.Pointer == vc.Pointer))
        {
            return false;
        }
        cars.Add(vc);
        return true;
    }

    public override void OnUpdate()
    {
        if (!Enabled)
        {
            return;
        }
        float now = Time.unscaledTime;
        bool check = now >= nextCheck;
        if (check)
        {
            nextCheck = now + CheckInterval;
            cars.RemoveAll(c => c == null);
        }
        if (!check && !Debug)
        {
            return;
        }
        foreach (VPVehicleController vc in cars)
        {
            if (check)
            {
                // Should stay quiet; a message here means a reset SpawnPatch doesn't catch.
                if (Extend($"'{vc.name}' (periodic check)", vc.gearbox))
                {
                    Log.Warning($"'{vc.name}' was back at {StockGears} gears; re-extended");
                }
            }
            if (Debug)
            {
                LogGearChange(vc);
            }
        }
    }

    private void LogGearChange(VPVehicleController vc)
    {
        Gearbox g = vc.m_gearbox;
        if (g == null)
        {
            return;
        }
        int gear = g.EngagedGear();
        if (!lastGear.TryGetValue(vc.Pointer, out int prev) || prev != gear)
        {
            lastGear[vc.Pointer] = gear;
            Log.Msg($"'{vc.name}' gear {gear} (ratio {F(g.GetCurrentGearRatio())}, {F(vc.speed * 3.6f)} km/h)");
        }
    }

    // Extend a 2-gear settings object to 4 gears. Returns true if it changed it.
    internal static bool Extend(string what, Gearbox.Settings gb)
    {
        if (gb == null)
        {
            return false;
        }
        Il2CppStructArray<float> fwd = gb.forwardGearRatios;
        if (fwd == null || fwd.Length != StockGears)
        {
            return false;
        }
        float g1 = fwd[0], g2 = fwd[1];
        if (!(g1 > g2 && g2 > 0f))
        {
            if (described.Add(gb.Pointer))
            {
                Log.Msg($"{what}: unexpected ratios {Ratios(fwd)}, left alone");
            }
            return false;
        }
        float step = g2 / g1;
        var ext = new Il2CppStructArray<float>(ModdedGears);
        ext[0] = g1;
        ext[1] = g2;
        ext[2] = g2 * step;
        ext[3] = g2 * step * step;
        gb.forwardGearRatios = ext;
        if (Debug)
        {
            Log.Msg($"{what}: {Ratios(fwd)} -> {Ratios(ext)} (step {F(step)})");
        }
        return true;
    }

    private static string Ratios(Il2CppStructArray<float> a)
    {
        return a == null ? "none" : "[" + string.Join(", ", a.Select(F)) + "]";
    }

    private static string F(float v)
    {
        return v.ToString("0.###", CultureInfo.InvariantCulture);
    }
}
