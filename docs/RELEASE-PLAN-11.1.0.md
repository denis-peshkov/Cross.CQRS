Ниже — **проблемы внутри библиотеки**, по уровню критичности. Аудит по дельте ветки относительно базовой ветки (обычно `master`).

> **Версия:** `11.1.0` · **ветка:** `hotfix/no-preview-git-tags` · **база:** `origin/master` (`v11.1.0` tag @ `d5ddc59` / merge #20) · **дата:** `2026-09-13`
>
> **Релиз (если есть):** https://github.com/denis-peshkov/Cross.CQRS/releases/tag/v11.1.0
>
> **Легенда:** ⬜ open · ✅ done · 🟨 partial / принято · ❌ blocker
>
> **Предыдущий план:** [`RELEASE-PLAN-11.0.0.md`](RELEASE-PLAN-11.0.0.md)
>
> Дельта: `origin/master...HEAD` — **0** коммитов (ветка ≡ master). Working tree: `.github/workflows/dotnet.yml`, `CONTRIBUTING.md` (+21 / −13). Open: **M8**, **L6**.

**CodeRabbit:** —

**PR:** —

---

## Критично (безопасность)

---

## Высокий (логика / auth model)

---

## Средний (противоречия / баги контрактов)

### M8. Tag `v11.1.0` уже на tip master (= merge #20 / продукт 11.0.0)
Локальный/remote tag `v11.1.0` указывает на `d5ddc59` (тот же commit, что и merge BREAKING 11.0.0). Нужно явно решить для publish: оставить как есть, поправить Release notes / NuGet story, или следующий ship — другой SemVer. Не трогать tag без ops-команды.

---

## Низкий (техдолг / несогласованности)

### L6. Закоммитить hotfix `no-preview-git-tags`
Изменения только в working tree: Create/Push git Tag gated by `!contains(env.semVer, '-')`; CONTRIBUTING обновлён. Нужен commit (+ PR/push) на `hotfix/no-preview-git-tags`.

---

## Принято (осознанный trade-off)

- Git tags только для **stable** SemVer (без `-preview` / `-dev` / …); NuGet по-прежнему может пушить pre-release пакеты с тех же eligible веток.
- Eligible branches для tag/NuGet без изменения списка: `master` / `release/*` / `hotfix/*` / `dev`.

---

## Закрыто (проверено в коде)

| # | Суть |
|---|------|
| ✅ CI git tag gate (WT) | `Create git Tag` / `Push git Tag`: `!contains(env.semVer, '-')` + branch allow-list (в working tree) |
| ✅ CONTRIBUTING tag policy (WT) | Документировано: stable tag only; release/hotfix/dev могут NuGet pre-release без git tag |

---

## Что в библиотеке уже нормально

- Продуктовая дельта 11.0.0 уже на `master` (PR #20); этот hotfix — только CI/docs tagging policy.
- Consumer `BREAKING.md` для 11.1.0 не требуется (нет API breaks).

---

## Приоритет фиксов

1. **L6** — commit hotfix на ветку.
2. **M8** — ops: что делать с уже существующим `v11.1.0` на commit 11.0.0.
3. Publish gate — [`RELEASE-PLAN-dev-to-master.md`](RELEASE-PLAN-dev-to-master.md).
