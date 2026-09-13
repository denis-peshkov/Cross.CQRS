База: **`{{BASE_TAG}}`**. Merge = `--no-ff`. Колонка **push** = +1 коммит на **source**-ветке.

| Ветка (source) | SemVer на source | после push (+1) | после squash | после merge |
|---|---|---|---|---|
{{BRANCH_ROWS}}

**Direct push в `master`:**

| Состояние | SemVer |
|---|---|
| на теге `{{BASE_TAG}}` | `{{DIRECT_AT_TAG}}` |
| push #1 = 1 коммит | `{{DIRECT_PUSH1_ONE}}` |
| push #1 = 10 коммитов | `{{DIRECT_PUSH1_TEN}}` |
| push #2 = 1 коммит (после CI-тега `v{{DIRECT_PUSH1_ONE}}`) | `{{DIRECT_PUSH2}}` |
