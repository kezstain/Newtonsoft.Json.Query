using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Query.Exceptions;

namespace Newtonsoft.Json.Query.TokenExpressions
{
    internal class JObjectOperationTokenExpression : IJObjectTokenExpression
    {
        private readonly IJObjectTokenExpression _leftSideLogic;
        private readonly string _leftSide;
        private readonly string _operation;
        private readonly string _rightSide;
        private readonly IJObjectTokenExpression _rightSideLogic;

        public JObjectOperationTokenExpression(string leftSide, string operation, string rightSide)
        {
            _leftSideLogic = JObjectTokenExpressionBuilder.GetOperatorLogic(leftSide);
            _leftSide = leftSide;
            _operation = operation;
            _rightSide = rightSide;
            _rightSideLogic = JObjectTokenExpressionBuilder.GetOperatorLogic(rightSide);
        }

        public JToken Evaluate(JObject jObject)
        {
            var left = (JValue)_leftSideLogic.Evaluate(jObject);
            var right = (JValue)_rightSideLogic.Evaluate(jObject);
            //if(left==null && left)

            switch (_operation)
            {
                case "&":
                    if (left.Type != JTokenType.Boolean && right.Type != JTokenType.Boolean)
                        throw new InvalidQueryException("Boolean values expected",$"{_leftSide}{_operation}^{_rightSide}");
                    return new JValue((bool)left && (bool)right);
                case "|":
                    if (left.Type != JTokenType.Boolean && right.Type != JTokenType.Boolean)
                        throw new InvalidQueryException("Boolean values expected",$"{_leftSide}{_operation}^{_rightSide}");
                    return new JValue((bool)left || (bool)right);
                case "=":
                    return new JValue(left.Equals(right));
                case "!=":
                    return new JValue(!left.Equals(right));
                case ">":
                    if (left.Type != right.Type)
                    {
                    
                        using var leftSw = new StringWriter(CultureInfo.InvariantCulture);
                        left.WriteTo(new JsonTextWriter(leftSw));

                        using var rightSw = new StringWriter(CultureInfo.InvariantCulture);
                        right.WriteTo(new JsonTextWriter(rightSw));

                        return new JValue(leftSw.ToString().CompareTo(rightSw.ToString()));

                    }
                    return new JValue(left.CompareTo(right)>0);
                case ">=":
                    if (left.Type != right.Type)
                        throw new InvalidQueryException("Boolean values expected",$"{_leftSide}{_operation}^{_rightSide}");
                    return new JValue(left.CompareTo(right)>=0);
                case "<":
                    if (left.Type != right.Type)
                        throw new InvalidQueryException("Boolean values expected",$"{_leftSide}{_operation}^{_rightSide}");
                    return new JValue(left.CompareTo(right)<0);
                case "<=":
                    if (left.Type != right.Type)
                        throw new InvalidQueryException("Boolean values expected",$"{_leftSide}{_operation}^{_rightSide}");
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
                    var leftValue = left.Value?.ToString() ?? string.Empty;
                    var rightValue = right.Value?.ToString() ?? string.Empty;
                    var startsWith = leftValue.Contains(rightValue);
                    return new JValue(startsWith);
                }
            }
            throw new NotImplementedException(_operation);
        }
    }
}