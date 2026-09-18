Ниже — **проблемы внутри библиотеки**, по уровню критичности. Аудит по дельте ветки относительно базовой ветки (обычно `master`).

> **Версия:** `11.3.0` (published / closed) · **ветка:** `release/command-query-records` · **база:** `origin/master` (`v11.2.0`) · **дата:** `2026-09-17`
>
> **Релиз (если есть):** https://github.com/denis-peshkov/Cross.CQRS/releases/tag/v11.3.0
>
> **Легенда:** ⬜ open · ✅ done · 🟨 partial / принято · ❌ blocker
>
> **Предыдущий план:** [RELEASE-PLAN-11.2.0.md](RELEASE-PLAN-11.2.0.md)
>
> Дельта: `origin/master...HEAD` — **8** коммита · **30** файлов · **+248 / −85**. Open C/H/M/L пустые (план закрыт).

**CodeRabbit:**
- `2026-09-17` · log `.cursor/skills/coderabbit/.cache/cr-release-command-query-records-vs-origin-master-all-20260917-153231.jsonl` · 3 findings (0 Critical, 0 Major, 3 Minor) → все закрыты в этом плане.
- `2026-09-17` · log `.cursor/skills/coderabbit/.cache/cr-release-command-query-records-vs-origin-master-all-20260917-155731.jsonl` · 2 findings (0 Critical, 1 Major, 1 Minor) → все закрыты в этом плане.

**PR:** —

---

## Критично (безопасность)

---

## Высокий (логика / licensing / auth model)

---

## Средний (противоречия / баги контрактов)

---

## Низкий (техдолг / несогласованности)

---

## Принято (осознанный trade-off)

- Handlers (`CommandHandler` / `QueryHandler` / `CommandEventHandler`) остаются `abstract class`; breaking только для inheritors `Command` / `Query` / нового `CommandEvent`.
- `ICommandEvent` по-прежнему можно реализовать вручную; канон — `record … : CommandEvent`.
- `ICommandEvent.CommandEventId` — намеренный breaking в **11.3.0** + `docs/BREAKING.md` (не 12.0.0 / не opt-in interface).
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
| ✅ #H8 CommandEventId SemVer | accepted: 11.3.0 + BREAKING (planned); не 12.0.0 / не opt-in |
| ✅ GitVersion /nofetch | `resolve-target-version.sh` — `/nofetch`, без hang на remote fetch |

---

## Что в библиотеке уже нормально

- Pipeline / `AddCQRS` / `LicenseCheckBehavior` **-2** / hosted validator в этой дельте не ломались.
- Handlers остаются классами.
- Breaking для потребителей **есть** и живёт только в `docs/BREAKING.md` (не дублировать в nuspec).

---

## Приоритет фиксов

_(пусто — релиз `11.3.0` закрыт; открытый backlog → [`TO-DO.md`](TO-DO.md).)_
