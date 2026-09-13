# Contributing to Cross.CQRS

Thank you for your interest in the project.

## Quick links

- [Report an issue](https://github.com/denis-peshkov/Cross.CQRS/issues/new/choose)
- [Open PRs](https://github.com/denis-peshkov/Cross.CQRS/pulls)
- [CI (.NET)](https://github.com/denis-peshkov/Cross.CQRS/actions/workflows/dotnet.yml)
- [CI (back-merge master → dev)](https://github.com/denis-peshkov/Cross.CQRS/actions/workflows/backmerge-master-to-dev.yml)
- [Branch policy](https://github.com/denis-peshkov/Cross.CQRS/actions/workflows/branch-policy.yml)
- [Triage](https://github.com/denis-peshkov/Cross.CQRS/actions/workflows/triage.yml)
- [SonarCloud](https://sonarcloud.io/summary/new_code?id=Cross.CQRS)
- [NuGet](https://www.nuget.org/packages/Cross.CQRS/)
- [README](README.md)
- [Release notes](docs/CHANGELOG.md) (shortcut: [ReleaseNotes.md](ReleaseNotes.md))
- Breaking changes: [`docs/BREAKING.md`](docs/BREAKING.md)
- Release readiness: [`docs/RELEASE-PLAN-dev-to-master.md`](docs/RELEASE-PLAN-dev-to-master.md)
- Open backlog: [`docs/TO-DO.md`](docs/TO-DO.md)

---

## What is Cross.CQRS?

**Cross.CQRS** is a NuGet library for .NET MediatR-based CQRS:

- Queries / Commands / CommandEvents and pipeline behaviors;
- FluentValidation, request/result filters;
- optional JWT licensing (`CqrsServiceConfiguration.LicenseKey`, `ILicenseProductInfo`);
- fluent registration via `services.AddCQRS(cfg => …)`.

Consumers call `AddCQRS` and send requests through MediatR (`IMediator` / `ISender`).

---

## How you can help

| Type | Examples |
|------|----------|
| **Report** | Bug with repro steps, expected/actual behavior, package version and TFM |
| **Fix** | Pipeline behavior regression, registration bug, licensing validation |
| **Build** | New filter/behavior, tests, SampleWebApp improvements |
| **Review** | PR review, especially licensing and DI registration |
| **Document** | README, `docs/CHANGELOG.md`, `docs/BREAKING.md`, release plans |

---

## Development principles

### Minimal diff

Do not mix refactoring, formatting untouched files, and a feature in one PR. Drive-by changes belong in a separate PR.

### Repository conventions

- `.editorconfig` — style source (UTF-8 BOM, CRLF, 4 spaces for `.cs`).
- `GlobalUsings.cs` — prefer shared usings; `ImplicitUsings` = `disable`.
- New `.cs` / `.csproj` / `.sln` / `.slnx` files — **UTF-8 with BOM**.
- Tests — **NUnit** + FluentAssertions; add/update tests with behavior changes.

### Licensing and secrets

Do not commit real license JWTs, private keys, or production secrets. Prefer placeholders in samples and issues.

---

## In scope / out of scope

### In scope

- `Cross.CQRS/` — library (commands, queries, events, behaviors, licensing, DI);
- `Cross.CQRS.Tests/` — unit / pipeline / licensing tests;
- `SampleWebApp/` — smoke host example;
- `README.md`, `docs/CHANGELOG.md`, `docs/BREAKING.md`, `Cross.CQRS/config.nuspec`;
- CI: `.github/workflows/dotnet.yml`, `branch-policy.yml`, `triage.yml`, `backmerge-master-to-dev.yml`.

### Out of scope (without maintainer discussion)

- Large architecture refactors “for aesthetics”;
- New external dependencies without a strong reason;
- Consumer-breaking changes without a `docs/BREAKING.md` entry;
- Secrets, keys, `.env` in commits.

---

## Branches and releases

| Branch | Purpose | Who |
|--------|---------|-----|
| `master` | Stable release; GitVersion, **stable** git tag (`vX.Y.Z`), NuGet push | **Owner only** — direct push and PRs |
| `release/*` | Release preparation; NuGet (may be `-preview.*`); **no** git tag for pre-releases | **Owner only** |
| `hotfix/*` | Urgent production patches; same tag/NuGet rules as `release/*` | **Owner only** |
| `dev` | Feature integration; NuGet pre-release (`-dev.*`); **never** creates git tags | **Default PR target** for contributors |
| `feature/*` | New functionality (build/test only — no tag/NuGet) | Contributors |
| `fix/*` | Bug fixes (build/test only — no tag/NuGet) | Contributors |
| `chore/*` | CI, deps, docs-only, maintenance (no tag/NuGet) | Contributors |

Git tags are created only for **stable** `X.Y.Z` (no `-` in `semVer`) on `master` / `release/*` / `hotfix/*`. **`dev` never creates git tags**; it may still push NuGet pre-release packages.

**Access rules (enforced in CI via `.github/workflows/branch-policy.yml`):**

- Contributors open PRs **only into `dev`** from `feature/*`, `fix/*`, or `chore/*`.
- PRs targeting **`master`** — repository owner only (`denis-peshkov`).
- Pushing to **`master`**, **`release/*`**, or **`hotfix/*`** — owner only.
- After changes land on **`master`**, CI (`backmerge-master-to-dev.yml`) merges `master` into `dev` using secret **`TAGTOKEN`**.

Optional GitHub Rulesets: import recipes from [`.github/rulesets/`](.github/rulesets/).

Versioning: **GitVersion** (`GitVersion.yml`). `dev` is pre-release (`-dev.N`). `commit-message-incrementing: Disabled`.

### Branch naming

Prefix + kebab-case:

```
release/11.0.0-short-name
hotfix/critical-license-check
feature/license-product-info-docs
fix/validation-behavior-order
chore/editorconfig-and-templates
```

---

## Commit messages

Use **clear English** messages in imperative/descriptive style:

```
Add LicenseCheckBehavior coverage tests
Fix FluentValidation scan for multiple assemblies
Update README licensing section
```

For breaking changes, include `BREAKING:` in the commit body or PR title/description.

---

## Pull request process

### 1. Preparation

```bash
git checkout dev
git pull origin dev
git checkout -b feature/short-description
```

### 2. Changes

- Follow existing folder layout (`Behaviors/`, `Licensing/`, `Extensions/`, …).
- Do not touch unrelated files.
- Breaking change → `docs/BREAKING.md` only (nuspec keeps a link, not a duplicate list).

### 3. Tests (required)

```bash
dotnet build Cross.CQRS.slnx
dotnet test Cross.CQRS.Tests/Cross.CQRS.Tests.csproj
```

### 4. Open PR

- **Base branch:** `dev` (required for contributors)
- Description: what, why, how to verify (**English** — for GitHub history and the triage bot).
- For licensing / security — explicitly note risks.
- Breaking consumer change → prefix title with `BREAKING:`.

### 5. CI

Must pass:

- `.NET` workflow (build + tests)
- Branch policy (`branch-policy.yml`)
- SonarCloud quality gate (on PR)
- Triage PR comment job when enabled (`CURSOR_API_KEY`)

CodeRabbit (`.coderabbit.yaml`): comment `@coderabbitai full review` when a full pass is needed.

### One PR rule

**One PR = one feature or one fix.** Split large changes.

---

## Pre-PR checklist

- [ ] Tests added/updated for changed behavior
- [ ] `dotnet test` — green locally
- [ ] No secrets in code or samples
- [ ] README / `docs/CHANGELOG.md` / `docs/BREAKING.md` / `config.nuspec` updated when the public surface changes

---

## Documentation

| What changed | Update |
|--------------|--------|
| Public API / registration | `README.md` |
| Breaking change for consumers | `docs/BREAKING.md` only |
| Released behavior | **`docs/CHANGELOG.md` (always on release work)** and short `config.nuspec` `releaseNotes` (+ link to BREAKING) |
| Packaging / dependencies | `Cross.CQRS/config.nuspec` |
| Release readiness | `docs/RELEASE-PLAN-*.md` |
| Deferred findings | `docs/TO-DO.md` |

---

## Security

For issues/PRs on licensing or pipeline security, include package version and a minimal repro.

**Do not publish** real license keys or live tokens in issues/PRs.

---

## License

Code is under [RPL 1.5](LICENSE.md) (Reciprocal Public License). By contributing, you agree that derivative works are distributed under the same terms, or under a [Peshkov commercial license](https://peshkov.biz/license).

There is no separate CLA — merging a PR means agreement with the repository license.

---

## Questions?

- Bugs and features: [GitHub Issues](https://github.com/denis-peshkov/Cross.CQRS/issues)

**Thank you for contributing to Cross.CQRS.**
