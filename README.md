# ⚽ Malaeb Booking API

A fully-featured **Stadium Booking REST API** built with **.NET 8**, following **Clean Architecture** and **Domain-Driven Design (DDD)** principles.

🔗 **Live API:** [https://malaeb-booking.runasp.net](https://malaeb-booking.runasp.net)
> ⚠️ This is a pure REST API. Append an endpoint to use it (e.g. `GET /api/stadiums`). The base URL alone returns nothing.

📦 **Source Code:** [github.com/a7medsale7/MalaebBooking](https://github.com/a7medsale7/MalaebBooking)

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Architecture](#-architecture)
- [Design Patterns](#-design-patterns)
- [Authentication & Authorization](#-authentication--authorization)
- [Background Jobs](#-background-jobs-hangfire)
- [Email Notifications](#-email-notifications)
- [Tech Stack](#-tech-stack)
- [Project Structure](#-project-structure)
- [Domain Entities](#-domain-entities)
- [Roles & Permissions](#-roles--permissions)
- [API Endpoints](#-api-endpoints)
- [Configuration](#-configuration)
- [Getting Started](#-getting-started)

---

## 🧩 Overview

**Malaeb Booking** is a stadium booking platform that serves three types of users:

| Role | Capabilities |
|---|---|
| **Player** | Browse stadiums, book time slots, submit payment proofs, leave reviews |
| **Owner** | Manage stadiums, time slots, approve/reject payments |
| **Admin** | Full platform control — users, roles, stadiums, bookings, sports |

The system handles the **full booking lifecycle**: discovery → booking → payment → confirmation → review.

---

## 🏗️ Architecture

The project is built on **Clean Architecture** — a strict layering system where dependencies only ever point inward.

```
MalaebBooking.Domain           ← Core business logic, entities, interfaces
MalaebBooking.Application      ← Use cases, services, DTOs, validators
MalaebBooking.Infrastructure   ← EF Core, SQL Server, JWT, mail, Hangfire
MalaebBooking.Api              ← Controllers, middleware, DI composition root
```

- The **Domain** layer has **zero external dependencies**. It knows nothing about EF Core, HTTP, or any framework.
- The **Application** layer depends only on the Domain.
- The **Infrastructure** layer implements domain interfaces.
- The **API** layer wires everything together.

---

## 🧠 Design Patterns

| Pattern | Usage |
|---|---|
| **Repository Pattern** | Repository interfaces defined in Domain, implemented in Infrastructure |
| **Unit of Work** | Wraps multiple repository operations in a single transaction |
| **Result Pattern** | Services return `Result<T>` or `Error` — no exception-driven flow |
| **Options Pattern** | Strongly-typed config binding for JWT, Mail, and Hangfire settings |
| **DDD** | Rich domain entities with encapsulated business rules |

---

## 🔐 Authentication & Authorization

### JWT Authentication
- **JWT Bearer tokens** with configurable expiry
- **Refresh Token rotation** — each refresh issues a new pair and revokes the old one
- **ASP.NET Identity** with required email confirmation before login

### Custom Permissions System
Standard role checks weren't enough. The system uses a **claims-based, granular permissions model**:

- Every permission is a claim embedded in the JWT (e.g. `Permissions.Payments.Approve`)
- A custom `PermissionAuthorizationHandler` validates permissions at request time
- A custom `PermissionAuthorizationPolicyProvider` creates policies dynamically — no static policy registration needed
- Each role has its own specific permission set seeded at startup

---

## ⏰ Background Jobs (Hangfire)

The system manages itself through 4 recurring Hangfire jobs:

| Job | Schedule | Purpose |
|---|---|---|
| `auto-complete-bookings` | Every **1 hour** | Auto-completes bookings whose time slot has passed |
| `auto-cancel-expired-pendings` | Every **15 minutes** | Cancels pending bookings that were never paid |
| `send-booking-reminders` | Every **30 minutes** | Sends email reminders for upcoming bookings |
| `cleanup-old-payments-storage` | Every **month** | Deletes old payment receipt images from disk |

**Hangfire Dashboard** is available at `/Jobs` — secured with Basic Authentication.

---

## 📧 Email Notifications

The system sends automatic emails for:

- ✅ **Registration** — email confirmation link on sign-up
- ⏰ **Booking Reminders** — sent before upcoming bookings
- ✅ **Payment Approved** — notifies player when their payment is confirmed
- ❌ **Payment Rejected** — notifies player so they can resubmit
- 🔑 **Password Reset** — reset link sent on forgotten password

---

## 🛠️ Tech Stack

| Category | Technology |
|---|---|
| Framework | .NET 8 / ASP.NET Core |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Identity | ASP.NET Identity |
| Authentication | JWT Bearer |
| Object Mapping | Mapster |
| Validation | FluentValidation + Auto-Validation middleware |
| Background Jobs | Hangfire (SQL Server storage) |
| Caching | HybridCache (.NET 9 in-process + distributed) |
| Logging | Serilog |
| Error Handling | Global Exception Handler + ProblemDetails |

---

## 📁 Project Structure

```
MalaebBooking/
├── MalaebBooking.Domain/
│   ├── Entities/              # Domain entities
│   ├── Abstractions/          # IRepository interfaces, IUnitOfWork
│   ├── Consts/                # Permissions, DefaultRoles, BookingRules
│   └── Enums/                 # BookingStatus, PaymentStatus, etc.
│
├── MalaebBooking.Application/
│   ├── Services/              # Business logic (10 services + Hangfire jobs)
│   ├── Contracts/             # Request/Response DTOs per feature
│   ├── Validators/            # FluentValidation validators
│   ├── Errors/                # Domain error definitions
│   └── Mapping/               # Mapster mapping configuration
│
├── MalaebBooking.Infrastructure/
│   ├── Repositories/          # EF Core repository implementations
│   ├── Authentication/        # JWT provider + permission filters
│   ├── BackgroundJobs/        # Hangfire job classes
│   ├── Mail/                  # Email service implementation
│   ├── EntitiesConfigurations/# EF Core Fluent API configs
│   ├── Persistence/           # ApplicationDbContext
│   ├── Migrations/            # EF Core migrations
│   └── UnitOfWork.cs
│
└── MalaebBooking.Api/
    ├── Controllers/           # 10 API controllers
    ├── Middleware/            # Global exception handler
    ├── Templates/             # Email HTML templates
    ├── DependencyInjection.cs # Main DI composition
    └── Program.cs
```

---

## 🗂️ Domain Entities

| Entity | Description |
|---|---|
| `ApplicationUser` | Extends IdentityUser — stores profile info and refresh tokens |
| `ApplicationRole` | Extends IdentityRole — holds permission claims |
| `Stadium` | Stadium with location, sport type, pricing, and active status |
| `StadiumImage` | Images linked to a stadium |
| `SportType` | Type of sport (Football, Basketball, etc.) |
| `TimeSlot` | Available booking windows for a stadium |
| `Booking` | Booking record linking player, stadium, and time slot |
| `Payment` | Payment record with receipt image and approval status |
| `Review` | Player review after a completed booking |
| `RefreshToken` | Tracks issued refresh tokens per user |

---

## 🔑 Roles & Permissions

Three roles are seeded automatically at startup:

### Admin
Full platform access: manage users, roles, stadiums, bookings, payments, sport types, and reviews.

### Owner
Can manage their own stadiums and images, view their bookings, approve or reject payments, and manage time slots.

### Player
Can view stadiums, create bookings, submit payment proofs, leave reviews, and manage their own profile.

Permissions are granular and checked at the endpoint level, for example:
- `Permissions.Payments.Approve`
- `Permissions.Bookings.Cancel`
- `Permissions.Stadiums.ToggleActive`

---

## 🌐 API Endpoints

> **Base URL:** `https://malaeb-booking.runasp.net`

### 🔒 Auth — `POST /Auth`
| Method | Endpoint | Description |
|---|---|---|
| POST | `/Auth` | Login — returns JWT + refresh token |
| POST | `/Auth/register` | Register new account |
| POST | `/Auth/confirm-email` | Confirm email with token |
| POST | `/Auth/resend-confirmation-email` | Resend confirmation email |
| POST | `/Auth/refresh` | Refresh JWT token |
| POST | `/Auth/revoke` | Revoke refresh token |

### 👤 Users — `api/users`
| Method | Endpoint | Auth | Description |
|---|---|---|---|
| GET | `/api/users/profile` | Player | Get own profile |
| PUT | `/api/users/profile` | Player | Update profile |
| PUT | `/api/users/change-password` | Player | Change password |
| POST | `/api/users/forgot-password` | Public | Send reset link |
| POST | `/api/users/reset-password` | Public | Reset password |
| GET | `/api/users/all` | Admin | Get all users |
| GET | `/api/users/GetById/{id}` | Admin | Get user by ID |
| POST | `/api/users` | Admin | Create user |
| PUT | `/api/users/{id}` | Admin | Update user |
| PATCH | `/api/users/{id}/toggle-status` | Admin | Enable/disable user |
| PATCH | `/api/users/{id}/unlock` | Admin | Unlock locked user |

### 🏟️ Stadiums — `api/stadiums`
| Method | Endpoint | Auth | Description |
|---|---|---|---|
| GET | `/api/stadiums` | Public | Get all stadiums |
| GET | `/api/stadiums/active` | Public | Get active stadiums |
| GET | `/api/stadiums/{id}` | Public | Get stadium by ID |
| GET | `/api/stadiums/{id}/details` | Public | Get stadium full details |
| GET | `/api/stadiums/sport/{sportTypeId}` | Public | Filter by sport type |
| POST | `/api/stadiums` | Owner/Admin | Create stadium |
| PUT | `/api/stadiums/{id}` | Owner/Admin | Update stadium |
| PATCH | `/api/stadiums/{id}/toggle-active` | Admin | Activate/deactivate |

### 🖼️ Stadium Images — `api/stadiumimages`
| Method | Endpoint | Auth | Description |
|---|---|---|---|
| GET | `/api/stadiumimages/{stadiumId}` | Public | Get stadium images |
| POST | `/api/stadiumimages` | Owner/Admin | Upload image |
| PUT | `/api/stadiumimages/{id}` | Owner/Admin | Update image |
| DELETE | `/api/stadiumimages/{id}` | Owner/Admin | Delete image |

### 🏅 Sport Types — `api/sporttypes`
| Method | Endpoint | Auth | Description |
|---|---|---|---|
| GET | `/api/sporttypes` | Public | Get all sport types |
| GET | `/api/sporttypes/{id}` | Public | Get by ID |
| POST | `/api/sporttypes` | Admin | Create sport type |
| PUT | `/api/sporttypes/{id}` | Admin | Update sport type |
| DELETE | `/api/sporttypes/{id}` | Admin | Delete sport type |
| POST | `/api/sporttypes/{id}/upload-icon` | Admin | Upload icon |

### 🕒 Time Slots — `api/timeslots`
| Method | Endpoint | Auth | Description |
|---|---|---|---|
| GET | `/api/timeslots/stadium/{stadiumId}` | Public | Get slots by stadium |
| GET | `/api/timeslots/{id}` | Public | Get slot by ID |
| POST | `/api/timeslots` | Owner/Admin | Create time slot |
| PUT | `/api/timeslots/{id}` | Owner/Admin | Update time slot |
| DELETE | `/api/timeslots/{id}` | Owner/Admin | Delete time slot |

### 📅 Bookings — `api/bookings`
| Method | Endpoint | Auth | Description |
|---|---|---|---|
| POST | `/api/bookings` | Player | Create booking |
| GET | `/api/bookings/{id}` | Authenticated | Get booking by ID |
| GET | `/api/bookings/my-bookings` | Player | Get own bookings |
| GET | `/api/bookings/stadium/{stadiumId}` | Owner/Admin | Get stadium bookings |
| GET | `/api/bookings` | Admin | Get all bookings |
| PUT | `/api/bookings/{id}/status` | Owner/Admin | Update booking status |
| POST | `/api/bookings/{id}/cancel` | Player | Cancel booking |
| DELETE | `/api/bookings/{id}` | Admin | Delete booking |

### 💳 Payments — `api/payments`
| Method | Endpoint | Auth | Description |
|---|---|---|---|
| GET | `/api/payments/booking/{bookingId}` | Player | Get payment info |
| POST | `/api/payments/booking/{bookingId}/submit` | Player | Submit payment proof (multipart/form-data) |
| PUT | `/api/payments/{id}/approve` | Owner/Admin | Approve payment |
| PUT | `/api/payments/{id}/reject` | Owner/Admin | Reject payment |

### ⭐ Reviews — `api/reviews`
| Method | Endpoint | Auth | Description |
|---|---|---|---|
| POST | `/api/reviews` | Player | Create review |
| PUT | `/api/reviews/{id}` | Player | Update review |
| DELETE | `/api/reviews/{id}` | Player | Delete review |
| GET | `/api/reviews/{id}` | Public | Get review by ID |
| GET | `/api/reviews/stadium/{stadiumId}` | Public | Get stadium reviews |
| GET | `/api/reviews/my-reviews` | Player | Get own reviews |
| GET | `/api/reviews/can-review/{stadiumId}` | Player | Check if player can review |

### 👥 Roles — `api/roles`
| Method | Endpoint | Auth | Description |
|---|---|---|---|
| GET | `/api/roles` | Admin | Get all roles |
| GET | `/api/roles/{id}` | Admin | Get role by ID |
| POST | `/api/roles` | Admin | Create role |
| PUT | `/api/roles/{id}` | Admin | Update role |
| PATCH | `/api/roles/{id}/toggle` | Admin | Activate/deactivate |

---

## ⚙️ Configuration

Add the following to your `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-sql-server-connection-string",
    "HangfireConnection": "your-hangfire-db-connection-string"
  },
  "JwtOptions": {
    "Key": "your-secret-key-min-32-chars",
    "Issuer": "MalaebBooking",
    "Audience": "MalaebBookingUsers",
    "ExpiryMinutes": 60
  },
  "MailSetting": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "UserName": "your-email@gmail.com",
    "Password": "your-app-password",
    "DisplayName": "Malaeb Booking"
  },
  "HangfireSettings": {
    "UserName": "admin",
    "Password": "your-dashboard-password"
  }
}
```

---

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (local or remote)
- SMTP-capable email account

### Steps

```bash
# 1. Clone the repository
git clone https://github.com/a7medsale7/MalaebBooking.git
cd MalaebBooking

# 2. Configure appsettings.json with your connection strings and settings

# 3. Apply database migrations
dotnet ef database update --project MalaebBooking.Infrastructure --startup-project MalaebBooking.Api

# 4. Run the API
dotnet run --project MalaebBooking.Api
```

The API will be available at `https://localhost:PORT`.

Hangfire Dashboard: `https://localhost:PORT/Jobs`

---

## 📄 License

This project is open source and available for learning and reference purposes.

---

> Built with ❤️ by [Ahmed Saleh](https://github.com/a7medsale7)