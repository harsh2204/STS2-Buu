#!/usr/bin/env python3
"""
Generate docs/data.json and copy Buu/images assets into docs/assets for GitHub Pages.
Run from repo root: python docs/build_data.py
Or from docs/: python build_data.py
"""
from __future__ import annotations

import html
import json
import re
import shutil
from pathlib import Path

DOCS = Path(__file__).resolve().parent
ROOT = DOCS.parent
CARDS_DIR = ROOT / "BuuCode" / "Cards"
RELICS_DIR = ROOT / "BuuCode" / "Relics"
LOC = ROOT / "Buu" / "localization" / "eng"
IMAGES = ROOT / "Buu" / "images"
OUT_ASSETS = DOCS / "assets"


def pascal_to_snake(name: str) -> str:
    """Match Buu.BuuCode.Extensions.StringExtensions.ToSnakeCase."""
    if not name:
        return name
    sb: list[str] = []
    for i, c in enumerate(name):
        if c.isupper() and i > 0:
            prev = name[i - 1]
            nxt = name[i + 1] if i + 1 < len(name) else ""
            if prev.islower() or (nxt and nxt.islower()):
                sb.append("_")
        sb.append(c.lower())
    return "".join(sb)


def resolve_portrait_snake(class_snake: str) -> str:
    return {
        "ki_strike": "ki_blast",
        "ki_barrier": "bubble_barrier",
        "ki_well": "regenerate",
        "majin_fury": "evil_emerges",
        "super_spirit": "super_aura",
        "regenerative_form": "reform",
    }.get(class_snake, class_snake)


def parse_card_files() -> list[dict]:
    pattern = re.compile(
        r"public sealed class (\w+)\(\)\s*:\s*BuuCard\([^,]+,\s*CardType\.(\w+),\s*CardRarity\.(\w+)",
    )
    big_img = re.compile(r'"(?P<file>[\w_]+)\.png"\s*\.BigCardImagePath\(\)')
    cards: list[dict] = []
    for path in CARDS_DIR.rglob("*.cs"):
        if path.name == "BuuCard.cs":
            continue
        text = path.read_text(encoding="utf-8")
        m = pattern.search(text)
        if not m:
            continue
        class_name, card_type, rarity = m.groups()
        class_snake = pascal_to_snake(class_name)
        loc_id = f"BUU-{class_snake.upper()}"
        portrait = resolve_portrait_snake(class_snake)
        big = big_img.search(text)
        use_big = big is not None
        if big:
            portrait = big.group("file")
        cards.append(
            {
                "className": class_name,
                "locId": loc_id,
                "type": card_type,
                "rarity": rarity,
                "portrait": portrait,
                "useBigPortrait": use_big,
                "sourceFile": str(path.relative_to(ROOT)),
            }
        )
    cards.sort(key=lambda c: (c["rarity"], c["locId"]))
    return cards


def parse_relic_files() -> list[dict]:
    pattern = re.compile(r"public sealed class (\w+)\(\)\s*:\s*BuuRelic")
    relics: list[dict] = []
    for path in RELICS_DIR.glob("*.cs"):
        if path.name == "BuuRelic.cs":
            continue
        text = path.read_text(encoding="utf-8")
        m = pattern.search(text)
        if not m:
            continue
        class_name = m.group(1)
        icon = pascal_to_snake(class_name)
        loc_id = f"BUU-{icon.upper()}"
        relics.append(
            {
                "className": class_name,
                "locId": loc_id,
                "icon": icon,
                "sourceFile": str(path.relative_to(ROOT)),
            }
        )
    relics.sort(key=lambda r: r["locId"])
    return relics


def load_loc_strings(path: Path) -> dict[str, str]:
    return json.loads(path.read_text(encoding="utf-8"))


def strip_bb(text: str) -> str:
    return re.sub(r"\[/?\w+\]", "", text)


def format_decimal(num: str) -> str:
    x = float(num)
    if x == int(x):
        return str(int(x))
    s = f"{x:.4f}".rstrip("0").rstrip(".")
    return s


def extract_cs_consts(cs_text: str) -> dict[str, str]:
    consts: dict[str, str] = {}
    for m in re.finditer(
        r"(?:private\s+|public\s+)?const\s+decimal\s+(\w+)\s*=\s*([\d.]+)m\b",
        cs_text,
    ):
        consts[m.group(1)] = format_decimal(m.group(2))
    for m in re.finditer(r"(?:private\s+|public\s+)?const\s+int\s+(\w+)\s*=\s*(\d+)\s*;", cs_text):
        consts[m.group(1)] = m.group(2)
    return consts


