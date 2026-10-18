# Log

## 2026-10-17

- Game uses Vehicle Physics Pro (`Il2CppVehiclePhysics`). Two separate systems:
  gearbox gears (`Gearbox.Settings.forwardGearRatios`, HUD `TextGear`, `NetworkedVehicleStatus.gearboxGear`)
  and a 4H/4L transfer case (`HighLowGearsDifferentialSettings`: 4H 8, 4L 20 on every category).
- Per-car setups (`VehicleControllerSetup`, ScriptableObject; linked via `CarInfo.carCategorySetup`), stock forward ratios:
  | Setup | 1st | 2nd | step | reverse |
  |---|---|---|---|---|
  | CarCategory_01_Trail | 4.2 | 2.5 | 0.595 | -4.2 |
  | CarCategory_02_Wagon | 4.7 | 2.6 | 0.553 | -4.5 |
  | CarCategory_03_Workhorse | 4.7 | 3.5 | 0.745 | -4.5 |
  | CarCategory_04_Expedition | 5.5 | 3.5 | 0.636 | -4.5 |
  All `Manual` type. VPP default prefabs (`Car_Base`, `Car_Cordillera`, `Car_Brigand`) have 5 gears and are left alone.
- `SpeedControl.speedLimiter` is true with 21.6–27 km/h limits, yet the Burro reached 26.6 km/h
  with a 21.6 km/h limit, so the limiter doesn't seem to cap normal driving. Watch top speed in 3rd/4th.
- v0.1.0: ratio extension works (Burro live gearbox `[4.7, 3.5, 2.606, 1.941]`), but shift-up
  in 2nd does nothing: the game's own handler caps `VehicleInput.selectedGear` at 2.
  `NetworkInputData.SelectedManualGear` is an int, so higher gears fit.
- v0.1.1: Harmony prefix/postfix on `VehicleInput.GearShiftUpPressed`. If the press (phase
  Performed) leaves `selectedGear` unchanged at >= 2 and the gearbox has more gears, raise it.
  Also traces `selectedGear`, `Gearbox.manualGearInput`, and engaged gear to find any later clamp.
- v0.1.1 test: `GearShiftUpPressed` fires only with phase `Started`, never `Performed`.
  `VehicleInput.selectedGear` stays 0 throughout (likely only for H-shifters/DirectInput),
  so it isn't the gear. Gearbox `manualGearInput` went 1 -> 2, then ignored 4 more presses.
  At press time the car reported 2 forward gears although its `vc.gearbox` had been extended
  earlier. Suspect the game hands the car a fresh settings copy and the one-shot scan
  (cached per vehicle) missed it. VPP's clamp then works off the 2-gear live copy.
- v0.1.2: removed the selectedGear workaround. Every 1 s, re-check setups, `vc.gearbox` and
  `vc.m_gearbox.settings`, and extend any 2-gear copy. Shift trace logs engaged gear,
  manualGearInput, and gear counts of the vehicle settings vs the live gearbox.
- v0.1.2 test: the per-second scan re-extended the Burro's `vc.gearbox` every second, and
  it was back at `[4.7, 3.5]` each time. At every shift press both `vc.gearbox` and
  `m_gearbox.settings` (same object) had 2 gears. Something rewrites `forwardGearRatios`
  continuously. Setup assets stay extended, so the 2-gear source is elsewhere.
- v0.1.3: hooks `VPVehicleController.OnAfterDeserialize`, `OnInitialize`, `DoUpdateData`,
  `CopyStateToBackingFields`, `CopyBackingFieldsToState` (Fusion state sync) and
  `EngineUpgradeEffect.ApplyEffect`. Each logs when it shrinks the gearbox (first 3, then
  every 500th) and re-extends right after. The scan tags its own fixes `[scan]` = missed by the hooks.
- v0.1.3 crashed on launch right after MelonLoader's support module loaded (hook setup). Dropped the OnAfterDeserialize hook (called on Unity loading threads) in v0.1.4.
- v0.1.4 test: **3rd and 4th work.** Burro: 2->3 at ~22 km/h (2.606), 3->4 at ~27 km/h
  (1.941), downshifts fine. The reset was `VPVehicleController.CopyBackingFieldsToState`
  (Fusion), once per car at spawn: "2 -> 2 gears, re-extended" for each car. After that,
  no further [scan] fixes on spawned cars. Game was very laggy: per-second
  `Resources.FindObjectsOfTypeAll` (scans all loaded objects) + per-frame `FindObjectsByType`
  in debug mode + 4 hooks incl. Fusion state sync.
- v0.2.0: lean version. One hook (`CopyBackingFieldsToState` postfix) extends and tracks the
  car. `FindObjectsOfTypeAll` only once per scene (`OnSceneWasInitialized`). A 2 s check
  touches only tracked cars and warns if it ever finds 2 gears again. Removed shift/reset
  diagnostics and the InputSystem reference.
- v0.3.0: speed readout above the gear display (clone of TextGear) worked; user asked to remove it. v0.4.0 removes it.

## 2026-10-17: Windows confirmed

A Windows player installed MelonLoader v0.7.3 + v0.4.0 per the README and reports it works. README updated: Windows is no longer marked untested. Multiplayer with mixed modded/unmodded players is still unconfirmed.

## 2026-10-17: v1.0.0

No code changes from v0.4.0. Version bumped to 1.0.0 for the Nexus Mods release, after Windows confirmed working.
