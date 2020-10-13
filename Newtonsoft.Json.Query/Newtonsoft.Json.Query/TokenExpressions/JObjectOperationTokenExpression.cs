using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Query.Exceptions;

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
                case "^=": //starts with
                {
                    if(left.Type!=JTokenType.String && right.Type!=JTokenType.String)
                        throw new FeatureNotSupportedException(); //only support string to string starts with
                    var startsWith = ((string) left).StartsWith((string) right);
                    return new JValue(startsWith);
                }
                case "$=": //ends with
                {
                    if(left.Type!=JTokenType.String && right.Type!=JTokenType.String)
                        throw new FeatureNotSupportedException(); //only support string to string starts with
                    var startsWith = ((string) left).EndsWith((string) right);
                    return new JValue(startsWith);
                }
                case "*=": //contains
                {
                    if(left.Type!=JTokenType.String && right.Type!=JTokenType.String)
                        throw new FeatureNotSupportedException(); //only support string to string starts with
                    var startsWith = ((string) left).Contains((string) right);
                    return new JValue(startsWith);
                }
            }
            throw new NotImplementedException(_operation);
        }
    }
}