# 🎟️ Ticket Booking Microservices Platform

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C# 13](https://img.shields.io/badge/C%23-13.0-239120?logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean%20%2F%20DDD-blue)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
[![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED?logo=docker&logoColor=white)](https://www.docker.com/)
[![CI/CD](https://img.shields.io/badge/CI%2FCD-GitHub%20Actions-2088FF?logo=githubactions&logoColor=white)](https://github.com/features/actions)
[![OpenAPI](https://img.shields.io/badge/API%20Docs-Scalar-black)](https://scalar.com/)

An enterprise-grade, distributed **Ticket Booking Microservices Platform** built with **.NET 10**, strictly adhering to **Clean Architecture**, **Domain-Driven Design (DDD)**, and **Containerized Cloud-Native Standards**.

---

## 🏗️ System Architecture

```
                              ┌─────────────────────────────────────────┐
                              │            Client / API Consumer        │
                              └────────────────────┬────────────────────┘
                                                   │
          ┌────────────────────────────────────────┼────────────────────────────────────────┐
          │                                        │                                        │
          ▼ (Port 5001)                            ▼ (Port 5002)                            ▼ (Port 5003)
┌───────────────────────────┐            ┌───────────────────────────┐            ┌───────────────────────────┐
│        AuthService        │            │       EventServices       │            │      BookingService       │
├───────────────────────────┤            ├───────────────────────────┤            ├───────────────────────────┤
│ • User Registration/Login │            │ • Event & Movie Catalog   │            │ • High-Concurrency Seats  │
│ • Refresh Token Rotation  │            │ • Venues & Screens (DDD)  │            │ • Redis Distributed Locks │
│ • CQRS + MediatR Pipeline │            │ • Show Schedule Collision │            │ • Idempotent Orders       │
│ • Fixed-Window Rate Limit │            │ • Dynamic EndTime Engine  │            │ • Saga State Machine      │
└─────────────┬─────────────┘            └─────────────┬─────────────┘            └─────────────┬─────────────┘
              │                                        │                                        │
              ▼                                        ▼                                        ▼
    ┌───────────────────┐                    ┌───────────────────┐                    ┌───────────────────┐
    │   Auth Database   │                    │  Events Database  │                    │   Redis Cache     │
    │   (SQL Server)    │                    │   (SQL Server)    │                    │   + Booking DB    │
    └───────────────────┘                    └───────────────────┘                    └───────────────────┘
```

---

## 🧩 Microservices Breakdown

### 1. 🔐 `AuthService` (Identity & Session Management)
- **Role-Based Access Control (RBAC):** Secure user registration, authentication, and authorization.
- **Refresh Token Rotation:** Mitigates replay attacks by rotating refresh token families on every refresh request.
- **CQRS Architecture:** Implemented via MediatR with decoupled commands, queries, and validation pipeline behaviors.
- **Rate Limiting:** Protects sensitive auth endpoints from brute-force attacks via ASP.NET Core Fixed-Window rate limiters.
- **Global Error Handling:** Adheres to **RFC 7807 Problem Details** specification.

### 2. 🎬 `EventServices` (Catalog & Show Scheduling)
- **Domain-Driven Design (DDD):** Treats `Venue` as an Aggregate Root managing child `Screen` entities to guarantee domain invariants.
- **Dynamic Show Duration:** Automatically calculates `Show.EndTime` based on `StartTime + Event.DurationInMinutes`.
- **Schedule Overlap Collision Prevention:** Enforces mathematical validation preventing any two shows from conflicting on the same screen.
- **Composite Unique Constraints:** Enforces database-level indexes on `(Name, City, Address)` for Venues and `(VenueId, Name)` for Screens.

### 3. 🎟️ `BookingService` *(In Progress)*
- **High-Concurrency Seat Engine:** Redis Distributed Locks (Redlock) for temporary seat holds during checkout.
- **Idempotent APIs:** Prevents duplicate payments and double-booking under race conditions.
- **Event-Driven Messaging:** MassTransit / RabbitMQ integration for asynchronous order completion.

---

## 🏛️ Clean Architecture Design

Each microservice follows the **Dependency Inversion Principle** across 4 isolated layers:

```
┌─────────────────────────────────────────────────────────────┐
│                    API / Presentation                       │ ◄── Controllers, Middlewares, Program.cs
│  ┌───────────────────────────────────────────────────────┐  │
│  │                    Infrastructure                     │  │ ◄── EF Core, Repositories, DB Context
│  │  ┌─────────────────────────────────────────────────┐  │  │
│  │  │                  Application                    │  │  │ ◄── DTOs, IServices, MediatR, Validators
│  │  │  ┌───────────────────────────────────────────┐  │  │  │
│  │  │  │                 Domain                    │  │  │  │ ◄── Entities, Enums (Zero Dependencies)
│  │  │  └───────────────────────────────────────────┘  │  │  │
│  │  └─────────────────────────────────────────────────┘  │  │
│  └───────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

---

## 🚀 Getting Started Locally

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### 🐳 Run the Entire Platform with Docker Compose

Clone the repository and run:

```powershell
# Start all microservices + SQL Server database
docker compose up --build
```

### 📖 Interactive API Documentation (Scalar)

Once containers are running, navigate to the modern Scalar API interfaces:

| Microservice | Interactive Scalar UI | Raw OpenAPI JSON |
| :--- | :--- | :--- |
| **AuthService** | [http://localhost:5001/scalar/v1](http://localhost:5001/scalar/v1) | [http://localhost:5001/openapi/v1.json](http://localhost:5001/openapi/v1.json) |
| **EventServices** | [http://localhost:5002/scalar/v1](http://localhost:5002/scalar/v1) | [http://localhost:5002/openapi/v1.json](http://localhost:5002/openapi/v1.json) |

To stop all containers:
```powershell
docker compose down
```

---

## ⚙️ DevOps & CI/CD Pipeline

The project uses **GitHub Actions** with **Path-Filtered Multi-Stage Workflows**:

- **CI (Continuous Integration):** Triggered on every pull request and push to `Dev` or `feature/**` branches:
  1. Restores NuGet dependencies with cached layers.
  2. Compiles solution in `Release` configuration.
  3. Builds and tests multi-stage Docker images (~110MB production footprints).
- **CD (Continuous Delivery):** Automatically publishes immutable, containerized images to **GitHub Container Registry (GHCR)** upon merging into `main`.

---

## 🗺️ Engineering Roadmap

- [x] Identity & JWT Refresh Token Rotation (`AuthService`)
- [x] Event & Venue Schedule Collision Engine (`EventServices`)
- [x] Multi-Stage Dockerfiles & Docker Compose Orchestration
- [x] Path-Filtered GitHub Actions CI/CD with GHCR Publishing
- [ ] Generic Offset-based Pagination & Filtering (`PagedResult<T>`)
- [ ] Distributed Caching with Redis (Cache-Aside Pattern)
- [ ] Asynchronous Messaging with RabbitMQ / MassTransit
- [ ] Seat Reservation Concurrency Engine (`BookingService`)