using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Query.TokenExpressions
{
    internal class JObjectValueTokenExpression : IJObjectTokenExpression
    {
        private readonly string _query;

        public JObjectValueTokenExpression(string query)
        {
            _query = query;
        }

        public JToken Evaluate(JObject jObject)
        {
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