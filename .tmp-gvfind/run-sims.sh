#!/usr/bin/env bash
set +e
GV=/Users/forgery/.dotnet/tools/dotnet-gitversion
WS=/Users/forgery/Work/github.com/denis-peshkov/Cross.CQRS/.tmp-gvfind
mkdir -p "$WS/cfgs"
cd "$WS"

write_cfg() { cat > "$1"; }

run_one() {
  local cfgname="$1" branch="$2" msg="$3"
  local dir="$WS/runs/${cfgname}__${branch//\//_}"
  rm -rf "$dir"; mkdir -p "$dir"; cd "$dir" || return 0
  GIT_TEMPLATE_DIR= git init -b master -q
  git config user.email t@t; git config user.name t
  git config core.hooksPath /dev/null; git config core.autocrlf false
  cp "$WS/cfgs/$cfgname.yml" GitVersion.yml
  echo base > f.txt; git add -A; git commit -qm init >/dev/null
  git tag v10.1.3
  git checkout -qb "$branch" >/dev/null
  echo w >> f.txt; git add -A; git commit -qm work >/dev/null
  # extra commits on dev to force minor-ish distance
  if [ "$branch" = "dev" ]; then
    echo d2 >> f.txt; git add -A; git commit -qm d2 >/dev/null
  fi
  SRC=$("$GV" /targetpath "$(pwd)" /output json 2>/dev/null)
  if ! printf '%s' "$SRC" | python3 -c 'import json,sys; json.load(sys.stdin)' 2>/dev/null; then
    SRC_SEM=FAIL; SRC_MMP=FAIL
  else
    SRC_SEM=$(printf '%s' "$SRC" | python3 -c 'import json,sys; d=json.load(sys.stdin); print(d.get("SemVer","?"))')
    SRC_MMP=$(printf '%s' "$SRC" | python3 -c 'import json,sys; d=json.load(sys.stdin); print(d.get("MajorMinorPatch","?"))')
  fi
  git checkout master -q
  git merge --no-ff "$branch" -m "$msg" >/dev/null
  TGT=$("$GV" /targetpath "$(pwd)" /output json 2>/dev/null)
  if ! printf '%s' "$TGT" | python3 -c 'import json,sys; json.load(sys.stdin)' 2>/dev/null; then
    TGT_SEM=FAIL; TGT_MMP=FAIL
  else
    TGT_SEM=$(printf '%s' "$TGT" | python3 -c 'import json,sys; d=json.load(sys.stdin); print(d.get("SemVer","?"))')
    TGT_MMP=$(printf '%s' "$TGT" | python3 -c 'import json,sys; d=json.load(sys.stdin); print(d.get("MajorMinorPatch","?"))')
  fi
  printf '%s\t%s\t%s\t%s\t%s\n' "$cfgname" "$branch" "$SRC_SEM" "$TGT_SEM" "$TGT_MMP"
}

# --- configs ---

write_cfg "$WS/cfgs/v1_none.yml" <<'EOF'
next-version: 1.0.0
branches:
  master:
    regex: ^master$|^main$
    mode: ContinuousDelivery
    tag: ''
    source-branches: [release, hotfix, dev]
    increment: None
    prevent-increment-of-merged-branch-version: true
    track-merge-target: true
    tracks-release-branches: true
    is-release-branch: false
    pre-release-weight: 0
  release:
    regex: ^release(s)?[/-]
    mode: ContinuousDelivery
    tag: preview
    source-branches: [master, release, dev]
    increment: Minor
    prevent-increment-of-merged-branch-version: false
    track-merge-target: true
    tracks-release-branches: true
    is-release-branch: true
    pre-release-weight: 30000
  hotfix:
    regex: ^hotfix(es)?[/-]
    mode: ContinuousDelivery
    tag: preview
    source-branches: [master, hotfix]
    increment: Patch
    prevent-increment-of-merged-branch-version: false
    track-merge-target: true
    tracks-release-branches: true
    is-release-branch: true
    pre-release-weight: 30000
  dev:
    regex: ^dev(elop)?(ment)?$
    mode: ContinuousDeployment
    tag: dev
    source-branches: [master]
    increment: Minor
    prevent-increment-of-merged-branch-version: false
    track-merge-target: true
    tracks-release-branches: true
    is-release-branch: false
    pre-release-weight: 40000
EOF

write_cfg "$WS/cfgs/v2_docs.yml" <<'EOF'
next-version: 1.0.0
branches:
  master:
    regex: ^master$|^main$
    mode: ContinuousDelivery
    tag: ''
    source-branches: [release, hotfix, dev]
    increment: Patch
    prevent-increment-of-merged-branch-version: true
    track-merge-target: false
    tracks-release-branches: true
    is-release-branch: false
    pre-release-weight: 0
  release:
    regex: ^release(s)?[/-]
    mode: ContinuousDelivery
    tag: preview
    source-branches: [master, release, dev]
    increment: Minor
    prevent-increment-of-merged-branch-version: true
    track-merge-target: false
    tracks-release-branches: false
    is-release-branch: true
    pre-release-weight: 30000
  hotfix:
    regex: ^hotfix(es)?[/-]
    mode: ContinuousDelivery
    tag: preview
    source-branches: [master, hotfix]
    increment: Patch
    prevent-increment-of-merged-branch-version: true
    track-merge-target: false
    tracks-release-branches: false
    is-release-branch: true
    pre-release-weight: 30000
  dev:
    regex: ^dev(elop)?(ment)?$
    mode: ContinuousDelivery
    tag: dev
    source-branches: [master]
    increment: Minor
    prevent-increment-of-merged-branch-version: true
    track-merge-target: false
    tracks-release-branches: false
    is-release-branch: false
    pre-release-weight: 40000
