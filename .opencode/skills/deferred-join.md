---
descrição: Implementa Deferred Join pagination para QuickGrid/EF Core
modo: subagente
ferramentas:
  escrever: verdadeiro
  editar: verdadeiro
---

# Deferred Join Pagination - Blazor QuickGrid + EF Core

Técnica de paginação baseada em OFFSET que otimiza consultas usando subquery para buscar apenas IDs antes de buscar linhas completas.

## Conceito

Ao invés de `SELECT * ORDER BY id LIMIT 15 OFFSET 10000` (escaneia e descarta 10k linhas completas), usar subquery para buscar apenas IDs primeiro:

```sql
-- Lento: lê 10k linhas completas, descarta
SELECT * FROM Employees ORDER BY EmployeeID LIMIT 15 OFFSET 10000;

-- Rápido: subquery escaneia índice estreito, busca apenas 15 linhas completas
SELECT e.*
FROM Employees e
INNER JOIN (
    SELECT EmployeeID FROM Employees ORDER BY EmployeeID LIMIT 15 OFFSET 10000
) AS page_ids USING (EmployeeID)
ORDER BY e.EmployeeID;
```

## Por que funciona

- Subquery usa **covering index** (apenas `EmployeeID`), examinando dados mínimos
- Query externa busca linhas completas apenas para os 15 IDs encontrados
- Reduz dramaticamente I/O em tabelas grandes

## Código EF Core (Employees.razor)

```csharp
// Query 1: buscar apenas IDs (covering index scan)
var ids = await query
    .Select(e => e.EmployeeID)
    .Skip(offset).Take(QTD_ITENS_PER_PAGE)
    .ToListAsync();

// Query 2: buscar linhas completas para os IDs encontrados
employeesPage = await context.Employees
    .AsNoTracking()
    .Where(e => ids.Contains(e.EmployeeID))
    .OrderBy(e => e.EmployeeID)
    .ToListAsync();
```

## Padrão de Paginação

```csharp
// Variáveis de estado
private int currentOffset = 0;
private int currentPage => currentOffset / QTD_ITENS_PER_PAGE + 1;
private int totalPages => (int)Math.Ceiling((double)filteredCount / QTD_ITENS_PER_PAGE);
private bool hasPreviousPage => currentOffset > 0;
private bool hasMore => currentOffset + QTD_ITENS_PER_PAGE < filteredCount;

// Navegação
private async Task GoToFirstPageAsync() => await LoadPageAsync(0);
private async Task GoToNextPageAsync() => await LoadPageAsync(currentOffset + QTD_ITENS_PER_PAGE);
private async Task GoToPreviousPageAsync() => await LoadPageAsync(Math.Max(0, currentOffset - QTD_ITENS_PER_PAGE));
private async Task GoToPageAsync(int pageNumber) => await LoadPageAsync((pageNumber - 1) * QTD_ITENS_PER_PAGE);
```

## Comparação: Keyset vs Deferred Join

| Aspecto | Keyset (`WHERE id > @cursor`) | Deferred Join (OFFSET) |
|---------|-------------------------------|------------------------|
| Performance | O(1) constante | O(n) - degrada com offset alto |
| Páginas arbitrárias | Não (só next/first) | Sim (pode ir a qualquer página) |
| Queries por página | 1 | 2 (subquery + join) |
| Complexidade | Média | Média |

## Quando usar

- **Deferred Join**: Quando precisa de navegação por número de página
- **Keyset**: Quando performance é crítica e navegação sequencial é suficiente

## Referência

- PlanetScale: https://planetscale.com/learn/courses/mysql-for-developers/examples/deferred-joins
- High Performance MySQL, 3rd Edition
- Arquivo: `Components/Pages/Employees.razor`
