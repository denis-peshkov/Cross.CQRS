#!/usr/bin/env python3
"""GitVersion strategy matrix runner for Cross.CQRS fixtures.

Prints the canonical markdown report (source / squash / merge / push + direct push).

Usage:
  python3 .cursor/skills/gitversion-strategy/scripts/run-matrix.py
  python3 .cursor/skills/gitversion-strategy/scripts/run-matrix.py --config GitVersion.yml --base-tag v10.1.3
  python3 .cursor/skills/gitversion-strategy/scripts/run-matrix.py --json
"""

from __future__ import annotations

import argparse
import json
import os
import re
import shutil
import subprocess
import sys
import tempfile
from pathlib import Path


DEFAULT_BRANCHES = [
    "release/11.0.0-new",
    "release/test-12-new",
    "release/test",
    "hotfix/test",
    "dev",
]

# Manual develop train in the matrix only (not applied to release/hotfix rows).
DEV_NEXT_VERSION = "11.3.3"


def cfg_for_branch(cfg_text: str, branch: str) -> str:
    """Overlay next-version for develop so source is `{DEV_NEXT_VERSION}-dev.N`."""
    if branch == "dev" or branch == "develop" or branch.startswith("dev/"):
        return re.sub(
            r"^next-version:\s*.*$",
            f"next-version: {DEV_NEXT_VERSION}",
            cfg_text,
            count=1,
            flags=re.M,
        )
    return cfg_text


def find_gv(explicit: str | None) -> str:
    if explicit:
        return explicit
    which = shutil.which("dotnet-gitversion")
    if which:
        return which
    home = Path.home() / ".dotnet" / "tools" / "dotnet-gitversion"
    if home.exists():
        return str(home)
    raise SystemExit("dotnet-gitversion not found (install GitVersion.Tool)")


def parse_json_blob(stdout: str) -> dict | None:
    start = stdout.find("{")
    if start < 0:
        return None
    depth = 0
    for i, ch in enumerate(stdout[start:], start):
        if ch == "{":
            depth += 1
        elif ch == "}":
            depth -= 1
            if depth == 0:
                try:
                    return json.loads(stdout[start : i + 1])
                except json.JSONDecodeError:
                    return None
    return None


def run_gv(gv: str, cwd: Path) -> str:
    p = subprocess.run(
        [gv, "/targetpath", str(cwd), "/output", "json"],
        capture_output=True,
        text=True,
        timeout=90,
    )
    data = parse_json_blob(p.stdout)
    if not data:
        err = ((p.stderr or "") + (p.stdout or ""))
        # keep table cells short
        if "No base versions" in err:
            return "FAIL:no-base-versions"
        if "orphaned" in err.lower():
            return "FAIL:orphaned-branch"
        return "FAIL"
    return str(data.get("SemVer") or "?")


def git(cwd: Path, *args: str, check: bool = True, capture: bool = False) -> subprocess.CompletedProcess:
    return subprocess.run(
        ["git", *args],
        cwd=cwd,
        check=check,
        capture_output=capture,
        text=True,
    )


def init_repo(path: Path, cfg_text: str, base_tag: str, siblings: bool = False) -> None:
    if path.exists():
        shutil.rmtree(path)
    path.mkdir(parents=True)
    env = {**os.environ, "GIT_TEMPLATE_DIR": ""}
    subprocess.run(["git", "init", "-b", "master", "-q"], cwd=path, check=True, env=env)
    for k, v in [
        ("user.email", "gitversion-matrix@local"),
        ("user.name", "gitversion-matrix"),
        ("core.hooksPath", "/dev/null"),
        ("core.autocrlf", "false"),
    ]:
        git(path, "config", k, v)
    (path / "GitVersion.yml").write_text(cfg_text, encoding="utf-8")
    (path / "f.txt").write_text("base\n", encoding="utf-8")
    git(path, "add", "-A")
    git(path, "commit", "-qm", "init")
    git(path, "tag", base_tag)
    if siblings:
        for b in ("dev", "release/placeholder", "hotfix/placeholder"):
            git(path, "branch", b)


def commit_work(path: Path, branch: str) -> None:
    git(path, "checkout", "-qb", branch)
    (path / "f.txt").write_text(f"{branch}\n", encoding="utf-8")
    git(path, "add", "-A")
    git(path, "commit", "-qm", f"work on {branch}")


def master_merge_message(branch: str) -> str:
    return f"Merge branch '{branch}'"


