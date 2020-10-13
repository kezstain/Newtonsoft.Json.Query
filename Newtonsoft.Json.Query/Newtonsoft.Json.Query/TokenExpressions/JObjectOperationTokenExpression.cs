using System;
using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Query.TokenExpressions
{
    internal class JObjectOperationTokenExpression : IJObjectTokenExpression
    {
        private readonly IJObjectTokenExpression _leftSideLogic;
        private readonly string _operation;
        private readonly IJObjectTokenExpression _rightSideLogic;

        public JObjectOperationTokenExpression(string leftSide, string operation, string rightSide)
        {
            _leftSideLogic = JObjectTokenExpressionBuilder.GetOperatorLogic(leftSide);
            _operation = operation;
            _rightSideLogic = JObjectTokenExpressionBuilder.GetOperatorLogic(rightSide);
        }

        public JToken Evaluate(JObject jObject)
        {
            var left = (JValue)_leftSideLogic.Evaluate(jObject);
            var right = (JValue)_rightSideLogic.Evaluate(jObject);
            switch (_operation)
            {
                case "=":
                    return new JValue(left.Equals(right));
                case "!=":
                    return new JValue(!left.Equals(right));
                case ">":
                    return new JValue(left.CompareTo(right)>0);
                case ">=":
                    return new JValue(left.CompareTo(right)>=0);
                case "<":
                    return new JValue(left.CompareTo(right)<0);
                case "<=":
                    return new JValue(left.CompareTo(right)<=0);
            }
            throw new NotImplementedException(_operation);
        }
    }
}