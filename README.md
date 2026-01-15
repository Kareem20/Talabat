# 📦 Talabat -- ASP.NET Core Web API

A **Talabat-inspired e-commerce backend API** built with **ASP.NET
Core**, designed to practice **real-world backend engineering concepts**
including **Clean Architecture**, **Redis caching**, **payment
integration**, and **scalable API design**.

This project focuses on **how production-ready backend systems are
structured**, not just CRUD endpoints.

------------------------------------------------------------------------

## 🎯 Project Goals

-   Practice **ASP.NET Core Web API** development
-   Apply **Clean / Onion Architecture**
-   Use **Redis distributed caching**
-   Implement **payment workflows**
-   Follow **SOLID principles & design patterns**
-   Build a scalable, maintainable backend system

------------------------------------------------------------------------

## 🏗 Architecture Overview

The project follows **Clean Architecture (Onion Architecture)**.

     Talabat
    ├── Talabat.Core
    │   ├── Entities
    │   ├── Interfaces
    │   └── Specifications
    │
    ├── Talabat.Repository
    │   ├── Data (DbContext)
    │   ├── Repositories
    │   └── Configurations
    │
    ├── Talabat.Service
    │   ├── Business Services
    │   └── Service Interfaces
    │
    └── Talabat.APIs
        ├── Controllers
        ├── DTOs
        ├── Middlewares
        └── Extensions

------------------------------------------------------------------------
## 📌 Architectural Principles

### Dependency Inversion:
High-level modules depend on abstractions, not implementations

### Separation of Concerns:
API, business logic, and data access are isolated

### Testability & Maintainability

### Framework Independence
------------------------------------------------------------------------
## Core Features
### 🔐 Authentication & Authorization

-  JWT-based authentication

-  Secure protected endpoints

-  Token validation & authorization middleware

### 📦 Product Catalog

-  Product listing with filtering

-  Pagination & sorting

-  Efficient querying using specifications

### 🛍 Basket & Orders

-  Add/remove items from the basket

-  Create orders from the basket

-  Retrieve order history

### 💳 Payments

-  Payment intent creation

-  Secure payment processing (e.g., Stripe)

-  Payment confirmation workflow

-  Designed to support multiple gateways

------------------------------------------------------------------------
## 🧩 Design Patterns & Practices

| Pattern / Practice           | Description                          |
|-----------------------------|--------------------------------------|
| Repository Pattern          | Abstracts data access logic          |
| Unit of Work                | Ensures transactional consistency   |
| Specification Pattern       | Encapsulates complex query logic     |
| DTO Pattern                 | Prevents domain leakage to API       |
| Dependency Injection        | Loosely coupled services             |
| Global Exception Handling   | Centralized error handling           |

---------------------------------------------------------------------------
# ⚡ Redis Distributed Caching

This project uses **Redis** as a distributed cache to optimize performance for **read-heavy endpoints**.

---

## 🧠 Why Redis?

Caching helps reduce:

- ⏱ **Response Time**
- 🗄 **Database Load**
- 📈 **Scalability Risk**

---

## 🛠 Caching Strategy (Cache-Aside Pattern)

1. API checks the Redis cache  
2. If **cache hit** → return cached data  
3. If **cache miss** → query database and store result in Redis  

**Flow:**

- Request → Cache → Database (on miss) → Cache


---

## ✨ Implementation Highlights

- Uses `IDistributedCache` abstraction  
- Redis configured using `StackExchange.Redis`  
- Supports **absolute** and **sliding** expiration  

---

## 🔧 Redis Configuration (C#)

```csharp
services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        configuration.GetConnectionString("Redis");
});
```
✅ Benefits

-  Faster API responses

-  Reduced SQL Server queries

-  Improved scalability

-  Stateless API design

------------------------------------------------------------------------
## 🛠 Technologies & Tools

| Technology            | Purpose             |
| --------------------- | ------------------- |
| ASP.NET Core          | Web API             |
| Entity Framework Core | ORM                 |
| SQL Server            | Relational database |
| Redis                 | Distributed caching |
| JWT                   | Authentication      |
| Stripe                | Payments            |
| Swagger / OpenAPI     | API documentation   |
| AutoMapper            | Object mapping      |


---------------------------------------------------------------------------
## ⚙ Getting Started

### 📋 Prerequisites

-  .NET SDK (6 / 7 / 8)

-  SQL Server

-  Redis

-  Stripe account (for payments)

### 📥 Installation

```bash
git clone https://github.com/Kareem20/Talabat.git
cd Talabat
```

### 🔧 Configuration

- Update `appsettings.json`:

  -  Database connection string

  -  Redis connection string

  -  JWT settings

  -  Stripe API keys
### Run the API
```
dotnet restore
dotnet build
dotnet run --project Talabat.APIs
```
---------------------------------------------------------------------------

## 📐 Design Decisions

### ✅ Clean Architecture

-  Core logic is independent of frameworks

-  Easy to test and extend

-  Infrastructure can be replaced without affecting business logic

### ✅ Redis at Infrastructure Layer

-  No Redis dependency in Core

-  Cache implementation is replaceable

### ✅ Payment Design

-  Isolated payment service

-  Easy to extend for additional providers

---------------------------------------------------------------------------

## 🧠 What This Project Demonstrates

-  Real-world ASP.NET Core API design

-  Performance optimization using Redis

-  Clean, scalable backend architecture

-  Payment workflows using the Stripe gateway

-  Professional backend engineering practices

---------------------------------------------------------------------------

📜 License

This project is created for learning and practice purposes.
