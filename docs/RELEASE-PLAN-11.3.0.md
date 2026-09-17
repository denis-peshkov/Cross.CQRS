Ниже — **проблемы внутри библиотеки**, по уровню критичности. Аудит по дельте ветки относительно базовой ветки (обычно `master`).

> **Версия:** `11.3.0` · **ветка:** `release/command-query-records` · **база:** `origin/master` (`v11.2.0`) · **дата:** `2026-09-17`
>
> **Релиз (если есть):** https://github.com/denis-peshkov/Cross.CQRS/releases/tag/v11.3.0
>
> **Легенда:** ⬜ open · ✅ done · 🟨 partial / принято · ❌ blocker
>
> **Предыдущий план:** [RELEASE-PLAN-11.2.0.md](RELEASE-PLAN-11.2.0.md)
>
> Дельта: `origin/master...HEAD` — **2** коммита · **23** файлов · **+127 / −65**. Open: C0 H0 M1 L0

**CodeRabbit:**
- не запускался.

**PR:** —

---

## Критично (безопасность)

---

## Высокий (логика / licensing / auth model)

---

## Средний (противоречия / баги контрактов)

### M11. `config.nuspec` releaseNotes без 11.3.0 BREAKING

`releaseNotes` всё ещё только 11.0.0 JWT / `#from-101x-to-1100`. Для 11.3.0 нужна ссылка на [`docs/BREAKING.md`](BREAKING.md) `#from-112x-to-1130` (nuspec не дублирует секции).

---

## Низкий (техдолг / несогласованности)

---

## Принято (осознанный trade-off)

- Handlers (`CommandHandler` / `QueryHandler` / `CommandEventHandler`) остаются `abstract class`; breaking только для inheritors `Command` / `Query` / нового `CommandEvent`.
- `ICommandEvent` по-прежнему можно реализовать вручную; канон — `record … : CommandEvent`.
- `License` / `LicenseProductInfo` — `internal record`; не consumer-breaking.
- `SkipNetCoreApp31Tests=true` по умолчанию на OSX Arm64 (тот же trade-off, что в TO-DO).

---

## Закрыто (проверено в коде)

| # | Суть |
|---|---|
| ✅ record Command/Query | `abstract record Command` / `Command<TResult>` / `Query<TResult>` |
| ✅ CommandEvent base | новый `abstract record CommandEvent` (`Guid commandId`, `EventFlowType`) |
| ✅ BREAKING 11.2.x→11.3.0 | TOC + секция; layout `---` + пустая строка |
| ✅ Sample + tests records | inheritors `record`; SampleWebApp events `: CommandEvent` |
| ✅ #L25 CHANGELOG v11.3.0 | `update-changelog.mjs --write`; секция уточнена |
| ✅ SkipNetCoreApp31Tests | csproj default skip 3.1 on OSX Arm64 |
| ✅ tests net6–net10 | 45 passed, `SkipNetCoreApp31Tests=true` |

---

## Что в библиотеке уже нормально

- Pipeline / `AddCQRS` / `LicenseCheckBehavior` **-2** / hosted validator в этой дельте не ломались.
- Handlers остаются классами.
- Breaking для потребителей **есть** и живёт только в `docs/BREAKING.md` (не дублировать в nuspec).

---

## Приоритет фиксов

1. **M11** — ссылка 11.3.0 в `config.nuspec` `releaseNotes`.
2. PR с префиксом `BREAKING:` (чеклист B2).
3. Sibling EF: Commands/Queries → `record`.
4. Кросс-версионный backlog — [`TO-DO.md`](TO-DO.md).
