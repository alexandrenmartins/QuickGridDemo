using System.Diagnostics;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using Microsoft.EntityFrameworkCore;
using QuickGridDemo.Data;
using QuickGridDemo.Models;

namespace QuickGridDemo.Components.Pages;

public class Employees4Adaptor : DataAdaptor
{
    private readonly IDbContextFactory<ApplicationDbContextPostgreSql> _dbContextFactory;
    private readonly SqlCaptureService _sqlCapture;

    private List<Employee>? _lastPage;
    private int _lastTotalCount;
    private int _lastSkip;
    private int _lastTake;
    private string? _lastFirstNameFilter;
    private string? _lastLastNameFilter;

    private int? _lastPageFirstId;
    private int? _lastPageLastId;

    public Employees4Adaptor(
        IDbContextFactory<ApplicationDbContextPostgreSql> dbContextFactory,
        SqlCaptureService sqlCapture)
    {
        _dbContextFactory = dbContextFactory;
        _sqlCapture = sqlCapture;
    }

    public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
    {
        _sqlCapture.Reset();

        string firstNameFilter = null, lastNameFilter = null;

        if (dm.Params != null)
        {
            if (dm.Params.TryGetValue("FirstNameFilter", out var fn) && fn is string fnStr)
                firstNameFilter = fnStr;
            if (dm.Params.TryGetValue("LastNameFilter", out var ln) && ln is string lnStr)
                lastNameFilter = lnStr;
        }

        bool samePage = _lastPage != null
            && _lastSkip == dm.Skip
            && _lastTake == dm.Take
            && _lastFirstNameFilter == firstNameFilter
            && _lastLastNameFilter == lastNameFilter;

        if (samePage && dm.Sorted != null && dm.Sorted.Count > 0)
        {
            _sqlCapture.Strategy = "cache-hit (re-sort in-memory)";
            _sqlCapture.IdsQuerySql = $"-- Dados do cache (página {_lastSkip / _lastTake + 1}, {_lastPage!.Count} itens)";

            var sort = dm.Sorted.First();
            bool descending = IsDescending(sort.Direction);
            var sorted = ApplySorting(_lastPage, sort.Name, descending).ToList();
            return new DataResult { Result = sorted, Count = _lastTotalCount };
        }

        using var context = await _dbContextFactory.CreateDbContextAsync();
        IQueryable<Employee> query = context.Employees.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(firstNameFilter))
            query = query.Where(e => e.FirstName.ToLower().Contains(firstNameFilter.ToLower()));

        if (!string.IsNullOrWhiteSpace(lastNameFilter))
            query = query.Where(e => e.LastName.ToLower().Contains(lastNameFilter.ToLower()));

        int totalCount = await query.CountAsync();

        int skip = dm.Skip;
        int take = dm.Take > 0 ? dm.Take : 15;

        var sw = Stopwatch.StartNew();
        var ids = await FetchIdsWithKeysetAsync(query, skip, take);
        sw.Stop();
        _sqlCapture.IdsQueryMs = sw.ElapsedMilliseconds;
        _sqlCapture.IdsQueryRows = ids.Count;

        var dataQuery = context.Employees
            .AsNoTracking()
            .Where(e => ids.Contains(e.EmployeeID))
            .OrderBy(e => e.EmployeeID);
        _sqlCapture.DataQuerySql = dataQuery.ToQueryString();

        sw.Restart();
        var page = ids.Count == 0
            ? new List<Employee>()
            : await dataQuery.ToListAsync();
        sw.Stop();
        _sqlCapture.DataQueryMs = sw.ElapsedMilliseconds;

        if (dm.Sorted != null && dm.Sorted.Count > 0)
        {
            var sort = dm.Sorted.First();
            bool descending = IsDescending(sort.Direction);
            page = ApplySorting(page, sort.Name, descending).ToList();
        }

        _lastPage = page;
        _lastTotalCount = totalCount;
        _lastSkip = skip;
        _lastTake = take;
        _lastFirstNameFilter = firstNameFilter;
        _lastLastNameFilter = lastNameFilter;
        _lastPageFirstId = ids.Count > 0 ? ids[0] : null;
        _lastPageLastId = ids.Count > 0 ? ids[^1] : null;

