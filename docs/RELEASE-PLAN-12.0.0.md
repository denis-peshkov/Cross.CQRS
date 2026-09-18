Ниже — **проблемы внутри библиотеки**, по уровню критичности. Аудит по дельте ветки относительно базовой ветки (обычно `master`).

> **Версия:** `12.0.0` (closed) · **ветка:** `feature/command-query-records-major` · **база:** `origin/master` (`v11.2.0`) · **дата:** `2026-09-18`
>
> **Релиз (если есть):** https://github.com/denis-peshkov/Cross.CQRS/releases/tag/v12.0.0
>
> **Легенда:** ⬜ open · ✅ done · 🟨 partial / принято · ❌ blocker
>
> **Предыдущий план:** [RELEASE-PLAN-11.2.0.md](RELEASE-PLAN-11.2.0.md)
>
> Дельта: `origin/master...HEAD` — **1** коммита · **7** файлов · **+50 / −118**. Open C/H/M/L пустые (план закрыт).

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

- Один ship **12.0.0** (major): library break Command/Query/`CommandEvent` + tooling; draft-номера 11.3.x не публиковались.
- Handlers (`CommandHandler` / `QueryHandler` / `CommandEventHandler`) остаются `abstract class`; breaking только для inheritors `Command` / `Query` / `CommandEvent`.
- `ICommandEvent` по-прежнему можно реализовать вручную; канон — `record … : CommandEvent`.
- `ICommandEvent.CommandEventId` — намеренный breaking в **12.0.0** + `docs/BREAKING.md` (не opt-in interface).
- `License` / `LicenseProductInfo` — `internal record`; не consumer-breaking.
- `SkipNetCoreApp31Tests=true` по умолчанию на OSX Arm64 (тот же trade-off, что в TO-DO).
- `GitVersion.yml` `next-version: 12.0.0`.

---

## Закрыто (проверено в коде)

| # | Суть |
|---|---|
| ✅ record Command/Query | `abstract record Command` / `Command<TResult>` / `Query<TResult>` |
| ✅ CommandEvent base | новый `abstract record CommandEvent` (`Guid commandId`, `EventFlowType`) |
| ✅ BREAKING 11.2.x→12.0.0 | TOC + секция; `Release:` pending до tag |
| ✅ Sample + tests records | inheritors `record`; SampleWebApp events `: CommandEvent` |
| ✅ #L25 CHANGELOG v12.0.0 | одна секция (library + tooling) |
| ✅ SkipNetCoreApp31Tests | csproj default skip 3.1 on OSX Arm64; skill передаёт `-p` только если свойство есть |
| ✅ tests net6–net10 | 45 passed, `-p:SkipNetCoreApp31Tests=true` |
| ✅ #M11 nuspec releaseNotes | `releaseNotes` → `CHANGELOG.md` + `BREAKING.md` (без дубля; B3/N1) |
| ✅ #M12 Command XML identity | base summaries kept (CR wording trimmed) |
| ✅ #M13 CommandEventId on CommandEvent | `Guid.NewGuid()` — value-equality при одном `CommandId` |
| ✅ #H8 CommandEventId SemVer | accepted: **12.0.0** major + BREAKING |
| ✅ GitVersion /nofetch + next-version | `/nofetch` в resolve; `next-version: 12.0.0`; `from`=`11.2.0` |
| ✅ drop local tag v11.3.0 | не было на origin / GitHub Release |
| ✅ feature branch | `feature/command-query-records-major` |
| ✅ pr-message discovery | build/test из PR template / discovery; без хардкода sln/csproj |
| ✅ changelog/triage tests | generic sample paths в `categorizePath` / `load-review-rules` |
| ✅ README / 101-cqrs wording | нет assume `db-scripts`; пайплайн CQRS без product-specific |
| ✅ to-master retarget | чеклист → `12.0.0` |

---

## Что в библиотеке уже нормально

- Pipeline / `AddCQRS` / `LicenseCheckBehavior` **-2** / hosted validator в этой дельте не ломались.
- Breaking для потребителей **есть** и живёт только в `docs/BREAKING.md` (не дублировать в nuspec).

---

## Приоритет фиксов

_(пусто — релиз `12.0.0` закрыт; открытый backlog → [`TO-DO.md`](TO-DO.md).)_
