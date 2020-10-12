using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Query
{
    public interface IJObjectTokenExpression
    {
        JToken Evaluate(JObject jObject);
    }
}