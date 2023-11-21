﻿using System;
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

        public JToken Evaluate(JObject jObject, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            throw new NotImplementedException();
        }
    }
}