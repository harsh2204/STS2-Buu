"""
Generate Godot 4 .import files for all PNGs under Buu/images/.
Run from repo root: python BuuMod/tools/generate_imports.py
Godot will reimport and fill in real .ctex hashes on first project open.
"""
from pathlib import Path
import hashlib

BUU_MOD_ROOT = Path(__file__).resolve().parent.parent
IMAGES_DIR = BUU_MOD_ROOT / "Buu" / "images"
RES_PREFIX = "res://Buu/images"

TEXTURE_IMPORT_TEMPLATE = """[remap]

importer="texture"
type="CompressedTexture2D"
uid="uid://{uid}"
path="res://.godot/imported/{basename}-{placeholder}.ctex"
metadata={{
"vram_texture": false
}}

[deps]

source_file="{source_file}"
dest_files=["res://.godot/imported/{basename}-{placeholder}.ctex"]

[params]

compress/mode=0
compress/high_quality=false
compress/lossy_quality=0.7
compress/uastc_level=0
compress/rdo_quality_loss=0.0
compress/hdr_compression=1
compress/normal_map=0
compress/channel_pack=0
mipmaps/generate=false
mipmaps/limit=-1
roughness/mode=0
roughness/src_normal=""
process/channel_remap/red=0
process/channel_remap/green=1
process/channel_remap/blue=2
process/channel_remap/alpha=3
process/fix_alpha_border=true
process/premult_alpha=false
process/normal_map_invert_y=false
process/hdr_as_srgb=false
process/hdr_clamp_exposure=false
process/size_limit=0
detect_3d/compress_to=1
"""


def main():
    if not IMAGES_DIR.is_dir():
        raise SystemExit(f"Images dir not found: {IMAGES_DIR}")

    count = 0
    for png in sorted(IMAGES_DIR.rglob("*.png")):
        rel = png.relative_to(IMAGES_DIR)
        rel_posix = rel.as_posix()
        source_file = f"{RES_PREFIX}/{rel_posix}"
        basename = png.name
        uid = hashlib.md5(source_file.encode()).hexdigest()[:12]
        placeholder = "0" * 32

        content = TEXTURE_IMPORT_TEMPLATE.format(
            uid=uid,
            basename=basename,
            placeholder=placeholder,
            source_file=source_file,
        )
        import_path = png.with_suffix(png.suffix + ".import")
        import_path.write_text(content, encoding="utf-8")
        count += 1

    print(f"Wrote {count} .import files under {IMAGES_DIR}")


if __name__ == "__main__":
    main()
