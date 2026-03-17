#!/usr/bin/env python3
"""
Center-zoom portrait optimizer with card-type masking.

Behavior:
- Keeps every image centered.
- Preserves original output dimensions.
- Uses card type to choose layout mask:
  - Skill -> full
  - Attack -> trapezoid
  - Power -> circular
- Fills transparent RGB with median opaque color (prevents black holes).
- Finds zoom that minimizes missing-content area inside the chosen mask.
"""

from __future__ import annotations

import argparse
import re
from dataclasses import dataclass
from pathlib import Path
from typing import Dict, Iterable, Optional, Tuple

from PIL import Image


DEFAULT_REPO_ROOT = Path(r"c:\Code\sts-map-export")
DEFAULT_PORTRAITS_DIR = DEFAULT_REPO_ROOT / "BuuMod" / "Buu" / "images" / "card_portraits"
DEFAULT_CARDS_CODE_DIR = DEFAULT_REPO_ROOT / "BuuMod" / "BuuCode" / "Cards"
DEFAULT_WATCHER_PORTRAITS_DIR = DEFAULT_REPO_ROOT / "WatcherMod" / "Watcher" / "images" / "card_portraits"
DEFAULT_FULL_MASK = "deceive_reality.png"
DEFAULT_TRAPEZOID_MASK = "ragnarok.png"
DEFAULT_CIRCULAR_MASK = "become_almighty.png"
DEFAULT_TARGET_WIDTH = 500
DEFAULT_TARGET_HEIGHT = 380


@dataclass(frozen=True)
class ZoomResult:
    zoom: float
    missing_ratio: float
    image: Image.Image


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="Center-zoom portraits to reduce empty pixels.")
    parser.add_argument("--portraits-dir", type=Path, default=DEFAULT_PORTRAITS_DIR)
    parser.add_argument("--cards-code-dir", type=Path, default=DEFAULT_CARDS_CODE_DIR)
    parser.add_argument("--watcher-portraits-dir", type=Path, default=DEFAULT_WATCHER_PORTRAITS_DIR)
    parser.add_argument("--single", type=str, default=None, help="Process one file only, e.g. absorb.png")
    parser.add_argument("--apply", action="store_true", help="Write output files. Default is dry-run.")
    parser.add_argument("--target-empty", type=float, default=0.07, help="Target missing-content ratio inside mask (0.07 = 7%).")
    parser.add_argument("--alpha-threshold", type=int, default=10, help="Alpha <= threshold counts as empty.")
    parser.add_argument("--max-zoom", type=float, default=2.00, help="Maximum zoom factor to try.")
    parser.add_argument("--zoom-step", type=float, default=0.01, help="Zoom increment per iteration.")
    parser.add_argument("--full-mask", type=str, default=DEFAULT_FULL_MASK)
    parser.add_argument("--trapezoid-mask", type=str, default=DEFAULT_TRAPEZOID_MASK)
    parser.add_argument("--circular-mask", type=str, default=DEFAULT_CIRCULAR_MASK)
    parser.add_argument("--target-width", type=int, default=DEFAULT_TARGET_WIDTH, help="Output width for normalization.")
    parser.add_argument("--target-height", type=int, default=DEFAULT_TARGET_HEIGHT, help="Output height for normalization.")
    return parser.parse_args()


def iter_targets(portraits_dir: Path, single: Optional[str]) -> Iterable[Path]:
    if single:
        target = portraits_dir / single
        if not target.exists():
            raise FileNotFoundError(f"--single not found: {target}")
        yield target
        return

    for image_path in sorted(portraits_dir.rglob("*.png")):
        yield image_path


def pascal_to_snake(name: str) -> str:
    return re.sub(r"(?<!^)(?=[A-Z])", "_", name).lower()


def resolve_buu_portrait_name(class_name_stem: str) -> str:
    remap = {
        "ki_strike": "ki_blast",
        "ki_barrier": "bubble_barrier",
        "ki_well": "regenerate",
        "majin_fury": "evil_emerges",
        "super_spirit": "super_aura",
        "regenerative_form": "reform",
    }
    default_name = pascal_to_snake(class_name_stem)
    return remap.get(default_name, default_name)


def parse_card_type_map(cards_code_dir: Path) -> Dict[str, str]:
    card_type_by_portrait: Dict[str, str] = {}
    card_type_pattern = re.compile(r"CardType\.(Attack|Skill|Power)")

    for cs_path in cards_code_dir.rglob("*.cs"):
        source = cs_path.read_text(encoding="utf-8", errors="ignore")
        match = card_type_pattern.search(source)
        if not match:
            continue
        card_type_by_portrait[resolve_buu_portrait_name(cs_path.stem)] = match.group(1)

    return card_type_by_portrait


