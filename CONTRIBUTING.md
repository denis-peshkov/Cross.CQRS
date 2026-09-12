# Contributing to Cross.CQRS

Thank you for your interest in the project.

## Quick links

- [Report an issue](https://github.com/denis-peshkov/Cross.CQRS/issues/new/choose)
- [Open PRs](https://github.com/denis-peshkov/Cross.CQRS/pulls)
- [CI (.NET)](https://github.com/denis-peshkov/Cross.CQRS/actions/workflows/dotnet.yml)
- [SonarCloud](https://sonarcloud.io/summary/new_code?id=Cross.CQRS)
- [NuGet](https://www.nuget.org/packages/Cross.CQRS/)
- [README](README.md)
- [Release notes](ReleaseNotes.txt)

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
| **Document** | README, `ReleaseNotes.txt`, `config.nuspec` release notes |

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
- `README.md`, `ReleaseNotes.txt`, `Cross.CQRS/config.nuspec`;
- CI: `.github/workflows/dotnet.yml`.

### Out of scope (without maintainer discussion)

- Large architecture refactors “for aesthetics”;
- New external dependencies without a strong reason;
- Consumer-breaking changes without release notes / README updates;
- Secrets, keys, `.env` in commits.

---

## Branches and releases

| Branch | Purpose |
|--------|---------|
| `master` | Stable line; release tags and NuGet publish |
| `feature/*` / `fix/*` / `chore/*` | Contributor work → PR |
| `release/*` / `hotfix/*` | Release / hotfixes (maintainer) |

Versioning: **GitVersion** (`GitVersion.yml`).

### Branch naming

Prefix + kebab-case:

```
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

---

## Pull request process

### 1. Preparation

```bash
git checkout master
git pull origin master
git checkout -b feature/short-description
```

### 2. Changes

- Follow existing folder layout (`Behaviors/`, `Licensing/`, `Extensions/`, …).
- Do not touch unrelated files.

### 3. Tests (required)

```bash
dotnet build Cross.CQRS.slnx
dotnet test Cross.CQRS.Tests/Cross.CQRS.Tests.csproj
```

### 4. Open PR

- Description: what, why, how to verify (**English**).
- For licensing / security — explicitly note risks.

### One PR rule

**One PR = one feature or one fix.** Split large changes.

---

## Pre-PR checklist

- [ ] Tests added/updated for changed behavior
- [ ] `dotnet test` — green locally
- [ ] No secrets in code or samples
- [ ] README / `ReleaseNotes.txt` / `config.nuspec` updated when the public surface changes

---

## Documentation

| What changed | Update |
|--------------|--------|
| Public API / registration | `README.md` |
| Released behavior | `ReleaseNotes.txt` and/or `Cross.CQRS/config.nuspec` `releaseNotes` |
| Packaging / dependencies | `Cross.CQRS/config.nuspec` |

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