def run_matrix(
    *,
    gv: str,
    cfg_text: str,
    base_tag: str,
    branches: list[str],
    work_root: Path,
) -> dict:
    rows: list[dict] = []
    for branch in branches:
        branch_cfg = cfg_for_branch(cfg_text, branch)

        # source SemVer + push (+1) on the same source branch
        d_src = work_root / f"src_{branch.replace('/', '_')}"
        init_repo(d_src, branch_cfg, base_tag)
        commit_work(d_src, branch)
        source = run_gv(gv, d_src)
        (d_src / "f.txt").write_text(f"{branch} push+1\n", encoding="utf-8")
        git(d_src, "add", "-A")
        git(d_src, "commit", "-qm", f"push +1 on {branch}")
        after_push = run_gv(gv, d_src)

        # squash → master (from one-commit source tip)
        d_sq = work_root / f"sq_{branch.replace('/', '_')}"
        init_repo(d_sq, branch_cfg, base_tag)
        commit_work(d_sq, branch)
        merge_msg = master_merge_message(branch)
        git(d_sq, "checkout", "-q", "master")
        git(d_sq, "merge", "--squash", branch, capture=True)
        git(d_sq, "commit", "-qm", merge_msg)
        after_squash = run_gv(gv, d_sq)

        # merge --no-ff → master
        d_mg = work_root / f"mg_{branch.replace('/', '_')}"
        init_repo(d_mg, branch_cfg, base_tag)
        commit_work(d_mg, branch)
        git(d_mg, "checkout", "-q", "master")
        mr = git(
            d_mg,
            "merge",
            "--no-ff",
            "-m",
            merge_msg,
            branch,
            check=False,
            capture=True,
        )
        after_merge = "FAIL" if mr.returncode != 0 else run_gv(gv, d_mg)

        rows.append(
            {
                "branch": branch,
                "source": source,
                "squash": after_squash,
                "merge": after_merge,
                "push": after_push,
            }
        )

    # Direct push: Inherit needs sibling refs in the fixture (real clones have them after fetch).
    d = work_root / "direct"
    init_repo(d, cfg_text, base_tag, siblings=True)
    at_tag = run_gv(gv, d)

    # push #1 with 1 commit
    (d / "f.txt").write_text("push1-c1\n", encoding="utf-8")
    git(d, "add", "-A")
    git(d, "commit", "-qm", "push1 one commit")
    push1_one = run_gv(gv, d)

    # push #1 with 10 commits — keep sibling tips at HEAD for Inherit
    d10 = work_root / "direct_10"
    init_repo(d10, cfg_text, base_tag, siblings=True)
    for i in range(1, 11):
        (d10 / "f.txt").write_text(f"push1-c{i}\n", encoding="utf-8")
        git(d10, "add", "-A")
        git(d10, "commit", "-qm", f"push1 commit {i}/10")
        for b in ("dev", "release/placeholder", "hotfix/placeholder"):
            git(d10, "branch", "-f", b, "HEAD")
    push1_ten = run_gv(gv, d10)

    # push #2 after CI tag
    if push1_one.startswith("FAIL") or "-" in push1_one:
        push2 = push1_one
    else:
        git(d, "tag", f"v{push1_one}")
        (d / "f.txt").write_text("push2-c1\n", encoding="utf-8")
        git(d, "add", "-A")
        git(d, "commit", "-qm", "push2 one commit")
        for b in ("dev", "release/placeholder", "hotfix/placeholder"):
            git(d, "branch", "-f", b, "HEAD")
        push2 = run_gv(gv, d)

    return {
        "base_tag": base_tag,
        "rows": rows,
        "direct": {
            "at_tag": at_tag,
            "push1_one": push1_one,
            "push1_ten": push1_ten,
            "push2": push2,
        },
    }


def render_markdown(result: dict) -> str:
    tag = result["base_tag"]
    lines = [
        f"База: **`{tag}`**. Merge = `--no-ff`. Колонка **push** = +1 коммит на **source**-ветке. Отдельно — Direct push в `master`.",
        "",
        "| Ветка (source) | SemVer на source | после push (+1) | после squash | после merge |",
        "|---|---|---|---|---|",
    ]
    for row in result["rows"]:
        lines.append(
            f"| `{row['branch']}` | `{row['source']}` | `{row['push']}` | `{row['squash']}` | `{row['merge']}` |"
        )
    d = result["direct"]
    lines.extend(
        [
            "",
            "**Direct push в `master`:** (один bump после тега; N коммитов в одном push = один номер; следующий bump — после CI-тега.)",
            "",
            "| Состояние | SemVer |",
            "|---|---|",
            f"| на теге `{tag}` | `{d['at_tag']}` |",
            f"| push #1 = 1 коммит | `{d['push1_one']}` |",
            f"| push #1 = 10 коммитов | `{d['push1_ten']}` |",
            f"| push #2 = 1 коммит (после CI-тега `v{d['push1_one']}`) | `{d['push2']}` |",
            "",
        ]
    )
    return "\n".join(lines)


def main() -> int:
    ap = argparse.ArgumentParser(description="GitVersion strategy matrix report")
    ap.add_argument(
        "--config",
        default="GitVersion.yml",
        help="Path to GitVersion.yml (default: ./GitVersion.yml)",
    )
    ap.add_argument("--base-tag", default="v10.1.3", help="Baseline git tag (default: v10.1.3)")
    ap.add_argument("--gv", default=None, help="Path to dotnet-gitversion")
    ap.add_argument(
        "--branch",
        action="append",
        dest="branches",
        default=None,
        help="Branch to test (repeatable). Default: built-in set.",
    )
    ap.add_argument("--json", action="store_true", help="Emit JSON instead of markdown")
    ap.add_argument(
        "--workdir",
        default=None,
        help="Fixture root (default: temp dir under .tmp-gvfind/matrix)",
    )
    args = ap.parse_args()

    cfg_path = Path(args.config).resolve()
    if not cfg_path.is_file():
        raise SystemExit(f"config not found: {cfg_path}")
    cfg_text = cfg_path.read_text(encoding="utf-8").replace("\r\n", "\n")

    repo = Path.cwd()
    if args.workdir:
        work_root = Path(args.workdir).resolve()
        work_root.mkdir(parents=True, exist_ok=True)
        cleanup = False
    else:
        base = repo / ".tmp-gvfind" / "matrix"
        base.mkdir(parents=True, exist_ok=True)
        work_root = Path(tempfile.mkdtemp(prefix="run-", dir=str(base)))
        cleanup = True

    gv = find_gv(args.gv)
    branches = args.branches or DEFAULT_BRANCHES

    try:
        result = run_matrix(
            gv=gv,
            cfg_text=cfg_text,
            base_tag=args.base_tag,
            branches=branches,
            work_root=work_root,
        )
    finally:
        if cleanup:
            shutil.rmtree(work_root, ignore_errors=True)

    if args.json:
        print(json.dumps(result, ensure_ascii=False, indent=2))
    else:
        sys.stdout.write(render_markdown(result))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
