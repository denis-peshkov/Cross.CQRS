---
name: gitversion-strategy
description: >-
  Подбирает стратегию GitVersion 6.x (GitVersion.yml) под целевое поведение
  веток release/hotfix/dev/master и прогоняет fixture-матрицу. Использовать при
  правке GitVersion.yml, выборе increment/mode/label, проверке squash/merge/push
  версий, игнорировании цифр в release/*, или когда пользователь просит прогон /
  отчёт SemVer по веткам.
---

# GitVersion strategy

## Когда использовать

- Правка / подбор `GitVersion.yml` (GV **6.x**, CI `versionSpec` ≥ 6.8.2)
- Нужен отчёт SemVer: source → squash / merge / push + direct push на `master`
- Цели вроде: цифры в `release/11.0.0-…` как у `release/test`; hotfix = Patch на master; direct push на master = **Patch**

## Жёсткие правила репо

1. **YAML-ключи GV6 built-in:** `main`, `develop` (не `master` / `dev`).
   Regex может матчить git-ветки `master` и `dev`. В `source-branches` — имена **ключей** (`main`, `develop`).
2. **Не гонять `dotnet-gitversion` на полном клоне** без нужды — зависает. Только fixture через скрипт.
3. Не коммитить / не пушить GitHub без явной команды пользователя.
4. Фикстуры класть под `.tmp-gvfind/` (уже в ignore / локальный мусор).

## Целевая матрица (default desired)

Пока пользователь не задал иное, целевое поведение:

| Source | На ветке | На `master` (squash/merge/push) |
|---|---|---|
| `release/*` (любое имя, в т.ч. с цифрами) | next **Minor** + `-preview.N` | тот же MMP **без** pre-release |
| `hotfix/*` | next **Patch** + `-preview.N` | **Patch** MMP без pre-release |
| `dev` | next **manual** (`next-version`, matrix: `11.3.3-dev.N`) | MMP без `-dev` (Inherit) |
| direct push на `master` | — | на теге = tag; +N коммитов = один **Patch** вверх, SemVer стабилен до нового тега (`ContinuousDeployment`) |

Конфликт: `main.increment: Patch` → hotfix/direct push Patch OK, но **release→master тоже Patch** (Minor с release теряется).
`main.increment: Inherit` → release Minor / hotfix Patch на squash; direct push находит increment у source-веток по истории/refs (в реальном клоне после fetch это обычно уже так).
`main.increment: Minor` → ломает hotfix→Patch.
Подбор = явный trade-off; зафиксировать в ответе.

## Workflow

### Phase 1 — Считать цель

Спросить / взять из чата:

- игнор цифр в `release/*`? (default: да)
- hotfix на master: Patch или как Minor?
- label: `preview` / `pre` / `rc` (default: `preview`)
- base tag для прогона (default: `v10.1.3`)

### Phase 2 — Кандидат конфига

Править только `GitVersion.yml` (или временную копию для сравнения). Ориентиры:

| Цель | Рычаг |
|---|---|
| Игнор цифр в имени ветки | Убрать `VersionInBranchName` из `strategies`; `version-in-branch-pattern` → заглушка; `track-merge-message: false` на `main`/`release`/`hotfix` |
| release auto-Minor | `release.increment: Minor`, `label: preview` |
| hotfix Patch на ветке | `hotfix.increment: Patch`, `label: preview` |
| hotfix Patch **на master** | `main.increment: Patch` или `Inherit` + `of-merged-branch: true` |
| master direct push = Patch | `main.increment: Patch` (не Inherit-from-develop) |
| master direct push = Minor | `main.increment: Minor` (ломает hotfix→Patch) |
| release Minor **на master** | `main.increment: Inherit` (не `Patch`/`Minor` на main) |
| стабильный SemVer без `-1` | `main.mode: ContinuousDeployment`, `label: ''`, `when-current-commit-tagged: true` |
| `dev` label | ключ `develop`, `label: dev`, `mode: ContinuousDelivery` |

Детали / грабли: [reference.md](reference.md).

### Phase 3 — Прогон

Из корня репо:

```bash
python3 .cursor/skills/gitversion-strategy/scripts/run-matrix.py --config GitVersion.yml --base-tag v10.1.3
```

Сравнение двух конфигов:

```bash
python3 .cursor/skills/gitversion-strategy/scripts/run-matrix.py --config /tmp/candidate.yml
python3 .cursor/skills/gitversion-strategy/scripts/run-matrix.py --config GitVersion.yml --json
```

Нужен `dotnet-gitversion` (global tool `GitVersion.Tool`).
Запуск скрипта — с полными правами на `git init` в фикстурах (sandbox иначе падает на `.git/config`).

### Phase 4 — Отчёт пользователю

**Всегда** отдавать прогон **целиком** в формате ниже (без урезания колонок).
Если подобрали стратегию — кратко: какой trade-off, затем таблицы.

Скопировать вывод скрипта as-is. Канон:

```markdown
База: **`v10.1.3`**. Merge = `--no-ff`. Колонка **push** = +1 коммит на **source**-ветке. Отдельно — Direct push в `master`.

| Ветка (source) | SemVer на source | после push (+1) | после squash | после merge |
|---|---|---|---|---|
| `release/11.0.0-new` | `10.2.0-preview.1` | `10.2.0-preview.2` | `10.2.0` | `10.2.0` |
| `release/test-12-new` | `10.2.0-preview.1` | `10.2.0-preview.2` | `10.2.0` | `10.2.0` |
| `release/test` | `10.2.0-preview.1` | `10.2.0-preview.2` | `10.2.0` | `10.2.0` |
| `hotfix/test` | `10.1.4-preview.1` | `10.1.4-preview.2` | `10.1.4` | `10.1.4` |
| `dev` | `11.3.3-dev.1` | `11.3.3-dev.2` | `11.3.3` | `11.3.3` |

**Direct push в `master`:** (Patch один раз после тега; следующий Patch — после CI-тега. N коммитов в одном push = один номер.)

| Состояние | SemVer |
|---|---|
| на теге `v10.1.3` | `10.1.3` |
| push #1 = 1 коммит | `10.1.4` |
| push #1 = 10 коммитов | `10.1.4` |
| push #2 = 1 коммит (после CI-тега `v10.1.4`) | `10.1.5` |
```

Числа в примере — иллюстрация целевой матрицы; в ответе подставлять **фактический** вывод `run-matrix.py`.

### Phase 5 — Сверка с целью

После прогона одной строкой:

- совпало / не совпало по hotfix→master, release digits, direct push;
- если не совпало — следующий рычаг из Phase 2 (не гадать без нового прогона).

## Args (пользователь / чат)

| Arg | Meaning |
|---|---|
| `base vX.Y.Z` | `--base-tag` |
| `config PATH` | другой yaml |
| `compare` | прогнать 2+ кандидата, показать оба отчёта |
| `apply` | записать выбранный конфиг в `GitVersion.yml` (без commit) |
