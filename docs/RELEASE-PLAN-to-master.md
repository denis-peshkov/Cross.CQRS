# Release readiness plan `dev` → `master`

> **Purpose:** checklist before merging into `master` for a NuGet release.
> **Product:** Cross.CQRS
> **Current target:** `11.2.0` · [`release/license-hosted-validator`](https://github.com/denis-peshkov/Cross.CQRS/tree/release/license-hosted-validator) · [`RELEASE-PLAN-11.2.0.md`](RELEASE-PLAN-11.2.0.md)
> **Legend:** ⬜ open · ✅ done · 🟨 partial · ❌ blocker
> **Related:** [`BREAKING.md`](BREAKING.md) · [`CHANGELOG.md`](CHANGELOG.md) · [`TO-DO.md`](TO-DO.md)
> **Updated:** 2026-09-16

**Change summary:** **22** items — ✅ **14** (64%) · 🟨 **1** (5%) · ⬜ **7** (32%) · ❌ **0** (0%)

---

## 1. Preconditions

| # | Item | Status |
|---|---|---|
| P1 | Target version agreed (GitVersion / tag `vX.Y.Z`) | ✅ `11.2.0` |
| P2 | Version plan `docs/RELEASE-PLAN-X.Y.Z.md` filled | ✅ [`RELEASE-PLAN-11.2.0.md`](RELEASE-PLAN-11.2.0.md) |
| P3 | `docs/TO-DO.md` — no unexpected C/H blockers | ✅ open C/H/M/L пустые (open **M10**/**L19**/**L20** в version plan) |
| P4 | Branch policy understood (`CONTRIBUTING.md`) | ✅ `release/*` → Minor |

---

## 2. Breaking changes

| # | Item | Status |
|---|---|---|
| B1 | Consumer breaks in `docs/BREAKING.md` | ✅ нет (additive hosted + Hosting.Abstractions) |
| B2 | PR title `BREAKING:` where applicable | ✅ N/A |
| B3 | `config.nuspec` `releaseNotes` → BREAKING | ✅ без новой breaking-секции |
| B4 | `docs/CHANGELOG.md` updated | ✅ `## v11.2.0` |

---

## 3. Build, tests, quality

| # | Item | Status |
|---|---|---|
| Q1 | `dotnet build` Release | ✅ restore + build в `dotnet test` |
| Q2 | `dotnet test` Release | ✅ net6–net10 44 passed; netcoreapp3.1 no x64 host (TO-DO) |
| Q3 | CI `.NET` green on release branch | ⬜ нет PR / tip CI |
| Q4 | SonarCloud / quality gate | ⬜ after tip CI |
| Q5 | SampleWebApp smoke | ✅ N/A — sample не менялся; host подхватит `IHostedService` |

---

## 4. Packaging and publish

| # | Item | Status |
|---|---|---|
| N1 | `config.nuspec` metadata | ✅ `Microsoft.Extensions.Hosting.Abstractions` per TFM |
| N2 | Secrets `NUGET_API_KEY`, `TAGTOKEN` | ✅ |
| N3 | Tag + NuGet push from CI | ⬜ ожидать `v11.2.0` |
| N4 | GitHub Release notes | ⬜ для `v11.2.0` |

---

## 5. After `master`

| # | Item | Status |
|---|---|---|
| A1 | Back-merge `master` → `dev` | ⬜ after land |
| A2 | Sibling packages consume core | 🟨 EF: `ILicenseProductInfo`, без второго pipeline slot |
| A3 | Leftover TO-DO C/H/M/L | ✅ open пуст |

---

## 6. Go / No-Go

| # | Item | Status |
|---|---|---|
| G1 | Go / No-Go recorded | ⬜ |
| G2 | Publish blockers cleared | ⬜ M10 + tip CI → N3/N4 |

- **Date:** 2026-09-16
- **Notes:** Version plan `11.2.0` drafted (open **M10**, **L19**, **L20**). PR ещё нет. Publish: PR → tip CI → tag `v11.2.0` / NuGet.
