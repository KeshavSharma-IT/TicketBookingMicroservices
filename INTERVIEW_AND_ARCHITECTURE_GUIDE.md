# 🎓 Ticket Booking Microservices — Senior System Design & Interview Guide

> **Target Audience:** .NET / Cloud / Microservices Engineers aiming for Senior & Staff Engineer roles.
> **Architecture Style:** Clean Architecture + Domain-Driven Design (DDD) + Event-Driven Microservices.
> **Framework:** .NET 10, C# 13, Entity Framework Core, SQL Server, Redis, RabbitMQ.

---

## 📑 Table of Contents
1. [Overall System Architecture & Boundaries](#1-overall-system-architecture--boundaries)
2. [Microservices Breakdown & Deep Architectural Rationales](#2-microservices-breakdown--deep-architectural-rationales)
   - [AuthService: Identity & Token Rotation](#authservice-identity--token-rotation)
   - [EventServices: Catalog, Aggregates & Scheduling](#eventservices-catalog-aggregates--scheduling)
   - [BookingService (Future): High-Concurrency Ticket Reservation](#bookingservice-future-high-concurrency-ticket-reservation)
3. [Deep-Dive Technical Concepts & Senior Interview Questions](#3-deep-dive-technical-concepts--senior-interview-questions)
   - [Q1: Pagination — Offset vs Keyset (Deep Paging Problem)](#q1-pagination--offset-vs-keyset-deep-paging-problem)
   - [Q2: Database Queries — `CountAsync()` vs `list.Count`](#q2-database-queries--countasync-vs-listcount)
   - [Q3: Clean Architecture — Why Domain vs Application vs Infrastructure?](#q3-clean-architecture--why-domain-vs-application-vs-infrastructure)
   - [Q4: Domain-Driven Design (DDD) — Aggregate Roots (`Venue` & `Screen`)](#q4-domain-driven-design-ddd--aggregate-roots-venue--screen)
   - [Q5: Security — JWT Lifecycles & Refresh Token Family Revocation](#q5-security--jwt-lifecycles--refresh-token-family-revocation)
   - [Q6: Distributed Caching — Cache-Aside, Stampedes & Invalidation](#q6-distributed-caching--cache-aside-stampedes--invalidation)
   - [Q7: Concurrency & Double Booking — Optimistic vs Pessimistic vs Distributed Locks](#q7-concurrency--double-booking--optimistic-vs-pessimistic-vs-distributed-locks)
4. [Master Cheat Sheet: Trade-offs & When to Choose What](#4-master-cheat-sheet-trade-offs--when-to-choose-what)

---

## 1. Overall System Architecture & Boundaries

```
                              ┌─────────────────────────────────────────┐
                              │           API Gateway / Ingress         │
                              └────────────────────┬────────────────────┘
                                                   │
          ┌────────────────────────────────────────┼────────────────────────────────────────┐
          │                                        │                                        │
          ▼                                        ▼                                        ▼
┌──────────────────┐                     ┌──────────────────┐                     ┌──────────────────┐
│   AuthService    │                     │  EventServices   │                     │  BookingService  │
│  (Port: 5001)    │                     │  (Port: 5002)    │                     │  (Port: 5003)    │
├──────────────────┤                     ├──────────────────┤                     ├──────────────────┤
│ • User Identity  │                     │ • Event Catalog  │                     │ • Seat Engine    │
│ • JWT / Refresh  │                     │ • Venues/Screens │                     │ • Order State    │
│ • Role-Based Auth│                     │ • Shows Timetable│                     │ • Distributed Lk │
└─────────┬────────┘                     └─────────┬────────┘                     └─────────┬────────┘
          │                                        │                                        │
          ▼                                        ▼                                        ▼
    ┌───────────┐                            ┌───────────┐                            ┌───────────┐
    │  Auth DB  │                            │ Events DB │                            │ Redis + DB│
    └───────────┘                            └─────┬─────┘                            └───────────┘
                                                   │
                                                   ▼
                                             ┌───────────┐
                                             │   Redis   │ (Read-Through / Distributed Cache)
                                             └───────────┘
```

### Microservice Principle: Database-Per-Service
Each microservice owns its own schema and data storage. **Never share databases between services.** If `BookingService` needs movie details, it either reads from a read-replica/cache or consumes asynchronous events published by `EventServices`.

---

## 2. Microservices Breakdown & Deep Architectural Rationales

### AuthService (Identity & Token Rotation)
* **Goal:** Secure identity issuance, RBAC (Role-Based Access Control), and session management.
* **Architecture Pattern:** CQRS via MediatR with Validation Pipeline Behaviors.

#### 💡 Architectural Highlights
1. **Separation of Concerns:** Authentication logic is completely detached from business domain services.
2. **Standardized Responses:** Uses **RFC 7807 Problem Details** for consistent global error handling (`400 Bad Request`, `401 Unauthorized`, `404 Not Found`).

---

### EventServices (Catalog, Aggregates & Scheduling)
* **Goal:** High-read, low-write catalog of Movies, Venues, Screens, and Show schedules.
* **Core Invariants & Business Logic:**
  * **Dynamic Duration:** `Show.EndTime = Show.StartTime + Event.DurationInMinutes`.
  * **Overlap Collision Prevention:** Screen schedules must never intersect `(StartA < EndB && EndA > StartB)`.
  * **Aggregate Root Boundaries:** `Screen` entities are modified only through `Venue`.

```
                    ┌───────────────────────────────┐
                    │      Venue (Aggregate Root)   │
                    │ ───────────────────────────── │
                    │ - Id: Guid                    │
                    │ - Name: string                │
                    │ - City: string                │
                    │ - Screens: List<Screen> ◄─────┼──── Screen cannot exist alone
                    └───────────────────────────────┘
```

---

## 3. Deep-Dive Technical Concepts & Senior Interview Questions

---

### Q1: Pagination — Offset vs Keyset (Deep Paging Problem)

#### 💬 Interview Question:
> *"How would you implement pagination for a high-traffic endpoint? What are the limitations of `Skip` and `Take` in SQL Server?"*

#### 🎯 Ideal Senior Answer:
There are two primary pagination strategies:

1. **Offset-Based Pagination (`.Skip().Take()`):**
   * Translates to: `SELECT ... OFFSET X ROWS FETCH NEXT Y ROWS ONLY`.
   * **Advantage:** Supports jumping to arbitrary pages (e.g., page 5) and sorting on any column.
   * **Bottleneck (Deep Paging):** At `OFFSET 500,000`, the database engine must physically read and discard 500,000 rows in memory before returning the next 10. Complexity is $O(N)$.
   * **Data Drift:** If a new record is inserted while a user navigates from Page 1 to Page 2, items shift, causing duplicate or missing records.

2. **Keyset / Cursor-Based Pagination:**
   * Translates to: `SELECT TOP(Y) ... WHERE Id > @LastSeenId ORDER BY Id ASC`.
   * **Advantage:** Always $O(1)$ constant time because it performs an **Index Seek** directly to `@LastSeenId`. Immune to data drift.
   * **Disadvantage:** Cannot jump to an arbitrary page (e.g., jump directly to page 20); only supports "Next" and "Previous" sequential scrolling (perfect for feeds).

#### 🏆 Our Decision:
For **EventServices Catalog**, we use **Offset Pagination with a capped PageSize (max 50)** because users need page-based navigation and filtering across cities.

---

### Q2: Database Queries — `CountAsync()` vs `list.Count`

#### 💬 Interview Question:
> *"Why can't we just call `.ToListAsync()` on our paginated query and check `.Count` for the total record count?"*

#### 🎯 The Critical Difference:

```
Total Database Rows = 1,000
Requested: Page 1, PageSize 10

Scenario A (Wrong):
  var items = await query.Skip(0).Take(10).ToListAsync();
  var total = items.Count; // ⚠️ Result: total = 10 (NOT 1,000!)
  // Frontend will think there is only 1 page and hide pagination buttons!

Scenario B (Correct):
  // Query 1: Total matching records across the entire table
  var totalCount = await query.CountAsync(); // SELECT COUNT(*) FROM Events WHERE ...

  // Query 2: Fetch only the current page's slice
  var items = await query.Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync(); // SELECT ... OFFSET 0 FETCH NEXT 10
```

#### ⚡ Senior Performance Optimization:
* Execute `CountAsync()` on the raw `IQueryable` **before** applying `Skip` and `Take`.
* If `totalCount == 0`, immediately return an empty `PagedResult<T>` without executing the second query!

---

### Q3: Clean Architecture — Why Domain vs Application vs Infrastructure?

```
┌─────────────────────────────────────────────────────────┐
│                    API / Presentation                   │ ◄── Controllers, Filters, Program.cs
│  ┌───────────────────────────────────────────────────┐  │
│  │                    Infrastructure                 │  │ ◄── EF Core, Repositories, Redis, Email
│  │  ┌─────────────────────────────────────────────┐  │  │
│  │  │                  Application                │  │  │ ◄── Use Cases, DTOs, IServices, Validators
│  │  │  ┌───────────────────────────────────────┐  │  │  │
│  │  │  │                 Domain                │  │  │  │ ◄── Entities, Enums, Exceptions (0 Dependencies)
│  │  │  └───────────────────────────────────────┘  │  │  │
│  │  └─────────────────────────────────────────────┘  │  │
│  └───────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
```

#### 💬 Interview Question:
> *"What is the Dependency Inversion Principle in Clean Architecture, and why is the Domain project reference-free?"*

#### 🎯 Senior Answer:
* **The Domain Layer** represents the pure business reality (Entities, Business Rules). It has **zero dependencies** on external frameworks, databases, or UI. This ensures business logic can be tested in isolation and remains unchanged even if the database changes from SQL Server to PostgreSQL or MongoDB.
* **The Application Layer** contains business use cases and interfaces (`IShowService`, `IEventRepository`).
* **The Infrastructure Layer** implements those interfaces (`EventRepository`, `DbContext`). The dependency flows **inward**.

---

### Q4: Domain-Driven Design (DDD) — Aggregate Roots (`Venue` & `Screen`)

#### 💬 Interview Question:
> *"Why don't we have a direct `ScreensController` that inserts a `Screen` directly into the database?"*

#### 🎯 Senior Answer:
In DDD, an **Aggregate Root** (`Venue`) is the only entry point for modifying its child entities (`Screens`).
1. **Invariant Protection:** A Screen must adhere to venue rules (e.g., unique screen names within the venue, screen capacity must not exceed venue total capacity).
2. **Encapsulation:** By modifying screens via `venue.AddScreen(...)`, the domain model enforces business invariants before persisting data to the database.

---

### Q5: Security — JWT Lifecycles & Refresh Token Family Revocation

#### 💬 Interview Question:
> *"Why not make JWT Access Tokens valid for 30 days so the user never has to log in again?"*

#### 🎯 Senior Answer:
* JWTs are **stateless**. Once signed and issued, an access token cannot be revoked before it expires (unless you maintain an expensive distributed token blocklist in Redis).
* If a 30-day token is intercepted by a malicious actor (XSS / Man-in-the-Middle), the attacker has 30 days of full access.
* **The Solution:** Short-lived access token (e.g., 15 minutes) + Long-lived refresh token (e.g., 7 days) stored securely in the database.
* **Token Reuse Detection:** If an old refresh token is reused, it indicates token theft. The backend immediately invalidates the **entire token family**, forcing all sessions for that user to re-authenticate.

---

### Q6: Distributed Caching — Cache-Aside, Stampedes & Invalidation

```
Client ──► API ──► Check Redis ──[HIT]──► Return Cached JSON
                     │
                   [MISS]
                     ▼
                 Read SQL DB ──► Save in Redis (TTL: 10m) ──► Return Data
```

#### 💬 Interview Question:
> *"What is a Cache Stampede (Thundering Herd) and how do you prevent it?"*

#### 🎯 Senior Answer:
* **Cache Stampede:** When a popular cached key (e.g., `shows_delhi_avengers`) expires during peak traffic, thousands of concurrent requests get a cache MISS simultaneously and all hammer the database at the exact same millisecond, taking down the database.
* **Solutions:**
  1. **Distributed Mutex / Lock (`SemaphoreSlim` or Redis Lock):** Only one request queries the DB and repopulates the cache; all other requests wait for the cache to update.
  2. **Probabilistic Early Expiration (XFetch):** Background worker refreshes the cache before it strictly expires.

---

### Q7: Concurrency & Double Booking — Optimistic vs Pessimistic vs Distributed Locks

#### 💬 Interview Question:
> *"Two users click 'Book Seat A1' at the exact same millisecond. How do you prevent double booking?"*

#### 🎯 Architectural Options & Comparison:

| Strategy | How it Works | Pros | Cons | Best For |
| :--- | :--- | :--- | :--- | :--- |
| **Pessimistic Locking** | `SELECT ... WITH (UPDLOCK, ROWLOCK)` locks the row in SQL Server. | Guaranteed consistency; zero duplicates. | Holds DB connection open; does not scale across distributed nodes. | Low-traffic, high-value financial transfers. |
| **Optimistic Locking** | Uses a `RowVersion` / `ConcurrencyToken` column. `UPDATE ... WHERE RowVersion = @original`. | High throughput; no DB locks held. | If collision occurs, one transaction throws `DbUpdateConcurrencyException` and fails. | General e-commerce item updates. |
| **Redis Distributed Lock (Redlock / Redisson)** | Acquires a key in Redis: `SET seat:show123:A1 user456 NX EX 600` (10-minute hold). | Ultra-fast ($<1\text{ms}$); offloads locking from SQL Server; supports temporary seat holds. | Requires Redis cluster reliability management. | **Cinema & Concert Seat Reservation (Our BookingService)** |

---

### Q8: Docker & Containerization — Multi-Stage Builds & Layer Caching

#### 💬 Interview Question:
> *"What is a Multi-Stage Dockerfile in .NET and how does Docker Layer Caching optimize CI build times?"*

#### 🎯 Senior Answer:
1. **Multi-Stage Build:**
   * **Stage 1 (`mcr.microsoft.com/dotnet/sdk:10.0`):** Heavy image (~850MB) containing the C# compiler, MSBuild, and NuGet tools to compile and publish the project.
   * **Stage 2 (`mcr.microsoft.com/dotnet/aspnet:10.0`):** Ultra-lightweight runtime image (~110MB) containing only the bare runtime.
   * **Outcome:** The production image contains only binaries, reducing attack surface (security) and network transfer times (performance).

2. **Docker Layer Caching:**
   * Docker builds images layer-by-layer. If a layer and its inputs don't change, Docker uses the cached layer.
   * **The Optimization:**
     ```dockerfile
     # Step 1: Copy ONLY .csproj files
     COPY AuthService/*.csproj AuthService/
     COPY AuthService.Application/*.csproj AuthService.Application/
     # Step 2: Restore NuGets (Cached unless .csproj changes)
     RUN dotnet restore AuthService/AuthService.csproj
     # Step 3: Copy source code (Frequent changes)
     COPY . .
     RUN dotnet build --no-restore
     ```
   * If you edit a `.cs` file, Docker skips Step 1 & 2 (reusing cached NuGets in 0.1s) and only recompiles Step 3.

---

### Q9: CI/CD in Microservices Monorepos — Path Filtering vs Matrix Builds

#### 💬 Interview Question:
> *"In a monorepo containing multiple microservices, how do you prevent unnecessary builds on every push?"*

#### 🎯 Senior Answer:
* Use **Path Filtering** in GitHub Actions workflows (`paths: ['AuthService/**']`).
* Changes to `EventServices` will not trigger the `AuthService` pipeline, saving CI runner minutes and preventing unnecessary deployments.

---

## 4. Master Cheat Sheet: Trade-offs & When to Choose What

| Architectural Choice | Choice A | Choice B | Why We Selected This |
| :--- | :--- | :--- | :--- |
| **API Paradigm** | REST + OpenAPI / Scalar | gRPC / GraphQL | REST provides universal client compatibility and simple caching for public catalogs. |
| **Pagination** | Offset-based (`Skip`/`Take`) | Keyset (`Seek`) | Catalog requires direct page jumping and multi-attribute sorting. |
| **Error Handling** | Custom JSON Errors | RFC 7807 Problem Details | RFC 7807 is an IETF industry standard understood by all modern API clients and gateways. |
| **Inter-service Sync** | Direct HTTP (REST) | Event-Driven (RabbitMQ) | Asynchronous messaging prevents cascading failures across microservices. |
