# BuuMod vs base game mechanics — coverage checklist

Reference sources (this repo):

- Enums: `spire-codex/extraction/decompiled/MegaCrit.Sts2.Core.Entities.Cards/` (`CardType`, `CardRarity`, `TargetType`, `CardKeyword`), `MegaCrit.Sts2.Core.Entities.Relics/RelicRarity.cs`
- Secondary resources: `spire-codex/extraction/decompiled/MegaCrit.Sts2.Core.Entities.Players/PlayerCombatState.cs` (`Stars`, `GainStars` / `LoseStars`)
- Orb models: `spire-codex/extraction/decompiled/MegaCrit.Sts2.Core.Models/OrbModel.cs` (valid orb IDs: Lightning, Frost, Dark, Plasma, Glass)
- Strings: `spire-codex/extraction/raw/localization/eng/modifiers.json`, `orbs.json`, `enchantments.json`, `afflictions.json`, `card_keywords.json`, `ascension.json`
- **Hooks & model receivers:** `spire-codex/extraction/decompiled/MegaCrit.Sts2.Core.Hooks/Hook.cs` (all engine entry points), `MegaCrit.Sts2.Core.Models/AbstractModel.cs` (virtual methods hooks delegate to)
- **Commands (effect API):** `spire-codex/extraction/decompiled/MegaCrit.Sts2.Core.Commands/*.cs` (see **§15**)
- **Character card pool distributions (type / energy / rarity / target):** `BuuMod/CHARACTER_CARD_DISTRIBUTION.md`

**Legend:** ✅ at least one Buu implementation · ❌ none yet · N/A not expected for a playable character pool (engine / other systems)

**Change tags** (used in **§12** and **§17**):

| Tag | Meaning |
|-----|--------|
| **`[ADD]`** | New card, relic, power, pool entry, localization, asset, orb type, etc. |
| **`[UPDATE]`** | Change behavior, numbers, or text of something **already in BuuMod**. |
| **`[REFACTOR]`** | Rework implementation (same or similar design intent); may touch many call sites without a wholly new feature. |
| **`[REMOVE]`** | Delete or **retire** content (drop from pools, stop using a type, delete files). |
| **`[PATCH]`** | Engine / **BaseLib** / injected change not expressible as normal mod content alone (e.g. new field on `PlayerCombatState`, new `Hook` wrapper). |
| **`[OPTIONAL]`** | Fork, stretch goal, or alternative approach — not required for core parity. |

Combinations are allowed (e.g. **`[REFACTOR]`** + **`[REMOVE]`** when replacing `KiPower` with a meter).

---

## 1. CardType (`CardType` enum)

| Value   | Meaning (typical use)     | BuuMod |
|---------|---------------------------|--------|
| None    | Placeholder               | N/A    |
| Attack  | Damage (and similar)      | ✅ Many (e.g. Ki Strike, Pink Nova) |
| Skill   | One-shot effects        | ✅ Many (e.g. Reform, Super Form) |
| Power   | Persistent buffs        | ✅ Many (e.g. Ki Well, Omega) |
| Status  | Non-deck / temp effects | ❌ No Buu **card** uses this rarity/type as a token (optional future token cards) |
| Curse   | Penalty cards           | ❌ No Buu curse in pool (normal for a character mod) |
| Quest   | Quest-linked            | ❌ None |

---

## 2. CardRarity (in character card pool)

| Value    | BuuMod |
|----------|--------|
| Basic    | ✅ Ki Strike, Ki Barrier, Good Form, etc. |
| Common   | ✅ |
| Uncommon | ✅ |
| Rare     | ✅ |
| Ancient  | ✅ `PrimordialBlast`, `EonCovenant` |
| Event    | ❌ None (usually event-only) |
| Token    | ❌ No dedicated token cards |
| Status   | ❌ |
| Curse    | ❌ |
| Quest    | ❌ |

---

## 3. TargetType (`TargetType` enum)