def load_mask(mask_path: Path) -> Image.Image:
    if not mask_path.exists():
        raise FileNotFoundError(f"Mask file not found: {mask_path}")
    return Image.open(mask_path).convert("RGBA").getchannel("A")


def choose_layout_name(card_type: str) -> str:
    if card_type == "Attack":
        return "trapezoid"
    if card_type == "Power":
        return "circular"
    return "full"


def median_rgb_of_opaque(image: Image.Image, alpha_threshold: int) -> Tuple[int, int, int]:
    rgba = image.convert("RGBA")
    red_channel, green_channel, blue_channel, alpha_channel = rgba.split()
    red_pixels = []
    green_pixels = []
    blue_pixels = []
    for red_value, green_value, blue_value, alpha_value in zip(
        red_channel.getdata(), green_channel.getdata(), blue_channel.getdata(), alpha_channel.getdata()
    ):
        if alpha_value > alpha_threshold:
            red_pixels.append(red_value)
            green_pixels.append(green_value)
            blue_pixels.append(blue_value)
    if not red_pixels:
        return 32, 32, 32

    red_pixels.sort()
    green_pixels.sort()
    blue_pixels.sort()
    median_index = len(red_pixels) // 2
    return red_pixels[median_index], green_pixels[median_index], blue_pixels[median_index]


def fill_transparent_with_median(image: Image.Image, alpha_threshold: int) -> Image.Image:
    rgba = image.convert("RGBA")
    fill_red, fill_green, fill_blue = median_rgb_of_opaque(rgba, alpha_threshold)
    red_channel, green_channel, blue_channel, alpha_channel = rgba.split()
    new_red = []
    new_green = []
    new_blue = []
    alpha_values = list(alpha_channel.getdata())
    for red_value, green_value, blue_value, alpha_value in zip(
        red_channel.getdata(), green_channel.getdata(), blue_channel.getdata(), alpha_values
    ):
        if alpha_value <= alpha_threshold:
            new_red.append(fill_red)
            new_green.append(fill_green)
            new_blue.append(fill_blue)
        else:
            new_red.append(red_value)
            new_green.append(green_value)
            new_blue.append(blue_value)
    return Image.merge(
        "RGBA",
        (
            Image.frombytes("L", rgba.size, bytes(new_red)),
            Image.frombytes("L", rgba.size, bytes(new_green)),
            Image.frombytes("L", rgba.size, bytes(new_blue)),
            alpha_channel,
        ),
    )


def missing_ratio_inside_mask(image: Image.Image, mask_alpha: Image.Image, alpha_threshold: int) -> float:
    alpha = image.getchannel("A")
    if alpha.size != mask_alpha.size:
        resized_mask = mask_alpha.resize(alpha.size, Image.Resampling.LANCZOS)
    else:
        resized_mask = mask_alpha
    mask_values = resized_mask.getdata()
    alpha_values = alpha.getdata()
    mask_pixels = 0
    missing_pixels = 0
    for mask_value, alpha_value in zip(mask_values, alpha_values):
        if mask_value > alpha_threshold:
            mask_pixels += 1
            if alpha_value <= alpha_threshold:
                missing_pixels += 1
    if mask_pixels == 0:
        return 0.0
    return missing_pixels / mask_pixels


def apply_layout_mask_and_fill(image: Image.Image, layout_mask: Image.Image, alpha_threshold: int) -> Image.Image:
    filled = fill_transparent_with_median(image, alpha_threshold)
    red_channel, green_channel, blue_channel, _ = filled.split()
    if layout_mask.size != image.size:
        resized_mask = layout_mask.resize(image.size, Image.Resampling.LANCZOS)
    else:
        resized_mask = layout_mask
    return Image.merge("RGBA", (red_channel, green_channel, blue_channel, resized_mask))


def centered_crop_box(source_size: Tuple[int, int], target_size: Tuple[int, int]) -> Tuple[int, int, int, int]:
    source_width, source_height = source_size
    target_width, target_height = target_size
    center_x = source_width / 2.0
    center_y = source_height / 2.0
    left = int(round(center_x - target_width / 2.0))
    top = int(round(center_y - target_height / 2.0))
    left = max(0, min(left, source_width - target_width))
    top = max(0, min(top, source_height - target_height))
    right = left + target_width
    bottom = top + target_height
    return left, top, right, bottom


