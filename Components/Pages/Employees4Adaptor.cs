using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using Microsoft.EntityFrameworkCore;
using QuickGridDemo.Data;
using QuickGridDemo.Models;

namespace QuickGridDemo.Components.Pages;

public class Employees4Adaptor : DataAdaptor
{
    private readonly IDbContextFactory<ApplicationDbContextPostgreSql> _dbContextFactory;

    private List<Employee>? _lastPage;
    private int _lastTotalCount;
    private int _lastSkip;
    private int _lastTake;
    private string? _lastFirstNameFilter;
    private string? _lastLastNameFilter;

    public Employees4Adaptor(IDbContextFactory<ApplicationDbContextPostgreSql> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
    {
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
            var sort = dm.Sorted.First();
            bool descending = IsDescending(sort.Direction);
            var sorted = ApplySorting(_lastPage!, sort.Name, descending).ToList();
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

        var ids = await query
            .OrderBy(e => e.EmployeeID)
            .Select(e => e.EmployeeID)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        var page = await context.Employees
            .AsNoTracking()
            .Where(e => ids.Contains(e.EmployeeID))
            .ToListAsync();

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

        return dm.RequiresCounts
            ? new DataResult { Result = page, Count = totalCount }
            : (object)page;
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