| Value               | BuuMod | Example / note |
|---------------------|--------|----------------|
| None                | ✅     | Most **Power** cards |
| Self                | ✅     | Stances, block skills |
| AnyEnemy            | ✅     | Most single-target attacks |
| AllEnemies          | ✅     | Pink Blast, Kid Buu Spark, Chocolate Rain, … |
| RandomEnemy         | ✅     | `KiFlurry`, `FleetingBlast` |
| AnyPlayer           | ❌     | Multiplayer / rare effects |
| AnyAlly             | ❌     | Multiplayer / coop |
| AllAllies           | ❌     | Multiplayer / coop |
| TargetedNoCreature  | ❌     | Special UI targets |
| Osty                | ❌     | Special / internal |

---

## 4. CardKeyword (`CardKeyword` enum)

| Keyword    | BuuMod | Notes |
|------------|--------|--------|
| Exhaust    | ✅     | e.g. `SplitOff`, `Reform` (`CanonicalKeywords`) |
| Innate     | ✅     | e.g. `SuperAura`, `MajinMark`; several stances gain on upgrade |
| Retain     | ✅     | e.g. `InnocentStare` / `SuperStare` on upgrade; `RetainedKi` power interacts with retain |
| Ethereal   | ✅     | `FleetingBlast` |
| Eternal    | ✅     | `EternalBuu` gains **Eternal** on upgrade |
| Sly        | ✅     | `CopyTechnique` |
| Unplayable | ❌     | Usually curses / statuses (optional future) |

**Note:** “Replay” appears in base **enchantments** (e.g. Glam, Spiral), not in the `CardKeyword` enum — see §9.

---

## 5. RelicRarity (`RelicRarity` enum)

| Value    | BuuMod |
|----------|--------|
| Starter  | ✅ `CandyShell` |
| Common   | ✅ Multiple |
| Uncommon | ✅ Multiple |
| Rare     | ✅ Multiple |
| Shop     | ✅ `SweetVendorChit` (`ModifyMerchantPrice`) |
| Event    | ✅ `RestSiteCandy` (`ModifyRestSiteHealAmount`) |
| Ancient  | ✅ `PrimordialSlime` (`AfterActEntered` heal) |

---

## 6. Custom run modifiers (`localization/eng/modifiers.json`)

These are **run seeds / modes**, not character cards. Buu does not “implement” them; the base game does.

| ID               | Title            |
|------------------|------------------|
| ALL_STAR         | All Star         |
| BIG_GAME_HUNTER  | Big Game Hunter  |
| BINARY           | Binary           |
| CURSED_RUN       | Cursed Run       |
| DEADLY_EVENTS    | Deadly Events    |
| DRAFT            | Draft            |
| FLIGHT           | Flight           |
| HOARDER          | Hoarder          |
| INSANITY         | Insanity         |
| MIDAS            | Midas            |
| MURDEROUS        | Murderous        |
| NIGHT_TERRORS    | Night Terrors    |
| SEALED_DECK      | Sealed Deck      |
| SPECIALIZED      | Specialized      |
| TERMINAL         | Terminal         |
| VINTAGE          | Vintage          |

---

## 7. Ki as secondary resource (Regent **Stars** pattern)

**Design target:** Ki should behave like **Regent’s stars** — a **combat resource** on the player, not a **power / buff** in the creature’s power row.

| Approach | Base game reference | BuuMod today |
|----------|---------------------|--------------|
| **First-class secondary meter** | `PlayerCombatState`: `Stars`, `GainStars` / `LoseStars`, `StarsChanged`, star costs in `HasEnoughResourcesFor`, hooks such as `ShouldPayExcessEnergyCostWithStars` (`spire-codex/.../PlayerCombatState.cs`) | ❌ Not used; Ki is **`KiPower`** on the creature |
| **Powers** | Strength, Vulnerable, etc. | ✅ Ki amount is tracked like other powers (icon in power bar) |

**Why align with Stars:** Stars read as **spendable currency** next to energy. `KiPower` reads as a **buff** and is easier to confuse with combat effects that should be removable or separate from “how much Ki you have.”

**Implementation paths (checklist):**

