using System;
using System.Linq.Expressions;

namespace RevStackCore.Extensions.SQL
{
    internal class TranslateResult
    {
        internal string CommandText;
        internal LambdaExpression Projector;
    }
}
