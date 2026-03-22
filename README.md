# Buu

A **Slay the Spire 2** playable character mod: **Majin Buu** from *Dragon Ball Z*, with three combat stances—**Regular** (Good Buu), **Majin** (Evil Buu), and **Super** (Super Buu)—plus a **Ki** secondary resource, a full card and relic pool, Godot-driven visuals, and localization under `Buu/localization/`.

| | |
| --- | --- |
| **Mod ID** | `Buu` |
| **Manifest** | [`Buu.json`](Buu.json) |
| **Depends on** | [BaseLib](https://github.com/Alchyr/BaseLib-StS2) (`BaseLib` in `Buu.json`) |
| **Game** | [Slay the Spire 2](https://store.steampowered.com/app/2868840/) (MegaCrit) |
| **Catalog (live)** | **[harsh2204.github.io/STS2-Buu](https://harsh2204.github.io/STS2-Buu/)** — cards, relics, stances, powers |

---

## Requirements

- A legal copy of **Slay the Spire 2** with mod support enabled.
- **[BaseLib](https://github.com/Alchyr/BaseLib-StS2)** installed in the game’s `mods` folder (same layout as the [BaseLib releases](https://github.com/Alchyr/BaseLib-StS2/releases): `BaseLib.dll`, `BaseLib.pck`, `BaseLib.json`). The project build can copy these from NuGet; see below.
- **.NET 9** SDK for building the C# assembly.
- **[Godot / MegaDot 4.5.1](https://godotengine.org/)** (C# / mono editor matching the game’s **Megadot** fork) if you want the post-build step to export `Buu.pck`. The `.csproj` notes that the game will not load a `.pck` built with a **newer** Godot than the one STS2 ships with—keep versions aligned.

Runtime references (ship with the game install, not committed here) include `sts2.dll`, **`0Harmony`**, and **SmartFormat**, resolved from your `data_sts2_*` folder via [`Buu.csproj`](Buu.csproj).

---

## Install (players)

1. Install **BaseLib** into `…/Slay the Spire 2/mods/BaseLib/` as documented in the [BaseLib-StS2 README](https://github.com/Alchyr/BaseLib-StS2).
2. Copy this mod’s folder into `…/mods/BuuMod/` (or your chosen folder name) so it contains at least:
   - `Buu.dll`
   - `Buu.json`
   - `Buu.pck` (if you use Godot assets from this repo)

Exact folder naming can follow your launcher; the important part is that **`Buu.json`** sits next to **`Buu.dll`** and the `.pck` when `has_pck` is true.

---

## Build (developers)

1. Clone the repository and open `Buu.sln` (or build from the CLI with `dotnet build`).
2. Set paths in **`Buu.csproj`** for your machine if defaults do not match:
   - **`GodotPath`** — executable used for `--export-pack` (Windows example in repo).
   - **`SteamLibraryPath`**, **`Sts2Path`**, **`ModsPath`**, **`Sts2DataDir`** — so `sts2.dll`, `0Harmony.dll`, and output `mods` folder resolve correctly.
3. Build: the project references **[Alchyr.Sts2.BaseLib](https://www.nuget.org/packages/Alchyr.Sts2.BaseLib)** and **[Alchyr.Sts2.ModAnalyzers](https://www.nuget.org/packages/Alchyr.Sts2.ModAnalyzers)** from NuGet, copies **`Buu.dll`** + **`Buu.json`** into your STS2 `mods` folder, copies **BaseLib** artifacts into `mods/BaseLib/`, and (when `GodotPath` exists) exports **`Buu.pck`**.

For a deeper checklist (scenes, localization, assets, stance design), see **[`PLAN.md`](PLAN.md)**.

---

## Catalog site

The mod’s **static catalog** (card pool with portraits, relics, stances, powers) is published at **[https://harsh2204.github.io/STS2-Buu/](https://harsh2204.github.io/STS2-Buu/)** via [GitHub Pages](https://docs.github.com/en/pages). Content is generated from source by [`docs/build_data.py`](docs/build_data.py) in [`.github/workflows/pages.yml`](.github/workflows/pages.yml).

**Local preview:** from the BuuMod root run `python docs/serve_local.py` — that regenerates data and serves the `docs/` folder over HTTP so `data.json` loads correctly (use `--no-browser` if you do not want a tab opened). Opening `docs/index.html` as a bare file will not load the catalog.

---

## Repository layout (short)

| Path | Role |
| --- | --- |
| `BuuCode/` | C#: character, card/relic/power models, stances, Harmony patches, nodes |
| `Buu/` | Godot resources: `scenes/`, `animation/`, `images/`, `localization/` |
| `docs/` | Static catalog for **[harsh2204.github.io/STS2-Buu](https://harsh2204.github.io/STS2-Buu/)**; CI runs [`docs/build_data.py`](docs/build_data.py) (see [`.github/workflows/pages.yml`](.github/workflows/pages.yml)) |
| `image_gen/` | Prompts and asset notes for generated art pipelines |

---

## Fan work and IP

This mod is a **non-commercial fan project**. *Dragon Ball* characters and iconography are the property of their respective rights holders. **Slay the Spire 2** is the property of **MegaCrit**. This project is not affiliated with or endorsed by either.

---

## Acknowledgments

These projects and communities made **Buu** practical to build and ship. Thank you.

### Modding stack (STS2)

- **[Alchyr / BaseLib-StS2](https://github.com/Alchyr/BaseLib-StS2)** — Shared library, UI hooks, multiplayer-minded patterns, and the **`BaseLib`** dependency every character mod expects. NuGet: [`Alchyr.Sts2.BaseLib`](https://www.nuget.org/packages/Alchyr.Sts2.BaseLib).
- **[Alchyr / ModTemplate-StS2](https://github.com/Alchyr/ModTemplate-StS2)** — **`dotnet new alchyrsts2charmod`** template this mod grew from (Godot `.pck` + C# DLL layout, manifest, BaseLib wiring).
- **[BaseLib Wiki](https://alchyr.github.io/BaseLib-Wiki/)** — Documentation for BaseLib-driven STS2 modding.

### Game engine and runtime libraries

- **[Godot Engine](https://github.com/godotengine/godot)** — Under the hood of STS2’s content pipeline; this mod uses **Godot 4.5**-compatible exports via **`Godot.NET.Sdk`** in the `.csproj`.
- **[Harmony](https://github.com/pardeike/Harmony)** (Andreas Pardeike) — Runtime method patching; the game supplies **`0Harmony.dll`** alongside `sts2.dll`.
- **[SmartFormat.NET](https://github.com/axuno/SmartFormat.NET)** — String formatting used by the game’s localization stack; referenced from the game data directory like Harmony.

### Research, data, and reverse-engineering tooling

Development and balance notes in the parent workspace lean on the same ecosystem as **[Spire Codex](https://spire-codex.com)** (decompiled `sts2.dll`, extracted packs, and structured game data). Helpful upstream tools called out there:

- **[ILSpy](https://github.com/icsharpcode/ILSpy)** — C# decompilation for inspecting `sts2.dll` and game APIs.
- **[GDRE Tools (gdsdecomp)](https://github.com/bruvzg/gdsdecomp)** — Godot `.pck` extraction and asset recovery.

### Animation

- **[Spine](https://esotericsoftware.com/)** / **[spine-runtimes](https://github.com/EsotericSoftware/spine-runtimes)** — Spine is the skeletal animation pipeline STS2 uses; Buu’s `Buu/animation/` content follows that workflow.

### Hosting and CI

- **[GitHub Actions](https://github.com/features/actions)** — Builds the **`docs/`** catalog and deploys it to **[harsh2204.github.io/STS2-Buu](https://harsh2204.github.io/STS2-Buu/)** with **`actions/deploy-pages`**.

---

## Author

**Harsh Gupta** — see [`Buu.json`](Buu.json) for the packaged `author` field and version.
