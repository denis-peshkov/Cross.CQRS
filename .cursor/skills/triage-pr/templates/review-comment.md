# PR Review Comment Template — Cross.CQRS

Comments in **English**.

```markdown
## Review

**Scope**: Security (auth/JWT/tokens), .NET quality, process engine flows, test coverage

### Summary

{1-2 sentences — main takeaway.}

### Critical Issues 🔴

{- `Cross.CQRS/Path/File.cs:42` — problem, impact, suggested fix.}

{If none: "None found."}

### Important Issues 🟠

{Significant issues with file:line citations.}

{If none: "None found."}

### Suggestions 🟡

{Nice-to-haves. Omit section if none.}

### What's Good ✅

{At least one specific positive point.}

---
*Automated review via [Cross.CQRS](https://github.com/denis-peshkov/Cross.CQRS) Cursor `/triage-pr`*
```

## Severity

- 🔴 Critical: security (token leak, auth bypass), data loss, broken auth flow, missing tests for security fix
- 🟠 Important: error handling gaps, breaking public API without docs, missing flow tests
- 🟡 Suggestion: naming, DRY, documentation

## Cross.CQRS checks (mention when relevant)

- No logging of passwords/tokens/codes (see `105-backend-security.mdc`)
- License JWT / pipeline security
- `docs/BREAKING.md` / README updated for public API changes
- `Cross.CQRS.Tests` coverage for new behavior
- `Nullable enable`, `Async` suffix, `.editorconfig`

**Tone**: professional, constructive. 200–400 words.
