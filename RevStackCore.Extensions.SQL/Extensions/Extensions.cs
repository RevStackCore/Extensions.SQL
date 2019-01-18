using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace RevStackCore.Extensions.SQL
{
    public static class Extensions
    {
        public static IQueryable<TEntity> Find<TEntity>(this IDbConnection src, Expression<Func<TEntity, bool>> predicate, SQLLanguageType type) where TEntity: class
        {
            SQLQueryProvider queryProvider = new SQLQueryProvider(src,type);
            return new Query<TEntity>(queryProvider).Where(predicate);
        }
    }
}
