Ниже — **проблемы внутри библиотеки**, по уровню критичности. Аудит по дельте ветки относительно базовой ветки (обычно `master`).

> **Версия:** `11.3.0` · **ветка:** `release/command-query-records` · **база:** `origin/master` (`v11.2.0`) · **дата:** `2026-09-17`
>
> **Релиз (если есть):** https://github.com/denis-peshkov/Cross.CQRS/releases/tag/v11.3.0
>
> **Легенда:** ⬜ open · ✅ done · 🟨 partial / принято · ❌ blocker
>
> **Предыдущий план:** [RELEASE-PLAN-11.2.0.md](RELEASE-PLAN-11.2.0.md)
>
> Дельта: `origin/master...HEAD` — **5** коммита · **29** файлов · **+234 / −83**. Open: C0 H1 M0 L0

**CodeRabbit:**
- `2026-09-17` · log `.cursor/skills/coderabbit/.cache/cr-release-command-query-records-vs-origin-master-all-20260917-153231.jsonl` · 3 findings (0 Critical, 0 Major, 3 Minor) → Open: C0 H1 M0 L0
- `2026-09-17` · log `.cursor/skills/coderabbit/.cache/cr-release-command-query-records-vs-origin-master-all-20260917-155731.jsonl` · 2 findings (0 Critical, 1 Major, 1 Minor) → Open: C0 H1 M0 L0

**PR:** —

---

## Критично (безопасность)

---

## Высокий (логика / licensing / auth model)

### H8. `ICommandEvent.CommandEventId` — SemVer major vs 11.3.0

На публичный интерфейс добавлен обязательный `Guid CommandEventId` — source/binary break для ручных implementors. CR: либо **next major** (12.0.0), либо вынести в opt-in interface. Сейчас шипится как **11.3.0** + `docs/BREAKING.md` (как class→record). Нужно явное решение maintainers.

---

## Средний (противоречия / баги контрактов)

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
| ✅ BREAKING 11.2.x→11.3.0 | TOC + секция; `Release:` pending до tag; layout `---` + пустая строка |
| ✅ Sample + tests records | inheritors `record`; SampleWebApp events `: CommandEvent` |
| ✅ #L25 CHANGELOG v11.3.0 | `update-changelog.mjs --write`; секция уточнена |
| ✅ SkipNetCoreApp31Tests | csproj default skip 3.1 on OSX Arm64; skill `release-plan` всегда передаёт `-p:SkipNetCoreApp31Tests=true` |
| ✅ tests net6–net10 | 45 passed, `-p:SkipNetCoreApp31Tests=true` |
| ✅ #M11 nuspec releaseNotes | `releaseNotes` → `CHANGELOG.md` + `BREAKING.md` (без дубля секций) |
| ✅ #M12 Command XML identity | `Command` / `Command<TResult>` summaries kept as base implementation (CR wording trimmed) |
| ✅ #M13 CommandEventId on CommandEvent | `CommandEventId = Guid.NewGuid()` — value-equality различает два события с одним `CommandId`; `Equal` в тесте ок |
| ✅ #L26 to-master B3 nuspec | B3/N1: `releaseNotes` → `docs/BREAKING.md` без якорей |

---

## Что в библиотеке уже нормально

- Pipeline / `AddCQRS` / `LicenseCheckBehavior` **-2** / hosted validator в этой дельте не ломались.
- Handlers остаются классами.
- Breaking для потребителей **есть** и живёт только в `docs/BREAKING.md` (не дублировать в nuspec).

---

## Приоритет фиксов

1. **H8** — решение: 12.0.0 vs оставить 11.3.0 + BREAKING (и/или opt-in interface).
2. PR с префиксом `BREAKING:` (чеклист B2).
3. Sibling EF: Commands/Queries → `record`.
4. Кросс-версионный backlog — [`TO-DO.md`](TO-DO.md).
