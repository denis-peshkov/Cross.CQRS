# Release readiness plan `dev` → `master`

> **Purpose:** checklist before merging the integration line into `master` for a NuGet release.  
> **Product:** Cross.CQRS  
> **Current target:** `11.0.0` / branch `release/11.0.0-new-license-improve-functionality` (see [`RELEASE-PLAN-11.0.0.md`](RELEASE-PLAN-11.0.0.md))  
> **Legend:** ⬜ open · ✅ done · 🟨 partial · ❌ blocker  
> **Related:** [`BREAKING.md`](BREAKING.md), [`CHANGELOG.md`](CHANGELOG.md), [`TO-DO.md`](TO-DO.md)  
> **Updated:** 2026-09-12

---

## 1. Preconditions

| # | Item | Status |
|---|------|--------|
| 1 | Target version agreed (GitVersion / tag `vX.Y.Z`) | ✅ `v11.0.0` |
| 2 | Version plan file `docs/RELEASE-PLAN-X.Y.Z.md` filled | ✅ [`RELEASE-PLAN-11.0.0.md`](RELEASE-PLAN-11.0.0.md) |
| 3 | Open backlog reviewed (`docs/TO-DO.md`) — no unexpected C/H blockers | ✅ |
| 4 | Branch policy / rulesets understood (`CONTRIBUTING.md`, `.github/rulesets/`) | ✅ recipes in repo; optional import = ops |

---

## 2. Breaking changes

| # | Item | Status |
|---|------|--------|
| 1 | All consumer breaks listed in `docs/BREAKING.md` (newest section on top) | ✅ |
| 2 | PR titles used `BREAKING:` where applicable | 🟨 |
| 3 | `config.nuspec` `releaseNotes` links to BREAKING (no full duplicate) | 🟨 long body remains |
| 4 | `docs/CHANGELOG.md` updated in English (**always** — every release prep / skill run) | ✅ |

---

## 3. Build, tests, quality

| # | Item | Status |
|---|------|--------|
| 1 | `dotnet build Cross.CQRS.slnx -c Release` | ✅ local 2026-09-12 |
| 2 | `dotnet test Cross.CQRS.Tests/Cross.CQRS.Tests.csproj -c Release` | ✅ local net6–net10 (43×); CI + netcoreapp3.1 TBD |
| 3 | CI `.NET` workflow green on release branch | 🟨 |
| 4 | SonarCloud / quality gate acceptable | 🟨 |
| 5 | SampleWebApp still starts / smoke paths OK | ⬜ |

---

## 4. Packaging and publish

| # | Item | Status |
|---|------|--------|
| 1 | `Cross.CQRS/config.nuspec` metadata (license, readme, TFM groups) | ✅ (trim releaseNotes still open) |
| 2 | Secrets: `NUGET_API_KEY`, `TAGTOKEN` valid | 🟨 API key updated; TAGTOKEN confirm |
| 3 | Tag push + NuGet push from CI succeed | ⬜ |
| 4 | GitHub Release notes published | ⬜ |

---

## 5. After `master`

| # | Item | Status |
|---|------|--------|
| 1 | Back-merge `master` → `dev` (`backmerge-master-to-dev.yml`) | ⬜ |
| 2 | Cross.CQRS.EF (sibling repo) can consume published version when needed | ⬜ |
| 3 | Close or defer leftover `TO-DO` items | ✅ |

---

## 6. Go / No-Go

Document decision and date here when ready:

- **Decision:** ⬜ Go / ⬜ No-Go  
- **Date:** 2026-09-12  
- **Notes:** Local build/test OK. Version-plan open: L4 (nuspec trim) — [`RELEASE-PLAN-11.0.0.md`](RELEASE-PLAN-11.0.0.md). H1 accepted (license check every request). Then CI/Sonar / SampleWebApp / tag+NuGet.
