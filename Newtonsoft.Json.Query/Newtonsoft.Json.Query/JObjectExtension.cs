﻿using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Query
{

    public static class JObjectExtension
    {
        public static bool IsMatch(this JObject obj, string query)
        {
            var jObjectFilter = JObjectTokenExpressionBuilder.GetOperatorLogic(query);
            return (bool)jObjectFilter.Evaluate(obj);
        }
    }
}
