# Buu Mod – Playability Checklist vs Watcher

Comparison to WatcherMod for **playability**: what is implemented, what is missing, and asset wiring status. Use this to fix “game doesn’t proceed” issues and to finish missing pieces.

---

## 1. Reward screen / game flow (first thing to fix)

**Symptom:** Game doesn’t proceed after selecting the first card reward.

**Likely causes and checks:**

| Check | Buu status | Notes |
|-------|------------|--------|
| Card pool registration | OK | `[Pool(typeof(BuuCardPool))]` on all Buu cards; `ModHelper.AddModelToPool` via BaseLib. |
| Portrait paths | Fixed | `StringExtensions` now use forward slashes so Godot/res:// paths resolve (no backslash on Windows). |
| Card portrait files in .pck | Verify | Ensure `Buu/images/card_portraits/*.png` (and `big/`) are included in export; missing files can break reward flow. |
| Pool / ModelDb resolution | OK | Character returns `ModelDb.CardPool<BuuCardPool>()`; pool has same shape as Watcher. |
| Localization for reward cards | Partial | Only basic cards + Candy Shell + Ki power; missing entries can cause analyzer errors or broken UI. |

**Next steps if it still hangs:**  
Check game logs after picking a reward; look for null reference, missing resource, or failed add-to-deck. Ensure no exception in card clone/add or in portrait loading.

---

## 2. Character model – implemented vs missing (vs Watcher)

| Feature | Watcher | Buu | Notes |
|---------|---------|-----|--------|
| CustomIconTexturePath | Yes | Yes | charui icon. |
| CustomCharacterSelectBg | Yes (scene) | Yes (scene) | Buu: `char_select_bg_buu.tscn`. |
| CustomCharacterSelectIconPath | Yes | Yes | packed/character_select. |
| CustomCharacterSelectLockedIconPath | Yes | Yes | packed/character_select. |
| CustomCharacterSelectTransitionPath | Yes (.tres) | No | Buu has `buu_transition.png`; game expects material .tres. |
| CustomMapMarkerPath | Yes | Yes | charui. |
| CustomEnergyCounter (layers 1–5 + colors) | Yes | Yes | Buu orb layers. |
| CustomEnergyCounterPath | Yes (own scene) | No | Buu uses default ironclad scene + Buu layers (BaseLib patch). |
| CustomVisualPath | Yes (watcher.tscn) | No | Placeholder: ironclad visual. |
| CustomTrailPath | Yes | No | Placeholder: ironclad trail. |
| CustomIconPath | Yes (scene) | No | Placeholder: ironclad icon. |
| CustomRestSiteAnimPath | Yes | No | Placeholder: ironclad rest site. |
| CustomMerchantAnimPath | Yes | No | Placeholder: ironclad merchant. |
| CustomArm* (hands) | Yes | Yes | Buu hands. |
| StartingDeck / StartingRelics | Yes | Yes | Buu deck + Candy Shell. |
| CardPool / RelicPool / PotionPool | Yes | Yes | Buu pools. |
| StartingGold | 99 | Not set | Uses default; can set if needed. |
| AttackAnimDelay / CastAnimDelay | Set | Not set | Uses default. |

---

## 3. Cards – implemented vs missing

| Area | Watcher | Buu | Notes |
|------|---------|-----|--------|
| Basic (Strike/Defend-style) | 4 + 2 stance | 5 (Ki Blast, Punch, Guard, Headbutt, Good Form) | Buu has no stance cards yet. |
| Common / Uncommon / Rare | Many | 0 | CARDS.md has 88; only basics wired. |
| Token cards | Yes (Miracle, etc.) | 0 | — |
| Card portraits in code | PortraitPath / CustomPortraitPath | Yes (BuuCard base) | Paths: `card_portraits/<id>.png`, `big/` for custom. |
| Card portraits on disk | Per card | Only basics + any you added | Must match C# card class names (snake_case). |

---

## 4. Relics – implemented vs missing

| Area | Watcher | Buu | Notes |
|------|---------|-----|--------|
| Starter | Holy Water / Pure Water | Candy Shell | Candy Shell: start 4 Ki, +2 Ki/turn. |
| Common / Uncommon / Rare | Several | 0 | CARDS.md / PLAN list more Buu relics. |
| Relic icons in code | PackedIconPath | Yes (BuuRelic base) | `relics/<id>.png`. |
| Relic icons on disk | Per relic | candy_shell + any added | — |

---

## 5. Powers – implemented vs missing

