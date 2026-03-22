# Buu

Playable character mod for **Slay the Spire 2**: **Majin Buu** from *Dragon Ball Z*. Buu uses three combat stances—Regular (Good Buu), Majin (Evil Buu), and Super (Super Buu)—a **Ki** secondary resource, and a full set of cards, relics, and powers. Visuals and UI assets live under Godot resources in `Buu/`; gameplay logic is C# under `BuuCode/`. English strings are in `Buu/localization/eng/`.

| | |
| --- | --- |
| Mod ID | `Buu` |
| Manifest | [`Buu.json`](Buu.json) |
| Depends on | [BaseLib-StS2](https://github.com/Alchyr/BaseLib-StS2) — required sibling mod `BaseLib` |
| Game | [Slay the Spire 2 on Steam](https://store.steampowered.com/app/2868840/) |
| Live catalog | [harsh2204.github.io/STS2-Buu](https://harsh2204.github.io/STS2-Buu/) |

---

## Requirements

**To play**

- Owned copy of Slay the Spire 2 with mod support enabled.
- [BaseLib-StS2](https://github.com/Alchyr/BaseLib-StS2) installed under the game’s `mods/BaseLib/` folder: `BaseLib.dll`, `BaseLib.pck`, and `BaseLib.json` as in the upstream release layout.

**To build this repository**

- [.NET 9 SDK](https://dotnet.microsoft.com/download).
- Game install path available so MSBuild can resolve `sts2.dll`, `0Harmony.dll`, and `SmartFormat.dll` from the platform-specific `data_sts2_*` directory inside the game folder. Paths are configured in [`Buu.csproj`](Buu.csproj).
- Optional: [Godot 4.5.1](https://godotengine.org/) or the **Megadot** editor build that matches Slay the Spire 2, if you want automated export of `Buu.pck`. The `.csproj` warns that a `.pck` produced with a **newer** editor than the game uses may not load; keep the editor version aligned with STS2.

---

## Installation

1. Install BaseLib as described in the [BaseLib-StS2 README](https://github.com/Alchyr/BaseLib-StS2).
2. Copy this mod into `Slay the Spire 2/mods/`, in a folder named to match the project output (for example `BuuMod/`). The game needs `Buu.dll` and `Buu.json` in the same directory. If the manifest sets `has_pck` to true, include `Buu.pck` as well.
3. Launch the game, enable mods, and select Buu from the character roster once dependencies load.

If you use a different mods directory name, keep `Buu.json` beside `Buu.dll` so the loader can bind the assembly to the manifest.

---

## Building from source

1. Clone the repository and open `Buu.sln`, or run `dotnet build` from the solution directory.
2. Edit [`Buu.csproj`](Buu.csproj) so `SteamLibraryPath`, `Sts2Path`, `ModsPath`, `Sts2DataDir`, and optionally `GodotPath` match your machine. On Windows the file includes an example `GodotPath`; Linux and macOS blocks use conventional Steam layout defaults.
3. Build the solution. NuGet supplies [Alchyr.Sts2.BaseLib](https://www.nuget.org/packages/Alchyr.Sts2.BaseLib) and [Alchyr.Sts2.ModAnalyzers](https://www.nuget.org/packages/Alchyr.Sts2.ModAnalyzers). The post-build steps copy `Buu.dll` and `Buu.json` into your configured `mods` folder, refresh `mods/BaseLib/` from the BaseLib package, and when `GodotPath` points to a valid executable, run a headless export so `Buu.pck` lands next to the DLL.

Implementation status, scene checklist, stance tuning notes, and asset tracking live in [`PLAN.md`](PLAN.md).

---

## Catalog website

The [STS2 Buu catalog](https://harsh2204.github.io/STS2-Buu/) is a static site that lists cards (with portraits and upgrade toggles), relics, stances, and powers. It is hosted on GitHub Pages.

- **Production:** pushes that touch `Buu/`, `BuuCode/`, `docs/`, or the Pages workflow trigger [`.github/workflows/pages.yml`](.github/workflows/pages.yml), which runs [`docs/build_data.py`](docs/build_data.py) and uploads the `docs/` tree as the site artifact.
- **Local preview:** from the repository root run `python docs/serve_local.py`. The script regenerates catalog data and serves over HTTP so the browser can fetch `data.json`. Pass `--no-browser` to skip opening a tab. Opening `docs/index.html` directly from disk will fail because browsers block that fetch.

---

## Project structure

| Path | Purpose |
| --- | --- |
| `BuuCode/` | C# gameplay code: character definition, card and relic models, stance powers, commands, Harmony patches, and custom nodes. |
| `Buu/` | Godot side: `scenes/`, Spine-driven `animation/`, `images/` for portraits and UI, and `localization/` JSON consumed by the game. |
| `docs/` | Source for the public catalog: `index.html`, styles, scripts, `build_data.py`, and `serve_local.py`. Generated assets such as `data.json` are produced by the build script or CI. |
| `image_gen/` | Prompts, size notes, and generated or hand-sliced art pipelines that feed into `Buu/images/`. |

---

## Fan work and intellectual property

This mod is a non-commercial fan work. *Dragon Ball* names and imagery belong to their respective owners. Slay the Spire 2 is the property of MegaCrit. This project is not affiliated with or endorsed by those rights holders.

---

## Credits

The following projects and tools made Buu possible. Thanks to everyone who maintains them.

**Slay the Spire 2 modding**

- [Alchyr / BaseLib-StS2](https://github.com/Alchyr/BaseLib-StS2) — shared mod library, UI hooks, and multiplayer-oriented helpers. Consumed via NuGet as [Alchyr.Sts2.BaseLib](https://www.nuget.org/packages/Alchyr.Sts2.BaseLib).
- [Alchyr / ModTemplate-StS2](https://github.com/Alchyr/ModTemplate-StS2) — `dotnet new alchyrsts2charmod` template: Godot pack export, C# mod layout, manifest, and BaseLib wiring.
- [BaseLib Wiki](https://alchyr.github.io/BaseLib-Wiki/) — documentation for BaseLib-based STS2 mods.
- [lamali292 / WatcherMod](https://github.com/lamali292/WatcherMod/) — early STS2 character mod; useful reference for stance-heavy characters and BaseLib plus Godot packaging.

**Engine and runtime shipped with the game**

- [Godot Engine](https://github.com/godotengine/godot) — STS2 content pipeline; this mod targets Godot 4.5 via `Godot.NET.Sdk` in the project file.
- [Harmony](https://github.com/pardeike/Harmony) by Andreas Pardeike — runtime IL patching; the game supplies `0Harmony.dll` next to `sts2.dll`.
- [SmartFormat.NET](https://github.com/axuno/SmartFormat.NET) — template formatting used with the game’s localization; referenced from the same data directory as Harmony.

**Game data research**

- [Spire Codex](https://spire-codex.com) — browsable database and API over parsed Slay the Spire 2 data.
- [ptrlrd / spire-codex](https://github.com/ptrlrd/spire-codex/) — open-source extraction, decompilation, parsers, and tooling behind that project.
- [ILSpy](https://github.com/icsharpcode/ILSpy) — decompilation of `sts2.dll` for reading game APIs and models.
- [GDRE Tools](https://github.com/bruvzg/gdsdecomp) — Godot `.pck` recovery and asset extraction.

**Animation**

- [Spine](https://esotericsoftware.com/) and [spine-runtimes](https://github.com/EsotericSoftware/spine-runtimes) — skeletal animation stack used by STS2; Buu’s animation assets under `Buu/animation/` follow that pipeline.

**Documentation site**

- [GitHub Actions](https://github.com/features/actions) and [GitHub Pages](https://docs.github.com/en/pages) — CI build for the catalog and deployment to `harsh2204.github.io`.

---

## Author

**Harsh Gupta** — author field and semantic version are recorded in [`Buu.json`](Buu.json).
