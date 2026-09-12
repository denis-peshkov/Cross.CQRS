# Cross.CQRS — open backlog (`TO-DO`)

Нерешённые пункты вне дельты version plan + кросс-версионные принятые trade-off’ы.

**Id high-water (не переиспользовать ≤):** `C0` `H0` `M0` `L3`

---

## Критично (безопасность)

---

## Высокий (логика / licensing)

---

## Средний (противоречия / баги контрактов)

---

## Низкий (техдолг / несогласованности)

---

## Принято (осознанный trade-off)

- SampleWebApp / Tests: `CA2007` в `NoWarn` (host/test style), library — `ConfigureAwait(false)`.
- Sibling EF-пакет (отдельный репозиторий): в core только интеграционные хуки (`InternalsVisibleTo`, product claim / filter, pipeline −1) — не часть NuGet description этого пакета.
- Test matrix keeps **netcoreapp3.1** to run against the library **netstandard2.1** build (`SkipNetCoreApp31Tests` for hosts without x64 3.1).
- Лицензия опциональна: без ключа — правила «optional license» из README.
- `CheckLicense` намеренно на **каждом** MediatR-запросе (`_licenseChecked` остаётся `false`).
- Tag + NuGet Push только с `master` / `release/*` / `hotfix/*` / `dev`.