def resolve_expr(expr: str, consts: dict[str, str]) -> str:
    expr = expr.strip()
    if re.fullmatch(r"[\d.]+m", expr):
        return format_decimal(expr[:-1])
    if re.fullmatch(r"\d+", expr):
        return expr
    m = re.fullmatch(r"\(int\)\s*(\w+)", expr)
    if m:
        ident = m.group(1)
        return consts.get(ident, "?")
    if expr in consts:
        return consts[expr]
    return "?"


def _paren_arg_after(text: str, open_paren_idx: int) -> str:
    """Text inside (...) starting at open_paren_idx."""
    if open_paren_idx < 0 or open_paren_idx >= len(text) or text[open_paren_idx] != "(":
        return ""
    depth = 1
    start = open_paren_idx + 1
    i = start
    while i < len(text) and depth:
        c = text[i]
        if c == "(":
            depth += 1
        elif c == ")":
            depth -= 1
        i += 1
    return text[start : i - 1] if depth == 0 else ""


def parse_dynamic_values(cs_text: str) -> dict[str, str]:
    consts = extract_cs_consts(cs_text)
    values: dict[str, str] = {}
    for m in re.finditer(r"new DamageVar\(([^,]+),", cs_text):
        values["Damage"] = resolve_expr(m.group(1), consts)
    for m in re.finditer(r"new BlockVar\(([^,]+),", cs_text):
        values["Block"] = resolve_expr(m.group(1), consts)
    for m in re.finditer(r"new CardsVar\(", cs_text):
        arg = _paren_arg_after(cs_text, m.end() - 1)
        values["Cards"] = resolve_expr(arg.strip(), consts)
    for m in re.finditer(r"new HealVar\(", cs_text):
        arg = _paren_arg_after(cs_text, m.end() - 1)
        values["Heal"] = resolve_expr(arg.strip(), consts)
    for m in re.finditer(r"new PowerVar<(\w+)>\(", cs_text):
        arg = _paren_arg_after(cs_text, m.end() - 1)
        values[m.group(1)] = resolve_expr(arg.strip(), consts)
    for m in re.finditer(r'new DynamicVar\("(\w+)",\s*([^)]+)\)', cs_text):
        values[m.group(1)] = resolve_expr(m.group(2).strip(), consts)
    return values


def eval_delta(expr: str, consts: dict[str, str]) -> float | None:
    """Evaluate UpgradeValueBy(...) argument: literals, const refs, and A - B."""
    expr = expr.strip()
    while expr.startswith("(int)"):
        expr = expr[5:].strip()
    while expr.startswith("(") and expr.endswith(")"):
        inner = expr[1:-1].strip()
        if not inner:
            break
        expr = inner
    if re.fullmatch(r"[\d.]+m", expr):
        return float(expr[:-1])
    if re.fullmatch(r"-?\d+", expr):
        return float(int(expr))
    if " - " in expr:
        a, b = expr.split(" - ", 1)
        va = eval_delta(a.strip(), consts)
        vb = eval_delta(b.strip(), consts)
        if va is not None and vb is not None:
            return va - vb
        return None
    if expr in consts:
        return float(consts[expr])
    return None


def iter_upgrade_ops(cs_text: str) -> list[tuple[str, str]]:
    """(localization_dynamic_key, delta_expression) from DynamicVars.*.UpgradeValueBy."""
    ops: list[tuple[str, str]] = []
    for m in re.finditer(r"DynamicVars\.Damage\.UpgradeValueBy\(([^)]+)\)", cs_text):
        ops.append(("Damage", m.group(1)))
    for m in re.finditer(r"DynamicVars\.Block\.UpgradeValueBy\(([^)]+)\)", cs_text):
        ops.append(("Block", m.group(1)))
    for m in re.finditer(r"DynamicVars\.Cards\.UpgradeValueBy\(([^)]+)\)", cs_text):
        ops.append(("Cards", m.group(1)))
    for m in re.finditer(r"DynamicVars\.Heal\.UpgradeValueBy\(([^)]+)\)", cs_text):
        ops.append(("Heal", m.group(1)))
    for m in re.finditer(r"DynamicVars\.Poison\.UpgradeValueBy\(([^)]+)\)", cs_text):
        ops.append(("PoisonPower", m.group(1)))
    for m in re.finditer(r"DynamicVars\.Vulnerable\.UpgradeValueBy\(([^)]+)\)", cs_text):
        ops.append(("VulnerablePower", m.group(1)))
    for m in re.finditer(r'DynamicVars\["([^"]+)"\]\.UpgradeValueBy\(([^)]+)\)', cs_text):
        ops.append((m.group(1), m.group(2)))
    return ops