1. **Preferred:** Extend combat state (or BaseLib patch) with a **Ki** counter mirroring `Stars` (gain/lose, history/events, UI hook, optional **Ki cost** on cards the way star costs work).
2. **Avoid unless deliberate:** Repurpose **`Stars`** for Buu only — only viable if Buu’s deck never uses real star costs and you accept hijacking that field.
3. **Until then:** Keep `KiPower` as a **stand-in**, and treat **§8 orbs** as an optional *different* system (Defect-style channel/evoke), not the same as “Ki meter.”

---

## 8. Orbs (base combat orbs — `orbs.json` + `OrbModel`)

| Orb      | Role (short)                          | BuuMod |
|----------|----------------------------------------|--------|
| LIGHTNING| Random-target damage (passive/evoke)  | ❌ No Buu-specific orb type |
| FROST    | Block                                   | ❌ |
| DARK     | Scaling damage to lowest HP             | ❌ |
| PLASMA   | Energy                                  | ❌ |
| GLASS    | AoE damage, decays                      | ❌ |

**Buu today:** Ki tracking uses **`KiPower`** (see **§7** — not the same as Regent’s `Stars` on `PlayerCombatState`). Ki is **not** implemented as `OrbModel` channel / evoke. There is **no** `OrbCmd.Channel<…>` usage in BuuMod yet.

**Desired direction (your ask):** custom **Ki** orb (e.g. passive: gain Ki; evoke: bonus Ki) and **damage** orb (e.g. evoke: AoE or focused damage), registered like `LightningOrb` under `MegaCrit.Sts2.Core.Models.Orbs`, plus `localization/eng/orbs.json` entries. **Blockers to confirm in-game:** whether Buu’s player has orb slots like Defect; if slots are 0, you may need a character hook or a card like `Modded` / `Capacitor` that adds slots before channeling works.

---

## 9. Card enchantments (`localization/eng/enchantments.json`)

Smith / special upgrades — not one-per-mod requirement, but full base set for transparency:

| Enchantment ID        | Title              |
|-----------------------|--------------------|
| ADROIT                | Adroit             |
| CLONE                 | Clone              |
| CORRUPTED             | Corrupted          |
| FAVORED               | Favored            |
| GLAM                  | Glam               |
| GOOPY                 | Goopy              |
| IMBUED                | Imbued             |
| INSTINCT              | Instinct           |
| MOMENTUM              | Momentum           |
| NIMBLE                | Nimble             |
| PERFECT_FIT           | Perfect Fit        |
| ROYALLY_APPROVED      | Royally Approved   |
| SHARP                 | Sharp              |
| SLITHER               | Slither            |
| SLUMBERING_ESSENCE    | Slumbering Essence |
| SOULS_POWER           | Soul's Power       |
| SOWN                  | Sown               |
| SPIRAL                | Spiral             |
| STEADY                | Steady             |
| SWIFT                 | Swift              |
| TEZCATARAS_EMBER      | Tezcatara's Ember  |
| VIGOROUS              | Vigorous           |

*(Excluded: mock / deprecated entries.)*

---

## 10. Card afflictions (`localization/eng/afflictions.json`)

Temporary card penalties in combat (base game). Buu cards are not required to apply each type; listed for completeness:

| ID        | Title      |
|-----------|------------|
| BOUND     | Bound      |
| ENTANGLED | Entangled  |
| GALVANIZED| Galvanized |
| HEXED     | Hexed      |
| RINGING   | Ringing    |
| SMOG      | Smog       |

---

## 11. Ascension (`ascension.json`)

Levels **0–10** with global run effects (elites, gold, potions, curse, scarcity, bosses, etc.). **Game system** — not per-mod.

---

## 12. Actionable gaps (if you want “≥1 of everything” **inside BuuMod**)

Priority order suggested:

