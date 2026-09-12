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
- Cross.CQRS.EF — отдельный репозиторий/пакет; в core только `InternalsVisibleTo` + product filter `Cross.CQRS.EF`.
- Test matrix keeps **netcoreapp3.1** to run against the library **netstandard2.1** build (`SkipNetCoreApp31Tests` for hosts without x64 3.1).
- Лицензия опциональна: без ключа — правила «optional license» из README.
- `CheckLicense` намеренно на **каждом** MediatR-запросе (`_licenseChecked` остаётся `false`).
