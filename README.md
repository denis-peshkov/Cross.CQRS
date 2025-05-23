[![Nuget](https://img.shields.io/nuget/v/Cross.CQRS.svg)](https://nuget.org/packages/Cross.CQRS/) [![Documentation](https://img.shields.io/badge/docs-wiki-yellow.svg)](https://github.com/denis-peshkov/Cross.CQRS/wiki)

# Cross.CQRS

Simple .NET MediatR base Query, Command, Event, Validation and Filter.

Event Queue, Validation and Filter behaviors written on C#.

Main Features:
* **Queries, and QueryHandlers**.

  Implemented base patterns to work with Queries. The Queries used just to get any data.

* **Commands and CommandHandlers**.

  Implemented base patterns to work with Commands. The Commands used to modify entities.

* **CommandEvents, CommandEventHandlers, CommandEventWriter, CommandEventReader and CommandEventQueueProcessBehavior**.

  Implemented base patterns to crate CommandEvents, approach how to write a new CommandEvents from the Commands, consuming patterns and behavior to handle it.
  The main idea is to do some actions after the Commands have to be finished, to avoid cases when one Command call another one. 
  Added possibility to exclude the processing of some CommandEvents from Command transaction (even on trow Exception).

* **Filters**.

  Here included filter behavior based on RequestFilter and ResultFilter.
  The RequestFilter allow to filter Queries and Commands requests before their execution.
  The ResultFilter allow to filter Queries and Commands results after their execution.

* **Validation**.

  Here included validation behavior based on FluentValidation, that allow to validate Queries and Command before their execution.

* **.NET frameworks and Source Linking**.

  The repository contains .NET Standard 2.0, .NET 6, .NET 7 and .NET 8 projects.
  Source linking enabled and symbol package is published to nuget symbols server, making debugging easier.

## Install with nuget.org:

https://www.nuget.org/packages/Cross.CQRS

## Installation

Clone repository or Install Nuget Package
```
Install-Package Cross.CQRS
```

## Issues and Pull Request

Contribution is welcomed. If you would like to provide a PR please add some testing.

## How To's

Please use [Wiki](https://github.com/denis-peshkov/Cross.CQRS/wiki) for documentation and usage examples.

### Complete usage examples can be found in the test project ###
Note - test project is not a part of nuget package. You have to clone repository.

## Roadmap:
- ~~Queries implementation~~
- ~~Commands Implementation~~
- ~~Validation behavior (based on FluentValidation)~~
- ~~Events~~
