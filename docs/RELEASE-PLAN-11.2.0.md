Ниже — **проблемы внутри библиотеки**, по уровню критичности. Аудит по дельте ветки относительно базовой ветки (обычно `master`).

> **Версия:** `11.2.0` · **ветка:** `release/license-hosted-validator` · **база:** `origin/master` (`v11.1.2`) · **дата:** `2026-09-17`
>
> **Релиз (если есть):** https://github.com/denis-peshkov/Cross.CQRS/releases/tag/v11.2.0
>
> **Легенда:** ⬜ open · ✅ done · 🟨 partial / принято · ❌ blocker
>
> **Предыдущий план:** [RELEASE-PLAN-11.1.2.md](RELEASE-PLAN-11.1.2.md)
>
> Дельта: `origin/master...HEAD` — **13** коммита · **66** файлов · **+3494 / −308**. Open: C0 H0 M0 L3

**CodeRabbit:**
- `2026-09-17` · pasted agent finding (EF SeedLookup MERGE) · 1 finding (0 Critical, 0 Major, 0 Minor; 1 Trivial) → Open: C0 H0 M0 L3
- `2026-09-17` · pasted agent finding (pr-message Shell perms) · 1 finding (0 Critical, 0 Major, 0 Minor; 1 Trivial) → Open: C0 H0 M0 L3

**PR:** [#25](https://github.com/denis-peshkov/Cross.CQRS/pull/25).

---

## Критично (безопасность)

---

## Высокий (логика / licensing / auth model)

---

## Средний (противоречия / баги контрактов)

---

## Низкий (техдолг / несогласованности)

### L19. `.cursor/README.md` — skills, которых нет в репо

`.cursor/rules/` уже в дельте. README всё ещё ссылается на `db-scripts`, `stripe-*`, `translate-resources` — этих skills в репо нет.

### L20. `LicenseHostedValidator.StartAsync` игнорирует `CancellationToken`

`CheckLicense()` синхронный; `cancellationToken` не используется. Для IHostedService на старте обычно терпимо, но контракт не соблюдён.

### L22. `LABELS.md` ссылается на `labels.yml`

[`.github/LABELS.md`](../.github/LABELS.md) линкует `labels.yml`, файл в git — `LABELS.yml`. На case-sensitive FS ссылка ломается.

---

## Принято (осознанный trade-off)

- Лицензия по-прежнему опциональна: тот же `CheckLicense` / `Validate`, что и в pipeline; без ключа host start не бросает (тест).
- Новая зависимость `Microsoft.Extensions.Hosting.Abstractions` (версия по TFM) — additive, без секции `docs/BREAKING.md`.
- Pipeline order **-2** для `LicenseCheckBehavior` сохранён; **-1** больше не резервируется под Cross.CQRS.EF (EF — через `ILicenseProductInfo`).
- Shared `.cursor/rules` pack включает Angular/EF/HTTP шаблоны; triage матчит по globs (на типичном PR этой библиотеки часто не срабатывают).
- `netcoreapp3.1` в локальном прогоне без x64 host abort — как в TO-DO (`SkipNetCoreApp31Tests`).
- `3_SeedLookup` MERGE + delete-not-in-source: канон = seed владеет всей lookup-таблицей; оговорки про partial ownership / owner-predicate в rule не нужны.

---

## Закрыто (проверено в коде)

| # | Суть |
|---|---|
| ✅ LicenseHostedValidator | `IHostedService` → `CheckLicense` на старте generic host; `TryAddEnumerable` в `AddCQRS` |
| ✅ Hosting.Abstractions | PackageReference + `config.nuspec` per TFM (как Logging) |
| ✅ Registration tests | descriptor `IHostedService`; start without key does not throw |
| ✅ README / CONTRIBUTING | host start + per-request license check |
| ✅ CodeRabbit licensing hint | instructions + `LicenseHostedValidator` |
| ✅ Cursor rules + triage | `.cursor/rules/*.mdc`; `load-review-rules.mjs`; static PR checklists удалены |
| ✅ GitHub labels | `.github/LABELS.yml` + `LABELS.md`; triage `docs`; без `question` template / `good first issue` |
| ✅ PR #25 | [#25](https://github.com/denis-peshkov/Cross.CQRS/pull/25); CI `build` + SonarCloud green |
| ✅ #L21 CHANGELOG v11.2.0 | `update-changelog.mjs --write`; секция уточнена (labels / templates) |
| ✅ #L23 SeedLookup MERGE delete | won’t-fix: канон = полный ownership таблицы; оговорку в `102-backend-efcore.mdc` не писать |
| ✅ #M10 pr-message EF paths | Phase 3 → `Cross.CQRS.slnx` / `Cross.CQRS.Tests` (как template) |
| ✅ #L24 pr-message Shell least privilege | build/test sandbox; elevate only when needed, smallest scope |

---

## Что в библиотеке уже нормально

- Публичный `AddCQRS` / handlers / filters / validation в дельте не ломались.
- `LicenseCheckBehavior` order **-2** на каждом MediatR request сохранён.
- Breaking для NuGet не оформляем: additive hosted service + новая package dependency; optional license без ключа не fail.

---

## Приоритет фиксов

1. **L19** / **L20** / **L22** — README leftover skills / `CancellationToken` / ссылка `LABELS.md`.
2. Кросс-версионный backlog — [`TO-DO.md`](TO-DO.md).
