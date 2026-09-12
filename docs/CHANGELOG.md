# Changelog — Cross.CQRS

Newest releases first. GitHub releases: <https://github.com/denis-peshkov/Cross.CQRS/releases>

Breaking upgrade notes for NuGet consumers: [`BREAKING.md`](BREAKING.md).

---

## v11.0.0 — 28 Mar 2026

### Licensing

- Optional JWT license key on `CqrsServiceConfiguration`; `LicenseAccessor` exposes a cached `License` built from claims (subscription, user, edition, product type, dates).
- `LicenseValidator.Validate(license, ILicenseProductInfo)` with default `LicenseProductInfo` (`Cross_CQRS` / `Cross_CQRS_EF`); extensions can register additional `ILicenseProductInfo` (e.g. Cross.CQRS.EF for stricter EF SKU rules).
- `CheckLicense` resolves all `ILicenseProductInfo` from DI and runs validation for each product line.
- `LicenseCheckBehavior` runs on every MediatR request; behavior order reserves a slot for Cross.CQRS.EF integration.
- `InternalsVisibleTo` Cross.CQRS.EF for shared licensing types consumed by the EF package.

### Registration and MediatR pipeline

- FluentValidation: `AddValidatorsFromAssemblies` uses the full assembly set from `CqrsServiceConfiguration` (handlers, validators, filters stay aligned).
- `AsyncRequestHandlerBase` removed; `CommandHandler<TCommand>` implements `IRequestHandler<TCommand>` (MediatR `Unit`) directly.

### Target frameworks and dependencies

- Libraries: `netstandard2.1`, `net6.0`, `net7.0`, `net8.0`, `net9.0`, `net10.0`.
- Microsoft.Extensions.Configuration / Logging versions vary by TFM (8.x on netstandard2.1 and net6–8, 9.0.14 on net9.0, 10.0.5 on net10.0), matching the project file.

### Solution, packaging, and repo

- Solution format `Cross.CQRS.slnx` replaces `Cross.CQRS.sln`.
- NuGet metadata in `Cross.CQRS/config.nuspec` with dependency groups per TFM; legacy `_nuget` scripts folder removed.
- GitHub Actions workflow, GitVersion.yml, README, LICENSE, and SampleWebApp updated for the new layout and targets.

### Tests

- Cross.CQRS.Tests coverage expanded: licensing, `CqrsServiceConfiguration`, registration and pipeline behavior, command/event queue, coverage-oriented tests.
- Test TFMs: `netcoreapp3.1` (exercises the library’s `netstandard2.1` asset), plus `net6.0`–`net10.0`; `SkipNetCoreApp31Tests` skips 3.1 when the host has no x64 3.1 runtime (e.g. Apple Silicon).

---

## v10.1.4 — 10 Sep 2025

1. Logging improvements:
   - Enhanced log message formatting (changed “for a {Elapsed} ms” to “in {Elapsed} ms”)
   - Optimized work with Stopwatch
   - Improved internal logging
2. Dependency updates:
   - FluentValidation upgraded to version 11.11.0
3. Architectural changes:
   - Removed `AsyncRequestHandlerBase` class
   - `CommandHandler<TCommand>` now directly implements `IRequestHandler<TCommand>`
4. Code optimizations:
   - Simplified some methods
   - Improved code formatting
   - Refactoring for cleaner architecture

---

## v10.1.3 — 14 Apr 2025

- Fix issue with logging of Command, EventCommand and Query objects.
- Optimize work with Stopwatch.
- Improve internal logging.

---

## v10.1.2 — 11 Apr 2025

- Fix log value for Command, Event, Query.

---

## v10.1.1 — 17 Mar 2025

- Fix log field name.

---

## v10.1.0 — 13 Mar 2025

- Make `ObjectExtensions.GetObjectSize()` safe for some cases.

---

## v10.0.1 — 13 Mar 2025

- Added `ILogger` into `CommandEventHandler`, `CommandHandler`, and `QueryHandler`.

---

## v9.2.0 — 09 Mar 2025

- Add `ILogger` on CommandEvent processing.

---

## v9.1.2 — 09 Mar 2025

- Hotfix / add some examples.

---

## v9.1.1 — 09 Mar 2025

- Add SonarCloud.

---

## v9.1.0 — 04 Mar 2025

- Added `CommandEventTypeEnum` to define a type of Event, which should process event inside or outside of a transaction.
- Renamed EventQueueProcessBehavior → CommandEventQueueProcessBehavior, EventHandler → CommandEventHandler, EventQueue → CommandEventQueue, IEvent → ICommandEvent, IEventQueue → ICommandEventQueue, IEventQueueReader → ICommandEventQueueReader, IEventQueueWriter → ICommandEventQueueWriter.

---

## v8.3.1 — 23 Feb 2025

- Small internal refactoring.
- Fix build pipeline.

---

## v8.3.0 — 03 Jul 2024

- Set the `RequestFilterBehavior` before the `ValidationBehavior`.

---

## v8.2.2 — 16 May 2024

- Fix pipeline.

---

## v8.2.1 — 16 May 2024

- Add autobuild/push NuGet packages.

---

## v8.2.0 — 16 Apr 2024

- Added `IRequestFilter` with Behaviour to filter request of query or command.
- Added `IResultFilter` with Behaviour to filter result of query or command.

---

## v8.1.2 — 3 Apr 2024

- Added `NoRegisterAutomaticallyAttribute` to avoid register instance of class automatically in DI.
- Small fixes.
- Added sample project.

---

## v8.0.1 — 24 Nov 2023

- Fixed unhandled exception on .NET 8 (`System.TypeLoadException` when resolving MediatR `IRequestHandler<TRequest,TResponse>` for command handlers).
- Upgrade packages.

---

## v8.0.0 — 18 Nov 2023

- Added support for .NET 8.

---

## v7.0.0 — 18 Nov 2023

- Removed `PaginatedQuery` as useless.
- Updated version to correlate with .NET version.

---

## v1.0.1 — 13 Nov 2023

- Readme updated.
- Icon updated.

---

## v1.0.0 — 31 Oct 2023

- Release.

---

## v0.6.0 — 4 July 2023

- Added Net 7.0 targeted libraries.

---

## v0.5.0 — 2 June 2023

- Added Net 6.0 targeted libraries.

---

## v0.4.0 — 12 May 2023

- Package updates.

---

## v0.3.0 — 23 Aug 2022

- Bugfixes.

---

## v0.2.0 — 01 Jan 2022

- Added NetStandard 2.0 targeted libraries.

---

## v0.1.0 — 01 Sep 2021

- Initial version.
