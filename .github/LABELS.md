# Cross.CQRS — лейблы GitHub

Снимок настроенных лейблов репозитория. Машиночитаемый файл: [`labels.yml`](labels.yml).

Восстановление:

```bash
gh label create "NAME" --color COLOR --description "DESC" --force
```

## Triage (ставит на PR автоматически)

Один **category** + один **priority** (если confidence ≥ 70).

### Categories

Сейчас в коде явной лестницы категорий нет — агент сам выбирает одну. По смыслу текущих правил triage логичный порядок такой (выше перебивает ниже):

| # | Категория | Почему выше |
|---|---|---|
| 1 | security | Уже в prompt: утечки/auth/лицензия → всегда security + high/critical |
| 2 | bug | Регрессии и падения важнее «новой фичи» |
| 3 | feature | Новое API/поведение важнее polish/docs |
| 4 | enhancement | Polish / perf / DX без нового публичного контракта |
| 5 | documentation | Документация |
| 6 | chore | CI/tooling/deps без продуктового эффекта |

**Как читать смешанный PR:** берёшь все подходящие категории по диффу, оставляешь **самую верхнюю** из таблицы. Пример: documentation + bugfix → `bug`; feature + workflow YAML → `feature`; только README + triage.yml → смотри объём/intent, обычно `documentation` или `chore`.

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

| Label | Color | Description (GitHub) | По-русски | Triage |
|---|---|---|---|---|
| `bug` | `#e8372a` | Something is broken | Что-то сломано / дефект | category |
| `chore` | `#1d76db` | Build, CI, tooling, deps | Сборка, CI, tooling, зависимости | category |
| `docs` | `#0075ca` | Documentation only | Только документация | — (legacy) |
| `documentation` | `#006b75` | Improvements or additions to documentation | Доработки / дополнения документации | category |
| `duplicate` | `#ffffff` | This issue or pull request already exists | Такой issue/PR уже есть | — |
| `enhancement` | `#a2eeef` | Polish, perf, or DX without new public contract | Polish / perf / DX без нового публичного контракта | category |
| `feature` | `#0e8a16` | New API or behavior | Новое API или поведение | category |
| `good first issue` | `#7057ff` | Good for newcomers | Подходит для новичков | — |
| `help wanted` | `#008672` | Extra attention is needed | Нужна помощь со стороны | — |
| `invalid` | `#fef2c0` | This doesn't seem right | Некорректно / не по делу | — |
| `priority:critical` | `#b60205` | Triage priority: critical | Приоритет triage: критично | priority |
| `priority:high` | `#d93f0b` | Triage priority: high | Приоритет triage: высокий | priority |
| `priority:low` | `#fbca04` | Triage priority: low | Приоритет triage: низкий | priority |
| `priority:medium` | `#fb8500` | Triage priority: medium | Приоритет triage: средний | priority |
| `question` | `#d876e3` | Further information is requested | Нужны уточнения / вопрос | — |
| `security` | `#5319e7` | 🔒 Auth/JWT/OAuth, secrets, licensing, PII, payment, or token security | Безопасность: auth, секреты, лицензия, ПДн и т.п. | category |
| `wontfix` | `#080808` | This will not be worked on | Не будем делать | — |

Note: triage пишет **`documentation`**. Лейбл `docs` на GitHub — legacy; при sync PR старый `docs` снимается.
`question` остаётся **ручным** лейблом GitHub (например на issues); это **не** категория PR-triage.
