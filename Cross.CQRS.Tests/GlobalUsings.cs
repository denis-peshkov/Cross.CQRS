// Global using directives

global using System;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.Linq;
global using System.Security.Claims;
global using System.Threading;
global using System.Threading.Tasks;
global using Cross.CQRS.Behaviors;
global using Cross.CQRS.Commands;
global using Cross.CQRS.Common;
global using Cross.CQRS.Events;
global using Cross.CQRS.Extensions;
global using Cross.CQRS.Filters;
global using Cross.CQRS.Licensing;
global using Cross.CQRS.Queries;
global using Cross.CQRS.Services;
global using FluentAssertions;
global using FluentValidation;
global using MediatR;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Logging.Abstractions;
global using NUnit.Framework;
global using License = Cross.CQRS.Licensing.License;