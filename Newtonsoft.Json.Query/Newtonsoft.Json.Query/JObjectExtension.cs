using Newtonsoft.Json.Linq;
using System;

namespace Newtonsoft.Json.Query
{

    public static class JObjectExtension
    {
        public static bool IsMatch(this JObject obj, string query, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            var jObjectFilter = JObjectTokenExpressionBuilder.GetOperatorLogic(query);
            return (bool)jObjectFilter.Evaluate(obj, stringComparison);
        }
    }
}
