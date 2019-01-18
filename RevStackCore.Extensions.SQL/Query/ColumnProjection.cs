using System;
using System.Linq.Expressions;

namespace RevStackCore.Extensions.SQL
{
    internal class ColumnProjection
    {
        internal string Columns;
        internal Expression Selector;
    }
}
