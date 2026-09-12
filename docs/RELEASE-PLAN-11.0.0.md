Ниже — **проблемы внутри библиотеки**, по уровню критичности. Аудит по дельте ветки относительно базовой ветки (обычно `master`).

> **Версия:** `11.0.0` · **ветка:** `release/11.0.0-new-license-improve-functionality` · **база:** `origin/master` (`v10.1.3`) · **дата:** `2026-09-12`
>
> **Релиз (если есть):** https://github.com/denis-peshkov/Cross.CQRS/releases/tag/v11.0.0
>
> **Легенда:** ⬜ open · ✅ done · 🟨 partial / принято · ❌ blocker
>
> **Предыдущий план:** —

Дельта: `origin/master...HEAD` — **74** коммита · **139** файлов · **+7809 / −415**. Чеклист publish/Go — [`RELEASE-PLAN-dev-to-master.md`](RELEASE-PLAN-dev-to-master.md); открытый backlog вне дельты — [`TO-DO.md`](TO-DO.md).

---

## Критично (безопасность)

---

## Высокий (логика / auth model)

---

## Средний (противоречия / баги контрактов)

---

## Низкий (техдолг / несогласованности)

### L4. `config.nuspec` `releaseNotes` дублирует docs

Длинный CDATA вместо короткой выжимки + ссылок на [`CHANGELOG.md`](CHANGELOG.md) / [`BREAKING.md`](BREAKING.md). В тексте ещё «netcoreapp3.1 dropped» (матрица тестов 3.1 сохранена) и маркетинг sibling-пакета **Cross.CQRS.EF** (отдельная репа) — убрать из nuspec notes.

---

## Принято (осознанный trade-off / контракт хоста)

- SampleWebApp / Tests: `CA2007` в `NoWarn` (host/test style); в библиотеке — `ConfigureAwait(false)`.
- Sibling EF-пакет (отдельный репозиторий): в core остаются только интеграционные хуки (`InternalsVisibleTo`, product claim / filter, pipeline −1) — не часть этого NuGet-описания.
- Матрица тестов держит **netcoreapp3.1**, чтобы гонять сборку библиотеки **netstandard2.1** (`SkipNetCoreApp31Tests` без x64 3.1 host).
- Лицензия опциональна: без ключа — правила «optional license» из README (не жёсткий fail без ключа).
- `CheckLicense` намеренно гоняет валидацию **на каждом** MediatR-запросе (`_licenseChecked` остаётся `false`) — не once-per-lifetime.
- Tag + NuGet Push только с `master` / `release/*` / `hotfix/*` / `dev` (не `feature`/`fix`/`chore`/PR).

---

## Закрыто (проверено в коде)

| # | Суть |
|---|------|
| ✅ #H1 LicenseCheck every request | accepted: by design — validate on every MediatR request |
| ✅ #L3 CA2007 library | `ConfigureAwait(false)` на await в библиотеке |
| ✅ #L2 NuGet publish secret | `NUGET_API_KEY` обновлён (ops); CI push больше не блокируется этим 403 |
| ✅ #L1 ReleaseNotes vs test TFMs | Notes: netcoreapp3.1 kept to exercise netstandard2.1 (not dropped) |
| ✅ JWT licensing pipeline | `LicenseKey`, `LicenseAccessor`, `LicenseValidator.Validate(license, ILicenseProductInfo)`, `LicenseCheckBehavior` (−2), product metadata DI |
| ✅ AddCQRS registration | `CqrsServiceConfiguration`; FluentValidation `AddValidatorsFromAssemblies` по полному набору сборок |
| ✅ TFMs / deps | `netstandard2.1;net6–net10`; Extensions.* по TFM |
| ✅ Solution / packaging | `Cross.CQRS.slnx`; `config.nuspec`; `_nuget` убран; `LICENSE.md` |
| ✅ Docs consumers | `docs/BREAKING.md` From 10.1.x→11.0.0 (layout OK); `docs/CHANGELOG.md` `## v11.0.0`; root `ReleaseNotes.md` → CHANGELOG |
| ✅ Tests NUnit | зоны Licensing / Registration / Behaviors / Queue / Core; матрица TFM + SkipNetCoreApp31 |
| ✅ CI tag/NuGet gates | `startsWith` для `release/*`/`hotfix/*`; ветки publish = master/release/hotfix/dev |
| ✅ Sonar key aligned | `projectKey=Cross.CQRS` в workflow + README/CONTRIBUTING badges |
| ✅ PR / issues templates | breaking → `docs/BREAKING.md`; placeholders `x.y.z`; legacy `ISSUE_TEMPLATE.md` удалён |
| ✅ Package description | nuspec + CI `-p:Description` без маркетинга sibling EF-пакета |

---

## Что в библиотеке уже нормально

- MediatR pipeline: license (−2) / reserved −1 / filters / validation согласованы с README.
- Публичный контракт breaking для апгрейда 10.1.x→11.0.0 описан только в `docs/BREAKING.md`.
- Local `dotnet build -c Release` и тесты net6–net10 зелёные (ранее в этой ветке); tip CI — подтвердить на HEAD.

---

## Приоритет фиксов

1. **L4** — укоротить `config.nuspec` `releaseNotes` + ссылки на CHANGELOG/BREAKING; убрать EF-маркетинг и «3.1 dropped».
2. Publish gate: CI/Sonar на **текущем HEAD**, SampleWebApp smoke, tag/`NUGET` — [`RELEASE-PLAN-dev-to-master.md`](RELEASE-PLAN-dev-to-master.md).
3. Open backlog вне дельты: [`TO-DO.md`](TO-DO.md) (C/H/M/L пусты).
