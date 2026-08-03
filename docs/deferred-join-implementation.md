# Deferred Join Pagination - QuickGrid + EF Core

## Visão Geral

Este documento descreve a implementação da técnica **Deferred Join Pagination** na página `Employees.razor` do projeto QuickGridDemo.

## O Problema

Paginação com `OFFSET` em tabelas grandes é ineficiente:

```sql
SELECT * FROM Employees ORDER BY EmployeeID LIMIT 15 OFFSET 10000;
```

O PostgreSQL escaneia **10.000 linhas completas** apenas para descartá-las e retornar as últimas 15.

## A Solução: Deferred Join

Técnica que usa **covering index** para buscar apenas IDs primeiro, depois busca linhas completas apenas para esses IDs:

```sql
-- Query 1: Subquery busca apenas IDs (covering index scan)
SELECT EmployeeID FROM Employees ORDER BY EmployeeID LIMIT 15 OFFSET 10000;

-- Query 2: Busca linhas completas apenas para os 15 IDs
SELECT e.*
FROM Employees e
INNER JOIN (
    SELECT EmployeeID FROM Employees ORDER BY EmployeeID LIMIT 15 OFFSET 10000
) AS page_ids USING (EmployeeID)
ORDER BY e.EmployeeID;
```

## Implementação em C# (EF Core)

### Código Completo do Método `LoadPageAsync`

```csharp
private async Task LoadPageAsync(int offset)
{
    using var context = DbContextFactory.CreateDbContext();

    IQueryable<Employee> query = context.Employees
        .AsNoTracking()
        .OrderBy(e => e.EmployeeID);

    // Aplicar filtros
    if (!string.IsNullOrWhiteSpace(firstNameFilter))
        query = query.Where(e => e.FirstName.ToLower().Contains(firstNameFilter.ToLower()));

    if (!string.IsNullOrWhiteSpace(lastNameFilter))
        query = query.Where(e => e.LastName.ToLower().Contains(lastNameFilter.ToLower()));

    filteredCount = await query.CountAsync();

    // Deferred Join: Query 1 - buscar apenas IDs (covering index scan)
    var ids = await query
        .Select(e => e.EmployeeID)
        .Skip(offset).Take(QTD_ITENS_PER_PAGE)
        .ToListAsync();

    // Deferred Join: Query 2 - buscar linhas completas para os IDs encontrados
    employeesPage = await context.Employees
        .AsNoTracking()
        .Where(e => ids.Contains(e.EmployeeID))
        .OrderBy(e => e.EmployeeID)
        .ToListAsync();

    currentOffset = offset;
}
```

### Variáveis de Estado

```csharp
private const int QTD_ITENS_PER_PAGE = 15;
private int currentOffset = 0;
private int filteredCount = 0;
private List<Employee> employeesPage = new();

// Propriedades derivadas
private int currentPage => currentOffset / QTD_ITENS_PER_PAGE + 1;
private int totalPages => (int)Math.Ceiling((double)filteredCount / QTD_ITENS_PER_PAGE);
private int startPosition => filteredCount > 0 ? currentOffset + 1 : 0;
private int endPosition => Math.Min(currentOffset + employeesPage.Count, filteredCount);
private bool hasPreviousPage => currentOffset > 0;
private bool hasMore => currentOffset + QTD_ITENS_PER_PAGE < filteredCount;
```

### Métodos de Navegação

```csharp
private async Task GoToFirstPageAsync()
{
    await LoadPageAsync(0);
}

private async Task GoToNextPageAsync()
{
    if (hasMore)
        await LoadPageAsync(currentOffset + QTD_ITENS_PER_PAGE);
}

private async Task GoToPreviousPageAsync()
{
    if (hasPreviousPage)
        await LoadPageAsync(Math.Max(0, currentOffset - QTD_ITENS_PER_PAGE));
}

private async Task GoToPageAsync(int pageNumber)
{
    var offset = Math.Max(0, (pageNumber - 1) * QTD_ITENS_PER_PAGE);
    var maxOffset = Math.Max(0, (int)(Math.Ceiling((double)filteredCount / QTD_ITENS_PER_PAGE) - 1) * QTD_ITENS_PER_PAGE);
    offset = Math.Min(offset, maxOffset);
    await LoadPageAsync(offset);
}
```

## Comparação: Keyset vs Deferred Join

| Aspecto | Keyset (`WHERE id > @cursor`) | Deferred Join (OFFSET) |
|---------|-------------------------------|------------------------|
| Performance | O(1) constante | O(n) - degrada com offset alto |
| Páginas arbitrárias | Não (só next/first) | Sim (pode ir a qualquer página) |
| Queries por página | 1 | 2 (subquery + join) |
| Complexidade | Média | Média |

### Quando usar cada uma

- **Keyset Pagination**: Performance crítica, navegação sequencial, datasets muito grandes
- **Deferred Join**: Navegação por número de página, UX que precisa de "Página X de Y"

## Referências

- [PlanetScale - Deferred Joins](https://planetscale.com/learn/courses/mysql-for-developers/examples/deferred-joins)
- [High Performance MySQL, 3rd Edition](https://www.oreilly.com/library/view/high-performance-mysql/9781492080534/)
- [PlanetScale Blog - Pagination](https://planetscale.com/blog/mysql-pagination)
- [Hack MySQL - Deferred Join Deep Dive](https://hackmysql.com/deferred-join-deep-dive/)

## Arquivo Relacionado

- `Components/Pages/Employees.razor` - Implementação completa

## Employees3.razor — CTE + Keyset + Deferred Join

- CTE: usa SQL cru com WITH:
- WITH page_ids AS (SELECT "EmployeeID" ...) em LoadPageAsync (Employees3.razor:341)
- WITH last_page_ids AS (... OFFSET ...) em GoToLastPageAsync (Employees3.razor:419)
- Keyset Pagination: cursor WHERE "EmployeeID" > {cursor} (Employees3.razor:343), currentCursorId + pilha cursorHistory (linhas 252-255, 383-386)
- Deferred Join: INNER JOIN page_ids p ON e."EmployeeID" = p."EmployeeID" (Employees3.razor:348) — os IDs são selecionados na CTE e as linhas completas buscadas só para eles, tudo numa única query (FromSqlRaw)


