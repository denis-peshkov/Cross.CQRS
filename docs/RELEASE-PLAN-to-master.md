# Release readiness plan `dev` → `master`

> **Purpose:** checklist before merging into `master` for a NuGet release.
> **Product:** Cross.CQRS
> **Current target:** `11.3.1` · [`master`](https://github.com/denis-peshkov/Cross.CQRS/tree/master) · [`RELEASE-PLAN-11.3.1.md`](RELEASE-PLAN-11.3.1.md)
> **Legend:** ⬜ open · ✅ done · 🟨 partial · ❌ blocker
> **Related:** [`BREAKING.md`](BREAKING.md) · [`CHANGELOG.md`](CHANGELOG.md) · [`TO-DO.md`](TO-DO.md)
> **Updated:** 2026-09-18

**Change summary:** **22** items — ✅ **12** (55%) · 🟨 **1** (5%) · ⬜ **9** (41%) · ❌ **0** (0%)

---

## 1. Preconditions

| # | Item | Status |
|---|---|---|
| P1 | Target version agreed (GitVersion / tag `vX.Y.Z`) | 🟨 override `11.3.1`; GitVersion still `11.3.0` until commit after `v11.3.0` |
| P2 | Version plan `docs/RELEASE-PLAN-X.Y.Z.md` filled | ✅ [`RELEASE-PLAN-11.3.1.md`](RELEASE-PLAN-11.3.1.md) |
| P3 | `docs/TO-DO.md` — no unexpected C/H blockers | ✅ TO-DO + version plan C/H/M/L пустые |
| P4 | Branch policy understood (`CONTRIBUTING.md`) | ✅ tagged `master` → Patch after commit |

---

## 2. Breaking changes

| # | Item | Status |
|---|---|---|
| B1 | Consumer breaks in `docs/BREAKING.md` | ✅ нет consumer break `11.3.0`→`11.3.1` (tooling) |
| B2 | PR title `BREAKING:` where applicable | ⬜ нет PR |
| B3 | `config.nuspec` `releaseNotes` → BREAKING | ✅ ссылка на `docs/BREAKING.md` |
| B4 | `docs/CHANGELOG.md` updated | ✅ `## v11.3.1` |

---

## 3. Build, tests, quality

| # | Item | Status |
|---|---|---|
| Q1 | `dotnet build` Release | ✅ `dotnet test` restore+build |
| Q2 | `dotnet test` Release | ✅ net6–net10 45 passed (`SkipNetCoreApp31Tests`) |
| Q3 | CI `.NET` green on release branch | ⬜ нет PR (работа на `master` WT) |
| Q4 | SonarCloud / quality gate | ⬜ after tip CI |
| Q5 | SampleWebApp smoke | ✅ N/A this patch (sample не в дельте) |

---

## 4. Packaging and publish

| # | Item | Status |
|---|---|---|
| N1 | `config.nuspec` metadata | ✅ releaseNotes → CHANGELOG + BREAKING |
| N2 | Secret `TAGTOKEN` | ✅ |
| N3 | Tag + NuGet push from CI | ⬜ ожидать `v11.3.1` (local tag `v11.3.0`; GitHub Release нет) |
| N4 | GitHub Release notes | ⬜ для `v11.3.1` |

---

## 5. After `master`

| # | Item | Status |
|---|---|---|
| A1 | Back-merge `master` → `dev` | ⬜ after `11.3.1` commit |
| A2 | Sibling packages consume core | ⬜ EF Commands/Queries → `record` |
| A3 | Leftover TO-DO C/H/M/L | ✅ open пуст |

---

## 6. Go / No-Go

| # | Item | Status |
|---|---|---|
| G1 | Go / No-Go recorded | ⬜ |
| G2 | Publish blockers cleared | ⬜ commit WT → CI → tag `v11.3.1` |

- **Date:** 2026-09-18
- **Notes:** Version plan `11.3.1` (tooling-only WT on tagged `master`). No consumer BREAKING. Publish: commit → tip CI → tag `v11.3.1` / NuGet. Local `v11.3.0` exists; GitHub Release `v11.3.0` still missing.
