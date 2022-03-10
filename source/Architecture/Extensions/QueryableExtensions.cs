using SqlSugar;
using System.Linq.Expressions;

namespace Architecture.Extensions;

public static class QueryableExtensions
{
    public static ISugarQueryable<T> Filter<T>(this ISugarQueryable<T> queryable, string property, object value)
    {
        return queryable.Filter(property, string.Empty, value);
    }

    public static ISugarQueryable<T> Filter<T>(this ISugarQueryable<T> queryable, string property, string comparison, object value)
    {
        if (string.IsNullOrWhiteSpace(property))
            return queryable;

        if(value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return queryable;

        var parameter = Expression.Parameter(typeof(T));

        var left = Create(property, parameter);

        try
        {
            if (left.Type.BaseType == typeof(Enum))
            {
                value = Enum.Parse(left.Type, value.ToString() ?? string.Empty);
            }
            value = Convert.ChangeType(value, left.Type);
        }
        catch
        {
            return queryable;
        }

        var right = Expression.Constant(value, left.Type);

        var body = Create(left, comparison, right);

        var expression = Expression.Lambda<Func<T, bool>>(body, parameter);

        return queryable.Where(expression);
    }

    public static ISugarQueryable<T> Order<T>(this ISugarQueryable<T> queryable, string property, bool ascending)
    {
        if (queryable is null || string.IsNullOrWhiteSpace(property)) return queryable;

        var parameter = Expression.Parameter(typeof(T));

        var body = Create(property, parameter);

        var expression = (dynamic)Expression.Lambda(body, parameter);

        return ascending ? Queryable.OrderBy(queryable, expression) : Queryable.OrderByDescending(queryable, expression);
    }

    public static ISugarQueryable<T> Page<T>(this ISugarQueryable<T> queryable, int index, int size)
    {
        if (queryable is null || index <= 0 || size <= 0) return queryable;

        return queryable.Skip((index - 1) * size).Take(size);
    }

    private static Expression Create(string property, Expression parameter)
    {
        return property.Split('.').Aggregate(parameter, Expression.Property);
    }

    private static Expression Create(Expression left, string comparison, Expression right)
    {
        if (string.IsNullOrWhiteSpace(comparison) && left.Type == typeof(string))
        {
            return Expression.Call(left, nameof(string.Contains), Type.EmptyTypes, right);
        }

        var type = comparison switch
        {
            "<" => ExpressionType.LessThan,
            "<=" => ExpressionType.LessThanOrEqual,
            ">" => ExpressionType.GreaterThan,
            ">=" => ExpressionType.GreaterThanOrEqual,
            "!=" => ExpressionType.NotEqual,
            _ => ExpressionType.Equal
        };

        return Expression.MakeBinary(type, left, right);
    }
}
