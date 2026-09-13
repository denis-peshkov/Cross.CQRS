# Release readiness plan `dev` → `master`

> **Purpose:** checklist before merging the integration line into `master` for a NuGet release.  
> **Product:** Cross.CQRS  
> **Current target:** `11.1.0` / branch `hotfix/no-preview-git-tags` (see [`RELEASE-PLAN-11.1.0.md`](RELEASE-PLAN-11.1.0.md); prior [`RELEASE-PLAN-11.0.0.md`](RELEASE-PLAN-11.0.0.md) finalized)  
> **Legend:** ⬜ open · ✅ done · 🟨 partial · ❌ blocker  
> **Related:** [`BREAKING.md`](BREAKING.md), [`CHANGELOG.md`](CHANGELOG.md), [`TO-DO.md`](TO-DO.md)  
> **Updated:** 2026-09-13

**Checklist summary:** **22** items — ✅ **13** (59%) · 🟨 **1** (5%) · ⬜ **8** (36%) · ❌ **0** (0%)

---

## 1. Preconditions

| # | Item | Status |
|---|------|--------|
| P1 | Target version agreed (GitVersion / tag `vX.Y.Z`) | 🟨 GitVersion `11.1.0`; tag `v11.1.0` already on master tip (`d5ddc59`) — see plan **M8** |
| P2 | Version plan file `docs/RELEASE-PLAN-X.Y.Z.md` filled (severity template) | ✅ [`RELEASE-PLAN-11.1.0.md`](RELEASE-PLAN-11.1.0.md) (active); `11.0.0` finalized |
| P3 | Open backlog reviewed (`docs/TO-DO.md`) — no unexpected C/H blockers | ✅ open C/H empty; plan open **M8**/**L6**; **Принято** kept |
| P4 | Branch policy / rulesets understood (`CONTRIBUTING.md`, `.github/rulesets/`) | ✅ + hotfix docs for stable-only git tags (WT) |

---

## 2. Breaking changes

| # | Item | Status |
|---|------|--------|
| B1 | All consumer breaks listed in `docs/BREAKING.md` (newest section on top) | ✅ no new consumer break for 11.1.0 (CI/docs only); 10.1.x→11.0.0 unchanged |
| B2 | PR titles used `BREAKING:` where applicable | ✅ N/A for this hotfix (not BREAKING) |
| B3 | `config.nuspec` `releaseNotes` links to BREAKING (no full duplicate) | ✅ unchanged from 11.0.0 |
| B4 | `docs/CHANGELOG.md` updated in English (**always** — every release prep / skill run) | ✅ `## v11.1.0` (CI / release process) |

---

## 3. Build, tests, quality

| # | Item | Status |
|---|------|--------|
| Q1 | `dotnet build Cross.CQRS.slnx -c Release` | ✅ carried from 11.0.0 prep |
| Q2 | `dotnet test Cross.CQRS.Tests/Cross.CQRS.Tests.csproj -c Release` | ✅ carried from 11.0.0 prep |
| Q3 | CI `.NET` workflow green on release branch | ⬜ after commit/push hotfix |
| Q4 | SonarCloud / quality gate acceptable | ⬜ after tip CI |
| Q5 | SampleWebApp still starts / smoke paths OK | ✅ carried (product unchanged) |

---

## 4. Packaging and publish

| # | Item | Status |
|---|------|--------|
| N1 | `Cross.CQRS/config.nuspec` metadata (license, readme, TFM groups) | ✅ unchanged |
| N2 | Secrets: `NUGET_API_KEY`, `TAGTOKEN` valid | ✅ present |
| N3 | Tag push + NuGet push from CI succeed | ⬜ stable tag policy in WT; verify no preview tags after land |
| N4 | GitHub Release notes published | ⬜ for `v11.1.0` / next stable as decided in **M8** |

---

## 5. After `master`

| # | Item | Status |
|---|------|--------|
| A1 | Back-merge `master` → `dev` (`backmerge-master-to-dev.yml`) | ⬜ after master land |
| A2 | Sibling extension package(s) can consume published core when needed | ⬜ as needed |
| A3 | Close or defer leftover open `TO-DO` C/H/M/L items | ✅ none in TO-DO open; plan **M8**/**L6** still open |

---

## 6. Go / No-Go

| # | Item | Status |
|---|------|--------|
| G1 | Go / No-Go decision recorded | ⬜ |
| G2 | Publish blockers cleared (L6 commit, M8 tag story, tip CI, N3) | ⬜ |

- **Date:** 2026-09-13  
- **Notes:** New active plan [`RELEASE-PLAN-11.1.0.md`](RELEASE-PLAN-11.1.0.md) for hotfix: git tags only when `semVer` has no `-` suffix. Committed delta vs master = 0 (changes in WT). Before Go: **L6** commit → CI → resolve **M8** → N3/N4.
