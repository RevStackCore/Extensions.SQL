using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Dapper;

namespace RevStackCore.Extensions.SQL
{
    public class SQLQueryProvider : QueryProvider
    {
        private readonly IDbConnection _connection;
        public SQLQueryProvider(IDbConnection connection, SQLLanguageType languageType):base(languageType)
        {
            _connection = connection;
        }

        public override object Execute(Expression expression)
        {
            string resultSql = Translate(expression);
            var results = SqlMapper.Query(_connection, resultSql) as IEnumerable<IDictionary<string, object>>;
            return results;
        }

        public override IEnumerable<T> ExecuteQuery<T>(Expression expression)
        {
            string resultSql = Translate(expression);
            Type elementType = TypeSystem.GetElementType(expression.Type);
            return SqlMapper.Query<T>(_connection, resultSql);
        }
    }
}
