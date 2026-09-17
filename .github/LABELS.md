# Cross.CQRS — GitHub labels

Snapshot of configured repository labels. Machine-readable: [`labels.yml`](labels.yml).

Restore example:

```bash
gh label create "NAME" --color COLOR --description "DESC" --force
```

## Triage (ставит на PR автоматически)

Один **category** + один **priority** (если confidence ≥ 70).

### Categories

Сейчас в коде явной лестницы категорий нет — агент сам выбирает одну. По смыслу текущих правил triage логичный порядок такой (выше перебивает ниже):

| # | Категория | Почему выше
|---|---|---|
| 1 | security | Уже в prompt: утечки/auth/лицензия → всегда security + high/critical |
| 2 | bug | Регрессии и падения важнее «новой фичи» |
| 3 | feature | Новое поведение важнее косметики/docs |
| 4 | enhancement | Улучшение без major behavior change |
| 5 | docs | Только документация |
| 6 | chore | CI/tooling/deps без продуктового эффекта |

**Как читать смешанный PR:** берёшь все подходящие категории по диффу, оставляешь **самую верхнюю** из таблицы. Пример: docs + bugfix → `bug`; feature + workflow YAML → `feature`; только README + triage.yml → смотри объём/intent, обычно `docs` или `chore`.

Priority (`critical`...`low`) — **отдельная ось**, не путать с этой лестницей категорий.

---

### Приоритет — «насколько срочно»

| Лейбл | По-русски |
|---|---|
| `priority:critical` | Критично (блокер / срочно) |
| `priority:high` | Высокий |
| `priority:medium` | Средний |
| `priority:low` | Низкий |

---

## All labels

| Label | Color | Description | Triage |
|---|---|---|---|
| `bug` | `#e8372a` | Something is broken | category |
| `chore` | `#1d76db` | Build, CI, tooling, deps | category |
| `docs` | `#0075ca` | Documentation only | category |
| `documentation` | `#006b75` | Improvements or additions to documentation | — |
| `duplicate` | `#ffffff` | This issue or pull request already exists | — |
| `enhancement` | `#a2eeef` | Improvement without major behavior change | category |
| `feature` | `#0e8a16` | New capability or flow | category |
| `good first issue` | `#7057ff` | Good for newcomers | — |
| `help wanted` | `#008672` | Extra attention is needed | — |
| `invalid` | `#fef2c0` | This doesn't seem right | — |
| `priority:critical` | `#b60205` | Triage priority: critical | priority |
| `priority:high` | `#b60205` | Triage priority: high | priority |
| `priority:low` | `#fbca04` | Triage priority: low | priority |
| `priority:medium` | `#d93f0b` | Triage priority: medium | priority |
| `question` | `#d876e3` | Further information is requested | — |
| `security` | `#b60205` | Auth/JWT/OAuth, secrets, licensing, PII, payment, or token security | category |
| `wontfix` | `#080808` | This will not be worked on | — |

Note: GitHub also has `documentation` (manual/issues). Triage writes `docs`, not `documentation`.
`question` remains a **manual** GitHub label (e.g. issues); it is **not** a triage PR category.
