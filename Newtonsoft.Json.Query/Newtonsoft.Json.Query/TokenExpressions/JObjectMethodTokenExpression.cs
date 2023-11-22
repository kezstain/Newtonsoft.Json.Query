using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Query.TokenExpressions
{
    internal class JObjectMethodTokenExpression : IJObjectTokenExpression
    {
        private readonly string _method;
        private readonly string _argument;
        private IJObjectTokenExpression _leftSideLogic;
        private readonly string _oper;
        private IJObjectTokenExpression _rightSideLogic;

        public JObjectMethodTokenExpression(string method, string argument)
        {
            _method = method;
            _argument = argument;
            _rightSideLogic = JObjectTokenExpressionBuilder.GetOperatorLogic(_argument);
        }

        public JToken Evaluate(JToken jObject, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            if (_method == "Any")
            {
                var result = (JArray)_rightSideLogic.Evaluate(jObject, stringComparison) ?? throw new InvalidOperationException();
                return new JValue(result.Any());
            }

            throw new NotImplementedException();
        }
    }
}