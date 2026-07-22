# AtDrive — Order Management Mini-System

A small but complete ASP.NET Core 8 MVC application for viewing, creating, and updating customer
orders with line items. Built for the AtDrive ASP.NET developer coding test.

## Tech stack

- ASP.NET Core 8 MVC with Razor views
- Entity Framework Core 8 (SQL Server) for data access; schema and seed data from a plain SQL script
- SQL Server LocalDB by default (any SQL Server works — just change the connection string)
- jQuery / AJAX for the create-order modal
- Cookie authentication securing the write actions (Create / Edit / Delete)
- xUnit + Moq for unit tests

## Architecture

The solution is layered — controllers never touch EF directly:

```
Controllers  →  Services (business logic)  →  Repositories (EF Core)  →  SQL Server
                     ↑ unit tested with a mocked repository
```

- `src/OrderManagement.Web` — the MVC application
  - `Controllers/` — `OrdersController` (CRUD + AJAX create + stored-proc filter), `AccountController` (demo sign-in), `HomeController` (error pages)
  - `Services/` — `OrderService`: order mapping, item reconciliation on update, and the LINQ total calculation (`Items.Sum(i => i.Quantity * i.UnitPrice)`)
  - `Repositories/` — `OrderRepository`: EF Core queries incl. `Include()` eager loading and the `GetOrdersByStatus` stored procedure via `FromSqlInterpolated`
  - `Data/` — `OrderDbContext` (EF entity-to-table mapping) and `DbInitializer` (runs the SQL script at startup)
  - `Database/AtDriveOrders.sql` — the single source of truth for the schema: both tables,
    the `GetOrdersByStatus` stored procedure, and the seed data, all as plain idempotent T-SQL
- `tests/OrderManagement.Tests` — xUnit tests for the service layer

## Feature checklist (vs. the brief)

| Requirement | Where |
|---|---|
| Layered Controller → Service → Repository | `Controllers/`, `Services/`, `Repositories/` |
| Orders + OrderItems tables (FK, cascade delete) | `Database/AtDriveOrders.sql` (+ EF mapping in `Data/OrderDbContext.cs`) |
| Index page with Create / Edit / Delete | `Views/Orders/Index.cshtml` |
| CRUD via EF Core + Razor views | `OrdersController`, `Views/Orders/` |
| Eager loading of line items | `OrderRepository.GetByIdAsync` (`.Include(o => o.Items)`) |
| AJAX create (no page reload) | Bootstrap modal + jQuery POST in `Index.cshtml` |
| Order total via LINQ | `OrderService.CalculateOrderTotal` |
| Stored procedure filter | `dbo.GetOrdersByStatus`, called from `OrderRepository.GetByStatusAsync` |
| Server-side validation | Data annotations on view models + `ModelState` checks |
| Friendly error handling | 404 "order not found" view, global error page, no stack traces outside Development |
| `[Authorize]` on write actions | `OrdersController` Create / Edit / Delete + cookie auth (viewing is public) |
| Unit tests | `tests/OrderManagement.Tests/OrderServiceTests.cs` (5 tests) |
| Bonus: search / filter | Status dropdown (stored proc) + customer name search on Index |
| Bonus: client-side validation | Highlighted invalid fields in the create modal; unobtrusive validation on Edit |
| Bonus: "not found" test | `DeleteOrderAsync_ReturnsFalse_...` / `GetOrderAsync_ReturnsNull_...` |

## Getting started

Prerequisites: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) and SQL Server
LocalDB (ships with Visual Studio; also in the SQL Server Express installer).

```bash
git clone <repo-url>
cd AtDrive
dotnet run --project src/OrderManagement.Web
```

Then open the URL shown in the console (http://localhost:5077 by default; from
Visual Studio just press F5).

### Database setup

None required. On startup the app creates the empty `AtDriveOrders` database if it
doesn't exist, then executes `src/OrderManagement.Web/Database/AtDriveOrders.sql`, which creates both tables,
the `GetOrdersByStatus` stored procedure, and ten seeded sample orders. The script is
idempotent, so restarts are safe; you can also run it manually against an empty
database if you prefer.

Using a different SQL Server instance? Edit `ConnectionStrings:DefaultConnection` in
`src/OrderManagement.Web/appsettings.json`, e.g.:

```
Server=.\SQLEXPRESS;Database=AtDriveOrders;Trusted_Connection=True;TrustServerCertificate=True
```

### Signing in (for Create / Edit / Delete)

Anyone can browse and view orders, but creating, editing, or deleting an order
requires authentication (`[Authorize]`). Use the demo account:

- Username: `admin`
- Password: `Admin@123`

(Configured in `appsettings.json` under `DemoAuth` — the brief asked for basic
authorization on one action, not a full identity provider.)

### Running the tests

```bash
dotnet test
```


