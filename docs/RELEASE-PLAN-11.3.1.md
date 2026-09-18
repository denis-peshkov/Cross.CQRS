Ниже — **проблемы внутри библиотеки**, по уровню критичности. Аудит по дельте ветки относительно базовой ветки (обычно `master`).

> **Версия:** `11.3.1` (closed) · **ветка:** `master` · **база:** `origin/master` (`v11.3.0`) · **дата:** `2026-09-18`
>
> **Релиз (если есть):** https://github.com/denis-peshkov/Cross.CQRS/releases/tag/v11.3.1
>
> **Легенда:** ⬜ open · ✅ done · 🟨 partial / принято · ❌ blocker
>
> **Предыдущий план:** [RELEASE-PLAN-11.3.0.md](RELEASE-PLAN-11.3.0.md)
>
> Дельта: `origin/master...HEAD` — **0** коммита · **0** файлов · **+0 / −0**. WT: **10** файлов · **+60 / −42**. Open C/H/M/L пустые (план закрыт).

**CodeRabbit:**
- не запускался.

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

- На tagged `master` (`v11.3.0`) GitVersion `MajorMinorPatch` ещё **11.3.0**; Patch **11.3.1** появится после commit поверх tag.
- Дельта этого плана — незакоммиченный tooling (не library); consumer BREAKING нет.

---

## Закрыто (проверено в коде)

| # | Суть |
|---|---|
| ✅ 11.3.0 published/closed | local tag `v11.3.0`; шапка предыдущего плана |
| ✅ pr-message discovery | build/test из PR template / discovery; без хардкода sln/csproj |
| ✅ release-plan Skip flag | `-p:SkipNetCoreApp31Tests=true` только если свойство есть в test csproj |
| ✅ changelog/triage tests | generic sample paths в `categorizePath` / `load-review-rules` |
| ✅ README / 101-cqrs wording | нет assume `db-scripts`; пайплайн CQRS без product-specific |
| ✅ tests net6–net10 | 45 passed, `-p:SkipNetCoreApp31Tests=true` |
| ✅ CHANGELOG v11.3.1 | `update-changelog.mjs --write`; секция уточнена |
| ✅ to-master retarget | чеклист → `11.3.1`; Change summary пересчитан |

---

## Что в библиотеке уже нормально

- Library / Sample / TFMs / BREAKING 11.2.x→11.3.0 в этой дельте не трогались.
- `SkipNetCoreApp31Tests` в test csproj по-прежнему есть — флаг в skill остаётся уместным.

---

## Приоритет фиксов

_(пусто — релиз `11.3.1` закрыт; открытый backlog → [`TO-DO.md`](TO-DO.md).)_
