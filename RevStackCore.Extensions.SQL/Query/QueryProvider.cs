using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RevStackCore.Extensions.SQL
{
    public abstract class QueryProvider : IQueryProvider
    {
        private readonly SQLLanguageType _languageTpe;
        private readonly string _entityType;
        protected QueryProvider(SQLLanguageType languageType)
        {
            _languageTpe = languageType;
        }

        protected QueryProvider(SQLLanguageType languageType, string type)
        {
            _languageTpe = languageType;
            _entityType = type;
        }

        IQueryable<T> IQueryProvider.CreateQuery<T>(Expression expression)
        {
            return new Query<T>(this, expression);
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(Query<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        T IQueryProvider.Execute<T>(Expression expression)
        {
            IEnumerable results = (IEnumerable)this.ExecuteQuery<T>(expression);
            IEnumerable<T> en = results.Cast<T>();
            return en.FirstOrDefault<T>();
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return this.Execute(expression);
        }

        protected string GetQueryText(Expression expression)
        {
            return this.Translate(expression);
        }

        protected string Translate(Expression expression)
        {
            if (!string.IsNullOrEmpty(_entityType))
            {
                return new QueryTranslator(_languageTpe, _entityType).Translate(expression).CommandText;
            }
            else
            {
                return new QueryTranslator(_languageTpe).Translate(expression).CommandText;
            }
        }

        public abstract object Execute(Expression expression);
        public abstract IEnumerable<T> ExecuteQuery<T>(Expression expression);
    }
}
