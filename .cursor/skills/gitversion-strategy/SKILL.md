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
5. **Отчёт матрицы:** источник структуры — [`templates/MATRIX-REPORT.md`](templates/MATRIX-REPORT.md).
   `scripts/run-matrix.mjs` меряет фикстуры по `GitVersion.yml` и **подставляет** placeholders.
   - Улучшать коллектор/фикстуры — можно, когда меняется контракт прогона.
   - Форму отчёта менять в **шаблоне**, не хардкодить в чате.
   - Не переписывать скрипт каждый раз ради «нужных» цифр — править `GitVersion.yml` и прогнать снова.
   - Ответ пользователю = **stdout скрипта as-is**.

## Целевая матрица (default desired)

Пока пользователь не задал иное:

| Source | На ветке | На `master` |
|---|---|---|
| `release/*` (цифры в имени игнор) | Minor + `-preview.N` | Minor MMP (**merge `--no-ff`**); squash может отличаться |
| `hotfix/*` | Patch + `-preview.N` | Patch MMP |
| `dev` | manual `next-version` (matrix: `11.3.3-dev.N`) | MMP без `-dev` |
| direct push на `master` | — | **Patch** после тега; N коммитов в одном push = один номер |

`commit-message-incrementing: Disabled`.

## Workflow

### Phase 1 — Считать цель

Спросить / взять из чата:

- игнор цифр в `release/*`? (default: да)
- hotfix на master: Patch или как Minor?
- label: `preview` / `pre` / `rc` (default: `preview`)
- base tag для прогона (default: `v10.1.3`)

### Phase 2 — Кандидат конфига

Править **только** `GitVersion.yml` (или временную копию).

| Цель | Рычаг |
|---|---|
| игнор цифр в `release/11.0.0-…` | без `VersionInBranchName`; stub `version-in-branch-pattern`; `track-merge-message: false` |
| hotfix / direct push = Patch на master | `main.increment: Inherit` + `develop.increment: Patch` |
| release = Minor на master | тот же `Inherit` + **merge `--no-ff`** (`release.increment: Minor`); squash часто даёт Patch |
| `label:` (не GV5 `tag:`) | `preview` / `dev` / `''` на main |
| стабильный MMP без `-1` | `main.mode: ContinuousDeployment`, `label: ''`, `when-current-commit-tagged: true` |

`commit-message-incrementing: Disabled` — без `+semver` в сообщениях.  
Чистым YAML нельзя одновременно: squash release→Minor **и** direct push→Patch при живом `dev`.

### Phase 3 — Прогон

Из корня репо:

```bash
node .cursor/skills/gitversion-strategy/scripts/run-matrix.mjs --config GitVersion.yml --base-tag v10.1.3
```

Скрипт меряет фикстуры → заполняет шаблон отчёта. Цифры в ячейках — только из прогона.

Нужен `dotnet-gitversion` (`GitVersion.Tool`). Запуск вне sandbox, если `git init` в фикстурах падает.

### Phase 4 — Отчёт пользователю

Скопировать **весь** stdout. Структура: [`templates/MATRIX-REPORT.md`](templates/MATRIX-REPORT.md) (как `release-plan` → `templates/RELEASE-PLAN.md`).

### Phase 5 — Сверка с целью

Одной строкой: совпало / не совпало. Если нет — править `GitVersion.yml`, снова Phase 3.

## Args (пользователь / чат)

| Arg | Meaning |
|---|---|
| `base vX.Y.Z` | `--base-tag` |
| `config PATH` | другой yaml |
| `compare` | прогнать 2+ кандидата, показать оба отчёта |
| `apply` | записать выбранный конфиг в `GitVersion.yml` (без commit) |
