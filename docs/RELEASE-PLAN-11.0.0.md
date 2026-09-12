Ниже — **проблемы внутри библиотеки**, по уровню критичности. Аудит по дельте ветки относительно базовой ветки (обычно `master`).

> **Версия:** `11.0.0` · **ветка:** `release/11.0.0-new-license-improve-functionality` · **база:** `origin/master` (`v10.1.3`) · **дата:** `2026-09-13`
>
> **Релиз (если есть):** https://github.com/denis-peshkov/Cross.CQRS/releases/tag/v11.0.0
>
> **Легенда:** ⬜ open · ✅ done · 🟨 partial / принято · ❌ blocker
>
> **Предыдущий план:** —

Дельта: `origin/master...HEAD` — **74** коммита · **139** файлов · **+7809 / −415**. CodeRabbit 2026-09-13: **13** findings (6 major / 7 minor); **#H1** skipped (уже в Закрыто). Чеклист publish/Go — [`RELEASE-PLAN-dev-to-master.md`](RELEASE-PLAN-dev-to-master.md); backlog — [`TO-DO.md`](TO-DO.md).

---

## Критично (безопасность)

---

## Высокий (логика / auth model)

---

## Средний (противоречия / баги контрактов)

### M5. RegistrationAndBehaviorTests — duplicate AddBehavior

Тест порядка behaviors не регистрирует один тип дважды — не ловит duplicate descriptors.

### M6. QueueAndExtensionsTests — exception-safe event assertion

После unmatched flow нужно assert’ить сохранение exception-safe event для targetId, не только otherId.

### M7. `post-pr-triage.mjs` — oversized patch stops loop

При превышении `maxChars` лучше `continue` (пропуск куска), а не обрыв цикла — иначе теряются следующие мелкие patches.

---

## Низкий (техдолг / несогласованности)

### L4. `config.nuspec` `releaseNotes` дублирует docs

Длинный CDATA вместо короткой выжимки + ссылок на [`CHANGELOG.md`](CHANGELOG.md) / [`BREAKING.md`](BREAKING.md). Убрать «netcoreapp3.1 dropped» и маркетинг sibling EF-пакета из notes.

---

## Принято (осознанный trade-off)

- SampleWebApp / Tests: `CA2007` в `NoWarn` (host/test style); в библиотеке — `ConfigureAwait(false)`.
- Sibling EF-пакет (отдельный репозиторий): в core только интеграционные хуки (`InternalsVisibleTo`, product claim / filter, pipeline −1) — не часть NuGet description этого пакета.
- Матрица тестов держит **netcoreapp3.1**, чтобы гонять сборку библиотеки **netstandard2.1** (`SkipNetCoreApp31Tests` без x64 3.1 host).
- Лицензия опциональна: без ключа — правила «optional license» из README.
- Tag + NuGet Push только с `master` / `release/*` / `hotfix/*` / `dev`.

---

## Закрыто (проверено в коде)

| # | Суть |
|---|------|
| ✅ #H1 LicenseCheck every request | accepted: by design — validate on every MediatR request (CR major skipped as dup) |
| ✅ #H2 SampleWebApp LicenseKey | fixed: placeholder `"<license key here>"` as in README; real JWT removed from sample |
| ✅ #H3 resolve-target-version bump rules | fixed: любая ветка → GitVersion `MajorMinorPatch`; ручной override `--version` |
| ✅ #H4 collect-data.sh JSONL files | fixed: `gh --jq` → `{number, files: [paths]}` (JSON-safe array) |
| ✅ #H5 post-pr-triage comment upsert | fixed: lookup by `TRIAGE_MARKER` + comment author (`gh api user` / `TRIAGE_COMMENT_AUTHOR`) |
| ✅ #H6 post-pr-triage labels gate | fixed: `TRIAGE_APPLY_LABELS` default on (`1`), opt-out `false`; confidence floor (default 70) + allowlist; comment always suggests |
| ✅ #M1 README license check frequency | fixed: removed redundant «first/every» pipeline blurb; frequency only in LicenseKey section |
| ✅ #M2 scaffold-breaking-section skip resolve | fixed: both `--from` + `--to`/`--version` → scaffold without GitVersion |
| ✅ #M3 release-plan-summary missing line | fixed: missing `**Checklist summary:**` → insert; «up to date» only when present+equal |
| ✅ #M4 queue process continues after publish fail | fixed: two StandardFlow events, first Publish throws — both attempted/Published, handler result ok |
| ✅ #L3 CA2007 library | `ConfigureAwait(false)` на await в библиотеке |
| ✅ #L2 NuGet publish secret | `NUGET_API_KEY` обновлён (ops); CI push больше не блокируется этим 403 |
| ✅ #L1 ReleaseNotes vs test TFMs | Notes: netcoreapp3.1 kept to exercise netstandard2.1 (not dropped) |
| ✅ JWT licensing pipeline | `LicenseKey`, `LicenseAccessor`, `LicenseValidator.Validate(license, ILicenseProductInfo)`, `LicenseCheckBehavior` (−2), product metadata DI |
| ✅ AddCQRS registration | `CqrsServiceConfiguration`; FluentValidation `AddValidatorsFromAssemblies` по полному набору сборок |
| ✅ TFMs / deps | `netstandard2.1;net6–net10`; Extensions.* по TFM |
| ✅ Solution / packaging | `Cross.CQRS.slnx`; `config.nuspec`; `_nuget` убран; `LICENSE.md` |
| ✅ Docs consumers | `docs/BREAKING.md` From 10.1.x→11.0.0; `docs/CHANGELOG.md` `## v11.0.0` |
| ✅ Tests NUnit | зоны Licensing / Registration / Behaviors / Queue / Core |
| ✅ CI tag/NuGet gates | `startsWith` для release/hotfix; publish = master/release/hotfix/dev |
| ✅ Sonar key aligned | `projectKey=Cross.CQRS` в workflow + README/CONTRIBUTING |
| ✅ PR / issues templates | breaking → `docs/BREAKING.md`; placeholders `x.y.z`; legacy template удалён |
| ✅ Package description | nuspec + CI Description без маркетинга sibling EF |

---

## Что в библиотеке уже нормально

- MediatR pipeline: license (−2) / reserved −1 / filters / validation согласованы с кодом.
- Breaking 10.1.x→11.0.0 — только в `docs/BREAKING.md`.
- Local Release build/tests (net6–net10) ранее зелёные на ветке.

---

## Приоритет фиксов

1. **M5–M6** — усилить тесты registration / queue exception-safe.
2. **M7** — triage oversized patch: `continue` not break.
3. **L4** — trim nuspec `releaseNotes`.
4. Publish gate — [`RELEASE-PLAN-dev-to-master.md`](RELEASE-PLAN-dev-to-master.md).
5. Ops: revoke JWT that was previously committed in SampleWebApp history.
