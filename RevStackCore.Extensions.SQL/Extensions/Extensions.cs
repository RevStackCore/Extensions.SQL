using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RevStackCore.DataAnnotations;

namespace RevStackCore.Extensions.SQL
{
    public static class Extensions
    {
        public static IQueryable<TEntity> Find<TEntity>(this IDbConnection src, Expression<Func<TEntity, bool>> predicate, SQLLanguageType type) where TEntity: class
        {
            SQLQueryProvider queryProvider;
            Type entityType = typeof(TEntity);
            var tableAttribute = entityType.GetCustomAttribute<TableAttribute>(true);
            if (tableAttribute != null && !string.IsNullOrEmpty(tableAttribute.Name))
            {
                queryProvider = new SQLQueryProvider(src, type, tableAttribute.Name);
                return new Query<TEntity>(queryProvider, tableAttribute.Name, true).Where(predicate);
            }
            else
            {
                queryProvider = new SQLQueryProvider(src, type);
                return new Query<TEntity>(queryProvider).Where(predicate);
            }
        }

        public static string ToPropertyName(this PropertyInfo property)
        {
            var columnAttribute = property.GetCustomAttribute<ColumnAttribute>(false);
            if (columnAttribute != null && !string.IsNullOrEmpty(columnAttribute.Name))
            {
                return columnAttribute.Name;
            }
            else
            {
                return property.Name;
            }
        }

        public static string ToTableName(this Type type)
        {
            var tableAttribute = type.GetCustomAttribute<TableAttribute>(true);
            if (tableAttribute != null && !string.IsNullOrEmpty(tableAttribute.Name))
            {
                return tableAttribute.Name;
            }
            else
            {
                return type.Name;
            }
        }


    }
}
