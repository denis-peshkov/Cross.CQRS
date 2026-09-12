# Cross.CQRS — open backlog (`TO-DO`)

Нерешённые пункты вне дельты version plan.

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

---

## Закрыто (проверено в коде)

| # | Суть |
|---|------|
| ✅ L3 CA2007 library | ConfigureAwait(false) на await в библиотеке |
| ✅ L2 NuGet publish secret | `NUGET_API_KEY` обновлён (ops, 2026-09-12); CI push больше не блокируется этим 403 |
| ✅ L1 ReleaseNotes vs test TFMs | Notes: netcoreapp3.1 kept to exercise netstandard2.1 (not dropped) |
