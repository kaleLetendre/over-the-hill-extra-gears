# Over the Hill: Extra Gears

A MelonLoader mod that adds **3rd and 4th gear** to every car in *Over the Hill*.

The new gears follow each car's own gearing. Whatever step a car has from 1st
to 2nd, it takes the same step again to 3rd and to 4th. Every upshift drops the
revs by the same proportion the 1→2 shift already does, so each car keeps its
character: a short-geared workhorse stays short-geared, a wagon keeps its long
legs. The 4H / 4L range switch is untouched and still works on top of all four gears.

| Car class | 1st | 2nd | **3rd** | **4th** | Step |
|---|---|---|---|---|---|
| Trail | 4.2 | 2.5 | **1.488** | **0.886** | 0.595 |
| Wagon | 4.7 | 2.6 | **1.438** | **0.796** | 0.553 |
| Workhorse | 4.7 | 3.5 | **2.606** | **1.941** | 0.745 |
| Expedition | 5.5 | 3.5 | **2.227** | **1.417** | 0.636 |

1st and 2nd are the game's own ratios, unchanged. 3rd = 2nd × step, 4th = 3rd × step,
with step = 2nd ÷ 1st.

**Status:** works on Windows and Linux (Proton), tested with manual shifting.

> **Untested:** automatic shifting and multiplayer. In multiplayer, everyone in the
> session should install the mod. [Reports welcome](https://github.com/kaleLetendre/over-the-hill-extra-gears/issues).

---

## What you need

- [ ] *Over the Hill* on Steam
- [ ] About 15 minutes, mostly waiting for the game's first start with MelonLoader

Any controller or keyboard works. Unlike
[Impulse Triggers](https://github.com/kaleLetendre/over-the-hill-trigger-rumble),
this mod needs no special controller setup.

Follow the guide for your system: [Linux](#setup-on-linux-steam--proton) or
[Windows](#setup-on-windows).

**Already have MelonLoader v0.7.3 in the game** (e.g. for another mod)? Skip
straight to [Install the mod](#step-5-install-the-mod).

---

## Setup on Linux (Steam + Proton)

### Step 1: Open the game folder

In Steam, right-click **Over the Hill** → **Manage** → **Browse local files**.
A file manager opens on a folder containing `over the hill.exe`. Keep it open;
the next steps all happen in this folder.

### Step 2: Install MelonLoader (the mod loader)

1. Download **MelonLoader v0.7.3**:
   [MelonLoader.x64.zip](https://github.com/LavaGang/MelonLoader/releases/download/v0.7.3/MelonLoader.x64.zip).
   It has to be this version; newer or older ones may not work with this mod.
2. Open the ZIP and extract **everything** into the game folder from Step 1.

Check: the game folder now has a `version.dll` file and a `MelonLoader` folder,
right next to `over the hill.exe`:

    MelonLoader/
    over the hill_Data/
    over the hill.exe
    UnityPlayer.dll
    version.dll
    ...

If you see a `MelonLoader.x64` folder instead, the files were extracted one level
too deep. Move its contents up into the game folder.

### Step 3: Tell Proton to load MelonLoader

1. In Steam, right-click **Over the Hill** → **Properties** → **General**.
2. In **Launch Options**, paste exactly:

       WINEDLLOVERRIDES="version=n,b" %command%

3. Close the window. Steam saves it automatically.

### Step 4: Start the game once, without the mod

1. Start Over the Hill from Steam.
2. A MelonLoader console window with scrolling text appears before the game.
   **The first start is slow, often several minutes,** while MelonLoader prepares
   itself. It can look frozen; it isn't. Wait.
3. When the game's main menu appears, quit the game.

Check: the game folder now has a `Mods` folder. If it doesn't, create one with
exactly that name.

### Step 5: Install the mod

1. Download [OTH.ExtraGears.dll](https://github.com/kaleLetendre/over-the-hill-extra-gears/releases/latest/download/OTH.ExtraGears.dll).
2. Put it in the `Mods` folder inside the game folder.

### Step 6: Start the game and check it loaded

Start the game again. In the MelonLoader console window, look for:

    ExtraGears v0.4.0
    ...
    Mods loaded.

Then load into the world and get in a car. The console shows one line per car as it
appears, for example:

    [ExtraGears] 'Car_Burro(Clone)': 4 forward gears

The same text is saved in `MelonLoader/Latest.log` in the game folder.

### Step 7: Drive it

Get going in 1st, shift up into 2nd as usual, and keep shifting up: 3rd and 4th
are there now. The HUD's gear readout shows 3 and 4. Shifting down works as before.

Done. You only do all this once. Next time, just start the game.

---

## Setup on Windows

### Step 1: Install MelonLoader v0.7.3

1. Download the latest
   [MelonLoader Installer](https://github.com/LavaGang/MelonLoader.Installer/releases/latest)
   (`MelonLoader.Installer.exe`) and run it.
2. Pick **Over the Hill** from the list. If it isn't listed, add it by browsing to
   `over the hill.exe`. To find that file: in Steam, right-click the game →
   **Manage** → **Browse local files**.
3. Choose version **v0.7.3**, then click **Install**.

No launch option is needed on Windows.

### Step 2: Start the game once

Start the game, wait for the main menu (the first start is slow while MelonLoader
prepares itself), then quit.

### Step 3: Install the mod

1. Download [OTH.ExtraGears.dll](https://github.com/kaleLetendre/over-the-hill-extra-gears/releases/latest/download/OTH.ExtraGears.dll)
   and put it in the `Mods` folder in the game folder.
2. If Windows marks the file as downloaded from the internet: right-click it →
   **Properties** → tick **Unblock** → **OK**.

Then continue with [Step 6 of the Linux guide](#step-6-start-the-game-and-check-it-loaded):
the checks are the same.

---

## Troubleshooting

Most answers are in `MelonLoader/Latest.log` in the game folder. Search it for `ExtraGears`.

**No MelonLoader console window, and no `MelonLoader/Latest.log`**
MelonLoader isn't loading. On Linux, check the launch option from
[Step 3](#step-3-tell-proton-to-load-melonloader) is exactly
`WINEDLLOVERRIDES="version=n,b" %command%`, and that `version.dll` is next to
`over the hill.exe`.

**The log doesn't mention `ExtraGears` at all, or says `0 Mods loaded`**
The DLL isn't in the `Mods` folder, or it's inside a subfolder. It must be
`Mods/OTH.ExtraGears.dll`. On Windows, also check it's unblocked.

**Shifting up in 2nd does nothing**
- Look for a `4 forward gears` line for your car in the log. If it's missing,
  the car spawned before the mod could catch it: return to the main menu and load
  the save again, or restart the game.
- Check `Enabled = true` under `[ExtraGears]` in the config file (below).
- If the log says `was back at 2 gears; re-extended`, the game reset the gearbox
  in a way the mod doesn't catch at spawn. Please
  [open an issue](https://github.com/kaleLetendre/over-the-hill-extra-gears/issues)
  with the log.

**The game crashes on start after installing the mod**
Remove `Mods/OTH.ExtraGears.dll` to play without it, and
[open an issue](https://github.com/kaleLetendre/over-the-hill-extra-gears/issues)
with `MelonLoader/Latest.log`. Say which other mods you have.

**Still stuck?** [Open an issue](https://github.com/kaleLetendre/over-the-hill-extra-gears/issues)
and attach `MelonLoader/Latest.log`. For more detail, set `DebugLog = true` (below) first.

---

## Settings

`UserData/MelonPreferences.cfg` in the game folder, section `[ExtraGears]`. The
section appears after the first run with the mod. **Edit it only while the game is
closed.** MelonLoader rewrites the file when the game quits.

| Key | Default | Meaning |
|---|---|---|
| `Enabled` | `true` | Add 3rd and 4th gears. Applies to cars spawned after a change |
| `DebugLog` | `false` | Log each car's old and new ratios, and every gear change with its speed |

## Other mods

Works alongside [Impulse Triggers](https://github.com/kaleLetendre/over-the-hill-trigger-rumble)
(tested together) and should work with
[TriggerFX](https://www.nexusmods.com/overthehill/mods/1): both react to the
game's gear-change events, which still fire for 3rd and 4th.

## Updating

Download the new `OTH.ExtraGears.dll` from
[Releases](https://github.com/kaleLetendre/over-the-hill-extra-gears/releases)
and replace the old one in `Mods`.

## Uninstall

- **Just this mod:** delete `Mods/OTH.ExtraGears.dll` from the game folder. Cars
  go back to two gears on the next start.
- **MelonLoader too:** delete `version.dll` and the `MelonLoader` folder from the
  game folder. On Linux, also clear the launch option from Step 3.

---

## How it works

- The game uses Vehicle Physics Pro. Each car's gearbox has a `forwardGearRatios`
  array, and the gearbox clamps gear selection to its length. The mod replaces the
  2-entry array with a 4-entry one.
- When a car spawns, Photon Fusion's `CopyBackingFieldsToState` saves the car's
  settings into network state, and the game restores the 2-gear version from
  there. A Harmony postfix on that method re-extends the gearbox right after it.
- Car setup assets and prefabs are extended once when a scene loads. A check every
  2 s looks only at cars already seen (no scene-wide searches) and logs a warning
  if one ever reverts to 2 gears.

Details and the investigation are in [notes/log.md](notes/log.md).

## Building from source

Only needed to change the mod. Players should use the DLL from Releases.

- Install MelonLoader and run the game once (Linux Steps 1–4), so
  `MelonLoader/Il2CppAssemblies` exists to build against.
- `scripts/install-melonloader.sh` does Linux Step 2 for you;
  `scripts/uninstall-melonloader.sh` reverses it.
- `scripts/build.sh [--no-copy]` builds with the .NET SDK (8+; `~/.dotnet` is picked
  up automatically) and copies the DLL into the game's `Mods` folder. Set `GAME_DIR`
  if the game isn't in `~/.steam/steam/steamapps/common/over the hill`.

## License

[MIT](LICENSE).
