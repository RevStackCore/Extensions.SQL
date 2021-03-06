﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RevStackCore.Extensions.SQL
{
 
    public class Query<T> : IQueryable<T>, IQueryable, IEnumerable<T>, IEnumerable, IOrderedQueryable<T>, IOrderedQueryable
    {
        IQueryProvider provider;
        Expression expression;
        private string _type;
        public Query(IQueryProvider provider)
            : this(provider, null)
        {
        }

        public Query(IQueryProvider provider, string type, bool sigFiller)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("Provider");
            }
            _type = type;
            this.provider = provider;
            this.expression = Expression.Constant(this);
        }

        public Query(IQueryProvider provider, Type staticType)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("Provider");
            }
            this.provider = provider;
            this.expression = staticType != null ? Expression.Constant(this, staticType) : Expression.Constant(this);
        }

        public Query(QueryProvider provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("Provider");
            }
            if (expression == null)
            {
                expression = Expression.Constant(this);
                this.expression = expression;
            }
            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }
            this.provider = provider;
            this.expression = expression;
        }

        public Expression Expression
        {
            get { return this.expression; }
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public string Type
        {
            get
            {
                if (!string.IsNullOrEmpty(_type))
                {
                    return _type;
                }
                else
                {
                    return typeof(T).Name;
                }
            }
        }

        public IQueryProvider Provider
        {
            get { return this.provider; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var type = typeof(T);
            MethodInfo method = this.provider.GetType().GetMethod("ExecuteQuery");
            MethodInfo generic = method.MakeGenericMethod(type);
            IEnumerable<T> results = (IEnumerable<T>)generic.Invoke(this.provider, new object[] { this.expression });
            IEnumerator<T> en = results.Cast<T>().GetEnumerator();
            return en;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.provider.Execute(this.expression)).GetEnumerator();
        }

        public override string ToString()
        {
            string type = typeof(T).Name;
            if (!string.IsNullOrEmpty(_type))
            {
                type = _type;
            }
            if (this.expression.NodeType == ExpressionType.Constant &&
                ((ConstantExpression)this.expression).Value == this)
            {
                return "Query(" + type + ")";
            }
            else
            {
                return this.expression.ToString();
            }
        }

        public string QueryText
        {
            get
            {
                IQueryText iqt = this.provider as IQueryText;
                if (iqt != null)
                {
                    return iqt.GetQueryText(this.expression);
                }
                return "";
            }
        }
    }
}