| Area | Watcher | Buu | Notes |
|------|---------|-----|--------|
| Stance powers | 3 (Calm, Wrath, Divinity) | 0 | Regular/Majin/Super not implemented. |
| Ki resource | — | KiPower (counter) | Used by Candy Shell; shows as power icon. |
| Other powers | Mantra, Foresight, etc. | 0 | — |
| Power icons | Per power | KiPower uses power.png | Add `ki_power.png` (or keep fallback). |

---

## 6. Ki indicator (custom UI)

| Area | Watcher | Buu | Notes |
|------|---------|-----|--------|
| Energy counter | Own scene (watcher_energy_counter) | Default scene + Buu layers | BaseLib swaps textures. |
| Secondary resource (Mantra / Ki) | Mantra = power icon only | Ki = power icon only | No orb-style “Ki counter” in UI yet. |
| Ki indicator scene | — | Added (see below) | `buu_ki_indicator.tscn` + script; can be wired like Watcher’s energy counter once hook exists. |

**Done:**  
- **Ki indicator:** `Buu/scenes/buu/buu_ki_indicator.tscn` + `BuuCode/Nodes/BuuKiIndicator.cs` (icon + label; shows "0" until wired to combat).  
- For now, Ki is visible as the **KiPower** icon + amount in the power bar.  
- To show Ki in an orb-style widget like Watcher’s energy: either use a game hook for “secondary resource” UI, or add a BaseLib patch that instantiates `buu_ki_indicator.tscn` in combat and the existing script will bind to KiPower when available.

---

## 7. Scenes – implemented vs missing (vs Watcher)

| Scene | Watcher | Buu | Notes |
|-------|---------|-----|--------|
| char_select_bg_* | Yes | Yes | Buu: char_select_bg_buu.tscn. |
| *_energy_counter | Yes | No (uses default) | Buu uses ironclad scene + Buu layers. |
| *_ki_indicator | — | Yes | Added; script updates label from KiPower. |
| watcher.tscn / buu.tscn | Yes | No | Combat character visual. |
| card_trail_* | Yes | No | Card trail VFX. |
| *_rest_site | Yes | No | Rest site character. |
| *_merchant | Yes | No | Merchant character. |
| *_icon | Yes | No | Top panel icon. |
| selection_reticle | Yes | No | Target reticle. |

---

## 8. Assets – what exists vs what’s wired

Paths under `Buu/images/` (and in .pck). “Wired” = referenced in C# or scene.