1. **`[PATCH]`** + **`[REFACTOR]`** + **`[UPDATE]`** (+ often **`[REMOVE]`** for `KiPower` as the *primary* Ki store): **Ki resource** — migrate from **`KiPower`** to a **`PlayerCombatState`–style secondary meter** (mirror **Regent `Stars`** — **§8**): gain/lose API, UI, then retarget every card/relic/power that reads or writes Ki; **remove** or demote `KiPower` if redundant.
2. ~~**`[ADD]`** (+ **`[UPDATE]`** on upgrades): **Card keywords** — at least one card (or upgrade line) with **Ethereal**, **Eternal**, **Sly**~~ — done (`FleetingBlast`, `EternalBuu` upgrade, `CopyTechnique`). **`[OPTIONAL]`** **`[ADD]`** **Status**/**Curse** token for testing — still open.
3. ~~**`[ADD]`**: **TargetType** — one **RandomEnemy** attack (or skill) for enum parity~~ — done (`KiFlurry`, `FleetingBlast`).
4. ~~**`[ADD]`** (+ **`[UPDATE]`** to `BuuRelicPool` / character): **Relic rarities** — one relic each **Shop**, **Event**, **Ancient** (icons + wiring)~~ — done (`SweetVendorChit`, `RestSiteCandy`, `PrimordialSlime`; pool attribute + `BuuStartingRelics.All`).
5. **`[ADD]`** + **`[PATCH]`** **`[OPTIONAL]`**: **Orbs** — `KiOrb` / `DamageOrb` + `orbs.json` + channel cards; may need slot/capacity **patch** for Buu (**§8**, **§7**).
6. ~~**`[ADD]`** **`[OPTIONAL]`**: **CardRarity.Ancient** — Ancient-era Buu cards + reward hooks~~ — cards done (`PrimordialBlast`, `EonCovenant`); reward / epoch hooks still optional polish.
7. **`[ADD]`** / **`[UPDATE]`**: **Engine hooks** — new or extended overrides on **`AbstractModel`** subclasses (relics, powers, character) for **`Hook`** callbacks (**§13–§14**).

---

## 13. Decompiled engine: how `Hook` works

| Piece | Path (under `spire-codex/extraction/decompiled/`) | Role |
|-------|--------------------------------------------------|------|
| **`Hook`** | `MegaCrit.Sts2.Core.Hooks/Hook.cs` | Static **`Before*` / `After*`** (async), **`Modify*`** (sync aggregators), **`Should*`** (boolean gates). |
| **Listeners** | Anything extending **`AbstractModel`** | Virtual methods; the game iterates **hook listeners** from combat or run state (`IterateHookListeners`). Cards/powers/relics/character models participate when registered. |
| **Practical note** | — | Your mod stack (e.g. **BaseLib**) may wrap or expose only part of this surface; use the decomp as the **full** contract when designing features (Ki UI, reward injection, orb hooks, etc.). |

---

## 14. `Hook.cs` — full API inventory (decompiled)

*Every name below is a `public static` method on `Hook`. Grouped for scanning; line order may differ in future game versions.*

### 14a. Async lifecycle — combat, cards, damage, block

`BeforeAttack`, `AfterAttack` · `BeforeBlockGained`, `AfterBlockGained` · `AfterBlockBroken`, `AfterBlockCleared` · `BeforeDamageReceived`, `AfterDamageReceived`, `AfterDamageGiven` · `BeforeDeath`, `AfterDeath` · `AfterCurrentHpChanged` · `AfterCreatureAddedToCombat` · `BeforePowerAmountChanged`, `AfterPowerAmountChanged` · `BeforeCardPlayed`, `AfterCardPlayed` (and models’ `AfterCardPlayedLate`) · `BeforeCardAutoPlayed` · `AfterCardDrawn` (early + main) · `AfterCardDiscarded` · `AfterCardExhausted` · `AfterCardRetained` · `AfterCardEnteredCombat` · `AfterCardGeneratedForCombat` · `AfterHandEmptied` · `BeforeHandDraw` · `AfterShuffle` · `BeforeFlush` · `AfterEnergyReset`, `AfterEnergySpent` · `BeforeTurnEnd`, `AfterTurnEnd` · `BeforeSideTurnStart`, `AfterSideTurnStart` · `AfterPlayerTurnStart` · `BeforePlayPhaseStart` · `AfterSummon` · `AfterOrbChanneled`, `AfterOrbEvoked` · `AfterOstyRevived` · `AfterTakingExtraTurn` · `AfterDiedToDoom` · `AfterPreventingBlockClear`, `AfterPreventingDeath`, `AfterPreventingDraw`

### 14b. Async lifecycle — run, map, rooms, rewards, economy

`AfterActEntered` · `BeforeCombatStart` · `AfterCombatEnd`, `AfterCombatVictory` · `BeforeRoomEntered`, `AfterRoomEntered` · `AfterMapGenerated` · `BeforeRewardsOffered` · `AfterRewardTaken`, `AfterModifyingRewards` · `AfterGoldGained` · `AfterItemPurchased` · `BeforeCardRemoved` · `AfterCardChangedPiles` (and `AfterCardChangedPilesLate`) · `AfterRestSiteHeal`, `AfterRestSiteSmith`, `AfterForge` · `AfterPotionDiscarded`, `AfterPotionProcured` · `BeforePotionUsed`, `AfterPotionUsed`

*Some flows call paired **Late** methods on individual `AbstractModel` instances (e.g. `BeforeFlush` + `BeforeFlushLate`, `AfterCardPlayed` + `AfterCardPlayedLate`) — see `AbstractModel`.*

### 14c. Stars (Regent secondary resource)

`AfterStarsGained`, `AfterStarsSpent` — pairs with **`ShouldGainStars`**, **`ShouldPayExcessEnergyCostWithStars`** (in **§14e**).

### 14d. `Modify*` — numeric / data shaping (sync)

`ModifyAttackHitCount` · `ModifyBlock` (+ `AfterModifyingBlockAmount`) · `ModifyDamage` (+ `AfterModifyingDamageAmount`; uses `ModifyDamageHookType`, preview mode; internal additive / multiplicative / cap pass) · `ModifyEnergyCostInCombat` · `ModifyHandDraw` (+ `AfterModifyingHandDraw`) · `ModifyMaxEnergy` · `ModifyHealAmount` · `ModifyHpLostBeforeOsty`, `ModifyHpLostAfterOsty` (+ matching `AfterModifying*` callbacks) · `ModifyPowerAmountGiven`, `ModifyPowerAmountReceived` (+ `AfterModifying*`) · `ModifyOrbValue`, `ModifyOrbPassiveTriggerCount` (+ `AfterModifyingOrbPassiveTriggerCount`) · `ModifyXValue` · `ModifyStarCost` · `ModifySummonAmount` · `ModifyUnblockedDamageTarget` · `ModifyCardPlayCount` (+ `AfterModifyingCardPlayCount`) · `ModifyCardPlayResultPileTypeAndPosition` · `ModifyShuffleOrder` · `ModifyCardBeingAddedToDeck` · `ModifyCardRewardCreationOptions`, `TryModifyCardRewardOptions`, `ModifyCardRewardAlternatives`, `ModifyCardRewardUpgradeOdds` (+ `AfterModifyingCardRewardOptions`) · `ModifyRewards` (+ `AfterModifyingRewards`) · `ModifyGeneratedMap`, `ModifyGeneratedMapLate` · `ModifyUnknownMapPointRoomTypes` · `ModifyOddsIncreaseForUnrolledRoomType` · `ModifyNextEvent` · `ModifyMerchantPrice`, `ModifyMerchantCardPool`, `ModifyMerchantCardCreationResults`, `ModifyMerchantCardRarity` · `ModifyRestSiteHealAmount`, `ModifyRestSiteOptions`, `ModifyRestSiteHealRewards` · `ModifyExtraRestSiteHealText`

### 14e. `Should*` — permission / flow gates (sync)

`ShouldAddToDeck` · `ShouldAfflict` (card **afflictions**) · `ShouldAllowAncient` · `ShouldAllowHitting` · `ShouldAllowMerchantCardRemoval` · `ShouldAllowSelectingMoreCardRewards` · `ShouldAllowTargeting` · `ShouldClearBlock` · `ShouldCreatureBeRemovedFromCombatAfterDeath` · `ShouldDie` (second pass on listeners uses `AbstractModel.ShouldDieLate`) · `ShouldDisableRemainingRestSiteOptions` · `ShouldDraw` · `ShouldEtherealTrigger` · `ShouldFlush` · `ShouldGainGold` · `ShouldGainStars` · `ShouldGenerateTreasure` · `ShouldPayExcessEnergyCostWithStars` · `ShouldPlay` · `ShouldPlayerResetEnergy` · `ShouldProceedToNextMapPoint` · `ShouldProcurePotion` · `ShouldRefillMerchantEntry` · `ShouldStopCombatFromEnding` · `ShouldTakeExtraTurn` · `ShouldForcePotionReward` · `ShouldPowerBeRemovedOnDeath`

---

## 15. `MegaCrit.Sts2.Core.Commands` — effect entry points (decompiled)

Static command facades your card/power/relic code (or patches) can mirror:

| Command | Typical use |
|---------|-------------|
| `CardCmd` | Play, exhaust, discard pile ops, transforms, etc. |
| `CardPileCmd` | Pile movement |
| `CardSelectCmd` | Choosers |
| `CreatureCmd` | Block, HP, damage helpers |
| `DamageCmd` | Damage pipeline |
| `PowerCmd` | Apply / modify powers |
| `OrbCmd` | Channel, evoke, slots |
| `PotionCmd` | Potions |
| `RelicCmd`, `RelicSelectCmd` | Relics |
| `RewardsCmd` | Reward flow |
| `ForgeCmd` | Smith / forging |
| `MapCmd` | Map manipulation |
| `PlayerCmd` | Player-scoped actions |
| `OstyCmd` | **Osty** (companion / revive-adjacent flow in hooks) |
| `VfxCmd`, `SfxCmd` | Presentation |
| `ThinkCmd`, `TalkCmd` | Dialogue / think bubbles |
| `Cmd` | Shared / base |

---

## 16. Other decompiled features worth remembering (hooks-adjacent)

| Area | Where to look | Why it matters for mods |
|------|----------------|-------------------------|
| **Osty** | `OstyCmd`, `AfterOstyRevived`, HP lost hooks with `Before`/`AfterOsty` | Pet / second-life style mechanics |
| **Summon** | `AfterSummon`, `ModifySummonAmount` | Summon value changes |
| **Extra turns** | `AfterTakingExtraTurn`, `ShouldTakeExtraTurn` | Time-walk style effects |
| **Orbs** | `OrbCmd`, `ModifyOrbValue`, orb channel/evoke **After*** hooks | Custom orbs + Focus-like scaling |
| **Multiplayer** | e.g. `NetFullCombatState` (stars, caps) | If you ship MP-safe behavior, sync fields may matter |
| **Damage shape** | `ModifyDamageHookType` (additive / multiplicative / cap paths) | Stance / vulnerability math |
| **Card rewards** | `TryModifyCardRewardOptions`, alternatives, upgrade odds | Pool injection, Du-Vu style |
| **Merchant / rest** | `ModifyMerchant*`, `ModifyRestSite*` | Economy and rest UX |
| **Map** | `ModifyGeneratedMap` (+ `Late`), unknown room types | Path / seed modifiers |

---

## 17. Design ideas — leveraging this foundation later

*Concrete directions that use **§7** (Ki vs stars), **§8** (orbs), **§12** (gaps), and **§13–§16** (hooks / commands). Treat as a backlog, not a commitment. Each bullet is tagged — see **change tags** under the legend.*

### Ki as a real secondary resource

- **`[PATCH]`** **`[ADD]`** **`[REFACTOR]`** — **`AfterStarsGained` / `AfterStarsSpent` pattern:** add engine/BaseLib **Ki gained / spent** hooks mirroring stars once Ki lives on combat state; **`[ADD]`** relics (“first Ki spent each turn…”).
- **`[PATCH]`** **`[OPTIONAL]`** — **`ModifyKiCost`** (or map Ki to star-cost plumbing for Buu only): keeps cost math out of power stacking.
- **`[ADD]`** relic + **`[UPDATE]`** to energy payment rules if you bridge via existing **`ShouldPayExcessEnergyCostWithStars`**-style hook for Ki — “pay missing energy with Ki at 2:1.”
- **`[ADD]`** relic(s): start combat +Ki; hand-size / draw riders using **`AfterTurnEnd`** (may **`[UPDATE]`** character or **`[PATCH]`** if max hand is engine-owned).

### Orbs (separate from the Ki meter)

- **`[ADD]`** orb model + loc + **`[ADD]`** card(s): Ki-channel orb (passive Ki, evoke damage/Vulnerable); uses **`AfterOrbChanneled`**, **`ModifyOrbValue`**.
- **`[ADD]`** orb + VFX + **`[ADD]`** relic: volatile damage orb; **`AfterOrbEvoked`** refunds Ki.
- **`[ADD]`** card: multi-channel with weighted orb types — **`OrbCmd`** + pool registration; may **`[PATCH]`** orb slots for Buu.

### Stances + combat hooks

- **`[REFACTOR]`** **`[UPDATE]`** — centralize stance damage in **`ModifyDamage`** (or one **Power**/character listener) instead of per-card duplication; **`[REMOVE]`** redundant modifiers from individual cards if folded here.
- **`[ADD]`** **Power** or **`[ADD]`** relic: **`AfterBlockGained`** + Regular-stance gate (+block first time / turn).
- **`[ADD]`** relic + **`[REFACTOR]`** **`[REMOVE]`** — move **Rage**-style stacking from a **Power** to a **relic** listener on **`AfterDamageReceived`**; may **`[UPDATE]`** or **`[REMOVE]`** existing **Rage** power card depending on design.