def apply_upgrades(base: dict[str, str], cs_text: str) -> dict[str, str]:
    consts = extract_cs_consts(cs_text)
    up = dict(base)
    for key, expr in iter_upgrade_ops(cs_text):
        delta = eval_delta(expr, consts)
        if delta is None:
            continue
        cur = up.get(key)
        if cur is None or cur == "?":
            continue
        try:
            up[key] = format_decimal(str(float(cur) + delta))
        except ValueError:
            continue
    return up


_VAR_PLACEHOLDER = re.compile(r"\{(\w+):diff\(\)\}")
_BBCODE = re.compile(r"\[(gold|pink)\](.*?)\[/\1\]", re.DOTALL)


def apply_bbcode_to_html(fragment: str) -> str:
    def repl(m: re.Match[str]) -> str:
        cls = "loc-gold" if m.group(1) == "gold" else "loc-pink"
        inner = apply_bbcode_to_html(m.group(2))
        return f'<span class="{cls}">{inner}</span>'

    return _BBCODE.sub(repl, fragment)


def format_game_description(raw: str, values: dict[str, str]) -> str:
    if not raw:
        return ""
    parts: list[str] = []
    pos = 0
    for m in _VAR_PLACEHOLDER.finditer(raw):
        parts.append(html.escape(raw[pos : m.start()]))
        key = m.group(1)
        v = values.get(key)
        if v is None:
            parts.append('<span class="loc-num loc-num--missing">?</span>')
        else:
            parts.append(f'<span class="loc-num">{html.escape(v)}</span>')
        pos = m.end()
    parts.append(html.escape(raw[pos:]))
    return apply_bbcode_to_html("".join(parts))


def plain_description_for_search(raw: str, values: dict[str, str]) -> str:
    def repl_var(m: re.Match[str]) -> str:
        return values.get(m.group(1), "?")

    s = _VAR_PLACEHOLDER.sub(repl_var, raw)
    return re.sub(r"\[/?\w+\]", "", s)


def portrait_fs_path(portrait: str, use_big: bool) -> Path | None:
    big = IMAGES / "card_portraits" / "big" / f"{portrait}.png"
    small = IMAGES / "card_portraits" / f"{portrait}.png"
    if use_big and big.exists():
        return big
    if small.exists():
        return small
    if big.exists():
        return big
    return None


def copy_card_portraits(cards: list[dict]) -> None:
    dest_dir = OUT_ASSETS / "cards"
    dest_dir.mkdir(parents=True, exist_ok=True)
    for c in cards:
        src = portrait_fs_path(c["portrait"], c["useBigPortrait"])
        if not src:
            c["imageRel"] = None
            continue
        sub = "big" if "big" in src.parts else "standard"
        name = f"{c['portrait']}.png"
        rel = f"assets/cards/{sub}/{name}"
        out = dest_dir / sub / name
        out.parent.mkdir(parents=True, exist_ok=True)
        shutil.copy2(src, out)
        c["imageRel"] = rel


def copy_relic_icons(relics: list[dict]) -> None:
    dest = OUT_ASSETS / "relics"
    dest.mkdir(parents=True, exist_ok=True)
    src_dir = IMAGES / "relics"
    for r in relics:
        name = f"{r['icon']}.png"
        sp = src_dir / name
        if sp.exists():
            shutil.copy2(sp, dest / name)
            r["imageRel"] = f"assets/relics/{name}"
        else:
            r["imageRel"] = None


def copy_character_art() -> dict:
    paths = {
        "icon": IMAGES / "charui" / "character_icon_buu.png",
        "select": IMAGES / "packed" / "character_select" / "character_select_buu.png",
        "merchant": IMAGES / "buu" / "buu_merchant.png",
    }
    out: dict[str, str | None] = {}
    char_dir = OUT_ASSETS / "character"
    char_dir.mkdir(parents=True, exist_ok=True)
    for key, src in paths.items():
        if src.exists():
            dst = char_dir / src.name
            shutil.copy2(src, dst)
            out[key] = f"assets/character/{src.name}"
        else:
            out[key] = None
    return out


