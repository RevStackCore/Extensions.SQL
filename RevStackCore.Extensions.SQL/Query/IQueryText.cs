using System;
using System.Linq.Expressions;

namespace RevStackCore.Extensions.SQL
{
    public interface IQueryText
    {
        string GetQueryText(Expression expression);
    }
}