### Card-keyword and targeting completion (§4, §3)

- **`[ADD]`** card: **Ethereal** “bomb” — tests **`ShouldEtherealTrigger`**.
- **`[ADD]`** card (or **`[UPDATE]`** upgrade): **Eternal** build-around.
- **`[ADD]`** relic + **`[ADD]`**/**`[UPDATE]`** cards: **Sly** + **`AfterCardDiscarded`** synergy.
- **`[ADD]`** card: **RandomEnemy** “Ki flurry” — new **TargetType** usage.

### Run layer — map, merchant, rest, rewards

- **`[ADD]`** **Shop** relic: **`ModifyMerchantPrice`** or **`ModifyMerchantCardPool`** (see **§12** item 4).
- **`[ADD]`** **Event** relic: **`ModifyNextEvent`** bias.
- **`[ADD]`** relic / event + **`[UPDATE]`** ancient rules: **`ShouldAllowAncient`** gating or softening.
- **`[ADD]`** rest option via **`ModifyRestSiteOptions`** (or **`[PATCH]`** if options are hard-coded).
- **`[ADD]`** relic or character hook: **`TryModifyCardRewardOptions`** / **`ModifyCardRewardUpgradeOdds`** + run flag from **`AfterCombatEnd`**.

### Relic rarities you do not have yet (§5)

- **`[ADD]`** **Shop** relic — same row as §17 “Run layer” (price / pool).
- **`[ADD]`** **Event** relic — transform card / Ki↔gold at events.
- **`[ADD]`** **Ancient** relic — act-wide rule; likely **`[PATCH]`** or **`[REFACTOR]`** if Ki persistence crosses combats.

### Potions, forge, deck rules

- **`[ADD]`** Buu potion pool + **`[ADD]`** relic: **`AfterPotionUsed`** / **`ShouldProcurePotion`**.
- **`[ADD]`** relic: **`AfterForge`** bonus Ki next combat.
- **`[ADD]`** relic: **`ModifyCardBeingAddedToDeck`** / **`ShouldAddToDeck`** (Hoarder synergy / curse block).

### Wildcard / late-game fantasy

- **`[ADD]`** card: extra turn + **`[UPDATE]`** cleanup in **`AfterTakingExtraTurn`** if Ki rules need reset.
- **`[ADD]`** cards/relics + **`[PATCH]`** **`[OPTIONAL]`**: summon / **Osty**-style line (**`ModifySummonAmount`**, **`AfterOstyRevived`**).
- **`[ADD]`** encounter + **`[PATCH]`** **`[OPTIONAL]`**: **`ShouldStopCombatFromEnding`** boss phase.

### Process tip

- **`[UPDATE]`** design docs: when an idea maps to **§12**, record **change tag** + **Hook / Command** names so implementers know **ADD vs REFACTOR vs REMOVE**.

---

*Last aligned to spire-codex extraction in this workspace (including `Hook.cs` and `MegaCrit.Sts2.Core.Commands`); re-run if the game patches.*
