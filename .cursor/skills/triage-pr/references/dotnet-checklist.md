# Cross.CQRS PR Review Checklist

Use for deep review of PRs in `triage-pr` and `bugbot`.

## Security & Licensing (critical)

- No logging of real license JWTs, private keys, or production secrets
- License validation: product name (`Cross.CQRS` / `Cross.CQRS.EF`), product type, edition, dates
- `LicenseCheckBehavior` order and reserved slot for Cross.CQRS.EF
- Input validation (FluentValidation) stays aligned with registered assemblies
- See `docs/BREAKING.md` when changing public licensing / registration surface

## .NET & Code Style

- `Nullable enable`, `Async` suffix on async methods
- `GlobalUsings.cs` in projects; `ImplicitUsings` = `disable`
- UTF-8 with BOM for `.cs`, `.csproj`, `.sln` / `.slnx`
- Follow `.editorconfig`
- Library awaits: `ConfigureAwait(false)` (CA2007); SampleWebApp / Tests may NoWarn CA2007

## Pipeline & Registration

- Behavior order: license → (EF reserved) → filters / validation / event queue as designed
- `AddCQRS` / `CqrsServiceConfiguration`: assemblies, validators, filters, optional `LicenseKey`
- Command / Query / CommandEvent contracts remain MediatR-compatible

## Tests

- New behavior covered in `Cross.CQRS.Tests/` (zones: `Licensing/`, `Registration/`, `Behaviors/`, `Queue/`, `Core/`, `Coverage/`)
- Prefer clear Given/When/Then style names; async tests end with `Async`
- Run: `dotnet test Cross.CQRS.Tests/Cross.CQRS.Tests.csproj`
- **Do not** require XML `/// <summary>` on test methods

## Breaking Changes

- Public NuGet API / registration / licensing — semver impact → `docs/BREAKING.md`
- Update `docs/CHANGELOG.md` and keep `config.nuspec` `releaseNotes` as a short summary + link, not a full duplicate list
