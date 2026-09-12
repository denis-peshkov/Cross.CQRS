# Release plan — Cross.CQRS `11.0.0`

> **Version:** `11.0.0` · **branch:** `release/11.0.0-new-license-improve-functionality` · **base:** `origin/master` (`v10.1.3`) · **date:** `2026-09-12`
>
> **Release (when published):** https://github.com/denis-peshkov/Cross.CQRS/releases/tag/v11.0.0
>
> **Legend:** ⬜ open · ✅ done · 🟨 partial / accepted · ❌ blocker
>
> **Previous plan:** —

**Delta:** `origin/master...HEAD` — **47** commits · **94** files · **+3208 / −373**

---

## Change summary

| Area | Role in release |
|------|-----------------|
| Licensing | JWT `LicenseKey`, `LicenseAccessor`, `LicenseValidator`, `ILicenseProductInfo`, `LicenseCheckBehavior`, product filter for `Cross.CQRS` + `Cross.CQRS.EF` |
| Registration | FluentValidation scan over full assembly set; pipeline order reserves EF slot |
| TFMs / deps | `netstandard2.1` + net6–net10; Extensions versions per TFM |
| Repo hygiene | `.editorconfig`, templates, `LICENSE.md`, CONTRIBUTING, workflows, GitVersion |
| Tests | Zones: Licensing / Registration / Behaviors / Queue / Core / Coverage |
| Packaging | `Cross.CQRS.slnx`, `config.nuspec`, remove `_nuget/` |

---

## Критично (безопасность)

---

## Высокий (логика / licensing)

---

## Средний

---

## Низкий

| # | Item | Status |
|---|------|--------|
| L1 | Sync ReleaseNotes wording vs kept `netcoreapp3.1` test TFM | ✅ kept: exercises netstandard2.1 |
| L2 | Fix `NUGET_API_KEY` (CI 403 on push) | ✅ ops updated |

---

## Checklist (release gate)

| # | Check | Status |
|---|-------|--------|
| 1 | `dotnet build Cross.CQRS.slnx` Release | 🟨 CI green on last code push; re-verify before tag |
| 2 | `dotnet test Cross.CQRS.Tests/Cross.CQRS.Tests.csproj` | 🟨 |
| 3 | SonarCloud quality gate | 🟨 |
| 4 | `docs/BREAKING.md` section 10.1.x → 11.0.0 complete | ✅ |
| 5 | `docs/CHANGELOG.md` + nuspec `releaseNotes` link to BREAKING | 🟨 nuspec still embeds long notes — trim to link |
| 6 | README licensing section | 🟨 verify |
| 7 | Import `.github/rulesets` (optional) + `TAGTOKEN` / `NUGET_API_KEY` / `CURSOR_API_KEY` | 🟨 `NUGET_API_KEY` updated (L2); rulesets / other secrets — verify |
| 8 | NuGet push from `release/*` succeeds | 🟨 unblocked after L2 — confirm on next CI run |
| 9 | Tag `v11.0.0` + GitHub release | ⬜ |
| 10 | Back-merge `master` → `dev` | ⬜ after master land |

---

## Go / No-Go

**Conditional Go** after green build/test + Sonar and a successful NuGet push dry-run on `release/*`. L1/L2 closed; remaining: tests gate, confirm NuGet push, tag/release.
