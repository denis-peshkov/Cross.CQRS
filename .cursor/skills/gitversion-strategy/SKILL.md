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

0. **КРИТИЧНО — править только `GitVersion.yml`.**
   Любые SemVer-цели достигаются **исключительно** конфигом GitVersion.
   - **Запрещено:** трогать `run-matrix.mjs`; CI/workflow; `/overrideconfig`; fallback/retry; эмуляция CI в матрице; `+semver` / любые трюки с commit messages; `commit-message-incrementing: Enabled` ради обхода.
   - `commit-message-incrementing: Disabled` — всегда.
   - `scripts/run-matrix.mjs` только измеряет YAML на фикстурах → [`templates/MATRIX-REPORT.md`](templates/MATRIX-REPORT.md). FAIL = честный результат YAML.
1. **YAML-ключи GV6 built-in:** `main`, `develop` (не `master` / `dev`).
   Regex может матчить git-ветки `master` и `dev`. В `source-branches` — имена **ключей** (`main`, `develop`).
2. **Не гонять `dotnet-gitversion` на полном клоне** без нужды — зависает. Только fixture через скрипт.
3. Не коммитить / не пушить GitHub без явной команды пользователя.
4. Фикстуры класть под `.tmp-gvfind/` (уже в ignore / локальный мусор).
5. **Отчёт матрицы:** форма — шаблон; цифры — stdout `run-matrix.mjs` as-is. Не править скрипт/сообщения коммитов ради цифр.

## Целевая матрица (default desired)

Пока пользователь не задал иное:

| Source | На ветке | На `master` |
|---|---|---|
| `release/*` (цифры в имени игнор) | Minor + `-preview.N` | **Minor** MMP (squash **и** merge) |
| `hotfix/*` | Patch + `-preview.N` | Patch MMP |
| `dev` | manual `next-version` (matrix: `11.3.3-dev.N`) | MMP без `-dev` |
| direct push на `master` | — | **Patch** после тега; N коммитов = один номер |

`commit-message-incrementing: Disabled`.  
Если YAML этого не даёт — в отчёте будет фактический SemVer или `FAIL`, не «желаемое».

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
| release squash+merge = Minor | `main.increment: Inherit` + `source-branches: [release, hotfix]` (**без** `develop`) |
| hotfix = Patch | `hotfix.increment: Patch` + Inherit |
| direct push = Patch | только через YAML (без CI/fallback/+semver); сейчас конфликтует с release Minor при orphaned master |
| `label:` (не GV5 `tag:`) | `preview` / `dev` / `''` на main |
| стабильный MMP без `-1` | `main.mode: ContinuousDeployment`, `label: ''`, `when-current-commit-tagged: true` |

`commit-message-incrementing: Disabled`. Никакого `+semver`.

### Phase 3 — Прогон

Из корня репо:

```bash
node .cursor/skills/gitversion-strategy/scripts/run-matrix.mjs --config GitVersion.yml --base-tag v10.1.3
```

Только YAML → шаблон. Никаких override.

Нужен `dotnet-gitversion` (`GitVersion.Tool`). Запуск вне sandbox, если `git init` в фикстурах падает.

### Phase 4 — Отчёт пользователю

Скопировать **весь** stdout. Структура: [`templates/MATRIX-REPORT.md`](templates/MATRIX-REPORT.md).

### Phase 5 — Сверка с целью

Одной строкой: совпало / не совпало. Если нет — править `GitVersion.yml`, снова Phase 3 (не трогать скрипт ради цифр).

## Args (пользователь / чат)

| Arg | Meaning |
|---|---|
| `base vX.Y.Z` | `--base-tag` |
| `config PATH` | другой yaml |
| `compare` | прогнать 2+ кандидата, показать оба отчёта |
| `apply` | записать выбранный конфиг в `GitVersion.yml` (без commit) |
