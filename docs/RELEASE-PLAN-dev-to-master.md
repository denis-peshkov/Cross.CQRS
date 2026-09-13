# Release readiness plan `dev` → `master`

> **Purpose:** checklist before merging the integration line into `master` for a NuGet release.  
> **Product:** Cross.CQRS  
> **Current target:** `11.1.1` / branch `hotfix/no-preview-git-tags` (see [`RELEASE-PLAN-11.1.1.md`](RELEASE-PLAN-11.1.1.md); prior [`RELEASE-PLAN-11.1.0.md`](RELEASE-PLAN-11.1.0.md) closed)  
> **Legend:** ⬜ open · ✅ done · 🟨 partial · ❌ blocker  
> **Related:** [`BREAKING.md`](BREAKING.md), [`CHANGELOG.md`](CHANGELOG.md), [`TO-DO.md`](TO-DO.md)  
> **Updated:** 2026-09-13

**Checklist summary:** **22** items — ✅ **14** (64%) · 🟨 **0** (0%) · ⬜ **8** (36%) · ❌ **0** (0%)

---

## 1. Preconditions

| # | Item | Status |
|---|------|--------|
| P1 | Target version agreed (GitVersion / tag `vX.Y.Z`) | ✅ цель **`11.1.1`**; `v11.2.0-dev.3` удалён (**H7** closed) |
| P2 | Version plan file `docs/RELEASE-PLAN-X.Y.Z.md` filled (severity template) | ✅ [`RELEASE-PLAN-11.1.1.md`](RELEASE-PLAN-11.1.1.md) (active); `11.1.0` closed |
| P3 | Open backlog reviewed (`docs/TO-DO.md`) — no unexpected C/H blockers | ✅ TO-DO open пуст; version plan open пуст |
| P4 | Branch policy / rulesets understood (`CONTRIBUTING.md`, `.github/rulesets/`) | ✅ stable-only git tags |

---

## 2. Breaking changes

| # | Item | Status |
|---|------|--------|
| B1 | All consumer breaks listed in `docs/BREAKING.md` (newest section on top) | ✅ нет нового consumer break |
| B2 | PR titles used `BREAKING:` where applicable | ✅ N/A |
| B3 | `config.nuspec` `releaseNotes` links to BREAKING (no full duplicate) | ✅ |
| B4 | `docs/CHANGELOG.md` updated in English (**always** — every release prep / skill run) | ✅ `## v11.1.1` (**L6** closed; `update-changelog.mjs`) |

---

## 3. Build, tests, quality

| # | Item | Status |
|---|------|--------|
| Q1 | `dotnet build Cross.CQRS.slnx -c Release` | ✅ carried |
| Q2 | `dotnet test Cross.CQRS.Tests/Cross.CQRS.Tests.csproj -c Release` | ✅ carried |
| Q3 | CI `.NET` workflow green on release branch | ⬜ после commit/push (WT: actions `@v4.7.0`) |
| Q4 | SonarCloud / quality gate acceptable | ⬜ after tip CI |
| Q5 | SampleWebApp still starts / smoke paths OK | ✅ carried |

---

## 4. Packaging and publish

| # | Item | Status |
|---|------|--------|
| N1 | `Cross.CQRS/config.nuspec` metadata (license, readme, TFM groups) | ✅ |
| N2 | Secrets: `NUGET_API_KEY`, `TAGTOKEN` valid | ✅ |
| N3 | Tag push + NuGet push from CI succeed | ⬜ после **H7**; ожидать tag `v11.1.1` |
| N4 | GitHub Release notes published | ⬜ для `v11.1.1` |

---

## 5. After `master`

| # | Item | Status |
|---|------|--------|
| A1 | Back-merge `master` → `dev` (`backmerge-master-to-dev.yml`) | ⬜ after master land |
| A2 | Sibling extension package(s) can consume published core when needed | ⬜ as needed |
| A3 | Close or defer leftover open `TO-DO` C/H/M/L items | ✅ TO-DO open пуст; **H7**/**L6** в «Закрыто» плана |

---

## 6. Go / No-Go

| # | Item | Status |
|---|------|--------|
| G1 | Go / No-Go decision recorded | ⬜ |
| G2 | Publish blockers cleared (actions tip CI → `11.1.1`, N3 tag/NuGet) | ⬜ commit/push hotfix → green CI |

- **Date:** 2026-09-13  
- **Notes:** Цель **`11.1.1`**. CHANGELOG `v11.1.1` + `update-changelog.mjs` готовы. Осталось: commit/push → CI → tag `v11.1.1`.