        return dm.RequiresCounts
            ? new DataResult { Result = page, Count = totalCount }
            : (object)page;
    }

    private async Task<List<int>> FetchIdsWithKeysetAsync(IQueryable<Employee> baseQuery, int skip, int take)
    {
        if (skip == 0)
        {
            var q = baseQuery
                .OrderBy(e => e.EmployeeID)
                .Select(e => e.EmployeeID)
                .Take(take);
            _sqlCapture.IdsQuerySql = q.ToQueryString();
            _sqlCapture.Strategy = "page1 (LIMIT sem cursor)";
            return await q.ToListAsync();
        }

        if (skip == _lastSkip + take && _lastPageLastId.HasValue)
        {
            var q = baseQuery
                .Where(e => e.EmployeeID > _lastPageLastId.Value)
                .OrderBy(e => e.EmployeeID)
                .Select(e => e.EmployeeID)
                .Take(take);
            _sqlCapture.IdsQuerySql = q.ToQueryString();
            _sqlCapture.Strategy = $"keyset-next (cursor > {_lastPageLastId.Value})";
            return await q.ToListAsync();
        }

        if (skip == _lastSkip - take && _lastPageFirstId.HasValue)
        {
            var q = baseQuery
                .Where(e => e.EmployeeID < _lastPageFirstId.Value)
                .OrderByDescending(e => e.EmployeeID)
                .Select(e => e.EmployeeID)
                .Take(take);
            _sqlCapture.IdsQuerySql = q.ToQueryString();
            _sqlCapture.Strategy = $"keyset-prev (cursor < {_lastPageFirstId.Value})";

            var ids = await q.ToListAsync();
            ids.Reverse();
            return ids;
        }

        var cursorQuery = baseQuery
            .OrderBy(e => e.EmployeeID)
            .Select(e => (int?)e.EmployeeID)
            .Skip(skip - 1)
            .Take(1);
        _sqlCapture.CursorLookupSql = cursorQuery.ToQueryString();

        var cursor = await cursorQuery.FirstOrDefaultAsync();

        if (cursor == null)
        {
            _sqlCapture.Strategy = "cursor-lookup (vazio — página além do fim)";
            return new List<int>();
        }

        var keysetQuery = baseQuery
            .Where(e => e.EmployeeID > cursor.Value)
            .OrderBy(e => e.EmployeeID)
            .Select(e => e.EmployeeID)
            .Take(take);
        _sqlCapture.IdsQuerySql = keysetQuery.ToQueryString();
        _sqlCapture.Strategy = $"cursor-lookup+keyset (cursor > {cursor.Value})";

        return await keysetQuery.ToListAsync();
    }

    private static bool IsDescending(object direction)
    {
        return direction switch
        {
            string s => s.Equals("Descending", StringComparison.OrdinalIgnoreCase),
            int i => i > 0,
            bool b => b,
            _ => false
        };
    }

    private static IOrderedEnumerable<Employee> ApplySorting(IEnumerable<Employee> items, string sortName, bool descending)
    {
        return sortName switch
        {
            nameof(Employee.EmployeeID) => descending
                ? items.OrderByDescending(e => e.EmployeeID)
                : items.OrderBy(e => e.EmployeeID),
            nameof(Employee.FirstName) => descending
                ? items.OrderByDescending(e => e.FirstName)
                : items.OrderBy(e => e.FirstName),
            nameof(Employee.LastName) => descending
                ? items.OrderByDescending(e => e.LastName)
                : items.OrderBy(e => e.LastName),
            nameof(Employee.Title) => descending
                ? items.OrderByDescending(e => e.Title)
                : items.OrderBy(e => e.Title),
            nameof(Employee.City) => descending
                ? items.OrderByDescending(e => e.City)
                : items.OrderBy(e => e.City),
            nameof(Employee.Country) => descending
                ? items.OrderByDescending(e => e.Country)
                : items.OrderBy(e => e.Country),
            nameof(Employee.HomePhone) => descending
                ? items.OrderByDescending(e => e.HomePhone)
                : items.OrderBy(e => e.HomePhone),
            nameof(Employee.HireDate) => descending
                ? items.OrderByDescending(e => e.HireDate)
                : items.OrderBy(e => e.HireDate),
            _ => descending
                ? items.OrderByDescending(e => e.EmployeeID)
                : items.OrderBy(e => e.EmployeeID),
        };
    }
}
