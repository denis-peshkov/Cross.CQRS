[![License](https://img.shields.io/github/license/denis-peshkov/Cross.CQRS)](LICENSE.md)
[![GitHub Release Date](https://img.shields.io/github/release-date/denis-peshkov/Cross.CQRS?label=released)](https://github.com/denis-peshkov/Cross.CQRS/releases)
[![NuGetVersion](https://img.shields.io/nuget/v/Cross.CQRS.svg)](https://nuget.org/packages/Cross.CQRS/)
[![NugetDownloads](https://img.shields.io/nuget/dt/Cross.CQRS.svg)](https://nuget.org/packages/Cross.CQRS/)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Cross.CQRS&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Cross.CQRS)
[![issues](https://img.shields.io/github/issues/denis-peshkov/Cross.CQRS)](https://github.com/denis-peshkov/Cross.CQRS/issues)
[![.NET PR](https://github.com/denis-peshkov/Cross.CQRS/actions/workflows/dotnet.yml/badge.svg?event=pull_request)](https://github.com/denis-peshkov/Cross.CQRS/actions/workflows/dotnet.yml)

![Size](https://img.shields.io/github/repo-size/denis-peshkov/Cross.CQRS)
[![GitHub contributors](https://img.shields.io/github/contributors/denis-peshkov/Cross.CQRS)](https://github.com/denis-peshkov/Cross.CQRS/contributors)
[![GitHub commits since latest release (by date)](https://img.shields.io/github/commits-since/denis-peshkov/Cross.CQRS/latest?label=new+commits)](https://github.com/denis-peshkov/Cross.CQRS/commits/master)
![Activity](https://img.shields.io/github/commit-activity/w/denis-peshkov/Cross.CQRS)
![Activity](https://img.shields.io/github/commit-activity/m/denis-peshkov/Cross.CQRS)
![Activity](https://img.shields.io/github/commit-activity/y/denis-peshkov/Cross.CQRS)

# Cross.CQRS

Simple .NET MediatR base Query, Command, Event, Validation and Filter.

Event Queue, Validation and Filter behaviors written on C#.

Main Features:

* **Queries, and QueryHandlers**.

  Implemented base patterns to work with Queries. The Queries used just to get any data.

* **Commands and CommandHandlers**.

  Implemented base patterns to work with Commands. The Commands used to modify entities.

* **CommandEvents, CommandEventHandlers, CommandEventWriter, CommandEventReader and CommandEventQueueProcessBehavior**.

  Implemented base patterns to create CommandEvents, approach how to write a new CommandEvents from the Commands, consuming patterns and behavior to handle it.

  The main idea is to do some actions after the Commands have to be finished, to avoid cases when one Command call another one.

  Added possibility to exclude the processing of some CommandEvents from Command transaction (even on throw Exception).

* **Filters**.

  Here included filter behavior based on RequestFilter and ResultFilter.

  The RequestFilter allow to filter Queries and Commands requests before their execution.

  The ResultFilter allow to filter Queries and Commands results after their execution.

* **Validation**.

  Here included validation behavior based on FluentValidation, that allow to validate Queries and Command before their execution.

* **.NET frameworks and Source Linking**.

  **Supported frameworks:** .NET Standard 2.1, .NET 6, .NET 7, .NET 8, .NET 9, .NET 10 (TFM-specific versions of `Microsoft.Extensions.*` are referenced in the package).

  Source linking enabled and symbol package is published to nuget symbols server, making debugging easier.

* **Licensing (optional JWT)**.

  You can set `LicenseKey` on `CqrsServiceConfiguration` to a Peshkov license JWT. Validation runs on **every** MediatR request via `LicenseCheckBehavior` / `CheckLicense` (for `ILicenseProductInfo` with `Product` of `"Cross.CQRS"` or `"Cross.CQRS.EF"`). License keys: [peshkov.biz](https://peshkov.biz/cqrs).


## Install NuGet package

Install the _Cross.CQRS_ [NuGet package](https://www.nuget.org/packages/Cross.CQRS/) into your ASP.NET Core project:

```powershell
Install-Package Cross.CQRS
```

or

```bash
dotnet add package Cross.CQRS
```

Either commands, from Package Manager Console or .NET Core CLI, will download and install Cross.CQRS and all required dependencies.

### Registering with `IServiceCollection`

Cross.CQRS supports `Microsoft.Extensions.DependencyInjection.Abstractions` directly. To register Cross.CQRS services and handlers, use the configuration overload:

```csharp
services.AddCQRS(cfg =>
{
    cfg.RegisterFromAssemblies(typeof(Startup).Assembly);
});
```

or with `RegisterFromAssemblyContaining`:

```csharp
services.AddCQRS(cfg =>
{
    cfg.RegisterFromAssemblyContaining<Startup>();
});
```

Multiple assemblies:

```csharp
services.AddCQRS(cfg =>
{
    cfg.RegisterFromAssemblies(typeof(Startup).Assembly, typeof(Other).Assembly);
});
```

With license key:

```csharp
services.AddCQRS(cfg =>
{
    cfg.RegisterFromAssemblyContaining<Startup>();
    cfg.LicenseKey = "<license key here>";
});
```

This registers (via MediatR with **Scoped** lifetime):
- `IMediator`, `ISender`, `IPublisher` — scoped
- `IRequestHandler<,>` and `IRequestHandler<>` implementations — scoped
- `INotificationHandler<>` implementations — scoped
- `IStreamRequestHandler<>` implementations — scoped
- `IRequestExceptionHandler<,,>` and `IRequestExceptionAction<,>` implementations — scoped

Additionally registered:
- FluentValidation validators scanned from **all** assemblies registered in `CqrsServiceConfiguration` (same set as MediatR and filters) — scoped
- `IResultFilter<,>` and `IRequestFilter<>` — scoped
- `IHandlerLocator` — singleton
- `LicenseAccessor`, `LicenseValidator`, default `ILicenseProductInfo` (`LicenseProductInfo`) — singleton
- `ICommandEventQueue` and its reader/writer — scoped
- Pipeline behaviors (scoped): `LicenseCheckBehavior`, `CommandEventQueueProcessBehavior`, `RequestFilterBehavior`, `ValidationBehavior`, `ResultFilterBehavior`

The method returns `CqrsRegistrationSyntax` for fluent configuration (e.g. adding custom behaviors).

License keys are available at [peshkov.biz](https://peshkov.biz/cqrs).


## Issues and Pull Request

Contribution is welcomed. See [CONTRIBUTING.md](CONTRIBUTING.md). If you would like to provide a PR please add some testing.

Release history (newest first): [docs/CHANGELOG.md](docs/CHANGELOG.md). Breaking upgrades for NuGet consumers: [docs/BREAKING.md](docs/BREAKING.md).


## How To's

Please use [Wiki](https://github.com/denis-peshkov/Cross.CQRS/wiki) for documentation and usage examples.

### Complete usage examples can be found in the test project ###
Note - test project is not a part of nuget package. You have to clone repository.


## Roadmap:
- ~~Queries implementation~~
- ~~Commands Implementation~~
- ~~Validation behavior (based on FluentValidation)~~
- ~~Events~~
