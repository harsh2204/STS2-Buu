# Buu Mod – 3 Stance Sprites (Regular / Majin / Super)

Three separate Spine skeletons (one per form) because multiple skins on one skeleton require Spine Pro. The mod **swaps the combat Visuals scene** when you enter or exit a stance.

---

## Layout: three skeletons, three scenes

Each stance has its own export folder, skeleton data resource, and scene.

```
Buu/animation/
  buu_skel_data.tres          # Regular (Good Buu) – references binary/ or json/
  buu_node.tscn              # Regular: SpineSprite using buu_skel_data.tres
  buu_node_majin.tscn        # Majin: SpineSprite using buu_majin_skel_data.tres
  buu_node_super.tscn        # Super: SpineSprite using buu_super_skel_data.tres
  buu_majin_skel_data.tres   # Majin skeleton data
  buu_super_skel_data.tres   # Super skeleton data
  binary/                    # or json/ – Regular
    buu.png, buu.atlas, buu.skel
  majin/
    buu_majin.png, buu_majin.atlas, buu_majin.skel
  super/
    buu_super.png, buu_super.atlas, buu_super.skel
```

You can keep `binary/` (or `json/`) for Regular at the top level and add `majin/` and `super/` for the other two exports.

---

## Scene and skeleton data

- **Regular (default):** `buu_node.tscn` → `buu_skel_data.tres` (current setup).
- **Majin:** Create `buu_node_majin.tscn` with a SpineSprite whose `skeleton_data_res` points to `buu_majin_skel_data.tres`. Use the same node name as in `buu_node.tscn` (e.g. root named `BuuNode` or keep as SpineSprite).
- **Super:** Create `buu_node_super.tscn` with a SpineSprite whose `skeleton_data_res` points to `buu_super_skel_data.tres`.

Each skeleton should expose the same animation names so the character animator keeps working: **idle**, **attack**, **defend**, **heal** (and optionally **hit** / **dead**).

---

## Runtime behaviour

When the player:

- **Enters Majin stance** → the mod replaces the creature’s `Visuals` node with an instance of `buu_node_majin.tscn`.
- **Enters Super stance** → replaces with an instance of `buu_node_super.tscn`.
- **Enters Regular stance** or **exits stance** → replaces with an instance of `buu_node.tscn` (Regular).

Position and scale of the Visuals node are preserved when swapping so the character stays in place.

---

## Checklist

1. Export three skeletons from Spine (Regular, Majin, Super) into the folders above.
2. Create `buu_majin_skel_data.tres` and `buu_super_skel_data.tres` in Godot (same setup as `buu_skel_data.tres`, pointing at each export).
3. Create `buu_node_majin.tscn` and `buu_node_super.tscn` (each a SpineSprite with the corresponding skeleton data).
4. Ensure all three skeletons use the same animation names: **idle**, **attack**, **defend**, **heal**.
