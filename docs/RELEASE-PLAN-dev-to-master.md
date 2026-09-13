# Release readiness plan `dev` → `master`

> **Purpose:** checklist before merging the integration line into `master` for a NuGet release.  
> **Product:** Cross.CQRS  
> **Current target:** `11.0.0` / branch `release/11.0.0-new-license-improve-functionality` (see [`RELEASE-PLAN-11.0.0.md`](RELEASE-PLAN-11.0.0.md))  
> **Legend:** ⬜ open · ✅ done · 🟨 partial · ❌ blocker  
> **Related:** [`BREAKING.md`](BREAKING.md), [`CHANGELOG.md`](CHANGELOG.md), [`TO-DO.md`](TO-DO.md)  
> **Updated:** 2026-09-13

**Checklist summary:** **22** items — ✅ **16** (73%) · 🟨 **1** (5%) · ⬜ **5** (23%) · ❌ **0** (0%)

---

## 1. Preconditions

| # | Item | Status |
|---|------|--------|
| P1 | Target version agreed (GitVersion / tag `vX.Y.Z`) | ✅ `v11.0.0` |
| P2 | Version plan file `docs/RELEASE-PLAN-X.Y.Z.md` filled (severity template) | ✅ [`RELEASE-PLAN-11.0.0.md`](RELEASE-PLAN-11.0.0.md) **finalized** (published / closed) |
| P3 | Open backlog reviewed (`docs/TO-DO.md`) — no unexpected C/H blockers | ✅ open C/H/M/L empty; **Принято** kept |
| P4 | Branch policy / rulesets understood (`CONTRIBUTING.md`, `.github/rulesets/`) | ✅ recipes in repo; optional import = ops |

---

## 2. Breaking changes

| # | Item | Status |
|---|------|--------|
| B1 | All consumer breaks listed in `docs/BREAKING.md` (newest section on top) | ✅ From 10.1.x → 11.0.0; layout per template |
| B2 | PR titles used `BREAKING:` where applicable | ✅ [#20](https://github.com/denis-peshkov/Cross.CQRS/pull/20) `BREAKING: Cross.CQRS 11.0.0 — …` |
| B3 | `config.nuspec` `releaseNotes` links to BREAKING (no full duplicate) | ✅ short blurb + CHANGELOG/BREAKING URLs (version-plan **L4**/**L5**) |
| B4 | `docs/CHANGELOG.md` updated in English (**always** — every release prep / skill run) | ✅ `## v11.0.0` |

---

## 3. Build, tests, quality

| # | Item | Status |
|---|------|--------|
| Q1 | `dotnet build Cross.CQRS.slnx -c Release` | ✅ local 2026-09-12 |
| Q2 | `dotnet test Cross.CQRS.Tests/Cross.CQRS.Tests.csproj -c Release` | ✅ local net6–net10 (43×); `SkipNetCoreApp31Tests` on Apple Silicon |
| Q3 | CI `.NET` workflow green on release branch | ✅ tip [`34727224354`](https://github.com/denis-peshkov/Cross.CQRS/actions/runs/34727224354) @ `c4eac82` (`build` pass) |
| Q4 | SonarCloud / quality gate acceptable | ✅ PR #20 `SonarCloud Code Analysis` pass (`projectKey=Cross.CQRS`) |
| Q5 | SampleWebApp still starts / smoke paths OK | ✅ `dotnet build SampleWebApp -c Release` 2026-09-13 (0 warnings); HTTP smoke not automated |

---

## 4. Packaging and publish

| # | Item | Status |
|---|------|--------|
| N1 | `Cross.CQRS/config.nuspec` metadata (license, readme, TFM groups) | ✅ description + tags + trimmed releaseNotes |
| N2 | Secrets: `NUGET_API_KEY`, `TAGTOKEN` valid | ✅ `NUGET_API_KEY`, `TAGTOKEN`, `SONAR_TOKEN`, `CURSOR_API_KEY` present in repo secrets |
| N3 | Tag push + NuGet push from CI succeed | ⬜ gates OK; stable `v11.0.0` tag / NuGet push **not** run (only `v11.0.0-preview.*`) |
| N4 | GitHub Release notes published | ⬜ no `v11.0.0` GitHub Release yet |

---

## 5. After `master`

| # | Item | Status |
|---|------|--------|
| A1 | Back-merge `master` → `dev` (`backmerge-master-to-dev.yml`) | ⬜ after master land |
| A2 | Sibling extension package(s) can consume published core when needed | ⬜ after NuGet publish |
| A3 | Close or defer leftover open `TO-DO` C/H/M/L items | ✅ none open; lasting trade-offs in **Принято** |

---

## 6. Go / No-Go

| # | Item | Status |
|---|------|--------|
| G1 | Go / No-Go decision recorded | ⬜ |
| G2 | Publish blockers cleared (B3/L4–L5, Q3–Q5 tip CI, N2 TAGTOKEN as needed) | 🟨 tip CI/Sonar/Sample build/secrets/B3 OK; remaining = **Go** + tag/NuGet (`N3`/`N4`) |

- **Date:** 2026-09-13  
- **Notes:** Version plan open C/H/M/L empty. PR [#20](https://github.com/denis-peshkov/Cross.CQRS/pull/20) open (`BREAKING:`). Tip CI green @ `c4eac82`; Sonar green on PR. Stable release **not** shipped yet (no `v11.0.0` Release — only previews). Before Go: confirm G1 → merge/tag path → `N3`/`N4` → `A1`/`A2`. See [`RELEASE-PLAN-11.0.0.md`](RELEASE-PLAN-11.0.0.md).