EOF

write_cfg "$WS/cfgs/v3_mainline.yml" <<'EOF'
mode: Mainline
next-version: 1.0.0
branches:
  master:
    regex: ^master$|^main$
    mode: Mainline
    tag: ''
    source-branches: [release, hotfix, dev]
    increment: Patch
    prevent-increment-of-merged-branch-version: true
    track-merge-target: true
    tracks-release-branches: true
    is-release-branch: false
    pre-release-weight: 0
  release:
    regex: ^release(s)?[/-]
    mode: ContinuousDelivery
    tag: preview
    source-branches: [master, release, dev]
    increment: Minor
    is-release-branch: true
    pre-release-weight: 30000
  hotfix:
    regex: ^hotfix(es)?[/-]
    mode: ContinuousDelivery
    tag: preview
    source-branches: [master, hotfix]
    increment: Patch
    is-release-branch: true
    pre-release-weight: 30000
  dev:
    regex: ^dev(elop)?(ment)?$
    mode: ContinuousDeployment
    tag: dev
    source-branches: [master]
    increment: Minor
    is-release-branch: false
    pre-release-weight: 40000
EOF

# v4: inherit off, None, track-merge-message style via prevent on all sources
write_cfg "$WS/cfgs/v4_none_prevent_all.yml" <<'EOF'
next-version: 1.0.0
branches:
  master:
    regex: ^master$|^main$
    mode: ContinuousDelivery
    tag: ''
    source-branches: [release, hotfix, dev]
    increment: None
    prevent-increment-of-merged-branch-version: true
    track-merge-target: true
    tracks-release-branches: true
    is-release-branch: false
    pre-release-weight: 0
  release:
    regex: ^release(s)?[/-]
    mode: ContinuousDelivery
    tag: preview
    source-branches: [master, release, dev]
    increment: Minor
    prevent-increment-of-merged-branch-version: true
    track-merge-target: true
    tracks-release-branches: true
    is-release-branch: true
    pre-release-weight: 30000
  hotfix:
    regex: ^hotfix(es)?[/-]
    mode: ContinuousDelivery
    tag: preview
    source-branches: [master, hotfix]
    increment: Patch
    prevent-increment-of-merged-branch-version: true
    track-merge-target: true
    tracks-release-branches: true
    is-release-branch: true
    pre-release-weight: 30000
  dev:
    regex: ^dev(elop)?(ment)?$
    mode: ContinuousDelivery
    tag: dev
    source-branches: [master]
    increment: Minor
    prevent-increment-of-merged-branch-version: true
    track-merge-target: true
    tracks-release-branches: true
    is-release-branch: false
    pre-release-weight: 40000
EOF

# v5: ContinuousDelivery everywhere, master Patch + prevent, tracks-release true
write_cfg "$WS/cfgs/v5_cd_patch.yml" <<'EOF'
next-version: 1.0.0
branches:
  master:
    regex: ^master$|^main$
    mode: ContinuousDelivery
    tag: ''
    source-branches: [release, hotfix, dev]
    increment: Patch
    prevent-increment-of-merged-branch-version: true
    track-merge-target: true
    tracks-release-branches: true
    is-release-branch: false
    pre-release-weight: 0
  release:
    regex: ^release(s)?[/-]
    mode: ContinuousDelivery
    tag: preview
    source-branches: [master, release, dev]
    increment: Minor
    prevent-increment-of-merged-branch-version: false
    track-merge-target: true
    tracks-release-branches: true
    is-release-branch: true
    pre-release-weight: 30000
  hotfix:
    regex: ^hotfix(es)?[/-]
    mode: ContinuousDelivery
    tag: preview
    source-branches: [master, hotfix]
    increment: Patch
    prevent-increment-of-merged-branch-version: false
    track-merge-target: true
    tracks-release-branches: true
    is-release-branch: true
    pre-release-weight: 30000
  dev:
    regex: ^dev(elop)?(ment)?$
    mode: ContinuousDelivery
    tag: dev
    source-branches: [master]
    increment: Minor
    prevent-increment-of-merged-branch-version: false
    track-merge-target: true
    tracks-release-branches: true
    is-release-branch: false
    pre-release-weight: 40000
EOF

echo -e "cfg\tbranch\tsource\ttarget SemVer\ttarget MMP"
for cfg in v1_none v2_docs v3_mainline v4_none_prevent_all v5_cd_patch; do
  run_one "$cfg" 'release/11.0.0-new-license' 'Merge pull request #20 from denis-peshkov/release/11.0.0-new-license'
  run_one "$cfg" 'release/test' 'Merge pull request #21 from denis-peshkov/release/test'
  run_one "$cfg" 'hotfix/test' 'Merge pull request #22 from denis-peshkov/hotfix/test'
  run_one "$cfg" 'dev' 'Merge pull request #23 from denis-peshkov/dev'
done
