using Newtonsoft.Json.Linq;
using System;

namespace Newtonsoft.Json.Query.TokenExpressions
{
    public interface IJObjectTokenExpression
    {
        JToken Evaluate(JObject jObject, StringComparison stringComparison = StringComparison.CurrentCulture);
    }
}