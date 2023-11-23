using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Query.TokenExpressions
{
    public class JObjectArrayTokenExpression : IJObjectTokenExpression
        {
            private readonly string _method;
            private readonly IJObjectTokenExpression _rightSideLogic;

            public JObjectArrayTokenExpression(ReadOnlySpan<char> method, ReadOnlySpan<char> argument)
            {
                _method = method.ToString();
                _rightSideLogic = JObjectTokenExpressionBuilder.GetOperatorLogic(argument);
            }
        

            internal static bool TryParse(ReadOnlySpan<char> query, out IJObjectTokenExpression expression)
            {
                expression = null;

                //if it ends with a square bracket we know these have been trimmed from both ends so it must be an array method
                if (!query.EndsWith("]"))
                    return false;

                var queryStart = query.IndexOf('[');
                if (queryStart > -1)
                {
                    var path = query[..queryStart];
                    var argument = query[queryStart..];
                    expression = new JObjectArrayTokenExpression(path, argument);
                    return true;
                }

                return false;
            }

            public JToken Evaluate(JToken jObject, StringComparison stringComparison = StringComparison.CurrentCulture)
            {
                var tokens = jObject.SelectToken(_method);

                var items = (((JArray)tokens) ?? throw new InvalidOperationException())
                    .Select(t => _rightSideLogic.Evaluate(t, stringComparison));
                
                return new JArray(items);
            }
        }
}