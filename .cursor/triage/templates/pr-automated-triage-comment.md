# PR Automated Triage Comment Template

Agent fills JSON fields; `format-pr-comment.mjs` renders this layout.

```markdown
> <img src="https://raw.githubusercontent.com/denis-peshkov/Cross.CQRS/master/icon.png" width="48" height="48" alt="Cross.CQRS"> **Cross.CQRS** · Automated triage by AI

## 🔍 Automated Triage

| | |
|---|---|
| ✨ **Category** | `{category}` |
| {priority_emoji} **Priority** | `{priority}` |
| 🎯 **Confidence** | {confidence}% |

### Summary

{summary}

{maintainer_hint_block}

<details>
<summary>📁 Relevant files</summary>

{relevant_files_list}

</details>

{security_block}

---
*Triaged automatically by [Cross.CQRS](https://github.com/denis-peshkov/Cross.CQRS) · [Cursor](https://cursor.com)* · This is an automated analysis, not a human review.
<!-- triage -->
```

## JSON schema (agent output)

```json
{
  "category": "feature|bug|enhancement|security|docs|chore|question",
  "priority": "critical|high|medium|low",
  "confidence": 85,
  "summary": "2-4 sentences in English.",
  "maintainerHint": "Optional one-line hint, e.g. simple fix / needs security review",
  "relevantFiles": ["Cross.CQRS/Licensing/LicenseValidator.cs"],
  "securityNotes": "Optional; omit if N/A"
}
```

## Category / priority rules (Cross.CQRS)

- **security** + **critical/high** for license JWT bypass, secret leak, pipeline tampering
- **bug** for broken pipeline/registration/tests
- **feature** for new behaviors/capabilities
- **enhancement** for refactors/perf without behavior change

## GitHub labels (CI)

After analysis, `post-pr-triage.mjs` syncs PR labels via `apply-pr-labels.mjs`:

| Field | Label |
|-------|--------|
| `category` | `feature` / `bug` / `enhancement` / `security` / `docs` / `chore` / `question` |
| `priority` | `priority:critical` / `priority:high` / `priority:medium` / `priority:low` |

Only these managed labels are added/removed; other PR labels are kept. Missing labels are created with `--force`.