| Asset path / area | Exists on disk | Wired in code | Notes |
|-------------------|----------------|----------------|--------|
| card_portraits/*.png (basics) | Yes | Yes (BuuCard) | ki_blast, punch, guard, headbutt, good_form. |
| card_portraits/big/*.png | If present | Yes (CustomPortraitPath) | — |
| packed/character_select/*.png | Yes | Yes (Buu.cs) | character_select_buu, character_select_locked_buu, char_select_bg_buu.jpg. |
| packed/character_select/buu_transition.png | Yes | No | Game expects transition material .tres. |
| charui/*.png | Yes | Yes | character_icon_buu, map_marker_buu, etc. |
| ui/combat/energy_counters/buu/*.png | Yes | Yes | buu_orb_layer_1..5, BuuEnergyCounter. |
| ui/combat/buu_energy_icon.png | Yes | Yes (BuuCardPool) | BigEnergyIconPath, TextEnergyIconPath. |
| buu/hands/*.png | Yes | Yes | multiplayer_hand_buu_*. |
| relics/*.png | Yes | Yes (BuuRelic) | candy_shell, etc. |
| powers/*.png | Yes | Yes (BuuPower/KiPower) | power.png used for KiPower. |
| packed/vfx/*.png | Yes | No | trail, small_card_silhouette; not yet used in scenes. |
| image_gen assets (raw/sliced) | Yes | No | Only what’s copied under Buu/images/ is in mod. |

**Summary:**  
- All **basic** card/relic/power assets under `Buu/images/` that are referenced by name in code are wired.  
- Many **image_gen** assets (e.g. extra card portraits, VFX) are not copied into `Buu/images/` or not referenced; add them and wire in C#/scenes as you add cards and effects.

---

## 9. Localization – implemented vs missing

| File | Buu status | Notes |
|------|------------|--------|
| characters.json | Basic | BUU-BUU.* (title, description, pronouns, banter placeholders). |
| cards.json | Basic only | Ki Blast, Punch, Guard, Headbutt, Good Form. |
| relics.json | Candy Shell | title, description, flavor. |
| powers.json | KiPower | title, description, smartDescription. |
| static_hover_tips / ancients / card_keywords | Minimal/empty | Add as you add content. |

---

## 10. Watcher animation folder – what the files are and path for Buu

Watcher’s combat character uses **Spine** (2D skeletal animation). The `WatcherMod/Watcher/animation/` folder holds the Spine runtime assets and Godot wrappers.

| File | What it is |
|------|------------|
| **watcher.png** | Texture atlas: one PNG with all character parts (face, robe, staff, legs, etc.) laid out in regions. |
| **skeleton.atlas** | Spine text atlas: image name (`the_watcher.png`) + region names and rects (xy, size, orig, offset). Source format for Spine. |
| **watcher.spatlas** | Spine atlas in Godot’s JSON format; embeds the same region data. Used at runtime. |
| **watcher.spskel** | Spine skeleton (binary): bones, slots, skins, and **animations** (Idle, Attack, etc.). Exported from Spine editor. |
| **watcher_skel_data.tres** | Godot resource (`SpineSkeletonDataResource`) that points to `watcher.spatlas` and `watcher.spskel`. |
| **watcher_node.tscn** | Scene with a single `SpineSprite` node using `watcher_skel_data.tres`. This is the “sprite” that gets instantiated. |
| **watcher.png.import** | Godot import config for the atlas texture. |

**How it’s used:**  
`CustomVisualPath` → `Watcher/scenes/watcher/watcher.tscn`. That scene has script `SNCreatureVisuals` (inherits game’s `NCreatureVisuals`) and a child **Visuals** that instances `watcher_node.tscn` (the SpineSprite). So the combat character is: Node2D + Visuals (SpineSprite) + Bounds, CenterPos, IntentPos.

**Path forward for Buu:**

1. **Create Spine assets (outside Godot)**  
   Use **Spine** (Esoteric Software) or a Spine-compatible pipeline:  
   - One PNG atlas with Buu body parts (torso, arms, legs, face, etc.) and a `.atlas` (or export) defining region names/rects.  
   - A Spine project with bones, slots, and animations (Idle, Attack, Block, Hit, etc.).  
   - Export: **Buu.spskel** (skeleton + animations) and **Buu.spatlas** (+ Buu.png).  

2. **Add under `BuuMod/Buu/animation/`**  
   - `buu.png` (atlas texture)  
   - `buu.spatlas` (and optional `skeleton.atlas` if you use it as source)  
   - `buu.spskel`  
   - **buu_skel_data.tres** – Godot resource: `atlas_res` → buu.spatlas, `skeleton_file_res` → buu.spskel (paths `res://Buu/animation/...`).  
   - **buu_node.tscn** – scene with one `SpineSprite` node, `skeleton_data_res` = buu_skel_data.tres.  

3. **Combat visual scene**  
   - **Buu/scenes/buu/buu.tscn** – same structure as watcher.tscn: Node2D with script inheriting `NCreatureVisuals`, child **Visuals** instancing `Buu/animation/buu_node.tscn`, plus **Bounds**, **CenterPos**, **IntentPos** (copy positions from watcher.tscn and tweak).  
   - In **Buu.cs**: `CustomVisualPath => "res://Buu/scenes/buu/buu.tscn"`.  

4. **Until you have Spine exports**  
   Keep `PlaceholderCharacterModel` (ironclad visual). The game will run; only the character appearance will be a placeholder.

**Summary:** The .spskel and atlas come from **Spine**, not from code. Once you have buu.spskel + buu.spatlas + buu.png, add the .tres and two .tscn files and point `CustomVisualPath` at buu.tscn.

---

## 11. Recommended order of work

1. **Confirm reward flow**  
   Build with path fix, run, pick first reward. If it still hangs, use logs to find null/missing resource or add-to-deck failure.

2. **Asset export**  
   Ensure `dotnet build` (and Godot export) includes `Buu/images/card_portraits/`, `big/`, and all paths used by BuuCard/BuuRelic/BuuPower so reward and combat don’t break.

3. **Ki indicator**  
   Ki already shows as power icon. The new `buu_ki_indicator` scene is ready to be hooked into combat UI when you have a hook (or patch).

4. **Character visuals (Spine animation)**  
   See **§10 Watcher animation** above. Add Buu Spine exports under `Buu/animation/`, then buu_skel_data.tres, buu_node.tscn, and buu.tscn; set `CustomVisualPath` in Buu.cs. Until then, placeholder (ironclad) remains.

5. **Stances + more cards/relics**  
   Implement Regular/Majin/Super and wire more cards/relics from CARDS.md and PLAN.md; link any new assets under `Buu/images/` as in this checklist.

---

## 12. Ki indicator (temporary – “copy from Watcher”)

- **Added:** `Buu/scenes/buu/buu_ki_indicator.tscn` – Control with icon (Buu orb) + Label.  
- **Added:** Script (or node logic) so the label shows Ki amount when in combat (or “0” until bound).  
- You can replace the icon/art later; the scene is set up so you can plug in a proper Ki asset without changing the wiring.