def resize_to_cover(image: Image.Image, target_size: Tuple[int, int]) -> Image.Image:
    source_width, source_height = image.size
    target_width, target_height = target_size
    if (source_width, source_height) == (target_width, target_height):
        return image.copy()

    scale = max(target_width / source_width, target_height / source_height)
    resized_width = max(1, int(round(source_width * scale)))
    resized_height = max(1, int(round(source_height * scale)))
    resized = image.resize((resized_width, resized_height), Image.Resampling.LANCZOS)

    crop_left, crop_top, crop_right, crop_bottom = centered_crop_box(
        source_size=(resized_width, resized_height),
        target_size=(target_width, target_height),
    )
    return resized.crop((crop_left, crop_top, crop_right, crop_bottom))


def center_zoom(image: Image.Image, zoom: float) -> Image.Image:
    if zoom <= 1.0:
        return image.copy()

    width, height = image.size
    crop_width = max(1, int(round(width / zoom)))
    crop_height = max(1, int(round(height / zoom)))
    crop_left, crop_top, crop_right, crop_bottom = centered_crop_box(
        source_size=(width, height),
        target_size=(crop_width, crop_height),
    )

    cropped = image.crop((crop_left, crop_top, crop_right, crop_bottom))
    return cropped.resize((width, height), Image.Resampling.LANCZOS)


def optimize_zoom(
    image: Image.Image,
    layout_mask: Image.Image,
    target_empty: float,
    alpha_threshold: int,
    max_zoom: float,
    zoom_step: float,
) -> ZoomResult:
    base = image.copy()
    base_missing = missing_ratio_inside_mask(base, layout_mask, alpha_threshold)
    best = ZoomResult(zoom=1.0, missing_ratio=base_missing, image=base)

    zoom = 1.0 + zoom_step
    while zoom <= max_zoom + 1e-9:
        candidate = center_zoom(base, zoom)
        candidate_missing = missing_ratio_inside_mask(candidate, layout_mask, alpha_threshold)

        # Prefer lower missing ratio; if tied, prefer lower zoom.
        if (candidate_missing < best.missing_ratio - 1e-9) or (
            abs(candidate_missing - best.missing_ratio) <= 1e-9 and zoom < best.zoom
        ):
            best = ZoomResult(zoom=zoom, missing_ratio=candidate_missing, image=candidate)

        # Stop at first zoom that satisfies target to preserve content.
        if candidate_missing <= target_empty:
            return ZoomResult(zoom=zoom, missing_ratio=candidate_missing, image=candidate)

        zoom += zoom_step

    return best


def main() -> None:
    args = parse_args()
    card_type_map = parse_card_type_map(args.cards_code_dir)
    masks = {
        "full": load_mask(args.watcher_portraits_dir / args.full_mask),
        "trapezoid": load_mask(args.watcher_portraits_dir / args.trapezoid_mask),
        "circular": load_mask(args.watcher_portraits_dir / args.circular_mask),
    }

    scanned = 0
    changed = 0
    target_size = (args.target_width, args.target_height)

    for path in iter_targets(args.portraits_dir, args.single):
        scanned += 1
        source = Image.open(path).convert("RGBA")
        normalized = resize_to_cover(source, target_size)
        card_type = card_type_map.get(path.stem, "Skill")
        layout_name = choose_layout_name(card_type)
        layout_mask = masks[layout_name]
        before_missing = missing_ratio_inside_mask(normalized, layout_mask, args.alpha_threshold)
        result = optimize_zoom(
            image=normalized,
            layout_mask=layout_mask,
            target_empty=args.target_empty,
            alpha_threshold=args.alpha_threshold,
            max_zoom=args.max_zoom,
            zoom_step=args.zoom_step,
        )
        output = apply_layout_mask_and_fill(result.image, layout_mask, args.alpha_threshold)

        meets_target = result.missing_ratio <= args.target_empty
        print(
            f"PLAN: {path.name} size={source.size}->{target_size} "
            f"type={card_type} layout={layout_name} "
            f"missing_before={before_missing*100:.2f}% "
            f"missing_after={result.missing_ratio*100:.2f}% "
            f"zoom={result.zoom:.2f} "
            f"target_met={meets_target}"
        )

        if args.apply:
            output.save(path)
            changed += 1

    print()
    print(f"Scanned: {scanned}")
    print(f"Changed: {changed}")
    print(f"Mode: {'APPLY' if args.apply else 'DRY-RUN'}")


if __name__ == "__main__":
    main()
