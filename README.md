# Crypto World - Microservices Demo

This project is part of a job interview task demonstrating microservices architecture using .NET core, MassTransit, and RabbitMQ.

## Architecture

- **RatesService**: Publishes real-time rate changes.
- **PositionsService**: Listens for rate changes and recalculates positions.

## Tech Stack

- .NET core (Minimal API)
- MassTransit + RabbitMQ
- Entity Framework Core (In-Memory)

## How to Run

1. Start RabbitMQ locally.
2. Run `RatesService`and `PositionsService` using dotnet clean, dotnet build, dotnet run commands.
3. Trigger `RateService` using http://localhost:5293/trigger-fetch GET endpoint.
