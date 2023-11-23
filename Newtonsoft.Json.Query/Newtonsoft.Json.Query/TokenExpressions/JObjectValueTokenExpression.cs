using Newtonsoft.Json.Linq;
using System;

namespace Newtonsoft.Json.Query.TokenExpressions
{
    internal class JObjectValueTokenExpression : IJObjectTokenExpression
    {
        private readonly string _query;

        public JObjectValueTokenExpression(ReadOnlySpan<char> query)
        {
            _query = query.ToString();
        }

        public JToken Evaluate(JToken jObject, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            if (_query.StartsWith(".."))
            {
                if (_query.Length <= 2) 
                    return jObject;

                var tokenPath = _query.Substring(2);
                return jObject.SelectToken(tokenPath);
            }

            if (_query.StartsWith("."))
            {
                var tokenPath = _query.Substring(1);
                return jObject.SelectToken(tokenPath);
            }

            //escape string values
            return JToken.Parse(_query);
        }
    }
}