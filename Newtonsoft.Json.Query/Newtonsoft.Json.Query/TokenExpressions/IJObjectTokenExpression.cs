using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Query.TokenExpressions
{
    public interface IJObjectTokenExpression
    {
        JToken Evaluate(JObject jObject);
    }
}