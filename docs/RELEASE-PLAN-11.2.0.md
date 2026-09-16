Ниже — **проблемы внутри библиотеки**, по уровню критичности. Аудит по дельте ветки относительно базовой ветки (обычно `master`).

> **Версия:** `11.2.0` · **ветка:** `release/license-hosted-validator` · **база:** `origin/master` (`v11.1.2`) · **дата:** `2026-09-16`
>
> **Релиз (если есть):** https://github.com/denis-peshkov/Cross.CQRS/releases/tag/v11.2.0
>
> **Легенда:** ⬜ open · ✅ done · 🟨 partial / принято · ❌ blocker
>
> **Предыдущий план:** [RELEASE-PLAN-11.1.2.md](RELEASE-PLAN-11.1.2.md)
>
> Дельта: `origin/master...HEAD` — **1** коммита · **14** файлов · **+244 / −8**. Open: C0 H0 M1 L2

**CodeRabbit:** не запускался.

**PR:** —

---

## Критично (безопасность)

---

## Высокий (логика / licensing / auth model)

---

## Средний (противоречия / баги контрактов)

### M10. pr-message skill: EF sln / test project

`.cursor/skills/pr-message/SKILL.md` Phase 3 гоняет `Cross.CQRS.EF.slnx` / `Cross.CQRS.EF.Tests` — в этом репо канон `Cross.CQRS.slnx` и `Cross.CQRS.Tests`. Авточеки Test plan на этом репо сломаны / копипаст из sibling.

---

## Низкий (техдолг / несогласованности)

### L19. `.cursor/README.md` — skills/rules, которых нет в репо

README ссылается на `rules/`, `db-scripts`, `stripe-*`, `translate-resources`. В репо skills: release-plan, coderabbit, triage*, pr-message; `.cursor/rules` нет.

### L20. `LicenseHostedValidator.StartAsync` игнорирует `CancellationToken`

`CheckLicense()` синхронный; `cancellationToken` не используется. Для IHostedService на старте обычно терпимо, но контракт не соблюдён.

---

## Принято (осознанный trade-off)

- Лицензия по-прежнему опциональна: тот же `CheckLicense` / `Validate`, что и в pipeline; без ключа host start не бросает (тест).
- Новая зависимость `Microsoft.Extensions.Hosting.Abstractions` (версия по TFM) — additive, без секции `docs/BREAKING.md`.
- Pipeline order **-2** для `LicenseCheckBehavior` сохранён; **-1** больше не резервируется под Cross.CQRS.EF (EF — через `ILicenseProductInfo`).
- `netcoreapp3.1` в локальном прогоне без x64 host abort — как в TO-DO (`SkipNetCoreApp31Tests`).

---

## Закрыто (проверено в коде)

| # | Суть |
|---|---|
| ✅ LicenseHostedValidator | `IHostedService` → `CheckLicense` на старте generic host; `TryAddEnumerable` в `AddCQRS` |
| ✅ Hosting.Abstractions | PackageReference + `config.nuspec` per TFM (как Logging) |
| ✅ Registration tests | descriptor `IHostedService`; start without key does not throw |
| ✅ README / CONTRIBUTING | host start + per-request license check |
| ✅ CodeRabbit licensing hint | instructions + `LicenseHostedValidator` |
| ✅ #L21 CHANGELOG v11.2.0 | `update-changelog.mjs --write`; Unreleased влит в `## v11.2.0`; секция уточнена |

---

## Что в библиотеке уже нормально

- Публичный `AddCQRS` / handlers / filters / validation в дельте не ломались.
- `LicenseCheckBehavior` order **-2** на каждом MediatR request сохранён.
- Breaking для NuGet не оформляем: additive hosted service + новая package dependency; optional license без ключа не fail.

---

## Приоритет фиксов

1. **M10** — поправить пути build/test в `pr-message` под Cross.CQRS.
2. **L19** / **L20** — README skills / `CancellationToken` на старте.
3. Кросс-версионный backlog — [`TO-DO.md`](TO-DO.md).
