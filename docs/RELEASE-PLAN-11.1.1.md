Ниже — **проблемы внутри библиотеки**, по уровню критичности. Аудит по дельте ветки относительно базовой ветки (обычно `master`).

> **Версия:** `11.1.1` · **ветка:** `hotfix/no-preview-git-tags` · **база:** `v11.1.0` / `origin/master` · **дата:** `2026-09-13`
>
> **Релиз (если есть):** https://github.com/denis-peshkov/Cross.CQRS/releases/tag/v11.1.1
>
> **Легенда:** ⬜ open · ✅ done · 🟨 partial / принято · ❌ blocker
>
> **Предыдущий план:** [RELEASE-PLAN-11.1.0.md](RELEASE-PLAN-11.1.0.md)
>
> Дельта: `v11.1.0...HEAD` — tip hotfix. Open C/H/M/L пустые.

**CodeRabbit:** findings → **#L7–#L12** закрыты (вкл. **#L12** JSDoc на `update-changelog.mjs`).

**PR:** [#22](https://github.com/denis-peshkov/Cross.CQRS/pull/22) (GitVersion actions v4.7 and stop tagging from dev).

---

## Критично (безопасность)

---

## Высокий (логика / auth model)

---

## Средний (противоречия / баги контрактов)

---

## Низкий (техдолг / несогласованности)

---

## Принято (осознанный trade-off)

- SemVer целится **только** через `GitVersion.yml` (без CI `/overrideconfig`, без matrix fallback, без `+semver`).
- `commit-message-incrementing: Disabled` — всегда.
- `main.increment: Inherit` + `source-branches: [release, hotfix]`; корневой `increment: Patch` для orphaned `master`.
- Цифры в имени `release/*` игнорируются.
- CI: GitVersion **6.8.2**; git tag только stable на `master`/`release`/`hotfix`; **`dev` никогда не тегает** (NuGet `-dev.*` можно).
- Локальные хвосты `release/*` тоже могут завышать SemVer — для publish ориентир = CI **после** очистки мусорных tags.

---

## Закрыто (проверено в коде)

| # | Суть |
|---|------|
| ✅ #H7 delete v11.2.0-dev.3 | удалён с origin; `dev` убран из Create/Push git Tag (NuGet на `dev` остаётся) |
| ✅ #L7 changelog --dry-run wins | любой `--dry-run` запрещает запись (даже с `--write`); только печать секции; тесты |
| ✅ #L8 changelog pathBullet neutral | только path-based bullets; без hardcoded v4.7 / tag-policy / strategy claims; тест |
| ✅ #L9 no quiet CR fixes | `coderabbit` Phase 4: fix+`✅ #Id` same turn (release-plan не дублировали) |
| ✅ #L10 changelog collectDelta fail-hard | `rev-parse v${from}`; diff/log без fail→`[]`; тесты |
| ✅ #L11 CR explain→plan | pasted finding / «объясни» → Phase 3 open row **same turn** (forbid explain-only) |
| ✅ #L12 changelog JSDoc coverage | JSDoc на всех функциях `update-changelog.mjs` (docstring threshold) |
| ✅ GitVersion PR Number capture | `pull-request.regex` + `(?<Number>\d+)` — CI PR не отдаёт `pr{Number}` (NU5010) |
| ✅ GitVersion.yml GV6 strategy | `Disabled`; root `Patch`; main `Inherit` + release/hotfix |
| ✅ gitversion-strategy skill | matrix + golden test |
| ✅ Matrix golden green | release Minor / hotfix Patch / direct push Patch |
| ✅ CI versionSpec 6.8.2 | + stable-only Create/Push tag |
| ✅ CHANGELOG v11.1.0 process | tags только stable |
| ✅ RELEASE-PLAN-11.1.0 closed | предыдущий план |
| ✅ .gitignore `.tmp-*` | |
| ✅ GitVersion actions v4.7.0 | setup/execute `@v4.7.0` для GV 6.8.x |
| ✅ #L6 CHANGELOG v11.1.1 | `update-changelog.mjs --write`; секция в `docs/CHANGELOG.md` |
| ✅ update-changelog.mjs | release-plan Phase 3 всегда пишет CHANGELOG |

---

## Что в библиотеке уже нормально

- API / licensing / MediatR в дельте не менялись.
- Breaking для NuGet не нужен.
- Без тега `v11.2.0-dev.3` clean clone master → SemVer **`11.1.1`**.

---

## Приоритет фиксов

1. Запушить tip hotfix → CI на [#22](https://github.com/denis-peshkov/Cross.CQRS/pull/22) → merge → tag `v11.1.1`.
2. Остальное → [`TO-DO.md`](TO-DO.md).
