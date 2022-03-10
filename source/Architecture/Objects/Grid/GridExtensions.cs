using SqlSugar;

namespace Architecture.Objects;

public static class GridExtensions
{
    public static Grid<T> Grid<T>(this ISugarQueryable<T> queryable, GridParameters parameters)
    {
        return new(queryable, parameters);
    }

    public static Task<Grid<T>> GridAsync<T>(this ISugarQueryable<T> queryable, GridParameters parameters)
    {
        return Task.FromResult(new Grid<T>(queryable, parameters));
    }
}
