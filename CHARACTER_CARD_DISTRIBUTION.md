# Character card pool distribution (type, energy, rarity, target)

**Scope**

- **Vanilla (Slay the Spire 2):** counts are from **`spire-codex/extraction/decompiled/MegaCrit.Sts2.Core.Models.CardPools/*CardPool.cs`** — every `ModelDb.Card<…>()` entry in `GenerateAllCards()` for that character. This is the **full** pool before **epoch / unlock** filtering (`FilterThroughEpochs` may hide cards until progression).
- **Buu (mod):** counts are from every concrete card class under `BuuMod/BuuCode/Cards/` whose declaration includes **`… : BuuCard(energy, CardType, CardRarity, TargetType)`** (primary constructors; excludes abstract `BuuCard.cs`).
- **Energy** = numeric first argument to `base` / primary constructor (`CardModel` energy cost). **`X`** = card overrides **`HasEnergyCostX => true`** (counts separately).
- **Star cost (Regent only):** cards with **`CanonicalStarCost >= 0`** or **`HasStarCostX`**; other characters use the default “no star cost” for most cards.

**Characters in this decompile**

| Pool class | Role |
|------------|------|
| `IroncladCardPool` | Red |
| `SilentCardPool` | Green |
| `DefectCardPool` | Blue |
| `RegentCardPool` | Purple / stars |
| `NecrobinderCardPool` | Additional playable |

---

## Totals

| Character | Cards in pool (vanilla rows) |
|-----------|------------------------------|
| Ironclad | 87 |
| Silent | 88 |
| Defect | 88 |
| Regent | 88 |
| Necrobinder | 88 |
| **Buu (mod)** | **113** |

---

## By `CardType`

| Character | Attack | Skill | Power | % Attack | % Skill | % Power |
|-----------|-------:|------:|------:|---------:|--------:|--------:|
| Ironclad | 38 | 28 | 21 | 44% | 32% | 24% |
| Silent | 28 | 41 | 19 | 32% | 47% | 22% |
| Defect | 29 | 39 | 20 | 33% | 44% | 23% |
| Regent | 33 | 36 | 19 | 38% | 41% | 22% |
| Necrobinder | 35 | 35 | 18 | 40% | 40% | 20% |
| Buu (mod) | 42 | 47 | 24 | 37% | 42% | 21% |

---

## By base energy cost (numeric + X)

| Cost | Ironclad | Silent | Defect | Regent | Necrobinder | Buu (mod) |
|-----:|---------:|-------:|-------:|-------:|------------:|----------:|
| 0 | 12 | 15 | 15 | 20 | 11 | 13 |
| 1 | 47 | 44 | 48 | 48 | 52 | 65 |
| 2 | 19 | 17 | 17 | 14 | 14 | 30 |
| 3 | 7 | 10 | 5 | 4 | 7 | 4 |
| 4 | — | — | — | 1 | 1 | — |
| 5 | — | — | 1 | — | — | — |
| 6 | — | — | — | — | 1 | — |
| **X** | 2 | 2 | 2 | 1 | 2 | 1 |

---

## By `CardRarity` (pool classification)

| Rarity | Ironclad | Silent | Defect | Regent | Necrobinder | Buu (mod) |
|--------|---------:|-------:|-------:|-------:|------------:|----------:|
| Basic | 3 | 4 | 4 | 4 | 4 | 5 |
| Common | 20 | 20 | 20 | 20 | 20 | 42 |
| Uncommon | 36 | 36 | 36 | 36 | 36 | 40 |
| Rare | 26 | 26 | 26 | 26 | 26 | 24 |
| Ancient | 2 | 2 | 2 | 2 | 2 | 2 |

---

## By `TargetType`

### Ironclad

| Target | Count |
|--------|------:|
| Self | 45 |
| AnyEnemy | 33 |
| AllEnemies | 7 |
| AnyAlly | 1 |
| RandomEnemy | 1 |

### Silent

| Target | Count |
|--------|------:|
| Self | 49 |
| AnyEnemy | 30 |
| AllEnemies | 7 |
| RandomEnemy | 2 |

### Defect

| Target | Count |
|--------|------:|
| Self | 57 |
| AnyEnemy | 25 |
| AllEnemies | 3 |
| RandomEnemy | 1 |
| AnyAlly | 1 |
| AllAllies | 1 |

### Regent

| Target | Count |
|--------|------:|
| Self | 51 |
| AnyEnemy | 27 |
| AllEnemies | 8 |
| AnyAlly | 1 |
| RandomEnemy | 1 |

### Necrobinder

| Target | Count |
|--------|------:|
| AnyEnemy | 39 |
| Self | 40 |
| AllEnemies | 7 |
| AllAllies | 2 |

### Buu (mod)

| Target | Count |
|--------|------:|
| Self | 47 |
| None | 24 |
| AnyEnemy | 35 |
| AllEnemies | 5 |
| RandomEnemy | 2 |

---

## Regent — secondary **star** cost (cards that define `CanonicalStarCost` or star-X)

| Star cost | Cards |
|----------:|------:|
| 1 | 2 |
| 2 | 6 |
| 3 | 6 |
| 4 | 2 |
| 5 | 3 |
| 6 | 1 |
| 7 | 1 |
| *Other pool cards* | *67 (energy only, default star cost unset / −1)* |

*Regent is the only vanilla pool here with a meaningful split between **energy** and **stars**; see `PlayerCombatState` / **§7** in `MECHANICS_COVERAGE_CHECKLIST.md`.*

---

## How to regenerate (vanilla)

From repo root, with Python 3:

1. Parse `ModelDb.Card<Name>()` from each `*CardPool.cs` in `spire-codex/extraction/decompiled/MegaCrit.Sts2.Core.Models.CardPools/`.
2. For each `Name`, read `MegaCrit.Sts2.Core.Models.Cards/Name.cs` and extract `: base(energy, CardType, …)` (or equivalent `base(` call).
3. Detect **X** energy via `HasEnergyCostX => true`.
4. For stars, grep `CanonicalStarCost` / `HasStarCostX`.

### Buu mod (this repo)

From repo root, Python 3 (counts sealed `: BuuCard(` rows, skips `BuuCard.cs`; add `HasEnergyCostX => true` checks per file for **X**):

```python
import re
from pathlib import Path
from collections import Counter

root = Path("BuuMod/BuuCode/Cards")
pat = re.compile(
    r":\s*BuuCard\s*\(\s*(\d+)\s*,\s*CardType\.(\w+)\s*,\s*CardRarity\.(\w+)\s*,\s*TargetType\.(\w+)\s*\)"
)
cards = []
for p in root.rglob("*.cs"):
    if p.name == "BuuCard.cs":
        continue
    t = p.read_text(encoding="utf-8")
    m = pat.search(t)
    if m:
        has_x = bool(re.search(r"HasEnergyCostX\s*=>\s*true", t))
        cards.append((int(m[1]), m[2], m[3], m[4], has_x))
print("total", len(cards))
```

---

*Generated from decompiled STS2 card pools + BuuMod sources in this workspace; re-run counts after game patches or mod changes.*
