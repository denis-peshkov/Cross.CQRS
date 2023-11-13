# Cross.CQRS

Simple .NET MediatR base Query, Command, Event and Validation.

Event Queue and Validation behaviors written on C#.

Main Features:
* **Queries, and QueryHandlers**. 

  Implemented base patterns to work with Queries. The Queries used just to get any data.

* **Commands and CommandHandlers**.

  Implemented base patterns to work with Commands. The Commands used to modify entities. 

* **Events, EventHandlers, EventWriter and EventReader**. 

  Implemented base patterns to crate Events, approach how to write a new Events from the Commands, consuming patterns and behavior to handle it.
  The main idea is to do some actions after the Commands have to be finished, to avoid cases when one Command call another one.

* **Validation**.

  Here included validation behavior based on FluentValidation, that allow to validate Queries and Command before their execution. 

* **.NET Standard 2.1 and Source Linking**. 

  From version 1.0 repository contains .NET Standard 2.0, .NET 6 and .NET 7 projects.
  Source linking enabled and symbol package is published to nuget symbols server, making debugging easier.

## Installation

Clone repository or Install Nuget Package
```
Install-Package Cross.CQRS
```
## How To's

Please use [Wiki](https://github.com/denis-peshkov/Cross.CQRS/wiki) for documentation and usage examples.

### Complete usage examples can be found in the test project ###
Note - test project is not a part of nuget package. You have to clone repository.

## Roadmap:
- ~~Queries implementation~~
- ~~Commands Implementation~~
- ~~Validation behavior~~ (based on FluentValidation)
- ~~Events~~
