# Buu Mod – Implementation Plan & Asset Checklist

Majin Buu (Dragon Ball Z) character mod for Slay the Spire 2. Three stances: **Regular** (Good/Innocent Buu), **Majin** (Evil Buu), **Super** (Super Buu). Structure mirrors WatcherMod (BaseLib, Godot .pck + C# DLL).

---

## 1. Character & Stance Lore (Reference)

| Stance   | DBZ form        | Trait summary |
|----------|-----------------|----------------|
| Regular  | Good Buu / Innocent Buu | Round, pink, friendly; first form that absorbed Grand Supreme Kai; can be playful. |
| Majin    | Evil Buu        | Skinny, darker pink/gray, pure rage and malice; destructive. |
| Super    | Super Buu       | Tall, muscular, intelligent and cunning; evil resurfaced; very powerful. |

- **Kid Buu**: Original pure form (optional future skin or reference).
- **Good Buu**: Result of expelling evil; identical look to Innocent Buu, pure good.
- **Super Buu**: Evil Buu absorbed Good Buu; same entity as Innocent but different look and personality.

---

## 1.5 Power level / balance baseline

Buu should feel strong but not trivialize the game. Target parity with base-game characters (e.g. Watcher).

- **HP**: 72 (same as Watcher).
- **Starting deck**: 3× Ki Blast, 2× Punch, 2× Guard, 2× Headbutt, 1× Good Form (10 cards). Basic damage/block in line with Strike/Defend (6 damage, 5 block).
- **Stances (design goals)**:
  - **Regular**: Defensive/heal theme; small upside, low risk (e.g. end-of-turn Regen or +Block on block cards).
  - **Majin**: High risk/reward (e.g. double damage dealt and taken).
  - **Super**: Strongest form; big upside with a drawback (e.g. exit after 1–2 turns or increased damage taken). Avoid “always Super” being optimal.
- **Ki**: Secondary resource; keep Ki-only and mixed-cost cards so energy isn’t the only bottleneck. Include cards that grant Ki so the resource stays relevant.
- Tune numbers so Buu doesn’t consistently outscale other characters; stances should reward setup and trade-offs, not free power.

---

## 2. Implementation Checklist

### 2.1 Project & Build
- [x] **Template:** `dotnet new alchyrsts2charmod -o BuuMod -n Buu -M "YourName"` (already applied).
- [x] `Buu.sln` / `Buu.csproj` – template (paths, ModId, copy-to-mods, Godot export, CheckDependencyPaths, NuGet BaseLib).
- [x] `Buu.json` – mod manifest (id, name, author, description, version, dependencies: BaseLib).
- [x] `MainFile.cs` – ModInitializer, Harmony, `ModId = "Buu"`.
- [x] `project.godot` / `export_presets.cfg` – Godot 4.5, BasicExport.
- [x] `.gitignore` / `.gitattributes`, `nuget.config`.
- [ ] Build copies DLL + manifest to mods folder; Godot exports `Buu.pck` (set `GodotPath` / `Sts2DataDir` in csproj if needed).

### 2.2 Code – Character
- [x] `BuuCode/Character/Buu.cs` – PlaceholderCharacterModel with overrides: CustomIconTexturePath, CustomCharacterSelectIconPath/Locked, CustomMapMarkerPath, CustomEnergyCounter (layers 1–5), CustomArm* (hands). Starting deck/relics use Buu cards and Candy Shell. Visual/trail/rest site/merchant/char select bg still use ironclad placeholder until Buu scenes exist.
- [x] `BuuCode/Character/BuuCardPool.cs` – CustomCardPoolModel, Title, H/S/V, DeckEntryCardColor, BigEnergyIconPath, TextEnergyIconPath.
- [x] `BuuCode/Character/BuuRelicPool.cs` – CustomRelicPoolModel, EnergyColorName, LabOutlineColor.
- [x] `BuuCode/Character/BuuPotionPool.cs` – CustomPotionPoolModel (if custom potions).

### 2.3 Code – Stances
- [ ] `BuuCode/Stances/StancePower.cs` – abstract base (or copy from Watcher), AuraScenePath, OnEnterStance/OnExitStance, aura spawn on StanceVfxContainer, icon path helper.
- [ ] `BuuCode/Stances/RegularStance.cs` – e.g. defensive/block or heal theme (Good Buu).
- [ ] `BuuCode/Stances/MajinStance.cs` – e.g. double damage given/taken (Evil Buu).
- [ ] `BuuCode/Stances/SuperStance.cs` – e.g. triple damage + energy or exit next turn (Super Buu).
- [ ] `BuuCode/Commands/StanceCmd.cs` (or equivalent) – EnterRegular, EnterMajin, EnterSuper, ExitStance using ModelDb.Power<RegularStance>() etc.

### 2.4 Code – Cards
- [x] `BuuCode/Extensions/StringExtensions.cs` – path helpers; CardImagePath, PowerImagePath, RelicImagePath use `MainFile.ModId`; added `ToSnakeCase()` for portrait/relic filenames to match `image_gen/CARDS.md` ids.
- [x] `BuuCode/Cards/BuuCard.cs` – base card model with PortraitPath/CustomPortraitPath using `GetType().Name.ToSnakeCase()` so filenames match `card_portraits/<id>.png`.
- [x] Basic: Ki Blast, Punch, Guard, Headbutt, Good Form (stub block for now; stance/Ki when Stances implemented). Evil Emerges, Super Form – pending StancePower/StanceCmd.
- [ ] Common / Uncommon / Rare / Token cards – full Buu set (see `image_gen/CARDS.md`, 88 cards); each card C# class + one portrait in `Buu/images/card_portraits/<id>.png`.

### 2.5 Code – Relics
- [x] `BuuCode/Relics/BuuRelic.cs` – icon paths use `GetType().Name.ToSnakeCase()` to match `relics/<id>.png`. Starting relic: Candy Shell.
- [ ] Add remaining Buu relics (Majin Crest, Absorption Cell, Regeneration Pod, Supreme Kai’s Influence, etc.); each C# class + icon in `Buu/images/relics/<id>.png`.

### 2.6 Code – Powers
- [ ] Stance powers (3) – icons in `Buu/images/powers/`.
- [ ] Replace/extend template `BuuCode/Powers/BuuPower.cs`; non-stance powers – each with icon in `Buu/images/powers/<id>.png`.

### 2.7 Code – Patches & Nodes
- [ ] Patches: character registration, dialogue (e.g. BuuDialoguePatch), ProgressSaveManager if needed.
- [ ] Custom nodes only if needed (otherwise reuse BaseLib/STS2); reference Watcher’s SNEnergyCounter, SNRestSiteCharacter, SNMerchantCharacter, SNSelectionReticle, SNCardTrailVfx.

### 2.8 Godot – Scenes
- [ ] `Buu/scenes/buu/buu.tscn` – main combat character (references Buu visual/animation).
- [ ] `Buu/scenes/buu/buu_rest_site.tscn` – rest site character.
- [ ] `Buu/scenes/buu/buu_merchant.tscn` – merchant screen.
- [ ] `Buu/scenes/buu/buu_icon.tscn` – top-panel icon (TextureRect → character_icon_buu.png).
- [ ] `Buu/scenes/buu/buu_energy_counter.tscn` – energy orb (layers 1–5, burst).
- [ ] `Buu/scenes/buu/selection_reticle.tscn` – target reticle.
- [ ] `Buu/scenes/buu/card_trail_buu.tscn` – card trail VFX.
- [ ] `Buu/scenes/buu/char_select_bg_buu.tscn` – character select background.
- [ ] `Buu/scenes/buu_mod/vfx/regular_aura.tscn`, `majin_aura.tscn`, `super_aura.tscn` – stance auras.

### 2.9 Godot – Animation
- [ ] `Buu/animation/buu_node.tscn` (or equivalent) – combat sprite/Spine; Idle, Attack, Cast, Hit, Dead, Relaxed.

### 2.10 Localization
- [ ] `Buu/localization/eng/cards.json` – BUU-<CARD_ID>.title, .description for every card.
- [ ] `Buu/localization/eng/characters.json` – BUU-BUU.title, .description, .pronounSubject, .possessiveAdjective, .banter.*, .goldMonologue, .eventDeathPrevention, .cardsModifierTitle/Description, .aromaPrinciple.
- [ ] `Buu/localization/eng/powers.json` – BUU-<POWER_ID>.title, .description, .smartDescription.
- [ ] `Buu/localization/eng/relics.json` – BUU-<RELIC_ID>.title, .description, .flavor.
- [ ] `Buu/localization/eng/ancients.json` – THE_ARCHITECT.talk.BUU-BUU.* dialogue.
- [ ] Optional: zhs, kor (or copy eng and translate later).

### 2.11 Assets – Image Checklist (paths under `Buu/images/`)

Hand-crafted assets from **image_gen/assets/hand-sliced/** are copied into **Buu/images/** and wired in code.

- [x] Character UI: charui (character_icon_buu, map_marker_buu), buu/hands (multiplayer_hand_buu_*).
- [x] UI/combat: buu_energy_icon, text_buu_energy_icon, energy_counters/buu (layers 1–5), combat_reticle.
- [x] Card portraits: 88 card portraits in card_portraits/ (filenames = snake_case card id, e.g. ki_blast.png).
- [x] Relic icons: candy_shell, majin_crest, absorption_cell, regeneration_pod, pink_crystal, supreme_kai_earring, buu_antenna_charm, capsule.
- [x] Power icons: regular_stance, majin_stance, super_stance + others in powers/.
- [x] VFX: vfx (brush_particle_2, vfx_ghostly_power_up/sparkle).
- [ ] Optional: custom character visual, rest site, merchant, char select bg (currently use ironclad placeholder scenes).

**Image generation:** Sizes and batched prompts live in **image_gen/**: **ASSET_SIZES.md**, **PROMPTS.md**. New assets: generate then crop; add to hand-sliced and re-copy to Buu/images/.

---

## 3. File Path Summary (Buu Mod – template layout)

```
BuuMod/
├── Buu.sln, Buu.csproj, Buu.json, MainFile.cs, nuget.config
├── project.godot, export_presets.cfg, .gitignore, .gitattributes, icon.svg
├── PLAN.md
├── image_gen/                 (ASSET_SIZES.md, PROMPTS.md)
├── BuuCode/
│   ├── Character/     (Buu.cs, BuuCardPool, BuuRelicPool, BuuPotionPool)
│   ├── Stances/        (StancePower, RegularStance, MajinStance, SuperStance)
│   ├── Commands/       (StanceCmd)
│   ├── Cards/          (Basic, Common, Uncommon, Rare, Token, CardModels; template has BuuCard.cs)
│   ├── Relics/         (template has BuuRelic.cs)
│   ├── Powers/         (template has BuuPower.cs)
│   ├── Patches/
│   ├── Extensions/     (StringExtensions.cs)
│   └── Nodes/
└── Buu/
    ├── images/
    │   ├── buu/                    (character, hands, transitions)
    │   ├── ui/top_panel/
    │   ├── ui/combat/               (energy_counters/buu/, energy_burst/)
    │   ├── card_portraits/          (one per card)
    │   ├── relics/
    │   ├── powers/
    │   ├── vfx/
    │   └── packed/vfx/
    ├── localization/eng/            (cards, characters, powers, relics, ancients)
    ├── scenes/buu/                  (buu, rest_site, merchant, icon, energy_counter, reticle, card_trail, char_select_bg)
    ├── scenes/buu_mod/vfx/          (regular_aura, majin_aura, super_aura)
    ├── animation/
    ├── materials/
    └── themes/
```

---

## 5. Quality Checklist

- [ ] All C# compiles; no missing references (BaseLib, sts2, Harmony, SmartFormat).
- [ ] Godot project loads; all scenes reference existing resources.
- [ ] All asset paths in code use `res://Buu/...` and match files under `Buu/`.
- [ ] Localization keys match card/relic/power/character IDs (BUU-*).
- [ ] Stance enter/exit and aura spawn tested in combat.
- [x] Card portrait and relic icon filenames use snake_case (e.g. ki_blast.png, candy_shell.png) via `GetType().Name.ToSnakeCase()`.
- [ ] Mod loads in STS2 with BaseLib; character selectable; no missing texture errors.

---

*End of PLAN.md*
