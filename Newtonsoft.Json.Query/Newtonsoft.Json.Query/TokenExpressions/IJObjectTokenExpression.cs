using Newtonsoft.Json.Linq;
using System;

namespace Newtonsoft.Json.Query.TokenExpressions
{
    public interface IJObjectTokenExpression
    {
        JToken Evaluate(JToken jObject, StringComparison stringComparison = StringComparison.CurrentCulture);
    }
}