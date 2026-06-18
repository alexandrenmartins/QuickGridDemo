# QuickGridDemo

Blazor Server demo app showcasing `Microsoft.AspNetCore.Components.QuickGrid` with multiple data sources.

## Build & Run

```bash
dotnet build
dotnet run
```

Dev URL: `http://localhost:5149` (http profile) or `https://localhost:7197` (https profile)

## Architecture

- **Framework**: ASP.NET Core 10, Blazor Server (InteractiveServer render mode)
- **Entry**: `Program.cs` → registers DbContexts, auto-migrates, seeds data, then runs the Blazor app
- **UI**: `Components/` → Razor pages and layout under `Components/Pages/` and `Components/Layout/`

### Database Contexts (all in `Data/`)

The app uses three separate EF Core DbContexts, each with its own database:

| Context | DB | Tables | Connection String Key |
|---|---|---|---|
| `ApplicationDbContextSqlServer` | SQL Server (LocalDB) | Customers, Suppliers | `QuickGridConnection` |
| `ApplicationDbContextPostgreSql` | PostgreSQL | Employees | `PostgreSqlConnection` |
| `ApplicationDbContextSqlite` | SQLite | (same as SqlServer) | `SqliteConnection` |

`ApplicationDbContext` is the base class. The PostgreSQL context overrides `OnModelCreating` to ignore `Customer` and `Supplier`.

**Important**: Migrations live in separate subfolders under `Data/Migrations/`:
- `SqlServerMigrations/` for SQL Server
- `PostgreSqlMigrations/` for PostgreSQL

When adding a migration, specify the target context:
```bash
dotnet ef migrations add <Name> --context ApplicationDbContextSqlServer --output-dir Data/Migrations/SqlServerMigrations
dotnet ef migrations add <Name> --context ApplicationDbContextPostgreSql --output-dir Data/Migrations/PostgreSqlMigrations
```

### Seed Data

`Data/SeedData.cs` uses Bogus to generate 10,000 records per table. Runs automatically on first startup when tables are empty.

## Pages

| Route | Data Source | Notes |
|---|---|---|
| `/` | In-memory `List<Car>` | Standard `PaginationState` pagination |
| `/suppliers` | SQL Server | QuickGrid with EF adapter |
| `/customers` | SQL Server | CRUD with modal forms |
| `/employees` | PostgreSQL | **Deferred Join pagination** (offset-based with subquery optimization) |
| `/books` | External API (OpenLibrary) | `GridItemsProvider` with `HttpClient` |

## Key QuickGrid Patterns

- **Offset pagination** (`PaginationState`): Used on Home and Books pages
- **Deferred Join pagination**: Used on Employees page — fetches IDs via subquery (covering index scan), then fetches full rows only for matching IDs
- **`GridItemsProvider`**: Used for server-side data (Books page fetches from external API)
- **`Items` binding**: Used for client-side data (Home, Employees, Suppliers)

## Keyset Pagination — Deferred Join Technique

The Employees page uses deferred join pagination with offset-based navigation. For deeper context on offset-based pagination optimization:

**Deferred Join** (from PlanetScale / High Performance MySQL): Instead of `SELECT * FROM t ORDER BY id LIMIT 15 OFFSET 10000` (scans + discards 10k full rows), use a subquery to fetch only IDs first:

```sql
-- Slow: reads 10k full rows, throws them away
SELECT * FROM Employees ORDER BY EmployeeID LIMIT 15 OFFSET 10000;

-- Faster: subquery scans narrow index, outer query fetches only 15 full rows
SELECT e.*
FROM Employees e
INNER JOIN (
    SELECT EmployeeID FROM Employees ORDER BY EmployeeID LIMIT 15 OFFSET 10000
) AS page_ids USING (EmployeeID)
ORDER BY e.EmployeeID;
```

**Why it works**: The subquery uses a covering index (only `EmployeeID`), so the DB examines minimal data before the offset. The outer query then fetches full rows only for the 15 matching IDs.

**Keyset vs Deferred Join**: Keyset (`WHERE id > @cursor`) is O(1) regardless of depth — the current Employees page pattern. Deferred join is an optimization for offset-based pagination when you need arbitrary page numbers.

**EF Core equivalent** (used in Employees.razor):
```csharp
var ids = await context.Employees
    .OrderBy(e => e.EmployeeID)
    .Select(e => e.EmployeeID)
    .Skip(offset).Take(pageSize)
    .ToListAsync();

var page = await context.Employees
    .Where(e => ids.Contains(e.EmployeeID))
    .OrderBy(e => e.EmployeeID)
    .ToListAsync();
```

**Full implementation**: See `docs/deferred-join-implementation.md` or `.opencode/skills/deferred-join.md`

## Conventions

- UI strings are in **Portuguese (Brazilian)**
- Validation messages use Portuguese (`ErrorMessage` attributes on models)
- Bootstrap 5 for styling (included in `wwwroot/lib/bootstrap/`)
- Custom CSS in `wwwroot/app.css`
- Page titles follow pattern: `QuickGridDemo - <Entity>`

## Configuration

- `appsettings.json`: Main config with connection strings
- `appsettings.Local.json`: Optional local overrides (loaded if present, gitignored)
- `appsettings.Development.json`: Dev-specific settings

## Gotchas

- PostgreSQL connection string in `appsettings.json` has an empty password — you must provide credentials
- Auto-migration runs on startup for SQL Server and PostgreSQL; no manual migration step needed for dev
- The PostgreSQL context explicitly ignores `Customer` and `Supplier` — do not add `DbSet<Customer>` to it
- Employees page uses keyset pagination, not `PaginationState` — when modifying, preserve this pattern
- `appsettings.Local.json` is optional and not committed — create it locally if you need local connection string overrides
