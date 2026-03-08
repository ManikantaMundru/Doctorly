# Doctorly Calendar API

**A clean, modern backend API for managing doctor scheduling and calendar events.**

This service supports full CRUD operations on calendar events, invitation responses, overlap prevention for doctors, optimistic concurrency, and iCal-style notification generation.

## Overview

Doctorly Calendar API is a RESTful backend service designed for healthcare scheduling. It allows creating, updating, cancelling, retrieving, searching events, and handling attendee responses (accept/decline/tentative).

Key goals:
- Prevent overlapping bookings for the same doctor
- Enforce business rules (e.g., no changes to cancelled events)
- Provide reliable optimistic concurrency control
- Generate standards-compliant iCal notifications

## Tech Stack

- **.NET 9**
- **ASP.NET Core Web API**
- **Entity Framework Core** (with SQL Server)
- **MediatR** (CQRS pattern)
- **FluentValidation**
- **Swagger / OpenAPI** (for interactive docs)
- **xUnit + Moq + FluentAssertions** (testing)

## Architecture

The project follows **Clean Architecture** principles with **CQRS-style separation** in the Application layer. Dependencies flow inward: Presentation → Application → Domain ← Infrastructure.

### Project Structure

- **Doctorly.Calendar.Api**  
  → Entry point, controllers, middleware, DI setup, Swagger

- **Doctorly.Calendar.Application**  
  → Commands, Queries, Handlers, Validators, Mappers, Interfaces

- **Doctorly.Calendar.Domain**  
  → Entities, Value Objects, Enums, Domain Events, Business Rules, Exceptions

- **Doctorly.Calendar.Infrastructure**  
  → EF Core DbContext, Repository implementations, Entity configurations, iCal generation, Notifications

- **Doctorly.Calendar.Tests**  
  → Unit tests for domain logic, application handlers, and value objects

### Key Design Decisions

- **Aggregate Root**: `CalendarEvent`
- **Child Entity**: `Attendee` (owned by `CalendarEvent`)
- **Doctor**: Treated as the scheduling resource (not an attendee)
- **Concurrency**: Optimistic locking via SQL Server `rowversion` (timestamp)
- **Validation**: FluentValidation on commands/queries
- **Notifications**: Abstracted service with iCal.ics content generation

## Features

- Create new calendar events
- Update event details (with concurrency check)
- Cancel events
- Retrieve event by ID
- List / paginated events
- Advanced search
- Respond to invitations (Pending / Accepted / Declined / Tentative)
- Prevent overlapping doctor schedules
- Block modifications on cancelled events
- Handle concurrency conflicts (409 Conflict)
- Generate iCal notification payloads

## Prerequisites

- .NET 9 SDK
- SQL Server (localdb, Express, or Azure SQL)


## Getting Started

### 1. Clone the repository

# Restore packages
    dotnet restore
	
# Add initial migration (if needed)
dotnet ef migrations add InitialTableCreate --project Doctorly.Calendar.Infrastructure --startup-project Doctorly.Calendar.Api --output-dir Persistence/Migrations

# Apply migrations to create/update database
dotnet ef database update --project Doctorly.Calendar.Infrastructure --startup-project Doctorly.Calendar.Api
 
# Run the API 
dotnet run --project Doctorly.Calendar.Api