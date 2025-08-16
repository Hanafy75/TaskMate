# 📝 TaskMate

**TaskMate** is the robust and scalable **backend** for a project management application.
It is built with **ASP.NET Core** following **Clean Architecture** principles.

---

## ✨ Key Features

* **User Authentication** – Secure registration and login with **JWT**
* **Project Management** – Full CRUD operations for projects
* **Board Management**

  * Create boards within projects
  * Create standalone boards independent of any project
  * Full CRUD support for boards
* **Task Management** – Add, update, and delete task items within boards
* **Enum Support** – Tasks have a defined state (`ToDo`, `InProgress`, etc.) managed via a flexible lookup system
* **Secure by Design** – Every endpoint is protected and enforces strict ownership checks

---

## 🚀 Technology Stack

* **.NET 9** – Latest LTS version
* **ASP.NET Core** – Cross-platform, high-performance API framework
* **Entity Framework Core** – ORM for database access
* **MediatR** – CQRS (Command/Query Responsibility Segregation) implementation
* **JWT Bearer Tokens** – Stateless, secure authentication
* **Mapster** – Lightweight object-to-object mapping
* **Custom API Response Wrapper** – Consistent API responses
* **Scalar** – API documentation & testing

---

## 🏗️ Architecture

TaskMate is designed to be **maintainable, scalable, and testable**, built on modern design principles:

* **Clean Architecture**
  Organized into `Domain`, `Application`, `Infrastructure`, and `WebAPI` layers.
  Dependency rule: all dependencies point inwards.

* **CQRS with MediatR**
  Each operation is a **Command** (write) or **Query** (read).
  Keeps business logic focused and testable.

* **Repository & Unit of Work Pattern**
  Abstracts data access to keep the Application layer independent from database tech.

* **Secure by Design**
  Backend is the **source of truth** (e.g., `UserId`).
  Frontend is never trusted with ownership checks.
  Every endpoint validates user authorization.

---

## 🏁 Getting Started

### ✅ Prerequisites

* **.NET 8 SDK**
* Database provider (SQL Server, PostgreSQL, or SQLite)
* IDE (Visual Studio, VS Code, JetBrains Rider)

---

### 🔧 Backend Setup

1. **Clone the repository**

   ```bash
   git clone https://your-repository-url.com/
   ```

2. **Configure User Secrets**

   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Your_Database_Connection_String"
   dotnet user-secrets set "JWT:Key" "YourSuperSecretKeyThatIsLongAndComplex"
   dotnet user-secrets set "JWT:Issuer" "https://localhost:7106"
   dotnet user-secrets set "JWT:Audience" "https://localhost:7106"
   ```

3. **Apply Database Migrations**

   ```bash
   dotnet ef database update
   ```

4. **Run the API**

   ```bash
   dotnet run
   ```

The API will be running at:
👉 [https://localhost:7106](https://localhost:7106)

API documentation available at:

* `/scalar`
* `/openapi`
