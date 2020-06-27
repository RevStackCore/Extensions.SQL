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
    }
}