def powers_from_loc(loc: dict[str, str]) -> list[dict]:
    powers: list[dict] = []
    seen: set[str] = set()
    for k in loc:
        if not k.startswith("BUU-") or not k.endswith(".title"):
            continue
        base = k[: -len(".title")]
        if "_POWER" not in base and base not in (
            "BUU-REGULAR_STANCE",
            "BUU-MAJIN_STANCE",
            "BUU-SUPER_STANCE",
        ):
            continue
        if base in seen:
            continue
        seen.add(base)
        desc_key = f"{base}.smartDescription"
        if desc_key not in loc:
            desc_key = f"{base}.description"
        powers.append(
            {
                "id": base,
                "title": loc[k],
                "description": strip_bb(loc.get(desc_key, "")),
            }
        )
    powers.sort(key=lambda p: p["id"])
    return powers


def main() -> None:
    cards = parse_card_files()
    relics = parse_relic_files()
    cards_loc = load_loc_strings(LOC / "cards.json")
    relics_loc = load_loc_strings(LOC / "relics.json")
    powers_loc = load_loc_strings(LOC / "powers.json")
    chars_loc = load_loc_strings(LOC / "characters.json")

    for card in cards:
        lid = card["locId"]
        card["title"] = cards_loc.get(f"{lid}.title", card["className"])
        cs_text = (ROOT / card["sourceFile"]).read_text(encoding="utf-8")
        dyn = parse_dynamic_values(cs_text)
        raw_desc = cards_loc.get(f"{lid}.description", "")
        dyn_up = apply_upgrades(dyn, cs_text)
        card["descriptionHtml"] = format_game_description(raw_desc, dyn)
        card["descriptionHtmlUpgraded"] = format_game_description(raw_desc, dyn_up)
        card["descriptionPlain"] = plain_description_for_search(raw_desc, dyn)
        card["descriptionPlainUpgraded"] = plain_description_for_search(raw_desc, dyn_up)

    for rel in relics:
        lid = rel["locId"]
        rel["title"] = relics_loc.get(f"{lid}.title", rel["className"])
        raw_d = relics_loc.get(f"{lid}.description", "")
        rel["descriptionHtml"] = format_game_description(raw_d, {})
        rel["descriptionPlain"] = plain_description_for_search(raw_d, {})
        raw_f = relics_loc.get(f"{lid}.flavor", "")
        rel["flavorHtml"] = format_game_description(raw_f, {}) if raw_f else ""
        rel["flavorPlain"] = plain_description_for_search(raw_f, {}) if raw_f else ""

    if OUT_ASSETS.exists():
        shutil.rmtree(OUT_ASSETS)
    copy_card_portraits(cards)
    copy_relic_icons(relics)
    character = copy_character_art()

    stances = [
        {
            "id": "BUU-REGULAR_STANCE",
            "title": powers_loc.get("BUU-REGULAR_STANCE.title", "Regular"),
            "description": strip_bb(
                powers_loc.get("BUU-REGULAR_STANCE.smartDescription", powers_loc.get("BUU-REGULAR_STANCE.description", ""))
            ),
        },
        {
            "id": "BUU-MAJIN_STANCE",
            "title": powers_loc.get("BUU-MAJIN_STANCE.title", "Majin"),
            "description": strip_bb(
                powers_loc.get("BUU-MAJIN_STANCE.smartDescription", powers_loc.get("BUU-MAJIN_STANCE.description", ""))
            ),
        },
        {
            "id": "BUU-SUPER_STANCE",
            "title": powers_loc.get("BUU-SUPER_STANCE.title", "Super"),
            "description": strip_bb(
                powers_loc.get("BUU-SUPER_STANCE.smartDescription", powers_loc.get("BUU-SUPER_STANCE.description", ""))
            ),
        },
    ]

    powers = [p for p in powers_from_loc(powers_loc) if p["id"] not in {s["id"] for s in stances}]

    data = {
        "meta": {
            "name": "Buu",
            "tagline": chars_loc.get("BUU-BUU.description", "A malleable Majin with stances and Ki."),
            "generated": True,
        },
        "character": {
            "names": {
                "default": chars_loc.get("BUU-BUU.title", "Buu"),
                "majin": chars_loc.get("BUU-BUU_MAJIN.title", "Majin Buu"),
                "super": chars_loc.get("BUU-BUU_SUPER.title", "Super Buu"),
            },
            "images": character,
        },
        "stances": stances,
        "powers": powers,
        "cards": cards,
        "relics": relics,
    }

    (DOCS / "data.json").write_text(json.dumps(data, indent=2), encoding="utf-8")
    print(f"Wrote {DOCS / 'data.json'} with {len(cards)} cards, {len(relics)} relics, {len(powers)} powers.")


if __name__ == "__main__":
    main()
