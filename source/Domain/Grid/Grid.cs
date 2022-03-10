using DotNetCore.Extensions;
using SqlSugar;

namespace DotNetCore.Objects;

public class Grid<T>
{
    public Grid(ISugarQueryable<T> queryable, GridParameters parameters)
    {
        Parameters = parameters;

        if (queryable is null || parameters is null) return;

        queryable = Filter(queryable, parameters.Filters);

        Count = queryable.Count();

        queryable = Order(queryable, parameters.Order);

        queryable = Page(queryable, parameters.Page);

        List = queryable.ToList();
    }

    public long Count { get; }

    public IEnumerable<T> List { get; }

    public GridParameters Parameters { get; }

    private static ISugarQueryable<T> Filter(ISugarQueryable<T> queryable, Filters filters)
    {
        return filters is null ? queryable : filters.Aggregate(queryable, (current, filter) => current.Filter(filter.Property, filter.Comparison, filter.Value));
    }

    private static ISugarQueryable<T> Order(ISugarQueryable<T> queryable, Order order)
    {
        return order is null ? queryable : queryable.Order(order.Property, order.Ascending);
    }

    private static ISugarQueryable<T> Page(ISugarQueryable<T> queryable, Page page)
    {
        return page is null ? queryable : queryable.Page(page.Index, page.Size);
    }
}
