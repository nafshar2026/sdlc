#!/usr/bin/env python3
"""Minimal skill loader for prompt assembly.

Reads LOAD-ORDER.md, optionally filters skill ids, and emits a single merged
markdown payload suitable for an LLM generation step.
"""

from __future__ import annotations

import argparse
import json
import pathlib
import re
import sys
from typing import Iterable

ENTRY_RE = re.compile(r"^\s*\d+\.\s+(.+\.md)\s*$")
SKILL_ID_RE = re.compile(r"SKILL-(\d+)-", re.IGNORECASE)


def load_skill_map(skill_map_path: pathlib.Path) -> dict:
    return json.loads(skill_map_path.read_text(encoding="utf-8"))


def parse_manifest(manifest_path: pathlib.Path) -> list[pathlib.Path]:
    entries: list[pathlib.Path] = []
    for line in manifest_path.read_text(encoding="utf-8").splitlines():
        match = ENTRY_RE.match(line)
        if not match:
            continue
        rel = match.group(1).strip()
        entries.append((manifest_path.parent / rel).resolve())
    return entries


def normalize_skill_ids(raw: str | None) -> set[str] | None:
    if not raw:
        return None
    values = [p.strip() for p in raw.split(",") if p.strip()]
    return {v.zfill(2) for v in values}


def detect_skill_ids(requirements_text: str | None, skill_map: dict) -> set[str]:
    selected = {skill_id.zfill(2) for skill_id in skill_map.get("defaultSkills", [])}
    if not requirements_text:
        return selected

    haystack = requirements_text.casefold()
    for skill in skill_map.get("skills", []):
        skill_id = str(skill.get("id", "")).zfill(2)
        for keyword in skill.get("keywords", []):
            if keyword.casefold() in haystack:
                selected.add(skill_id)
                break

    return selected


def pick_entries(entries: Iterable[pathlib.Path], skill_ids: set[str] | None) -> list[pathlib.Path]:
    if skill_ids is None:
        return list(entries)

    selected: list[pathlib.Path] = []
    for entry in entries:
        name = entry.name
        match = SKILL_ID_RE.search(name)
        if not match:
            continue
        if match.group(1).zfill(2) in skill_ids:
            selected.append(entry)
    return selected


def build_payload(manifest: pathlib.Path, selected: list[pathlib.Path]) -> str:
    try:
        manifest_label = str(manifest.relative_to(pathlib.Path.cwd()))
    except ValueError:
        manifest_label = str(manifest)

    lines: list[str] = []
    lines.append("# Loaded Skill Context")
    lines.append("")
    lines.append(f"Manifest: {manifest_label}")
    lines.append(f"Loaded count: {len(selected)}")
    lines.append("")

    for skill_file in selected:
        lines.append(f"## BEGIN {skill_file.name}")
        lines.append("")
        lines.append(skill_file.read_text(encoding="utf-8").rstrip())
        lines.append("")
        lines.append(f"## END {skill_file.name}")
        lines.append("")

    return "\n".join(lines).rstrip() + "\n"


def main() -> int:
    parser = argparse.ArgumentParser(description="Load and merge skill markdown files")
    parser.add_argument(
        "--manifest",
        default=str(pathlib.Path(__file__).resolve().parent.parent / "LOAD-ORDER.md"),
        help="Path to LOAD-ORDER.md",
    )
    parser.add_argument(
        "--skills",
        default=None,
        help="Comma-separated skill ids to include, e.g. 01,03,07",
    )
    parser.add_argument(
        "--requirements-file",
        default=None,
        help="Optional path to a requirements text file for automatic skill selection.",
    )
    parser.add_argument(
        "--requirements-text",
        default=None,
        help="Optional inline requirements text for automatic skill selection.",
    )
    parser.add_argument(
        "--skill-map",
        default=str(pathlib.Path(__file__).resolve().parent.parent / "SKILL-MAP.json"),
        help="Path to SKILL-MAP.json",
    )
    parser.add_argument(
        "--out",
        default=None,
        help="Optional output file. If omitted, writes to stdout.",
    )

    args = parser.parse_args()
    manifest = pathlib.Path(args.manifest).resolve()
    skill_map_path = pathlib.Path(args.skill_map).resolve()

    if not manifest.exists():
        print(f"Manifest not found: {manifest}", file=sys.stderr)
        return 2
    if not skill_map_path.exists():
        print(f"Skill map not found: {skill_map_path}", file=sys.stderr)
        return 2

    entries = parse_manifest(manifest)
    if not entries:
        print("No skill entries found in manifest.", file=sys.stderr)
        return 3

    requirements_text = args.requirements_text
    if args.requirements_file:
        requirements_path = pathlib.Path(args.requirements_file).resolve()
        if not requirements_path.exists():
            print(f"Requirements file not found: {requirements_path}", file=sys.stderr)
            return 2
        requirements_text = requirements_path.read_text(encoding="utf-8")

    skill_ids = normalize_skill_ids(args.skills) or set()
    auto_skill_ids = detect_skill_ids(requirements_text, load_skill_map(skill_map_path))
    combined_skill_ids = skill_ids | auto_skill_ids
    selected = pick_entries(entries, skill_ids)

    if combined_skill_ids:
        selected = pick_entries(entries, combined_skill_ids)

    if not selected:
        print("No matching skills selected.", file=sys.stderr)
        return 4

    payload = build_payload(manifest, selected)

    if args.out:
        out_path = pathlib.Path(args.out).resolve()
        out_path.parent.mkdir(parents=True, exist_ok=True)
        out_path.write_text(payload, encoding="utf-8")
        print(f"Wrote merged skills payload: {out_path}")
    else:
        sys.stdout.write(payload)

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
