using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Query.TokenExpressions
{
    public class JObjectArrayTokenExpression : IJObjectTokenExpression
        {
            private readonly string _method;
            private readonly string _argument;
            private IJObjectTokenExpression _leftSideLogic;
            private readonly string _oper;
            private IJObjectTokenExpression _rightSideLogic;

            public JObjectArrayTokenExpression(string method, string argument)
            {
                _method = method;
                _argument = argument;
                _rightSideLogic = JObjectTokenExpressionBuilder.GetOperatorLogic(_argument);
            }

            public JToken Evaluate(JToken jObject, StringComparison stringComparison = StringComparison.CurrentCulture)
            {
                var tokens = jObject.SelectToken(_method);

                var items = (((JArray)tokens) ?? throw new InvalidOperationException())
                    .Select(t => _rightSideLogic.Evaluate(t));
                
                return new JArray(items);
            }
        }
}