# Release readiness plan `dev` → `master`

> **Purpose:** checklist before merging the integration line into `master` for a NuGet release.  
> **Product:** Cross.CQRS  
> **Current target:** `11.0.0` / branch `release/11.0.0-new-license-improve-functionality` (see [`RELEASE-PLAN-11.0.0.md`](RELEASE-PLAN-11.0.0.md))  
> **Legend:** ⬜ open · ✅ done · 🟨 partial · ❌ blocker  
> **Related:** [`BREAKING.md`](BREAKING.md), [`CHANGELOG.md`](CHANGELOG.md), [`TO-DO.md`](TO-DO.md)  
> **Updated:** 2026-09-12

**Checklist summary:** **22** items — ✅ **10** (45%) · 🟨 **5** (23%) · ⬜ **7** (32%) · ❌ **0** (0%)

---

## 1. Preconditions

| # | Item | Status |
|---|------|--------|
| P1 | Target version agreed (GitVersion / tag `vX.Y.Z`) | ✅ `v11.0.0` |
| P2 | Version plan file `docs/RELEASE-PLAN-X.Y.Z.md` filled (severity template) | ✅ [`RELEASE-PLAN-11.0.0.md`](RELEASE-PLAN-11.0.0.md) |
| P3 | Open backlog reviewed (`docs/TO-DO.md`) — no unexpected C/H blockers | ✅ open C/H/M/L empty; **Принято** kept |
| P4 | Branch policy / rulesets understood (`CONTRIBUTING.md`, `.github/rulesets/`) | ✅ recipes in repo; optional import = ops |

---

## 2. Breaking changes

| # | Item | Status |
|---|------|--------|
| B1 | All consumer breaks listed in `docs/BREAKING.md` (newest section on top) | ✅ From 10.1.x → 11.0.0; layout per template |
| B2 | PR titles used `BREAKING:` where applicable | 🟨 verify on merge PRs if any |
| B3 | `config.nuspec` `releaseNotes` links to BREAKING (no full duplicate) | 🟨 long CDATA remains — version-plan **L4** |
| B4 | `docs/CHANGELOG.md` updated in English (**always** — every release prep / skill run) | ✅ `## v11.0.0` |

---

## 3. Build, tests, quality

| # | Item | Status |
|---|------|--------|
| Q1 | `dotnet build Cross.CQRS.slnx -c Release` | ✅ local 2026-09-12 |
| Q2 | `dotnet test Cross.CQRS.Tests/Cross.CQRS.Tests.csproj -c Release` | ✅ local net6–net10 (43×); `SkipNetCoreApp31Tests` on Apple Silicon |
| Q3 | CI `.NET` workflow green on release branch | 🟨 last green [`34708836618`](https://github.com/denis-peshkov/Cross.CQRS/actions/runs/34708836618) @ `35c05ca`; HEAD ahead — re-confirm |
| Q4 | SonarCloud / quality gate acceptable | 🟨 `projectKey=Cross.CQRS` aligned in CI/docs; gate on tip not re-checked |
| Q5 | SampleWebApp still starts / smoke paths OK | ⬜ |

---

## 4. Packaging and publish

| # | Item | Status |
|---|------|--------|
| N1 | `Cross.CQRS/config.nuspec` metadata (license, readme, TFM groups) | ✅ description OK; releaseNotes trim = **L4** / B3 |
| N2 | Secrets: `NUGET_API_KEY`, `TAGTOKEN` valid | 🟨 `NUGET_API_KEY` updated (L2 closed); `TAGTOKEN` not re-verified |
| N3 | Tag push + NuGet push from CI succeed | ⬜ gates fixed (`master`/`release`/`hotfix`/`dev`); not run for `v11.0.0` |
| N4 | GitHub Release notes published | ⬜ |

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
| G2 | Publish blockers cleared (B3/L4, Q3–Q5 tip CI, N2 TAGTOKEN as needed) | ⬜ |

- **Date:** 2026-09-12  
- **Notes:** Open = **L4** (nuspec `releaseNotes`). CI tag/NuGet gates + Sonar key + PR/issue templates closed in version plan. Before Go: L4 → CI/Sonar on **HEAD** → optional SampleWebApp → tag + NuGet. See [`RELEASE-PLAN-11.0.0.md`](RELEASE-PLAN-11.0.0.md).
