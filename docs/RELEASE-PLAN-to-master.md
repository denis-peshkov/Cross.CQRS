# Release readiness plan `dev` → `master`

> **Purpose:** checklist before merging into `master` for a NuGet release.
> **Product:** Cross.CQRS
> **Current target:** `11.3.0` · [`release/command-query-records`](https://github.com/denis-peshkov/Cross.CQRS/tree/release/command-query-records) · [`RELEASE-PLAN-11.3.0.md`](RELEASE-PLAN-11.3.0.md)
> **Legend:** ⬜ open · ✅ done · 🟨 partial · ❌ blocker
> **Related:** [`BREAKING.md`](BREAKING.md) · [`CHANGELOG.md`](CHANGELOG.md) · [`TO-DO.md`](TO-DO.md)
> **Updated:** 2026-09-17

**Change summary:** **22** items — ✅ **12** (55%) · 🟨 **1** (5%) · ⬜ **9** (41%) · ❌ **0** (0%)

---

## 1. Preconditions

| # | Item | Status |
|---|---|---|
| P1 | Target version agreed (GitVersion / tag `vX.Y.Z`) | ✅ `11.3.0` |
| P2 | Version plan `docs/RELEASE-PLAN-X.Y.Z.md` filled | ✅ [`RELEASE-PLAN-11.3.0.md`](RELEASE-PLAN-11.3.0.md) |
| P3 | `docs/TO-DO.md` — no unexpected C/H blockers | ✅ TO-DO C/H/M/L пустые; version plan **H8** open |
| P4 | Branch policy understood (`CONTRIBUTING.md`) | ✅ `release/*` → Minor |

---

## 2. Breaking changes

| # | Item | Status |
|---|---|---|
| B1 | Consumer breaks in `docs/BREAKING.md` | ✅ From 11.2.x to 11.3.0 (records) |
| B2 | PR title `BREAKING:` where applicable | ⬜ нет PR |
| B3 | `config.nuspec` `releaseNotes` → BREAKING | ✅ ссылка на `docs/BREAKING.md` |
| B4 | `docs/CHANGELOG.md` updated | ✅ `## v11.3.0` |

---

## 3. Build, tests, quality

| # | Item | Status |
|---|---|---|
| Q1 | `dotnet build` Release | ✅ `dotnet test` restore+build |
| Q2 | `dotnet test` Release | ✅ net6–net10 45 passed (`SkipNetCoreApp31Tests` OSX Arm64) |
| Q3 | CI `.NET` green on release branch | ⬜ нет PR |
| Q4 | SonarCloud / quality gate | ⬜ after tip CI |
| Q5 | SampleWebApp smoke | 🟨 sample inheritors → `record`; smoke host не гоняли |

---

## 4. Packaging and publish

| # | Item | Status |
|---|---|---|
| N1 | `config.nuspec` metadata | ✅ releaseNotes → CHANGELOG + BREAKING |
| N2 | Secret `TAGTOKEN` | ✅ |
| N3 | Tag + NuGet push from CI | ⬜ ожидать `v11.3.0` |
| N4 | GitHub Release notes | ⬜ для `v11.3.0` |

---

## 5. After `master`

| # | Item | Status |
|---|---|---|
| A1 | Back-merge `master` → `dev` | ⬜ after land |
| A2 | Sibling packages consume core | ⬜ EF Commands/Queries → `record` |
| A3 | Leftover TO-DO C/H/M/L | ✅ open пуст |

---

## 6. Go / No-Go

| # | Item | Status |
|---|---|---|
| G1 | Go / No-Go recorded | ⬜ |
| G2 | Publish blockers cleared | ⬜ **H8** + `BREAKING:` PR → N3/N4 |

- **Date:** 2026-09-17
- **Notes:** Version plan `11.3.0` (open **H8**; ✅ **#L26** B3/N1). Breaking: Command/Query/CommandEvent records + `CommandEventId`. Publish: решение H8 → PR `BREAKING:` → tip CI → tag `v11.3.0` / NuGet.
