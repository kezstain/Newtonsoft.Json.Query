using System;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Query.TokenExpressions
{
    internal class JObjectMethodTokenExpression : IJObjectTokenExpression
    {
        private readonly string _method;
        private readonly IJObjectTokenExpression _methodLogic;

        private JObjectMethodTokenExpression(ReadOnlySpan<char> method, ReadOnlySpan<char> argument)
        {
            _method = method.ToString();
            _methodLogic = JObjectTokenExpressionBuilder.GetOperatorLogic(argument);
        }

        internal static bool TryParse(ReadOnlySpan<char> query, out JObjectMethodTokenExpression expression)
        {
            expression = null;
            //if it ends with a round bracket we know these have been trimmed from both ends so it must be an array method
            if (!query.EndsWith(")"))
                return false;

            if (query.StartsWith("Any("))
            {
                var method = query[..3];
                var argument = query[3..];
                {
                    expression = new JObjectMethodTokenExpression(method, argument);
                    return true;
                }
            }

            if (query.StartsWith("Sum("))
            {
                var method = query[..3];
                var argument = query[3..];
                {
                    expression = new JObjectMethodTokenExpression(method, argument);
                    return true;
                }
            }

            return false;
        }

        public JToken Evaluate(JToken jObject, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            if (_method == "Any")
            {
                var result = (JArray)_methodLogic.Evaluate(jObject, stringComparison) ?? throw new InvalidOperationException();
                return new JValue(result.Any(r => r.Value<bool>()));
            }
            if (_method == "Sum")
            {
                var result = (JArray)_methodLogic.Evaluate(jObject, stringComparison) ?? throw new InvalidOperationException();
                return new JValue(result.Sum(r => r.Value<double>()));
            }

            throw new NotImplementedException();
        }
    }
}