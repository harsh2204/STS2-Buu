#!/usr/bin/env python3
"""
Regenerate data.json + assets, then serve this folder over HTTP.

Same first step as .github/workflows/pages.yml — use this for local checks.

From the BuuMod repo root:
  python docs/serve_local.py

Then open http://127.0.0.1:8765/ (browser may open automatically).
"""
from __future__ import annotations

import argparse
import subprocess
import sys
import webbrowser
from http.server import SimpleHTTPRequestHandler, ThreadingHTTPServer
from pathlib import Path

DOCS = Path(__file__).resolve().parent
ROOT = DOCS.parent
DEFAULT_PORT = 8765


def run_build() -> None:
    build_script = DOCS / "build_data.py"
    print("Running build_data.py …")
    subprocess.check_call([sys.executable, str(build_script)], cwd=str(ROOT))


def main() -> None:
    parser = argparse.ArgumentParser(description="Build docs data and serve locally.")
    parser.add_argument(
        "--no-build",
        action="store_true",
        help="Skip build_data.py (only start the server).",
    )
    parser.add_argument(
        "-p",
        "--port",
        type=int,
        default=DEFAULT_PORT,
        metavar="N",
        help=f"Port (default {DEFAULT_PORT}).",
    )
    parser.add_argument(
        "--no-browser",
        action="store_true",
        help="Do not open a browser tab.",
    )
    args = parser.parse_args()

    if not args.no_build:
        run_build()

    handler = SimpleHTTPRequestHandler
    httpd = ThreadingHTTPServer(("127.0.0.1", args.port), handler)
    url = f"http://127.0.0.1:{args.port}/"

    import os

    os.chdir(DOCS)
    print(f"Serving {DOCS}")
    print(url)
    print("Ctrl+C to stop")

    if not args.no_browser:
        webbrowser.open(url)

    try:
        httpd.serve_forever()
    except KeyboardInterrupt:
        print()


if __name__ == "__main__":
    main()
