# Cross.CQRS — open backlog (`TO-DO`)

Нерешённые пункты вне дельты version plan + кросс-версионные принятые trade-off’ы.

**Id high-water (не переиспользовать ≤):** `C0` `H8` `M13` `L26`

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

- Git tags: never from `dev`; only stable SemVer on `master` / `release/*` / `hotfix/*` (NuGet pre-release on `dev` OK).
- SemVer только через `GitVersion.yml` (без CI override / matrix fallback / `+semver`); `commit-message-incrementing: Disabled`.
- `main`: Inherit от `release`/`hotfix`; корневой `increment: Patch` для orphaned master; цифры в `release/*` игнорируются.
- CI GitVersion 6.8.2; git tag только для stable SemVer без pre-release suffix.
- SampleWebApp / Tests: `CA2007` в `NoWarn` (host/test style), library — `ConfigureAwait(false)`.
- Sibling EF-пакет (отдельный репозиторий): в core только интеграционные хуки (`InternalsVisibleTo`, product claim / `ILicenseProductInfo`) — не часть NuGet description этого пакета.
- Test matrix keeps **netcoreapp3.1** to run against the library **netstandard2.1** build (`SkipNetCoreApp31Tests` for hosts without x64 3.1).
- Лицензия опциональна: без ключа — правила «optional license» из README; host start (`LicenseHostedValidator`) тот же `CheckLicense` / `Validate`, без ключа не бросает.
- Tag только с `master` / `release/*` / `hotfix/*` (stable SemVer). NuGet Push также с `dev` (pre-release).
- Git tags только для **stable** SemVer (без `-preview` / `-dev` / …); `dev` **не** создаёт git tags.
- SonarCloud display name меняется только анализом main (`master`); PR analysis не переименовывает проект.
- Sonar `qualitygate.wait` только на `pull_request`; push/publish QG не ждёт.
- Локальные хвосты `release/*` могут завышать SemVer — для publish ориентир = CI **после** очистки мусорных tags.
- Корневой `ReleaseNotes.md` не нужен — канон `docs/CHANGELOG.md` (+ ссылка в README).
- Новая зависимость `Microsoft.Extensions.Hosting.Abstractions` (версия по TFM) — additive, без секции `docs/BREAKING.md`.
- Pipeline order **-2** для `LicenseCheckBehavior` сохранён; **-1** больше не резервируется под Cross.CQRS.EF (EF — через `ILicenseProductInfo`).
- Shared `.cursor/rules` pack включает Angular/EF/HTTP шаблоны; triage матчит по globs (на типичном PR этой библиотеки часто не срабатывают).
- `3_SeedLookup` MERGE + delete-not-in-source: канон = seed владеет всей lookup-таблицей; оговорки про partial ownership / owner-predicate в rule не нужны.
- Handlers (`CommandHandler` / `QueryHandler` / `CommandEventHandler`) остаются `abstract class`; breaking только для inheritors `Command` / `Query` / `CommandEvent`.
- `ICommandEvent` по-прежнему можно реализовать вручную; канон — `record … : CommandEvent`.
- `ICommandEvent.CommandEventId` — намеренный breaking + `docs/BREAKING.md` (не next major / не opt-in interface).
- `License` / `LicenseProductInfo` — `internal record`; не consumer-breaking.
